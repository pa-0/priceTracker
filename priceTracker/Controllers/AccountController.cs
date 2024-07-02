using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using priceTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace priceTracker
{
    public class AccountController : Controller
    {
        MyDbcontext db = new MyDbcontext();
        public IActionResult Login()
        {
            return View();
        }

        public ActionResult Validate(Employe employes)
        {
            var _admin = db.Employes.Where(s => s.Mail == employes.Mail);
            if(_admin.Any()){
                if(_admin.Where(s => s.Password == employes.Password).Any()){
                    
                    return Json(new { status = true, message = "Login Successfull!"});
                }
                else
                {
                    return Json(new { status = false, message = "Invalid Password!"});
                }
            }
            else
            {
                return Json(new { status = false, message = "Invalid Email!"});
            }
        }
    }
}