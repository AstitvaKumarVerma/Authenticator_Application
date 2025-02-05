using Authenticator_Application_Backend.Model;
using Authenticator_Application_Backend.Models;
using Authenticator_Application_Backend.Services.Interfaces;
using Google.Authenticator;

namespace Authenticator_Application_Backend.Services
{
    public class GoogleAuthenticatorService: IGoogleAuthenticatorService
    {
        private readonly IConfiguration _config;
        private readonly AuthenticatorDbContext _context;

        public GoogleAuthenticatorService(
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
            return $"{ _config["GoogleAuthenticator:SecretKey"] }{email}";
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
            SetupCode setupCode = twoFactor.GenerateSetupCode(_config["GoogleAuthenticator:Issuer"], email,
                TwoFactorSecretKey(email), false,3);  

            if (setupCode != null)
            {
                authProviderModel.Account = setupCode.Account;
                authProviderModel.ManualEntryKey = setupCode.ManualEntryKey;
                authProviderModel.QrCodeSetupImageUrl = setupCode.QrCodeSetupImageUrl;

                return authProviderModel;
            }
            return authProviderModel;
        }

        /// <summary>
        /// Valideting the Two Factor Pin
        /// </summary>
        /// <param name="authInputModel"></param>
        /// <returns></returns>
        public async Task<bool> ValidateTwoFactorPin(AuthInputModel authInputModel)
        {
            TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
            bool isValid = twoFactor.ValidateTwoFactorPIN(TwoFactorSecretKey(authInputModel.Email), authInputModel.Pin.ToString().Replace(" ", ""));
            if (!isValid)
                return isValid;

            var userData = _context.Users.FirstOrDefault(x => x.Email == authInputModel.Email);
            if (userData != null)
            {
                userData.IsAuthenticated = true;
                await _context.SaveChangesAsync();
            }
            return isValid;
        }

        /// <summary>
        /// Disble 2FA through Google Authenticator
        /// </summary>
        /// <param name="authInputModel"></param>
        /// <returns></returns>
        public async Task<bool> Disable2Fa(AuthInputModel authInputModel)
        {
            TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();

            bool isValid = twoFactor.ValidateTwoFactorPIN(TwoFactorSecretKey(authInputModel.Email), authInputModel.Pin.ToString().Replace(" ", ""));
            if (!isValid)
                return isValid;

            var user = _context.Users.FirstOrDefault(x => x.Email == authInputModel.Email);

            if (user != null)
            {
                user.IsAuthenticated = false;
                await _context.SaveChangesAsync();
            }
            return isValid;
        }
    }
}
