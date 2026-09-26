using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class VwChiTietDonHangDayDu
{
    public string MaDon { get; set; } = null!;

    public string TenSanPham { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public decimal? ThanhTien { get; set; }
}
