using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class DanhMuc
{
    public int Id { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public string? MoTa { get; set; }

    public byte? TrangThai { get; set; }
}
