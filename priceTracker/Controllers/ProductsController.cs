using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using priceTracker.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;


namespace priceTracker
{
    //[Authorize]
    public class ProductsController : Controller
    {
        private MyDbcontext db = new MyDbcontext();

        /*private readonly MyDbcontext _db;

        public ProductsController(MyDbcontext db)
        {
            _db = db;
        }*/

        
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Create(int? entryId)
        {
            if (entryId == null)
            {
                return RedirectToAction("Index", "Entrys");
            }

            var entry = db.Entrys.FirstOrDefault(e => e.Id == entryId);
            if (entry == null)
            {
                return RedirectToAction("Index", "Entrys");
            }

            var products = new List<Product>();
            for (int i = 1; i <= 5; i++)
            {
                string urlProperty = $"Url{i}";
                string urlValue = entry.GetType().GetProperty(urlProperty)?.GetValue(entry, null)?.ToString() ?? "";

                if (!string.IsNullOrEmpty(urlValue))
                {
                    program2.ProcessUrl(urlValue, out string productName, out string siteName, out float price);

                    if (!string.IsNullOrEmpty(productName))
                    {
                        var product = new Product
                        {
                            ProductName = productName,
                            SiteName = siteName,
                            Price = price,
                            Date = entry.StartDate,
                            Url = urlValue,
                            UrlNumber = i,
                            ProdId = entry.Id 
                        };

                        products.Add(product);
                    }
                }
            }

            foreach (var product in products)
            {
                db.Products.Add(product);
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public ActionResult CreateProduct(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /*public ActionResult Edit(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }*/


        /*[HttpPost]
        public ActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }*/


        public ActionResult SelectProduct(int prodId)
        {
            var products = db.Products.Where(p => p.ProdId == prodId).ToList();
            return View(products);  
        }

        /*public IActionResult Update(Product product)
        {
            var old_product = db.Products.FirstOrDefault(e => e.Id == product.Id);
            //product.ProdId = old_product.ProdId;
            product.Date = old_product.Date;
            db.Entry(old_product).CurrentValues.SetValues(product);
            db.SaveChanges();
            return RedirectToAction("DisplayProducts", "Display");
        }*/

        public IActionResult Edit(int Id)
        {
            var product1 = db.Products.FirstOrDefault(e => e.Id == Id);
            return View(product1);
        }

        

        public IActionResult Update(Product product)
        {
            if (product == null)
            {
                return NotFound();  
            }

            var old_product = db.Products.AsNoTracking().FirstOrDefault(e => e.Id == product.Id);
            if (old_product == null)
            {
                return NotFound();  
            }

            if(product.Url != null){
            program2.ProcessUrl(product.Url, out string productName, out string siteName, out float price);
              if(product.ProductName == null){
            product.ProductName = productName;}
              if(product.SiteName == null){
            product.SiteName = siteName;}
              if(product.Price == null){
            product.Price = price;}
            }

            product.UrlNumber =old_product.UrlNumber;
            product.ProdId = old_product.ProdId;
            product.Date = old_product.Date;

            var trackedProduct = db.Products.FirstOrDefault(p => p.Id == product.Id);
            if (trackedProduct != null)
            {
                db.Entry(trackedProduct).CurrentValues.SetValues(product);
                db.SaveChanges();
                return RedirectToAction("DisplayProducts", "Display", new { prodId = old_product.ProdId });
            }
            else
            {
                return NotFound();  
            }
        }

        [HttpPost]
        public ActionResult CreateProductFromEntry(int entryId)
        {
            bool entryUsed = db.Products.Any(p => p.ProdId == entryId);
            if (entryUsed)
            {
                return BadRequest("Products for this entry have already been created.");
            }

            var entry = db.Entrys.FirstOrDefault(e => e.Id == entryId);
            if (entry == null)
            {
                return NotFound("Entry not found.");
            }

            var products = new List<Product>();
            for (int i = 1; i <= 5; i++)
            {
                string urlProperty = $"Url{i}";
                string urlValue = entry.GetType().GetProperty(urlProperty)?.GetValue(entry, null)?.ToString() ?? "";

                if (!string.IsNullOrEmpty(urlValue))
                {
                    program2.ProcessUrl(urlValue, out string productName, out string siteName, out float price);

                    if (!string.IsNullOrEmpty(productName))
                    {
                        var product = new Product
                        {
                            ProductName = productName,
                            SiteName = siteName,
                            Price = price,
                            Date = entry.StartDate,
                            Url = urlValue,
                            UrlNumber = i,
                            ProdId = entry.Id 
                        };

                        products.Add(product);
                    }
                }
            }

            foreach (var product in products)
            {
                db.Products.Add(product);
            }

            db.SaveChanges();
            return Ok("Products created successfully.");
        }

    }
}







