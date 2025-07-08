using System;
using System.Collections.Generic;

namespace lab2._1.Models;

public partial class V1
{
    public int StId { get; set; }

    public string? StFname { get; set; }

    public string? StLname { get; set; }

    public string? StAddress { get; set; }

    public int? StAge { get; set; }

    public int? DeptId { get; set; }

    public int? StSuper { get; set; }
}
