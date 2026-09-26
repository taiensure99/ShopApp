using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class VwDonHangDayDu
{
    public int Id { get; set; }

    public string MaDon { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public decimal TongTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public DateTime NgayDat { get; set; }

    public int? SoSanPham { get; set; }
}
