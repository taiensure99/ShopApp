using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class PhienDangNhap
{
    public int Id { get; set; }

    public int KhachHangId { get; set; }

    public DateTime ThoiGianDangNhap { get; set; }

    public string? DiaChiIp { get; set; }

    public virtual KhachHang KhachHang { get; set; } = null!;
}
