using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;
using ShopApp.Repositories.Interfaces;
using System.Security.AccessControl;

namespace ShopApp.Services
{
    public class DonHangService : IDonHangService
    {
        private readonly IDonHangRepository _repository;

        public DonHangService(IDonHangRepository repository)
        {
            _repository = repository;
        }

        public DonHang DatHang(DatHangRequest request)
        {
            throw new NotImplementedException();
        }

        public int DemDonHangTheoTrangThai(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                return 0;
            }

            string status = trangThai.Trim().ToLower();
            return _repository.Get()
                              .Count(d => d.TrangThai != null && d.TrangThai.Trim().ToLower() == status);
        }

        public DonHang ThemDonHang(DonHang donHang)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DonHang>> ChiTietDonHang()
        {
            var ketQua = await _repository.Get()
                                .Include(dh => dh.KhachHang)
                                .Include(dh => dh.ChiTietDonHangs)
                                    .ThenInclude(ctdh => ctdh.SanPham)
                                .ToListAsync();
            return ketQua;
        }

        public async Task<List<DonHang>> ChiTietDayDu()
        {
            var ketQua = await _repository.Get()
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietDonHangs)
                .SelectMany(dh => dh.ChiTietDonHangs, (dh, ctdh) => new DonHang
                {
                    Id = dh.Id,
                    MaDon = dh.MaDon,
                    KhachHangId = dh.KhachHangId,
                    NgayDat = dh.NgayDat,
                    TrangThai = dh.TrangThai,
                    TongTien = dh.TongTien,
                    KhachHang = dh.KhachHang,
                    ChiTietDonHangs = new List<ChiTietDonHang> { ctdh }
                }).ToListAsync();

            return ketQua;
        }

        public async Task<List<DonHangKhacHangDTO>> ChiTietDonHangKemTenKhach()
        {
            var ketQua = await _repository.Get()
                .Where(dh => dh.KhachHang != null)
                .Select(dh => new DonHangKhacHangDTO
                {
                    Id = dh.Id,
                    MaDon = dh.MaDon,
                    KhachHangId = dh.KhachHangId,
                    NgayDat = dh.NgayDat,
                    TrangThai = dh.TrangThai,
                    TongTien = dh.TongTien,
                    TenKhachHang = dh.KhachHang.HoTen
                }).ToListAsync();

            return ketQua;
        }

        public async Task<List<ThongKeKhanhHangVip>> ThongKeTheoKhach()
        {
            var ketQua = await _repository.Get()
                .Where(dh => dh.TrangThai == "da_giao")
                .GroupBy(dh => dh.KhachHangId)
                .Select(g => new ThongKeKhanhHangVip
                {
                    Id = g.Key,
                    SoLuongDon = g.Count(),
                    TongChiTieu = g.Sum(dh => dh.TongTien)
                })
                .ToListAsync();

            return ketQua;
        }

        public async Task<List<DonHang>> GetAll()
        {
            var ketQua = await _repository.Get()
                .Include(dh => dh.KhachHang)
                .ToListAsync();

            foreach (var donHang in ketQua)
            {
                Console.WriteLine($"DonHang Id: {donHang.Id}, MaDon: {donHang.MaDon}, Ten Khach Hang: {donHang.KhachHang?.HoTen}");
            }

            return ketQua;
        }

        public async Task<List<DonHang>> GetById(int id)
        {
            var donHang = await _repository.Get()
                .Where(dh => dh.Id == id) 
                .Include(dh => dh.KhachHang)
                .Select(dh => new DonHang
                {
                    Id = dh.Id,
                    MaDon = dh.MaDon,
                    KhachHangId = dh.KhachHangId,
                    NgayDat = dh.NgayDat,
                    TrangThai = dh.TrangThai,
                    TongTien = dh.TongTien,
                    KhachHang = dh.KhachHang
                })
                .ToListAsync();

            return donHang;
        }

        public async Task<List<DonHang>> ChiTietDonHangKemKhachHang(int id)
        {
            var donHang = await _repository.Get()
                .Where(dh => dh.Id == id)
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietDonHangs)
                    .ThenInclude(ctdh => ctdh.SanPham)
                .Select(dh => new DonHang
                {
                    Id = dh.Id,
                    MaDon = dh.MaDon,
                    KhachHangId = dh.KhachHangId,
                    NgayDat = dh.NgayDat,
                    TrangThai = dh.TrangThai,
                    TongTien = dh.TongTien,
                    KhachHang = dh.KhachHang,
                    ChiTietDonHangs = dh.ChiTietDonHangs
                })
                .ToListAsync();

            return donHang;
        }

        public async Task<List<DonHang>> DonHangKemChiTietSanPham(int id)
        {
            var donHang = await _repository.Get()
                .Where(dh => dh.Id == id)
                .Include(dh => dh.KhachHang)
                .Include(dh => dh.ChiTietDonHangs)
                    .ThenInclude(ctdh => ctdh.SanPham)
                .AsSplitQuery()
                .Select(dh => new DonHang
                {
                    Id = dh.Id,
                    MaDon = dh.MaDon,
                    KhachHangId = dh.KhachHangId,
                    NgayDat = dh.NgayDat,
                    TrangThai = dh.TrangThai,
                    TongTien = dh.TongTien,
                    KhachHang = dh.KhachHang,
                    ChiTietDonHangs = dh.ChiTietDonHangs
                })
                .ToListAsync();

            return donHang;
        }


        public async Task<int> DeleteDonHang(int id)    
        {
            var result = await _repository.Get()
                .Where(dh => dh.Id == id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
