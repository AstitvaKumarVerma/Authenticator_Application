using Authenticator_Application_Backend.Services;
using Authenticator_Application_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Authenticator_Application_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MicrosoftAuthenticatorController : ControllerBase
    {
        #region DI

        private readonly IMicrosoftAuthenticatorService _authService;

        #endregion


        #region ctor

        public MicrosoftAuthenticatorController(
            IMicrosoftAuthenticatorService authService
            )
        {
            _authService = authService;
        }

        #endregion

        /// <summary>
        /// Generates the QR code and secret key for Microsoft Authenticator setup.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>An AuthProviderModel object containing account, manual entry key, and QR code setup image URL.</returns>
        /// <response code="200">QR code and key generated successfully.</response>
        /// <response code="500">Internal server error occurred.</response>
        [HttpGet("setup/{email}")]
        public IActionResult GetSetupInfo(string email)
        {
            try
            {
                var setupInfo = _authService.GenerateQrSetup(email);
                return Ok(setupInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while generating setup information." });
            }
        }

        /// <summary>
        /// Verifies the TOTP code entered by the user.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="code">The TOTP code entered by the user.</param>
        /// <returns>True if the code is valid, false otherwise.</returns>
        /// <response code="200">Code verified successfully (true/false).</response>
        /// <response code="400">Invalid code or request.</response>
        /// <response code="500">Internal server error occurred.</response>
        [HttpPost("verify")]
        public IActionResult VerifyCode(string email, string code)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
            {
                return BadRequest(new {StatusCodes.Status400BadRequest , Message = "Email and code are required." });
            }

            try
            {
                var isVerified = _authService.VerifyTotpCode(email, code);
                return Ok(isVerified);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while verifying the code." });
            }
        }

    }
}
