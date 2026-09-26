using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class NhanVien
{
    public int Id { get; set; }

    public string HoTen { get; set; } = null!;

    public string ChucVu { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly NgayVaoLam { get; set; }
}
