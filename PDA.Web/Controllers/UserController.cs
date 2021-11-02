using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDA.Entities;
using PDA.Repository;
using PDA.Web.Common;
using PDA.Web.Extensions;
using PDA.Web.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _env;

        public UserController(IUserRepository userRepository,
            IWebHostEnvironment env)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _env = env;
        }

        private async Task<UserViewModel> BindData()
        {
            var _Result = await _userRepository.GetUsers();
            var viewModel = new UserViewModel
            {
                TblUsers = _Result.Select(sel => new UserViewModel
                {
                    UserID = sel.UserID,
                    FirstName = sel.FirstName,
                    LastName = sel.LastName,
                    FullName = sel.FirstName + " " + sel.LastName,
                    EmailId = sel.EmailId,
                    Gender = sel.Gender,
                    PhoneNumber = sel.PhoneNumber,
                    LoginName = sel.LoginName,
                    Password = sel.Password,
                    ConfirmPassword = sel.Password,
                    UserPhotoPath = sel.UserPhotoPath,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.FirstName).ToList()
            };
            return viewModel;
        }

        public async Task<IActionResult> Index()
        {
            var model = await BindData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string userModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(userModel);
            if (ModelState.IsValid)
            {
                var _user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailId = model.EmailId,
                    Gender = Enum.GetName(typeof(GenderEnum), Convert.ToInt32(model.Gender)),
                    PhoneNumber = model.PhoneNumber,
                    LoginName = model.LoginName,
                    Password = ComputeSha256Hash.ComputeSha256HashPassword(model.Password),
                    UserPhotoPath = model.UserPhotoPath,
                    IsActive = model.IsActive,
                    CreatedBy = HttpContext.Session.GetString("Portal.LoginName"), //"Admin",
                    CreatedAt = DateTime.Now,
                };
                var result = await _userRepository.Add(_user);
                if (_user.UserID != 0)
                {
                    model.Message = "Save";
                    model.TblUsers = BindData().Result.TblUsers;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string userModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<UserViewModel>(userModel);

            if (ModelState.IsValid)
            {
                var _user = await _userRepository.GetUserById(model.UserID);

                _user.FirstName = model.FirstName;
                _user.LastName = model.LastName;
                _user.EmailId = model.EmailId;
                _user.Gender = Enum.GetName(typeof(GenderEnum), Convert.ToInt32(model.Gender));
                _user.PhoneNumber = model.PhoneNumber;
                _user.LoginName = model.LoginName;
                //_user.Password = model.Password;
                _user.UserPhotoPath = model.UserPhotoPath;
                _user.IsActive = model.IsActive;
                _user.ModifiedBy = HttpContext.Session.GetString("Portal.LoginName"); //"Admin";
                _user.ModifiedAt = DateTime.Now;

                var result = await _userRepository.Update(_user);
                model.Message = "Update";
                model.TblUsers = BindData().Result.TblUsers;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeleteUser(int UserId)
        {
            bool result = false;
            var model = new UserViewModel();

            if (UserId != 0)
            {
                result = _userRepository.Delete(UserId);
                if (result)
                {
                    model.TblUsers = BindData().Result.TblUsers;
                }
            }
            return Json(model);
        }

        public IActionResult PhotoUpload(IFormFile MyUploader)
        {
            if (MyUploader != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "Photos");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + MyUploader.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    MyUploader.CopyTo(fileStream);
                }
                return new ObjectResult(new { status = "success", fileName = uniqueFileName });
            }
            return new ObjectResult(new { status = "fail" });
        }
    }
}
