using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDA.Entities;
using PDA.Repository;
using PDA.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.Controllers
{
    public class TerminalController : Controller
    {
        private readonly ITerminalRepository _terminalRepository;
        private readonly IPortRepository _portRepository;

        public TerminalController(ITerminalRepository terminalRepository, IPortRepository portRepository)
        {
            _terminalRepository = terminalRepository ?? throw new ArgumentNullException(nameof(terminalRepository));
            _portRepository = portRepository ?? throw new ArgumentNullException(nameof(portRepository));
        }

        private async Task<TerminalViewModel> BindData()
        {
            var ports = await _portRepository.GetPorts();
            ViewBag.PortDropdown = new SelectList(ports.Where(whr => whr.IsActive == true), "PortID", "PortName");

            var _Result = await _terminalRepository.GetTerminals();
            var viewModel = new TerminalViewModel
            {
                TblTerminal = _Result.Select(sel => new TerminalViewModel
                {
                    TerminalID = sel.TerminalID,
                    TerminalName = sel.TerminalName,
                    PortID = sel.PortID,
                    PortName = ports.Where(whr => whr.PortID == sel.PortID).Select(sel => sel.PortName).FirstOrDefault(),
                    IsActive = sel.IsActive,
                    CreatedBy = sel.CreatedBy,
                    CreatedAt = sel.CreatedAt,
                    ModifiedBy = sel.ModifiedBy,
                    ModifiedAt = sel.ModifiedAt
                }).OrderBy(ord => ord.TerminalName).ToList()
            };

            return viewModel;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await BindData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTerminal(string terminalModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<TerminalViewModel>(terminalModel);
            if (ModelState.IsValid)
            {
                var _terminal = new Terminal
                {
                    TerminalName = model.TerminalName,
                    PortID = model.PortID,
                    IsActive = model.IsActive,
                    CreatedBy = HttpContext.Session.GetString("Portal.LoginName"),
                    CreatedAt = DateTime.Now,
                };
                var result = await _terminalRepository.Add(_terminal);
                if (_terminal.TerminalID != 0)
                {
                    model.Message = "Save";
                    model.TblTerminal = BindData().Result.TblTerminal;
                }
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTerminal(string terminalModel)
        {
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<TerminalViewModel>(terminalModel);

            if (ModelState.IsValid)
            {
                var _terminal = await _terminalRepository.GetTerminalById(model.TerminalID);

                _terminal.TerminalName = model.TerminalName;
                _terminal.PortID = model.PortID;
                _terminal.IsActive = model.IsActive;
                _terminal.ModifiedBy = HttpContext.Session.GetString("Portal.LoginName"); // "Admin";
                _terminal.ModifiedAt = DateTime.Now;

                var result = await _terminalRepository.Update(_terminal);
                model.Message = "Update";
                model.TblTerminal = BindData().Result.TblTerminal;
            }
            return Json(model);
        }

        [HttpPut]
        public JsonResult DeleteTerminal(int terminalId)
        {
            bool result = false;
            var model = new TerminalViewModel();

            if (terminalId != 0)
            {
                result = _terminalRepository.Delete(terminalId);
                if (result)
                {
                    model.TblTerminal = BindData().Result.TblTerminal;
                }
            }
            return Json(model);
        }
    }
}
