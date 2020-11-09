using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CloseOff.Controllers
{
	public class BaseController : Controller
	{
		protected readonly IUnitOfWork _unitOfWork;
		protected readonly UserManager<User> _userManager;
		public BaseController(IUnitOfWork unitOfWork, UserManager<User> usermanager)
		{
			_userManager = usermanager;
			_unitOfWork = unitOfWork;
		}

		protected override void Dispose(bool disposing)
		{
			_unitOfWork.Dispose();
			base.Dispose(disposing);
		}
		protected int CurrentUserId => Int32.Parse(_userManager.GetUserId(HttpContext.User));
	}
}
