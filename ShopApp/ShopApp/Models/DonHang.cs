using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class DonHang
{
    public int Id { get; set; }

    public string MaDon { get; set; } = null!;

    public int KhachHangId { get; set; }

    public DateTime NgayDat { get; set; }

    public string TrangThai { get; set; } = null!;

    public decimal TongTien { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual KhachHang KhachHang { get; set; } = null!;
}
