using Microsoft.AspNetCore.Mvc;
using priceTracker.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace priceTracker
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RunController : Controller
    {
        private MyDbcontext db = new MyDbcontext();

        public ActionResult Update()
        {
            var highestIdProducts = db.Products
                .GroupBy(p => p.ProductName)
                .Select(g => g.OrderByDescending(p => p.Id).FirstOrDefault())
                .AsNoTracking()
                .ToList();

            var updatedProducts = new List<Product>();

            foreach (var oldProduct in highestIdProducts)
            {
                program2.ProcessUrl(oldProduct.Url, out string productName, out string siteName, out float priceNow);

                var newProduct = new Product
                {
                    ProductName = oldProduct.ProductName,
                    SiteName = oldProduct.SiteName,
                    Price = priceNow,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    UrlNumber = oldProduct.UrlNumber,
                    ProdId = oldProduct.ProdId,
                    Url = oldProduct.Url
                };

                updatedProducts.Add(newProduct);
            }

            db.Products.AddRange(updatedProducts);
            db.SaveChanges();

            CreateExcel();

            return RedirectToAction("Index", "Products");
        }


private void CreateExcel()
{
    string connectionString = "Server=.\\;User Id=sa;Password=momo;Database=priceTracker;TrustServerCertificate=True;";
    string query = "SELECT prodId, productName, price, date, url, id FROM dbo.Products ORDER BY productName ASC, id DESC";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                using (DataTable data = new DataTable())
                {
                    adapter.Fill(data);

                    var filteredData = data.AsEnumerable()
                        .GroupBy(row => row["prodId"])
                        .Select(group => new 
                        {
                            ProdId = group.Key.ToString(),
                            Products = group
                                .GroupBy(row => row["productName"])
                                .SelectMany(productGroup => productGroup.OrderByDescending(row => Convert.ToInt32(row["id"])).Take(2))
                                .ToList()
                        })
                        .OrderBy(g => g.ProdId);

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Products");
                        ws.ShowGridLines = false; 
                        int currentRow = 5; 

                        foreach (var group in filteredData)
                        {
                            var prodIdCell = ws.Range(currentRow, 1, currentRow + 4, 1).Merge();
                            prodIdCell.Value = group.ProdId;
                            prodIdCell.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                            prodIdCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            prodIdCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin; 
                            prodIdCell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            int columnOffset = 1;
                            var productNames = group.Products.GroupBy(p => p["productName"]).ToList();

                            foreach (var productGroup in productNames)
                            {
                                var productName = productGroup.Key.ToString();
                                var products = productGroup.OrderBy(p => Convert.ToInt32(p["id"])).ToList();

                                var productNameCell = ws.Range(currentRow, columnOffset + 1, currentRow + 1, columnOffset + products.Count * 2).Merge();
                                productNameCell.Value = productName;
                                productNameCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                productNameCell.Style.Alignment.WrapText = true;
                                productNameCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                productNameCell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                for (int i = 0; i < products.Count; i++)
                                {
                                    var row = products[i];

                                    var dateCell = ws.Range(currentRow + 2, columnOffset + 1 + i * 2, currentRow + 2, columnOffset + 2 + i * 2).Merge();
                                    dateCell.Value = Convert.ToDateTime(row["date"]).ToShortDateString();
                                    dateCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                    dateCell.Style.Alignment.WrapText = true;
                                    dateCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin; 
                                    dateCell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                    var priceCell = ws.Range(currentRow + 3, columnOffset + 1 + i * 2, currentRow + 3, columnOffset + 2 + i * 2).Merge();
                                    priceCell.Value = Convert.ToDecimal(row["price"]);
                                    priceCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                    priceCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin; 
                                    priceCell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                                }

                                var urlRange = ws.Range(currentRow + 4, columnOffset + 1, currentRow + 4, columnOffset + products.Count * 2).Merge();
                                var urlCell = urlRange.FirstCell();
                                urlCell.Value = "[link]";
                                urlCell.SetHyperlink(new XLHyperlink(products.First()["url"].ToString()));
                                urlCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                urlCell.Style.Alignment.WrapText = true;
                                urlCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin; 
                                urlCell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                columnOffset += products.Count * 2;
                            }

                            var currentRange = ws.Range(currentRow, 1, currentRow + 4, columnOffset - 1);
                            currentRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            currentRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            currentRow += 5; 
                        }

                        string folderPath = "C:\\Users\\user\\Downloads";
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string fileName = $"ExcelExport_{timestamp}.xlsx";
                        string fullPath = Path.Combine(folderPath, fileName);
                        wb.SaveAs(fullPath);
                    }
                }
            }
        }
    }
}



























        /*private void CreateExcel()
        {
            string connectionString = "Server=.\\;User Id=sa;Password=momo;Database=priceTracker;TrustServerCertificate=True;";
            string query = "SELECT prodId, productName, price, date, url FROM dbo.Products ORDER BY prodId ASC, date ASC, productName ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        using (DataTable data = new DataTable())
                        {
                            adapter.Fill(data);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("Products");
                                int currentRow = 1, currentColumn = 2; 
                                string lastProdId = null;
                                DateTime? lastDate = null;

                                foreach (DataRow row in data.Rows)
                                {
                                    string prodId = row["prodId"].ToString();
                                    string productName = row["productName"].ToString();
                                    decimal price = Convert.ToDecimal(row["price"]);
                                    DateTime date = Convert.ToDateTime(row["date"]);
                                    string url = row["url"].ToString();
                                    string details = $"{productName}\n{price}\n{date.ToShortDateString()}\n{url}";

                                    if (lastProdId != prodId || lastDate != date)
                                    {   
                                        currentRow += 5;
                                        currentColumn = 1; 
                                        ws.Cell(currentRow, currentColumn).Value = prodId;
                                        currentColumn = 2; 
                                        lastProdId = prodId;
                                        lastDate = date;
                                    }

                                    ws.Cell(currentRow, currentColumn).Value = productName;
                                    ws.Cell(currentRow, currentColumn).Style.Alignment.WrapText = true;
                                    ws.Cell(currentRow+1,currentColumn).Value = price;
                                    ws.Cell(currentRow+2,currentColumn).Value = date;
                                    ws.Cell(currentRow+3,currentColumn).Value = url;
                                    ws.Cell(currentRow+3, currentColumn).Style.Alignment.WrapText = true;
                                    currentColumn++; 
                                }

                                string folderPath = "C:\\Users\\user\\Downloads";
                                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                string fileName = $"ExcelExport_{timestamp}.xlsx";
                                string fullPath = Path.Combine(folderPath, fileName);
                                wb.SaveAs(fullPath);
                            }
                        }
                    }
                }
            }
        }*/




        /*private void CreateExcel()
        {
            string connectionString = "Server=.\\;User Id=sa;Password=momo;Database=priceTracker;TrustServerCertificate=True;";
            string query = "SELECT productName, price, date, url FROM dbo.Products ORDER BY productName ASC, date ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        using (DataTable data = new DataTable())
                        {
                            adapter.Fill(data);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(data, "Excel Export");

                                string folderPath = "C:\\Users\\user\\Downloads"; 
                                
                                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                string fileName = $"ExcelExport_{timestamp}.xlsx";
                                string fullPath = Path.Combine(folderPath, fileName);

                                wb.SaveAs(fullPath);
                            }
                        }
                    }
                }
            }
        }*/

    }
}








