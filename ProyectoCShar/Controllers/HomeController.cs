using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.Models;
using System.Diagnostics;

namespace ProyectoCShar.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult dashboard()
        {
            return View("~/Views/Home/dashboard.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Control de Errores Extras

       /* public IActionResult NotFound(int? statusCode = null)
        {
            if (statusCode.HasValue && statusCode.Value == 404)
            {
                return View("Error");
            }
            return View("Error");
        }*/
    }
}