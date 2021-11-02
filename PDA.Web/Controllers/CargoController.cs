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
    public class CargoController : Controller
    {
        private readonly ICargoRepository _cargoRepository;

        public CargoController(ICargoRepository cargoRepository)
        {
            _cargoRepository = cargoRepository ?? throw new ArgumentNullException(nameof(cargoRepository));
        }

        private async Task<CargoViewModel> BindData()
        {
            var _Result = await _cargoRepository.GetCargos();
            var viewModel = new CargoViewModel
            {
                TblCargo = _Result.Select(sel => new CargoViewModel
                {
                    CargoID = sel.CargoID,
                    CargoName = sel.CargoName,
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.CargoName).ToList()
            };

            return viewModel;
        }

        public async Task<IActionResult> Index()
        {
            var model = await BindData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCargo(string cargoModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<CargoViewModel>(cargoModel);
            if (ModelState.IsValid)
            {
                var _cargo = new Cargo
                {
                    CargoName = model.CargoName,
                    IsActive = model.IsActive,
                    CreatedBy = HttpContext.Session.GetString("Portal.LoginName"), //"Admin",
                    CreatedAt = DateTime.Now,
                };
                var result = await _cargoRepository.Add(_cargo);
                if (_cargo.CargoID != 0)
                {
                    model.Message = "Save";
                    model.TblCargo = BindData().Result.TblCargo;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCargo(string cargoModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<CargoViewModel>(cargoModel);
            if (ModelState.IsValid)
            {
                var _cargo = await _cargoRepository.GetCargoById(model.CargoID);

                _cargo.CargoName = model.CargoName;
                _cargo.IsActive = model.IsActive;
                _cargo.ModifiedBy = HttpContext.Session.GetString("Portal.LoginName"); // "Admin";
                _cargo.ModifiedAt = DateTime.Now;

                var result = await _cargoRepository.Update(_cargo);
                model.Message = "Update";
                model.TblCargo = BindData().Result.TblCargo;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeletePort(int CargoId)
        {
            bool result = false;
            var model = new CargoViewModel();

            if (CargoId != 0)
            {
                result = _cargoRepository.Delete(CargoId);
                if (result)
                {
                    model.TblCargo = BindData().Result.TblCargo;
                }
            }
            return Json(model);
        }

    }
}
