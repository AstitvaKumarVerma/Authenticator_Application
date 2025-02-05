namespace Authenticator_Application_Backend.Enums
{
    public enum StatusCodesEnum
    {
        LoginSucess = 1,
        TwoFactorAuthenticatorActivated = 2,
        IncorrectPin = 3,
        BadRequest = 4,
        Success = 5,
        UserNameAlreadyExist = 6,
        Error = 7,
        EnterUniquePassword = 8,
        UserDoesNotExist = 9,
        TwoFactorAuthenticatorDeactivated = 10
    }
}
