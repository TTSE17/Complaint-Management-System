using System.Linq.Expressions;
using Attachment = Data_Access_Layer.Attachment;

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
                Attachments = c.Attachments.Select(attachment => attachment.FilePath).ToList(),
                Description = c.Description,
                Title = c.Title,
                Location = c.Location,
                Status = c.ComplaintStatus.ToString(),
                StartDate = c.StartDate
            })
            .ToListAsync();

        response.Result = mapper.Map<List<GetComplaintDto>>(complaints);

        response.Success = true;

        return response;
    }

    public async Task<Response<GetComplaintDto>> Add(AddComplaintDto dto)
    {
        var response = new Response<GetComplaintDto>();

        try
        {
            var complaintToAdd = mapper.Map<Complaint>(dto);

            var paths = dto.Paths;

            if (paths != null && paths.Count != 0)
            {
                foreach (var attachment in paths.Select(path => new Attachment
                         {
                             ComplaintId = complaintToAdd.Id,
                             FilePath = path,
                             UploadedAt = DateTime.Now
                         }))
                {
                    // await context.Attachments.AddAsync(attachment);

                    complaintToAdd.Attachments.Add(attachment);
                }
            }

            var complaint = await context.Complaints.AddAsync(complaintToAdd);

            await context.SaveChangesAsync();

            var complaintHistory = new ComplaintHistory
            {
                ComplaintId = complaintToAdd.Id,
                UserId = dto.CitizenId,
                Comment = "Add Complaint",
                CreatedAt = DateTime.Now,
            };

            await context.ComplaintHistories.AddAsync(complaintHistory);

            await context.SaveChangesAsync();

            // await firebaseService.NotifyAdmin("New Complaint",complaintToAdd.Title + " : " + complaintToAdd.Description);

            response.Result = mapper.Map<GetComplaintDto>(complaint.Entity);

            response.Result.Attachments = paths;

            response.Success = true;
        }
        catch (Exception e)
        {
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
                Status = c.ComplaintStatus.ToString(),
                StartDate = c.StartDate,
                ComplaintHistories = c.ComplaintHistories.Select(ch => new GetComplaintHistoryDto
                    { Comment = ch.Comment, CreatedAt = ch.CreatedAt }).ToList()
            })
            .FirstAsync();

        response.Result = complaint;

        response.Success = true;

        return response;
    }
}