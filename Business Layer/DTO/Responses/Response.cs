using Microsoft.AspNetCore.Mvc;

namespace Business_Layer.DTO.Responses
{
    public class Response<T> : ActionResult
    {
        public T Result { get; set; }

        public bool Success { get; set; }

        public object Error { get; set; }

        // public bool UnAuthorizedRequest { get; set; }

        public int TotalPages { get; set; }
    }
}