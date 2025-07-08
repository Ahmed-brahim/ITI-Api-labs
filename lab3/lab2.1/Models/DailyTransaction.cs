using System;
using System.Collections.Generic;

namespace lab2._1.Models;

public partial class DailyTransaction
{
    public int UserId { get; set; }

    public int? Amount { get; set; }
}
