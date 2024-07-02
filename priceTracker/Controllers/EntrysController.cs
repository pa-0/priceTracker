using Microsoft.AspNetCore.Mvc;
using priceTracker.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace priceTracker
{
    [Authorize]
    public class EntrysController : Controller
    {
        MyDbcontext db = new MyDbcontext();

        public ActionResult Index()
        {
            return View(db.Entrys.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEntry(Entry entrys)
        {   
            if (ModelState.IsValid)
            {
                var entry = new Entry
                {
                    Status = entrys.Status
                };

                 int currentEmpId = 2;  
                entrys.EmpId = currentEmpId;

                entrys.RecordDate = DateOnly.FromDateTime(DateTime.Today);
                db.Entrys.Add(entrys);
                db.SaveChanges();
                return RedirectToAction("Create", "Products", new { entryId = entrys.Id });
            }
            return RedirectToAction("Index", "Entrys");
           
        }
    }
}
