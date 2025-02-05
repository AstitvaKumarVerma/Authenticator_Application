using Authenticator_Application_Backend.Models;
using Authenticator_Application_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Authenticator_Application_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatorController : ControllerBase
    {
        #region DI
        private readonly IUserRegistration _userRegistration;
        #endregion


        #region ctor
        public AuthenticatorController(IUserRegistration userRegistration)
        {
            _userRegistration = userRegistration;
        }
        #endregion


        #region methods

            #region login

                /// <summary>
                /// User Login
                /// </summary>
                /// <param name="model"></param>
                /// <returns></returns>
                [HttpPost]
                [Route("login")]
                public IActionResult LoginUserAsync(LoginUserModel model)
                {
                    var result = _userRegistration.Login(model);
                    return Ok(result);
                }

            #endregion


            #region User Registration

                /// <summary>
                /// User Registration
                /// </summary>
                /// <param name="model"></param>
                /// <returns></returns>
                [HttpPost]
                [Route("user/registration")]
                public IActionResult UserRegistration(UserRegistrationModel model)
                {
                    var result = _userRegistration.RegisterUser(model);
                    return Ok(result);
                }

            #endregion


            #region Send Otp To Emails Method

                /// <summary>
                /// Send Otp To Emails Method
                /// </summary>
                /// <param name="Email"></param>
                /// <returns></returns>
                [HttpPost]
                [Route("send-otp-to-email")]
                public IActionResult SendOtpToEmail(OtpToEmailModel email)
                {
                    var result = _userRegistration.SendOtpToEmail(email);
                    return Ok(result);
                }

        #endregion


            #region Otp Verification

                /// <summary>
                /// for verifing the otp
                /// </summary>
                /// <param name="model"></param>
                /// <returns></returns>
                [HttpPost]
                [Route("otpVerification")]
                public IActionResult VerifyOtp(OtpModel model)
                {
                    var result = _userRegistration.VerifyOtp(model);
                    return Ok(result);
                }

        #endregion


            #region Disable Authentication
                /// <summary>
                /// Disable Authentication
                /// </summary>
                /// <param name="email"></param>
                /// <returns></returns>
                [HttpPost]
                [Route("disable-authentication")]
                public IActionResult DisableAuthentication(OtpToEmailModel email)
                {
                    var result = _userRegistration.DisableAuthentication(email);
                    return Ok(result);
                }
            #endregion

        #endregion
    }
}
