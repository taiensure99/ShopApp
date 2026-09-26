using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class VwSanPhamConHang
{
    public int Id { get; set; }

    public string Ten { get; set; } = null!;

    public decimal GiaBan { get; set; }

    public string DanhMuc { get; set; } = null!;

    public int TonKho { get; set; }
}
