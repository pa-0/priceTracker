using System;
using System.Collections.Generic;

namespace priceTracker.Models;

public partial class Entry
{
    public bool? Status { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? FinishDate { get; set; }

    public DateOnly? RecordDate { get; set; }

    public int? EmpId { get; set; }

    public string? Url1 { get; set; }

    public string? Url2 { get; set; }

    public string? Url3 { get; set; }

    public string? Url4 { get; set; }

    public string? Url5 { get; set; }

    public int Id { get; set; }

    public virtual Employe? Emp { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
