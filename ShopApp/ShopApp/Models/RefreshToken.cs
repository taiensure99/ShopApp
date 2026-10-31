using System;
using System.Collections.Generic;

namespace ShopApp.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int MaKhachHang { get; set; }

    public string TokenHash { get; set; } = null!;

    public DateTime NgayTao { get; set; }

    public DateTime HetHan { get; set; }

    public bool DaThuHoi { get; set; }

    public string? ThayBangTokenHash { get; set; }
}
