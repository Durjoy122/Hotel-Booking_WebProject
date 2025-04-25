using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Domain.Entities;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Repository;


namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaNumber = new VillaNumber(),  // Initialize the VillaNumber object to avoid null reference
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                }).ToList()
            };
            return View(villaNumberVM);
        }


        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (roomNumberExists)
            {
                TempData["error"] = "Villa Number already exists";
                obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                });
                return View(obj);
            }

            _unitOfWork.VillaNumber.Add(obj.VillaNumber);
            _unitOfWork.Save();
            TempData["success"] = "Villa Number has been created successfully";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int villaId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaId)
            };
            if(villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                
                villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                });
                return View(villaNumberVM);
            }
            _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
            _unitOfWork.Save();
            TempData["success"] = "Villa Number has been Updated successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int villaId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaId)
            };
            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDB = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if(objFromDB is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDB);
                _unitOfWork.Save();
                TempData["success"] = "Villa Number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Villa Number could not be deleted";
            return View();
        }
    }
}