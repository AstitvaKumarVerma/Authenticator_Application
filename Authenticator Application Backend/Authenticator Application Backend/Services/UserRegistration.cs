using Authenticator_Application_Backend.Messages;
using Authenticator_Application_Backend.Model;
using Authenticator_Application_Backend.Models;
using Authenticator_Application_Backend.Response;
using Authenticator_Application_Backend.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Authenticator_Application_Backend.Services
{
    public class UserRegistration : IUserRegistration
    {
        #region DI
            private readonly AuthenticatorDbContext _context;
            private readonly IConfiguration _config;
        #endregion

        #region ctor
        public UserRegistration(
                AuthenticatorDbContext context, 
                IConfiguration config
                )
            {
                _context = context;
                _config = config;
            }
        #endregion

        #region methods

            #region GenerateOtp
                public string GenerateOtp()
                {
                    Random random = new Random();
                    int otp = random.Next(100000, 999999);
                    return otp.ToString();
                }
            #endregion


            #region User Registration Method
                public UserRegistrationResponseModel RegisterUser(UserRegistrationModel model)
                {
                    UserRegistrationResponseModel response = new UserRegistrationResponseModel();
                    try
                    {
                        var userData = _context.Users.FirstOrDefault(x => x.Email == model.Email);
                        if (userData == null)
                        {
                            User user = new User
                            {
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Email = model.Email,
                                Password = model.Password,
                                Otp = null,
                                OtpCreatedAt = null,
                                IsActive = true,
                                //Google_2FA_Enabled = false,
                                //Microsoft_2FA_Enabled = false,
                                OtpVerification_Done = false,
                                IsAuthenticated = false,
                                CreatedOn = DateTime.UtcNow,
                                IsDeleted = false,
                                DeletedOn = null,   
                            };

                            _context.Users.Add(user);
                            _context.SaveChangesAsync();
                            response.Message = CommonSuccessMessages.UserRegisteredSuccesfully;
                            response.Status = StatusCodes.Status200OK;
                        }
                        else
                        {
                            response.Message = CommonErrorMessages.UserAlreadyExists;
                            response.Status = StatusCodes.Status400BadRequest;
                        }
                        return response;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Status = StatusCodes.Status500InternalServerError;
                        return response;
                    }

                }
            #endregion


            #region Login User Method

                /// <summary>
                /// Login Method: User will be verified here
                /// </summary>
                /// <param name="model"></param>
                /// <returns></returns>
                public UserRegistrationResponseModel Login(LoginUserModel model)
                {
                    UserRegistrationResponseModel response = new UserRegistrationResponseModel();
                    try
                    {
                        var data = _context.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

                        if (data != null)
                        {
                            response.Status = StatusCodes.Status200OK;
                            response.Message = CommonSuccessMessages.LoginSuccess;
                        }
                        else
                        {
                            response.Status = StatusCodes.Status404NotFound;
                            response.Message = CommonErrorMessages.LoginFailed;
                        }

                        return response;
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.Message;
                        response.Status = StatusCodes.Status500InternalServerError;
                        return response;
                    }
                }
            #endregion


            #region Send Otp To Emails Method

                /// <summary>
                /// For Sending Otp on Email
                /// </summary>
                /// <param name="model"></param>
                /// <returns></returns>
                public EmailResponseModel SendOtpToEmail(OtpToEmailModel model)
                {
                    EmailResponseModel emailResponse = new EmailResponseModel();

                    try
                    {
                        if (!string.IsNullOrEmpty(model.UserEmail))
                        {
                            var smtpServer = _config["EmailConfiguration:SmtpServer"];
                            var smtpPort = int.Parse(_config["EmailConfiguration:Port"]);
                            var smtpUsername = _config["EmailConfiguration:Username"];
                            var smtpPassword = _config["EmailConfiguration:Password"];

                            using (var client = new SmtpClient(smtpServer, smtpPort))
                            {
                                client.UseDefaultCredentials = false;
                                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                                client.EnableSsl = true;

                                string Otp = GenerateOtp();
                                var mail = new MailMessage(smtpUsername, model.UserEmail)
                                {
                                    Subject = "OTP Verification",
                                    Body = $"Your OTP is: {Otp}",
                                    IsBodyHtml = false
                                };

                                client.Send(mail);

                                var userData = _context.Users.FirstOrDefault(x => x.Email == model.UserEmail);
                                if (userData != null)
                                {
                                    userData.Otp = Otp;
                                    userData.OtpCreatedAt = DateTime.UtcNow;
                                    
                                    _context.Users.Update(userData);
                                    _context.SaveChanges();
                                }

                                emailResponse.Message = CommonSuccessMessages.OptSent;
                                emailResponse.Status = StatusCodes.Status200OK;
                            }
                        }
                        else
                        {
                            emailResponse.Message = CommonErrorMessages.EmailNotFound;
                            emailResponse.Status = StatusCodes.Status404NotFound;
                        }

                    }
                    catch (Exception ex)
                    {
                        emailResponse.Message = ex.Message;
                        emailResponse.Status = StatusCodes.Status500InternalServerError;
                    }

                    return emailResponse;
                }
            #endregion


            #region Verify Otp Method

                /// <summary>
                /// For verifying the Otp
                /// </summary>
                /// <param name="model"></param>
                /// <returns></returns>
                public UserRegistrationResponseModel VerifyOtp(OtpModel model)
                {
                    UserRegistrationResponseModel response = new UserRegistrationResponseModel();
                    try
                    {
                        var data = _context.Users.FirstOrDefault(x => x.Email == model.UserEmail);

                        if (data != null)
                        {
                            bool IsVerified = data.Otp == model.Otp ? true : false;

                            if (IsVerified)
                            {
                                data.OtpVerification_Done = true;
                                data.IsAuthenticated = true;
                                _context.SaveChanges();

                                response.Status = StatusCodes.Status200OK;
                                response.Message = CommonSuccessMessages.UserVerified;
                            }
                            else
                            {
                                response.Status = StatusCodes.Status400BadRequest;
                                response.Message = CommonErrorMessages.OtpNotMatched;
                            }
                        }
                        return response;
                    }
                    catch (Exception ex)
                    {
                        response.Status = StatusCodes.Status500InternalServerError; ;
                        response.Message = ex.Message;
                        return response;
                    }
                }
        #endregion


            #region Disable Authentication
            
                /// <summary>
                /// For Disable Authentication
                /// </summary>
                /// <param name="model"></param>
                /// <returns></returns>
                public BaseResponse DisableAuthentication(OtpToEmailModel model)
                {
                    BaseResponse res = new BaseResponse();

                    var data = _context.Users.FirstOrDefault(x => x.Email == model.UserEmail);

                    if (data != null)
                    {
                        data.Otp = null; 
                        data.OtpVerification_Done =false;
                        data.IsAuthenticated = false;
                        _context.Update(data);
                        _context.SaveChanges();

                        res.Status = StatusCodes.Status200OK;
                        res.Message = CommonSuccessMessages.UserAuthenticationDisableSuccessfully;
                        return res;
                    }

                    res.Status = StatusCodes.Status404NotFound;
                    res.Message = CommonErrorMessages.UserDoesNotExists;
                    return res;

                }

            #endregion


        #endregion
    }
}
