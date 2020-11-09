using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CloseOff.Controllers
{
    public class UserController : BaseController
	{
		public UserController(IUnitOfWork unitOfWork,
		 UserManager<User> userManager)
		 : base(unitOfWork, userManager)
		{
			
		}

		#region Patients
		public async Task<ActionResult> AllPatients()
		{
			 List<PatientModel> list = await _unitOfWork.UserRepository.GetAllPatient();
		     return View(list);
		}
		
		[HttpGet]
		public IActionResult AddPatient()
		{
			PatientModel model = new PatientModel();
			return PartialView("AddPatient", model);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			PatientModel model = await _unitOfWork.UserRepository.GetPatientById(id);
			return PartialView("EditPatient", model);
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(PatientModel model)
		{
			if (ModelState.IsValid)
			{
				await _unitOfWork.UserRepository.EditPatient(model);
				return RedirectToAction("AllPatients");
			}
			return PartialView("EditPatient", model);
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddPatient(PatientModel model)
		{
			if (ModelState.IsValid)
			{
				await _unitOfWork.UserRepository.AddPatient(model);
				return RedirectToAction("AllPatients");
			}
			return PartialView("AddPatient", model);
		}
		public async Task<IActionResult> PatientDetails(int Id)
		{
			var patient = await _unitOfWork.UserRepository.GetPatientById(Id);
			return View("Details", patient);

		}
		#endregion

		#region Isolated
		public async Task<IActionResult> AllIsolated()
        {
			var users = await _unitOfWork.UserRepository.GetAllPatient();
			return View(users);
		}
		#endregion


	}
}
