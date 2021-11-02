using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDA.Entities;
using PDA.Repository;
using PDA.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.Controllers
{
    public class VesselTypeController : Controller
    {
        private readonly IVesselTypeRepository _vesselTypeRepository;

        public VesselTypeController(IVesselTypeRepository vesselTypeRepository)
        {
            _vesselTypeRepository = vesselTypeRepository ?? throw new ArgumentNullException(nameof(vesselTypeRepository));
        }

        private async Task<VesselTypeViewModel> BindData()
        {
            var _Result = await _vesselTypeRepository.GetVesselTypes();
            var viewModel = new VesselTypeViewModel
            {
                TblVesselType = _Result.Select(sel => new VesselTypeViewModel
                {
                    VesselTypeID = sel.VesselTypeID,
                    VesselTypeName = sel.VesselTypeName,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.VesselTypeName).ToList()
            };

            return viewModel;
        }

        public async Task<IActionResult> Index()
        {
            var model = await BindData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVesselType(string vesselTypeModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<VesselTypeViewModel>(vesselTypeModel);
            if (ModelState.IsValid)
            {
                var _vesselType = new VesselType
                {
                    VesselTypeName = model.VesselTypeName,
                    IsActive = model.IsActive,
                    CreatedBy = HttpContext.Session.GetString("Portal.LoginName"), //"Admin",
                    CreatedAt = DateTime.Now,
                };
                var result = await _vesselTypeRepository.Add(_vesselType);
                if (_vesselType.VesselTypeID != 0)
                {
                    model.Message = "Save";
                    model.TblVesselType = BindData().Result.TblVesselType;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditVesselType(string vesselTypeModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<VesselTypeViewModel>(vesselTypeModel);

            if (ModelState.IsValid)
            {
                var _vesselType = await _vesselTypeRepository.GetVesselTypeById(model.VesselTypeID);

                _vesselType.VesselTypeName = model.VesselTypeName;
                _vesselType.IsActive = model.IsActive;
                _vesselType.ModifiedBy = HttpContext.Session.GetString("Portal.LoginName"); // "Admin";
                _vesselType.ModifiedAt = DateTime.Now;

                var result = await _vesselTypeRepository.Update(_vesselType);
                model.Message = "Update";
                model.TblVesselType = BindData().Result.TblVesselType;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeleteVesselType(int VesselTypeID)
        {
            bool result = false;
            var model = new VesselTypeViewModel();

            if (VesselTypeID != 0)
            {
                result = _vesselTypeRepository.Delete(VesselTypeID);
                if (result)
                {
                    model.TblVesselType = BindData().Result.TblVesselType;
                }
            }
            return Json(model);
        }
    }
}
