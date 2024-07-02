using System;
using System.Collections.Generic;

namespace priceTracker.Models;

public partial class Employe
{
    public string? Name { get; set; }

    public string? Mail { get; set; }

    public string? Password { get; set; }

    public string? EmpType { get; set; }

    public bool? IsMail { get; set; }

    public int Id { get; set; }

    public virtual ICollection<Entry> Entries { get; set; } = new List<Entry>();
}
