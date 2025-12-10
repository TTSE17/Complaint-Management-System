using System.Linq.Expressions;
using System.Text.Json;

namespace Business_Layer.Services;

public class ComplaintService(AppDbContext context, IMapper mapper, IFirebaseService firebaseService)
    : IComplaintService
{
    [TracingAspect("View Complaints")]
    public async Task<Response<List<GetComplaintDto>>> GetAll(Expression<Func<Complaint, bool>>? criteria = null)
    {
        var response = new Response<List<GetComplaintDto>>();

        var query = context.Complaints.AsNoTracking();

        if (criteria != null)
        {
            query = query.Where(criteria);
        }

        var complaints = await query
            .Select(c => new GetComplaintDto
            {
                Id = c.Id,
                CitizenId = c.CitizenId,
                CitizenName = c.Citizen.User.FirstName + " " + c.Citizen.User.LastName,
                DepartmentId = c.DepartmentId,
                DepartmentName = c.Department.Name,
                // Attachments = c.Attachments.Select(attachment => attachment.FilePath).ToList(),
                Description = c.Description,
                Title = c.Title,
                Location = c.Location,
                Status = c.Status.ToString(),
                StartDate = c.CreatedAt
            })
            .ToListAsync();

        response.Result = complaints;

        response.Success = true;

        return response;
    }

    public async Task<Response<GetComplaintDto>> Add(AddComplaintDto dto)
    {
        var response = new Response<GetComplaintDto>();

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var complaintToAdd = mapper.Map<Complaint>(dto);

            var paths = dto.Paths;

            if (paths.Count > 0)
            {
                complaintToAdd.Attachments = paths.Select(path =>
                    new Attachment
                    {
                        FilePath = path,
                        UploadedAt = DateTime.UtcNow
                    }).ToList();
            }

            complaintToAdd.HistoryJson = JsonSerializer.Serialize(new List<ComplaintSnapshot>
            {
                new()
                {
                    Version = 1,
                    UserId = complaintToAdd.CitizenId,
                    DepartmentId = complaintToAdd.DepartmentId,
                    Status = complaintToAdd.Status,
                    Title = complaintToAdd.Title,
                    Description = complaintToAdd.Description,
                    Location = complaintToAdd.Location,
                    Action = "Created",
                    ChangedAt = DateTime.UtcNow,
                    Attachments = paths
                }
            });

            var complaint = (await context.Complaints.AddAsync(complaintToAdd)).Entity;

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            await firebaseService.NotifyAdmin("New Complaint",
                complaintToAdd.Title + " : " + complaintToAdd.Description);

            response.Result = mapper.Map<GetComplaintDto>(complaint);

            response.Result.Attachments = paths;

            response.Success = true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();

            response.Error = e.Message;
        }

        return response;
    }

    public async Task<Response<GetComplaintDto>> GetDetails(int id)
    {
        var response = new Response<GetComplaintDto>();

        var query = context.Complaints.AsNoTracking().Where(c => c.Id == id);

        if (!query.Any())
        {
            response.Error = $"Complaint {id} does not exist";

            return response;
        }

        var complaint = await query
            .Select(c => new GetComplaintDto
            {
                Id = c.Id,
                CitizenId = c.CitizenId,
                CitizenName = c.Citizen.User.FirstName + " " + c.Citizen.User.LastName,
                DepartmentId = c.DepartmentId,
                DepartmentName = c.Department.Name,
                Attachments = c.Attachments.Select(attachment => attachment.FilePath).ToList(),
                Description = c.Description,
                Title = c.Title,
                Location = c.Location,
                Status = c.Status.ToString(),
                StartDate = c.CreatedAt
            })
            .FirstAsync();

        response.Result = complaint;

        response.Success = true;

        return response;
    }

    public async Task<Response<GetComplaintDto>> Update(UpdateComplaintDto dto)
    {
        var response = new Response<GetComplaintDto>();

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var complaintId = dto.Id;

            var complaint = await context.Complaints.FirstOrDefaultAsync(i => i.Id == complaintId);

            if (complaint == null)
            {
                response.Error = $"Complaint {complaintId} not found";

                return response;
            }

            var action = "Updated";

            mapper.Map(dto, complaint);

            complaint = context.Complaints.Update(complaint).Entity;

            await context.SaveChangesAsync();

            var paths = dto.Paths;

            var currentAttachments = await context.Attachments
                .Where(a => a.ComplaintId == complaintId)
                .ToListAsync();

            var attachmentsToRemove = currentAttachments
                .Where(attachment => !paths.Contains(attachment.FilePath))
                .ToList();

            var removedAttachments = attachmentsToRemove.Count > 0;

            if (removedAttachments)
            {
                action += ", Remove Attachments";

                context.Attachments.RemoveRange(attachmentsToRemove);
            }

            var newAttachments = false;

            if (paths.Count > 0)
            {
                foreach (var attachment in from path in paths
                         where currentAttachments.All(a => a.FilePath != path)
                         select new Attachment
                         {
                             ComplaintId = complaint.Id,
                             FilePath = path,
                             UploadedAt = DateTime.Now
                         })
                {
                    newAttachments = true;
                    complaint.Attachments.Add(attachment);
                }
            }

            if (newAttachments)
                action += ", New Attachments";

            var historyList = complaint.HistoryJson == ""
                ? []
                : JsonSerializer.Deserialize<List<ComplaintSnapshot>>(complaint.HistoryJson) ?? [];

            var snapshot = new ComplaintSnapshot
            {
                Version = historyList.Count + 1,
                UserId = complaint.CitizenId,
                DepartmentId = complaint.DepartmentId,
                Status = complaint.Status,
                Title = complaint.Title,
                Description = complaint.Description,
                Location = complaint.Location,
                Action = action,
                ChangedAt = DateTime.UtcNow,
                Attachments = paths
            };

            historyList.Add(snapshot);

            complaint.HistoryJson = JsonSerializer.Serialize(historyList);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            // await firebaseService.NotifyAdmin("New Complaint",complaintToAdd.Title + " : " + complaintToAdd.Description);

            response.Result = mapper.Map<GetComplaintDto>(complaint);

            response.Result.Attachments = paths;

            response.Success = true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();

            response.Error = e.Message;
        }

        return response;
    }

    public async Task<Response<bool>> Delete(int id)
    {
        var response = new Response<bool>();

        try
        {
            var complaintToDelete = await context.Complaints.FindAsync(id);

            if (complaintToDelete == null)
                response.Error = "Complaints could not be found";
            else
            {
                context.Complaints.Remove(complaintToDelete);

                await context.SaveChangesAsync();

                response.Success = true;
                response.Result = true;
            }
        }
        catch (Exception e)
        {
            response.Error = e.Message;
        }

        return response;
    }

    public async Task<Response<List<GetComplaintSnapshot>>> GetComplaintHistory(int complaintId)
    {
        var response = new Response<List<GetComplaintSnapshot>>();

        try
        {
            var complaint = await context.Complaints.FirstOrDefaultAsync(i => i.Id == complaintId);

            if (complaint == null)
            {
                response.Error = $"Complaint {complaintId} not found";

                return response;
            }

            var history = complaint.HistoryJson == ""
                ? []
                : JsonSerializer.Deserialize<List<ComplaintSnapshot>>(complaint.HistoryJson) ?? [];

            if (history.Count == 0)
            {
                response.Result = [];

                response.Success = true;

                return response;
            }

            // Get unique user and department IDs
            var userIds = history.Select(s => s.UserId).Distinct().ToList();
            var departmentIds = history.Select(s => s.DepartmentId).Distinct().ToList();

            // Preload users & departments
            var users = await context.Users.AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, FullName = u.FirstName + " " + u.LastName })
                .ToDictionaryAsync(u => u.Id, u => u.FullName);

            var departments = await context.Departments.AsNoTracking()
                .Where(d => departmentIds.Contains(d.Id))
                .Select(d => new { d.Id, d.Name })
                .ToDictionaryAsync(d => d.Id, d => d.Name);

            // Map to DTOs
            var historyDto = history.Select(snapshot =>
                new GetComplaintSnapshot
                {
                    Version = snapshot.Version,
                    UserId = snapshot.UserId,
                    UserName = users[snapshot.UserId],
                    DepartmentId = snapshot.DepartmentId,
                    DepartmentName = departments[snapshot.DepartmentId],
                    Status = snapshot.Status.ToString(),
                    Title = snapshot.Title,
                    Description = snapshot.Description,
                    Location = snapshot.Location,
                    Action = snapshot.Action,
                    ChangedAt = snapshot.ChangedAt,
                    Attachments = snapshot.Attachments
                }).ToList();

            response.Result = historyDto;

            response.Success = true;
        }
        catch (Exception e)
        {
            response.Error = e.Message;
        }

        return response;
    }
}