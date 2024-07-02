using System;
using System.Collections.Generic;

namespace priceTracker.Models;

public partial class Product
{
    public int? ProdId { get; set; }

    public int? UrlNumber { get; set; }

    public string? ProductName { get; set; }

    public string? SiteName { get; set; }

    public double? Price { get; set; }

    public DateOnly? Date { get; set; }

    public string? Url { get; set; }

    public int Id { get; set; }

    public virtual Entry? Prod { get; set; }
}
