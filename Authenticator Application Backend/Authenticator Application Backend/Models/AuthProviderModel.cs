namespace Authenticator_Application_Backend.Models
{
    public class AuthProviderModel
    {
        public string Account { get; set; }
        public string ManualEntryKey { get; set; }
        public string QrCodeSetupImageUrl { get; set; }
    }
}
