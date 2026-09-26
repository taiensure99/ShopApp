using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class DanhGiaSanPham
{
    public int Id { get; set; }

    public int? SanPhamId { get; set; }

    public int KhachHangId { get; set; }

    public int SoSao { get; set; }

    public string? NoiDung { get; set; }

    public DateTime NgayDanhGia { get; set; }

    public virtual KhachHang KhachHang { get; set; } = null!;

    public virtual SanPham? SanPham { get; set; }
}
