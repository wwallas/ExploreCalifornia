using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifornia.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return new ContentResult { Content = "Hello, ASP.NET Core MVC!" };
            //return "Hello, asp.net core MVC";
            return View();
        }
    }
}
