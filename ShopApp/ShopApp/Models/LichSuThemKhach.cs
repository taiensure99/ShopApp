using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class LichSuThemKhach
{
    public int Id { get; set; }

    public int KhachHangId { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime ThoiGianThem { get; set; }
}
