using System;
using System.Collections.Generic;

namespace Pharmacy.Models;

public partial class AdminLog
{
    public int LogId { get; set; }

    public int AdminId { get; set; }

    public string ActionType { get; set; } = null!;

    public DateTime ActionTime { get; set; }

    public virtual Admin Admin { get; set; } = null!;
}
