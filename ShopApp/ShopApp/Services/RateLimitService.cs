using Microsoft.Extensions.Configuration;
using ShopApp.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

public class RateLimitService : IRateLimitService
{
    // Khai báo đầy đủ kiểu dữ liệu để tránh lỗi cú pháp Generic
    private readonly Dictionary<string, List<DateTime>>_requestTracker = new Dictionary<string, List<DateTime>>();
    private readonly int _gioiHanMoiPhut;
    private readonly object _lock = new object();

    public RateLimitService(IConfiguration configuration)
    {
        _gioiHanMoiPhut = configuration.GetValue("RateLimiting:GioiHanMoiPhut", 5);
    }

    public bool KiemTraVuotGioiHan(string diaChiIp)
    {
        lock (_lock)
        {
            if (!_requestTracker.ContainsKey(diaChiIp))
            {
                return false;
            }

            var thoiDiemHienTai = DateTime.UtcNow;
            var motPhutTruoc = thoiDiemHienTai.AddMinutes(-1);

            // Dọn dẹp: Xóa bỏ các mốc thời gian đã cũ hơn 1 phút khỏi danh sách của IP này
            _requestTracker[diaChiIp].RemoveAll(thoiDiem => thoiDiem < motPhutTruoc);

            // Nếu số lượng request còn lại trong 1 phút qua lớn hơn hoặc bằng giới hạn
            return _requestTracker[diaChiIp].Count >= _gioiHanMoiPhut;
        }
    }

    public void GhiNhanRequest(string diaChiIp)
    {
        lock (_lock)
        {
            if (!_requestTracker.ContainsKey(diaChiIp))
            {
                _requestTracker[diaChiIp] = new List<DateTime>();
            }

            _requestTracker[diaChiIp].Add(DateTime.UtcNow);
        }
    }
}
