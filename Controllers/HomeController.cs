using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaEfler.Models;

namespace InmobiliariaEfler.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //private RepositorioUsuario repoUsuario;


    public HomeController(ILogger<HomeController> logger)//,IConfiguration configuration
    {
        _logger = logger;
        //repoUsuario= new RepositorioUsuario(configuration);
    }

    public IActionResult Index()
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
