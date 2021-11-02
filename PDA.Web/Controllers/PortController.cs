using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDA.Entities;
using PDA.Repository;
using PDA.Web.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.Controllers
{
    public class PortController : Controller
    {
        private readonly IPortRepository _portRepository;

        public PortController(IPortRepository portRepository)
        {
            _portRepository = portRepository ?? throw new ArgumentNullException(nameof(portRepository));
        }

        private async Task<PortViewModel> BindData()
        {
            var _Result = await _portRepository.GetPorts();
            var viewModel = new PortViewModel
            {
                TblPort = _Result.Select(sel => new PortViewModel
                {
                    PortID = sel.PortID,
                    PortName = sel.PortName,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.PortName).ToList()
            };
            return viewModel;
        }

        public async Task<IActionResult> Index()
        {
            var model = await BindData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePort(string portModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<PortViewModel>(portModel);
            if (ModelState.IsValid)
            {
                var _port = new Port
                {
                    PortName = model.PortName,
                    IsActive = model.IsActive,
                    CreatedBy = HttpContext.Session.GetString("Portal.LoginName"), //"Admin",
                    CreatedAt = DateTime.Now,
                };
                var result = await _portRepository.Add(_port);
                if (_port.PortID != 0)
                {
                    model.Message = "Save";
                    model.TblPort = BindData().Result.TblPort;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPort(string portModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<PortViewModel>(portModel);
            if (ModelState.IsValid)
            {
                var _port = await _portRepository.GetPortById(model.PortID);

                _port.PortName = model.PortName;
                _port.IsActive = model.IsActive;
                _port.ModifiedBy = HttpContext.Session.GetString("Portal.LoginName"); // "Admin";
                _port.ModifiedAt = DateTime.Now;

                var result = await _portRepository.Update(_port);
                model.Message = "Update";
                model.TblPort = BindData().Result.TblPort;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeletePort(int PortId)
        {
            bool result = false;
            var model = new PortViewModel();

            if (PortId != 0)
            {
                result = _portRepository.Delete(PortId);
                if (result)
                {
                    model.TblPort = BindData().Result.TblPort;
                }
            }
            return Json(model);
        }
    }
}
