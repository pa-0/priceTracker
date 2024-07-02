using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using priceTracker.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace priceTracker
{
    [Authorize]
    public class DisplayController : Controller
    {
        private MyDbcontext db = new MyDbcontext();

        public ActionResult SelectProdId()
        {
            var prodIds = db.Products.Select(p => p.ProdId).Distinct().OrderBy(p => p).ToList();
            var selectList = new SelectList(prodIds.Select(x => new SelectListItem 
            {
                Text = x.ToString(),  
                Value = x.ToString()
            }), "Value", "Text");

            ViewBag.ProdIds = selectList;
            return View();
        }

        public ActionResult DisplayProducts(int prodId)
        {
            var products = db.Products.Where(p => p.ProdId == prodId).ToList();
            return View(products);
        }

        /*public ActionResult EditProduct(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();  
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DisplayProducts");  
            }
            return View(product);
        }*/

        [HttpPost]
        public IActionResult EditProduct([FromBody] Product product)
        {
        if (ModelState.IsValid)
        {
        var existingProduct = db.Products.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct != null)
        {
            existingProduct.ProductName = product.ProductName;
            existingProduct.Price = product.Price;
            existingProduct.Url = product.Url;

            db.SaveChanges();

            return Json(new { success = true, message = "Product updated successfully." });
        }
        else
        {
            return Json(new { success = false, message = "Product not found." });
        }
        }
        return Json(new { success = false, message = "Validation errors.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }



    }
}
