using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifornia.ViewComponents
{
    [ViewComponent]
    public class MontlySpecialsViewComponent : ViewComponent
    {
        private readonly BlogDataContext db;

        public MontlySpecialsViewComponent(BlogDataContext db)
        {
            this.db = db;
        }
        public IViewComponentResult Invoke()
        {
            var specials = db.MonthlySpecials.ToArray();
            return View(specials);
        }
    }
}
