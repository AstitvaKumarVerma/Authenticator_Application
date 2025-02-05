using Authenticator_Application_Backend.Models;

namespace Authenticator_Application_Backend.Services.Interfaces
{
    public interface IGoogleAuthenticatorService
    {
        public AuthProviderModel GenerateQrSetup(string email);
        Task<bool> ValidateTwoFactorPin(AuthInputModel authInputModel);
        Task<bool> Disable2Fa(AuthInputModel authInputModel);
    }
}
