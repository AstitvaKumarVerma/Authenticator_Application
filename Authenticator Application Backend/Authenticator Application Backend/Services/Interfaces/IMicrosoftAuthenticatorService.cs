using Authenticator_Application_Backend.Models;

namespace Authenticator_Application_Backend.Services.Interfaces
{
    public interface IMicrosoftAuthenticatorService
    {
        public AuthProviderModel GenerateQrSetup(string email);
        public bool VerifyTotpCode(string email, string userCode);
    }
}
