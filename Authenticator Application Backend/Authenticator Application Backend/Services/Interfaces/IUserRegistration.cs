using Authenticator_Application_Backend.Models;
using Authenticator_Application_Backend.Response;

namespace Authenticator_Application_Backend.Services.Interfaces
{
    public interface IUserRegistration
    {
        public UserRegistrationResponseModel RegisterUser(UserRegistrationModel model);
        public UserRegistrationResponseModel Login(LoginUserModel model);
        public EmailResponseModel SendOtpToEmail(OtpToEmailModel Email);
        public UserRegistrationResponseModel VerifyOtp(OtpModel otp);
        public BaseResponse DisableAuthentication(OtpToEmailModel Email);
    }
}
