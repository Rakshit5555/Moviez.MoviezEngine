using System;
namespace Moviez.MoviezEngine.Models.ResponseModels
{
    public class BaseResponseModelForUser
    {
        
        public bool Status { get; set; }
        public string Message { get; set; }
    }
    public class BaseResponseModel : BaseResponseModelForUser
    {
        public long Id { get; set; }
    }
}

