using Authenticator_Application_Backend.Model;
using Authenticator_Application_Backend.Models;
using Authenticator_Application_Backend.Services.Interfaces;
using Google.Authenticator;
using OtpNet;
using System.Text;

namespace Authenticator_Application_Backend.Services
{
    public class MicrosoftAuthenticatorService: IMicrosoftAuthenticatorService
    {
        private readonly IConfiguration _config;
        private readonly AuthenticatorDbContext _context;

        public MicrosoftAuthenticatorService(
            AuthenticatorDbContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// for generating Secret Key for Google Authenticator
        /// </summary>
        /// <returns></returns>
        private string TwoFactorSecretKey(string email)
        {
            return $"{_config["MicrosoftAuthenticator:SecretKey"]}{email}";
        }

        public string GenerateTotpCode(string email)
        {
            var secretKey = TwoFactorSecretKey(email);
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var totp = new Totp(secretKeyBytes);
            return totp.ComputeTotp();  
        }

        /// <summary>
        /// Generating QR Code and Key
        /// </summary>
        /// <returns></returns>
        public AuthProviderModel GenerateQrSetup(string email)
        {
            AuthProviderModel authProviderModel = new AuthProviderModel();

            TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();

            /*   GenerateSetupCode function takes 5 parameters ( Issuer, userdata, secretKey, isStringBase32, sizeOfQR )   */
            SetupCode setupCode = twoFactor.GenerateSetupCode(_config["MicrosoftAuthenticator:Issuer"], email,
                TwoFactorSecretKey(email), false, 3);

            if (setupCode != null)
            {
                authProviderModel.Account = setupCode.Account;
                authProviderModel.ManualEntryKey = setupCode.ManualEntryKey;
                authProviderModel.QrCodeSetupImageUrl = setupCode.QrCodeSetupImageUrl;

                return authProviderModel;
            }
            return authProviderModel;
        }


        public bool VerifyTotpCode(string email, string userCode)
        {
            var secretKey = TwoFactorSecretKey(email);
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var totp = new Totp(secretKeyBytes, VerificationWindow: new VerificationWindow(previous: 1, future: 1));

            long timeStepMatched; // Stores the time step that matched the code (optional)
            return totp.VerifyTotp(userCode, out timeStepMatched);
        }
    }
}
