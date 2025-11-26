using System.Linq.Expressions;

namespace Business_Layer.Services;

public class ComplaintService(AppDbContext context, IMapper mapper) : IComplaintService
{
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

            if (paths?.Count > 0)
            {
                // foreach (var path in paths)
                // {
                //     var attachment = new Attachment
                //     {
                //         ComplaintId = complaintToAdd.Id,
                //         FilePath = path,
                //         UploadedAt = DateTime.Now
                //     };
                //
                //     complaintToAdd.Attachments.Add(attachment);
                // }

                complaintToAdd.Attachments = paths.Select(path =>
                    new Attachment
                    {
                        FilePath = path,
                        UploadedAt = DateTime.UtcNow
                    }).ToList();
            }

            var complaint = await context.Complaints.AddAsync(complaintToAdd);

            await context.SaveChangesAsync();

            // Map
            var complaintHistory = new ComplaintHistory
            {
                ComplaintId = complaintToAdd.Id,
                UserId = dto.CitizenId,
                DepartmentId = dto.DepartmentId,
                Title = dto.Title,
                Location = dto.Location,
                Description = dto.Description,
                Status = complaintToAdd.Status,
                CreatedAt = DateTime.Now,
            };

            await context.ComplaintHistories.AddAsync(complaintHistory);

            await context.SaveChangesAsync();

            await transaction.CommitAsync();

            // await firebaseService.NotifyAdmin("New Complaint",complaintToAdd.Title + " : " + complaintToAdd.Description);

            response.Result = mapper.Map<GetComplaintDto>(complaint.Entity);

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
}