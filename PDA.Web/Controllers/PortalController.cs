using DNTCaptcha.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDA.Repository;
using PDA.Web.Extensions;
using PDA.Web.ViewModel;
using System;

namespace PDA.Web.Controllers
{
    public class PortalController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly IUserRepository _userRepository;

        public PortalController(IHttpContextAccessor httpContextAccessor,
                                IDNTCaptchaValidatorService validatorService,
                                IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _validatorService = validatorService;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var token = RandomUniqueToken.Value();
            LoginViewModel loginView = new LoginViewModel()
            {
                Hdrandomtoken = token
            };

            HttpContext.Session.SetString("Hdrandomtoken", token);
            return View(loginView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            var token = RandomUniqueToken.Value();

            if (ModelState.IsValid)
            {
                if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    TempData["LoginErrorMessage"] = "Please enter valid security code";
                    return RedirectToAction("Login");
                }


                if (!_userRepository.CheckUsernameExists(loginViewModel.Username))
                {
                    TempData["LoginErrorMessage"] = "Entered Username or Password is Invalid";
                    return RedirectToAction("Login");
                }
                else
                {
                    var loggedInuserdetails = _userRepository.GetUserDetailsByLoginName(loginViewModel.Username);

                    var userviewModel = new UserViewModel
                    {
                        UserID = loggedInuserdetails.UserID,
                        FirstName = loggedInuserdetails.FirstName,
                        LastName = loggedInuserdetails.LastName,
                        EmailId = loggedInuserdetails.EmailId,
                        PhoneNumber = loggedInuserdetails.PhoneNumber,
                        LoginName = loggedInuserdetails.LoginName,
                        Password = loggedInuserdetails.Password,
                        IsActive = loggedInuserdetails.IsActive,
                        RoleId = (int)RolesHelper.Roles.SystemUser,
                        RoleName = RolesHelper.Roles.SystemUser.ToString(),
                        UserPhotoPath = loggedInuserdetails.UserPhotoPath == null ? "~/img/user-no.jpg" :
                        "~/Photos/" + loggedInuserdetails.UserPhotoPath,
                    };


                    if (loggedInuserdetails == null)
                    {
                        TempData["LoginErrorMessage"] = "Entered Username or Password is Invalid";
                        return RedirectToAction("Login");
                    }

                    if (loggedInuserdetails.IsActive == false)
                    {
                        TempData["LoginErrorMessage"] = "Your Account is InActive Contact Administrator";
                        return RedirectToAction("Login");
                    }

                    var hiddentoken = HttpContext.Session.GetString("Hdrandomtoken");

                    if (ConcateTokenandPassword(loggedInuserdetails.Password, hiddentoken) == loginViewModel.Password)
                    {
                        SetAuthenticationCookie();
                        SetApplicationSession(userviewModel);

                        if (userviewModel.IsActive == true)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        TempData["LoginErrorMessage"] = "Entered Username or Password is Invalid";
                        return RedirectToAction("Login");
                    }
                }
            }

            loginViewModel.Hdrandomtoken = token;
            HttpContext.Session.SetString("Hdrandomtoken", token);
            return View(loginViewModel);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            try
            {
                CookieOptions option = new CookieOptions();
                if (Request.Cookies[AllSessionKeys.AuthenticationToken] != null)
                {
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Append(AllSessionKeys.AuthenticationToken, "", option);
                }

                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Portal");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        private string ConcateTokenandPassword(string storedDatabasePassword, string hiddenrandomtoken)
        {
            try
            {
                return ComputeSha256Hash.ComputeSha256HashPassword(hiddenrandomtoken + storedDatabasePassword);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetAuthenticationCookie()
        {
            string strAuthToken = Guid.NewGuid().ToString();
            HttpContext.Session.SetString(AllSessionKeys.AuthenticationToken, strAuthToken);
            Response.Cookies.Append(AllSessionKeys.AuthenticationToken, strAuthToken);
        }

        private void SetApplicationSession(UserViewModel commonUser)
        {
            HttpContext.Session.SetInt32(AllSessionKeys.RoleId, commonUser.RoleId);
            HttpContext.Session.SetInt32(AllSessionKeys.UserId, Convert.ToInt32(commonUser.UserID));
            HttpContext.Session.SetString(AllSessionKeys.UserName, Convert.ToString(commonUser.FirstName + " " + commonUser.LastName));
            HttpContext.Session.SetString(AllSessionKeys.RoleIdString, Convert.ToString(commonUser.RoleId));
            HttpContext.Session.SetString(AllSessionKeys.RoleName, Convert.ToString(commonUser.RoleName));
            if (commonUser.FirstName != null)
                HttpContext.Session.SetString(AllSessionKeys.FirstName, Convert.ToString(commonUser.FirstName));
            if (commonUser.LastName != null)
                HttpContext.Session.SetString(AllSessionKeys.LastName, Convert.ToString(commonUser.LastName));
            HttpContext.Session.SetString(AllSessionKeys.FullName, Convert.ToString(commonUser.FirstName + " " + commonUser.LastName));
            if (commonUser.LoginName != null)
                HttpContext.Session.SetString(AllSessionKeys.LoginName, Convert.ToString(commonUser.LoginName));
            HttpContext.Session.SetString(AllSessionKeys.EmailId, Convert.ToString(commonUser.EmailId));
            HttpContext.Session.SetString(AllSessionKeys.MobileNo, Convert.ToString(commonUser.PhoneNumber));
            HttpContext.Session.SetString(AllSessionKeys.UserPhoto, Convert.ToString(commonUser.UserPhotoPath));
        }
    }
}
