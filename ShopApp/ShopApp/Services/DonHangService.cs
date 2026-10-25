using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;
using ShopApp.Repositories.Interfaces;

namespace ShopApp.Services
{
    public class DonHangService : IDonHangService
    {
        private readonly IRepository<DonHang> _DonHangRepo;
        private readonly IRepository<KhachHang> _khachHangRepo;

        public DonHangService(IUnitOfWork unitOfWork)
        {
            _DonHangRepo = unitOfWork.DonHangRepository;
            _khachHangRepo = unitOfWork.KhachHangRepository;
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
            return _DonHangRepo.Get()
                              .Count(d => d.TrangThai != null && d.TrangThai.Trim().ToLower() == status);
        }

        public DonHang ThemDonHang(DonHang donHang)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DonHang>> ChiTietDonHang()
        {
            var ketQua = await _DonHangRepo.Get()
                                .Include(dh => dh.KhachHang)
                                .Include(dh => dh.ChiTietDonHangs)
                                    .ThenInclude(ctdh => ctdh.SanPham)
                                .ToListAsync();
            return ketQua;
        }

        public async Task<List<DonHang>> ChiTietDayDu()
        {
            var ketQua = await _DonHangRepo.Get()
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
            var DonHangs =  _DonHangRepo.Get();
            var KhachHangs =  _khachHangRepo.Get();

            var ketQua = await DonHangs.Join(KhachHangs,
                dh => dh.KhachHangId,
                kh => kh.Id,
                (dh, kh) => new DonHangKhacHangDTO
                {
                    Id = dh.Id,
                    MaDon = dh.MaDon,
                    KhachHangId = dh.KhachHangId,
                    NgayDat = dh.NgayDat,
                    TrangThai = dh.TrangThai,
                    TongTien = dh.TongTien,
                    TenKhachHang = kh.HoTen
                }).ToListAsync();
            return ketQua;
        }

        public async Task<List<ThongKeKhanhHangVip>> ThongKeTheoKhach()
        {
            var ketQua = await _DonHangRepo.Get()
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
            var ketQua = await  _DonHangRepo.Get()
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
            var donHang = await _DonHangRepo.Get()
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
            var donHang = await _DonHangRepo.Get()
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
            var donHang = await _DonHangRepo.Get()
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
            var result = await _DonHangRepo.Get()
                .Where(dh => dh.Id == id)
                .ExecuteDeleteAsync();

            return result;
        }
    }
}
