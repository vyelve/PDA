using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDA.Repository;
using PDA.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.Controllers
{
    public class PilotageController : Controller
    {
        private readonly IPilotageRepository _pilotageRepository;
        private readonly IPortRepository _portRepository;


        public PilotageController(IPilotageRepository pilotageRepository,
                                  IPortRepository portRepository)
        {
            _pilotageRepository = pilotageRepository ?? throw new ArgumentNullException(nameof(pilotageRepository));
            _portRepository = portRepository ?? throw new ArgumentNullException(nameof(portRepository));
        }

        private async Task<PilotageViewModel> BindData()
        {
            var ports = await _portRepository.GetPorts();
            ViewBag.PortDropdown = new SelectList(ports.Where(whr => whr.IsActive == true), "PortID", "PortName");

            var _result = await _pilotageRepository.GetPilotages();
            var viewmodel = new PilotageViewModel
            {
                TblPilotage = _result.Select(sel => new PilotageViewModel
                {
                    PilotageID = sel.PilotageID,
                    PortId = sel.PortId,
                    PilotType = sel.PilotType,
                    Fixed_Tariff = sel.Fixed_Tariff,
                    Tariff = sel.Tariff,
                    Range1 = sel.Range1,
                    Range2 = sel.Range2,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.PilotageID)
            };
            return viewmodel;
        }

        public async Task<IActionResult> Index()
        {
            var model = await BindData();
            return View(model);
        }
    }
}
