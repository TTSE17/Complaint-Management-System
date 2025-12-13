using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTO.Requests
{
    public class AdminCountsDto
    {
        public int UsersCount { get; set; }
        public int ComplaintsCount { get; set; }
        public int ComplaintsRejected { get; set; }
        public int ComplaintsResolved { get; set; }
        public int ComplaintsPending { get; set; }
    }

}
