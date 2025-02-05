using Authenticator_Application_Backend.Enums;

namespace Authenticator_Application_Backend.Response
{
    public class APIResponseModel
    {
        public APIResponseModel(StatusCodesEnum status, object result, string fileType = null)
        {
            StatusCode = status;
            Result = result;
            FileType = fileType;
        }

        public APIResponseModel(StatusCodesEnum status)
        {
            StatusCode = status;
        }
        public object Result { get; set; }
        public StatusCodesEnum StatusCode { get; set; }
        public string FileType { get; set; }
    }
}
