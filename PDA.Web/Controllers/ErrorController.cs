using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDA.Web.Extensions;
using System;
using System.Net;

namespace PDA.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{StatusCode}")]
        public IActionResult Error(int statusCode)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { responseText = "Ajax Error" });
            }

            if (statusCode == 404)
            {
                ViewBag.ErrorId = "404";
            }

            if (statusCode == 500)
            {
                ViewBag.ErrorId = "500";
            }

            CookieOptions option = new CookieOptions();

            if (Request.Cookies[AllSessionKeys.AuthenticationToken] != null)
            {
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(AllSessionKeys.AuthenticationToken, "", option);
            }

            HttpContext.Session.Remove(AllSessionKeys.RoleId);
            HttpContext.Session.Clear();

            return View();
        }

        public IActionResult SessionOut()
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { responseText = "Ajax Error" });
            }
            ViewBag.ErrorId = "440";

            CookieOptions option = new CookieOptions();

            if (Request.Cookies[AllSessionKeys.AuthenticationToken] != null)
            {
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(AllSessionKeys.AuthenticationToken, "", option);
            }

            HttpContext.Session.Remove(AllSessionKeys.RoleId);
            HttpContext.Session.Clear();

            return View();
        }
    }
}
