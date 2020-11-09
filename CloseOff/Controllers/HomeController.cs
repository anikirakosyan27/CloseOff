using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CloseOff.Models;
using Microsoft.AspNetCore.Identity;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace CloseOff.Controllers
{
    public class HomeController : BaseController
    {
		public HomeController(IUnitOfWork unitOfWork,
		 UserManager<User> userManager)
		 : base(unitOfWork, userManager)
		{
			
		}
		public  IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
