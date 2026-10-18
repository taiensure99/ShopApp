using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopApp.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly IKhachHangRepository _repository;
        public KhachHangService(IKhachHangRepository repository)
        {
            _repository = repository;
        }

        public bool KiemTraEmailTonTai(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return _repository.Get().Any(k => k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public int DemTongKhach() => _repository.Get().Count();

        public async Task<bool> ThemKhachHang(KhachHang khachHang)
        {
            await _repository.AddAsync(khachHang);
            var kq = await _repository.SaveChangesAsync();
            return kq > 0;
        }

        public async Task<List<KhachHang>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<KhachHang> GetById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<KhachHang>       > TimKiemTheoTen(string ten)
        {
            if (string.IsNullOrWhiteSpace(ten)) return await _repository.GetAllAsync();

            return _repository.Get()
                .Where(k => k.HoTen != null &&
                            k.HoTen.Contains(ten, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public KhachHang GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return _repository.Get().FirstOrDefault(
                k => k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> UpdateKhachHang(int id, KhachHang khachHang)
        {
            var existingKhachHang = await _repository.GetByIdAsync(id);
            if (existingKhachHang == null) return false;

            existingKhachHang.HoTen = khachHang.HoTen;
            existingKhachHang.Email = khachHang.Email;
            // TODO: cập nhật thêm các field khác nếu có (SoDienThoai, DiaChi,...)
            _repository.Update(existingKhachHang);
            var kq = await _repository.SaveChangesAsync();
            return kq > 0;
        }
        public async Task<bool> DeleteKhachHang(int id)
        {
            var existingKhachHang = await _repository.GetByIdAsync(id);
            if (existingKhachHang == null)
            {
                throw new ArgumentException($"Không tìm thấy khách hàng Id={id}");
            }
            _repository.Remove(existingKhachHang);
            var kq = await _repository.SaveChangesAsync();
            return kq > 0;

        }

        public async Task<List<KhachHang>> LayDonHangTheoKhachHang(int id)
        {
            var KetQua = await _repository.Get()
                .Where(kh => kh.Id == id)
                .Include(kh => kh.DonHangs.Where(dh => dh.TrangThai != "da_huy"))
                .Select(kh => new KhachHang
                {
                    Id = kh.Id,
                    HoTen = kh.HoTen,
                    DonHangs = kh.DonHangs
                        .Where(dh => dh.TrangThai != "da_huy")
                        .Select(dh => new DonHang
                        {
                            Id = dh.Id,
                            MaDon = dh.MaDon,
                            KhachHangId = dh.KhachHangId,
                            NgayDat = dh.NgayDat,
                            TrangThai = dh.TrangThai,
                            TongTien = dh.TongTien
                        }).ToList()
                })
                .ToListAsync();

            return KetQua;
        }
    }
}