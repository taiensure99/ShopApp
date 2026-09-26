using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class SanPham
{
    public int Id { get; set; }

    public string Ten { get; set; } = null!;

    public decimal GiaBan { get; set; }

    public int TonKho { get; set; }

    public string DanhMuc { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<DanhGiaSanPham> DanhGiaSanPhams { get; set; } = new List<DanhGiaSanPham>();
}
