using System.Diagnostics;
using CreatorSuite.Models;
using Microsoft.AspNetCore.Mvc;

namespace CreatorSuite.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            string id = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View(new ErrorModel(id));
        }
    }
}