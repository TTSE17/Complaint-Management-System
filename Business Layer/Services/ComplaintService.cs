using Attachment = Data_Access_Layer.Attachment;

namespace Business_Layer.Services;

public class ComplaintService(AppDbContext context, IMapper mapper) : IComplaintService
{
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
            .Include(c => c.Citizen)
            .ThenInclude(c => c.User)
            .Include(c => c.Department)
            .Include(c => c.Attachments)
            .FirstAsync();

        response.Result = mapper.Map<GetComplaintDto>(complaint);

        response.Success = true;

        return response;
    }
}