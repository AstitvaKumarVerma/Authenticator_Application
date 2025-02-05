using Authenticator_Application_Backend.Enums;
using Authenticator_Application_Backend.Model;
using Authenticator_Application_Backend.Models;
using Authenticator_Application_Backend.Response;
using Authenticator_Application_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Authenticator_Application_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthenticatorController : ControllerBase
    {
        #region DI

        private readonly AuthenticatorDbContext _context;
        private readonly IGoogleAuthenticatorService _googleAuthenticatorService;

        #endregion


        #region ctor

        public GoogleAuthenticatorController(
            AuthenticatorDbContext context,
            IGoogleAuthenticatorService googleAuthenticatorService
            )
        {
            _context = context;
            _googleAuthenticatorService = googleAuthenticatorService;
        }

        #endregion


        #region methods

            #region generating the google authenticator Key and QR code

                /// <summary>
                /// it is used to generate the google authenticator code and QR code also.
                /// </summary>
                /// <param name="email"></param>
                /// <returns></returns>
                [HttpGet("generateQrCode")]
                public async Task<APIResponseModel> GenerateSetupCodeAsync(string email)
                {
                    try
                    {
                        var result = _googleAuthenticatorService.GenerateQrSetup(email);

                        if (result != null)
                        {
                            return new APIResponseModel(StatusCodesEnum.Success, result);
                        }
                        else
                        {
                            return new APIResponseModel(StatusCodesEnum.Error, result);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new APIResponseModel(StatusCodesEnum.BadRequest, ex);
                    }

                }

            #endregion


            #region for validating 2FA Pin
                /// <summary>
                /// this method will validate the Two Factor Pin
                /// </summary>
                /// <param name="authInputModel"></param>
                /// <returns></returns>
                [HttpPost("validateTwoFactorPinAsync")]
                public async Task<APIResponseModel> ValidateTwoFactorPinAsync(AuthInputModel authInputModel)
                {
                    try
                    {
                        var result = await _googleAuthenticatorService.ValidateTwoFactorPin(authInputModel);

                        if (result)
                        {
                            return new APIResponseModel(StatusCodesEnum.TwoFactorAuthenticatorActivated, result);
                        }
                        else
                        {
                            return new APIResponseModel(StatusCodesEnum.IncorrectPin, result);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new APIResponseModel(StatusCodesEnum.BadRequest, ex);
                    }

                }

            #endregion


            #region for disabling 2FA   

                /// <summary>
                /// for disabling Two factor Authentication
                /// </summary>
                /// <param name="email"></param>
                /// <returns></returns>
                [HttpDelete("disableTfa")]
                public async Task<APIResponseModel> DisableTfa(AuthInputModel authInputModel)
                {
                    try
                    {
                        var result = await _googleAuthenticatorService.Disable2Fa(authInputModel);

                        if (result)
                        {
                            return new APIResponseModel(StatusCodesEnum.TwoFactorAuthenticatorDeactivated, result);
                        }
                        else
                        {
                            return new APIResponseModel(StatusCodesEnum.IncorrectPin, result);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new APIResponseModel(StatusCodesEnum.BadRequest, ex);
                    }

                }

            #endregion

        #endregion
    }
}
