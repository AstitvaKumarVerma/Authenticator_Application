using Authenticator_Application_Backend.Model;

namespace Authenticator_Application_Backend.Messages
{
    public static class CommonErrorMessages
    {
        public const string LoginFailed = "!!! Login Failed !!!";
        public const string UserAlreadyExists ="User Already Exists.";
        public const string UserDoesNotExists = "User does not exist.";
        public const string EmailNotFound = "Email Not Found.";
        public const string OtpNotMatched = "Otp not matched";
        public const string UnableToGenerateQrCode = " Unable to generate Setup Code.";
    }
}