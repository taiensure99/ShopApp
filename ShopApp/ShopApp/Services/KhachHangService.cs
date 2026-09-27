using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopApp.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly ShopAppDbContext _context;
        public KhachHangService(ShopAppDbContext context)
        {
            _context = context;
        }

        public bool KiemTraEmailTonTai(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return _context.KhachHangs.Any(k => k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public int DemTongKhach() => _context.KhachHangs.Count();

        public async Task<bool> ThemKhachHang(KhachHang khachHang)
        {
            _context.KhachHangs.Add(khachHang);
            var kq = await _context.SaveChangesAsync();
            return kq > 0;
        }

        public async Task<List<KhachHang>> GetAll()
        {
            return await _context.KhachHangs.ToListAsync();
        }

        public async Task<KhachHang> GetById(int id)
        {
            return await _context.KhachHangs.FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<List<KhachHang>       > TimKiemTheoTen(string ten)
        {
            if (string.IsNullOrWhiteSpace(ten)) return _context.KhachHangs.ToList();

            return _context.KhachHangs
                .Where(k => k.HoTen != null &&
                            k.HoTen.Contains(ten, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public KhachHang GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return _context.KhachHangs.FirstOrDefault(
                k => k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> UpdateKhachHang(int id, KhachHang khachHang)
        {
            var existingKhachHang = await _context.KhachHangs.FindAsync(id);
            if (existingKhachHang == null) return false;

            existingKhachHang.HoTen = khachHang.HoTen;
            existingKhachHang.Email = khachHang.Email;
            // TODO: cập nhật thêm các field khác nếu có (SoDienThoai, DiaChi,...)

            var kq = await _context.SaveChangesAsync();
            return kq > 0;
        }
        public async Task<bool> DeleteKhachHang(int id)
        {
            var existingKhachHang = await _context.KhachHangs.FindAsync(id);
            if (existingKhachHang == null)
            {
                throw new ArgumentException($"Không tìm thấy khách hàng Id={id}");
            }
            _context.KhachHangs.Remove(existingKhachHang);
            var kq = await _context.SaveChangesAsync(); 
            return kq > 0;

        }
    }
}