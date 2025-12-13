using System;
using System.Collections.Generic;

namespace Pharmacy.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Drug> Drugs { get; set; } = new List<Drug>();
}
