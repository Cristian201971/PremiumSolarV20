//using PanelSolar.Models;
//using PanelSolar.Repositories;
using Microsoft.AspNetCore.Mvc;
using PanelSolar.Models;
using System.Diagnostics;

namespace PanelSolar.Controllers
{
    public class HomeController : Controller
    {
        //private RepositoryJSON repo;

        //public HomeController(RepositoryJSON repo)
        //{
        //    this.repo = repo;
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Index()
        //{
        //    List<Producto> producto = this.repo.GetProductos();
        //    return View(producto);
        //}


        private readonly ILogger _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //_logger.LogInformation("Se inicializa Home");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
