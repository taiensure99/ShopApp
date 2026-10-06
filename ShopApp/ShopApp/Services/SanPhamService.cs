using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;

namespace ShopApp.Services
{
    public class SanPhamService : ISanPhamService
    {

        private readonly ShopAppDbContext _context;

        public SanPhamService(ShopAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> KiemTraTonTai(int id)
        {
            bool tonTai = await _context.SanPhams.AnyAsync(sp => sp.Id == id);
            return tonTai;
        }
        public async Task<SanPham> GetById(int id) //async await
        {

            return await _context.SanPhams.FirstOrDefaultAsync(sp => sp.Id == id);
        }


        public async Task<decimal> TinhTongTien(int sanPhamId, int soLuong)
        {
            var sanPham = await GetById(sanPhamId); //async await => 1 luồng khác hoàng toàn ko chung luông với thằng này

            if (sanPham == null)
            {
                throw new ArgumentException($"Sản phẩm với Id {sanPhamId} không tồn tại.");
            }
            return decimal.Multiply((decimal)sanPham.GiaBan, soLuong);
        }

        public async Task<List<SanPham>> GetAllSanPham()
        {
            return await _context.SanPhams.ToListAsync();
        }

        public async Task<bool> CreateSanPham(SanPham sanPham)
        {
            _context.SanPhams.Add(sanPham);
            var kq = await _context.SaveChangesAsync(); //cập nhất xuống db thành công thì kq >0 ngược lại thất bại kq = 0
            return kq > 0; //trả true false > 0 => thêm thành công  ngược thêm thất bại
        }

        public async Task<bool> UpdateSanPham(int id, SanPham sanPham)
        {
            var existingSanPham = await _context.SanPhams.FindAsync(id);
            if (existingSanPham == null)
            {
                throw new ArgumentException($"Không tìm thấy sản phẩm Id={id}");
            }
            existingSanPham.Ten = sanPham.Ten;
            existingSanPham.GiaBan = sanPham.GiaBan;
            existingSanPham.TonKho = sanPham.TonKho;
            existingSanPham.DanhMuc = sanPham.DanhMuc;
            existingSanPham.IsActive = sanPham.IsActive;
            var kq = await _context.SaveChangesAsync(); //cập nhất xuống db thành công thì kq >0 ngược lại thất bại kq = 0
            return kq > 0; //trả true false > 0 => thêm thành công  ngược thêm thất bại
        }

        public async Task<bool> DeleteSanPham(int id)
        {
            var existingSanPham = await _context.SanPhams.FindAsync(id);
            if (existingSanPham == null)
            {
                throw new ArgumentException($"Không tìm thấy sản phẩm Id={id}");
            }
            _context.SanPhams.Remove(existingSanPham);
            var kq = await _context.SaveChangesAsync(); //cập nhất xuống db thành công thì kq >0 ngược lại thất bại kq = 0
            return kq > 0; //trả true false > 0 => thêm thành công  ngược thêm thất bại
        }

        public async Task<List<SanPham>> LocAsync(string danhMuc, decimal? giaToiDa)
        {
            var query = _context.SanPhams.AsQueryable();
            if (!string.IsNullOrEmpty(danhMuc))
            {
                query = query.Where(sp => sp.DanhMuc == danhMuc);
            }
            if (giaToiDa.HasValue)
            {
                query = query.Where(sp => sp.GiaBan <= giaToiDa.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<Dictionary<string, int>> ThongKeTheoDanhMucAsync()
        {
            var result = await _context.SanPhams
                .GroupBy(sp => sp.DanhMuc)
                .Select(g => new { DanhMuc = g.Key, SoLuong = g.Count() })
                .ToDictionaryAsync(x => x.DanhMuc, x => x.SoLuong);
            return result;
        }

        public async Task<bool> ChuyenSanPhamGiuaDanhMucAsync(int spId1, int spId2)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sp1 = await _context.SanPhams.FindAsync(spId1);
                var sp2 = await _context.SanPhams.FindAsync(spId2);
                if (sp1 == null || sp2 == null)
                {
                    throw new ArgumentException("Một trong hai sản phẩm không tồn tại.");
                }
                var tempDanhMuc = sp1.DanhMuc;
                sp1.DanhMuc = sp2.DanhMuc;
                sp2.DanhMuc = tempDanhMuc;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; 
            }
        }

        public async Task<List<SanPham>> LocGiaAsync(decimal giamin, decimal giamax)
        {
            var query = _context.SanPhams.AsQueryable();
            query = query.Where(sp => sp.GiaBan >= giamin && sp.GiaBan <= giamax).OrderBy(sp => sp.GiaBan);
            return await query.ToListAsync();
            //var List = await _context.SanPhams.Where(sp => sp.GiaBan >= giamin && sp.GiaBan <= giamax).OrderBy(sp => sp.GiaBan).ToListAsync();
            //return List;
        }

        public async Task<List<SanPham>> DanhSachRutGonAsync()
        {
            return await _context.SanPhams.Select(sp => new SanPham
            {
                Id = sp.Id,
                Ten = sp.Ten,
                GiaBan = sp.GiaBan
            }).ToListAsync();
        }
        public async Task<DenSoDonHang> DemTheoDanhMucAsync()
        {
            var result = await _context.SanPhams
                .GroupBy(sp => sp.DanhMuc)
                .Select(g => new { DanhMuc = g.Key, SoLuong = g.Count() })
                .ToListAsync();
            return new DenSoDonHang
            {
                DanhMuc = result.FirstOrDefault()?.DanhMuc,
                SoLuong = result.Sum(x => x.SoLuong)
            };
        }
        public async Task<List<SanPham>> TopDatNhatTheoDanhMucAsync()
        {
            var result = await _context.SanPhams
                .GroupBy(sp => sp.DanhMuc)
                .Select(g => new { DanhMuc = g.Key, TopSanPham = g.OrderByDescending(sp => sp.GiaBan).Take(1) })
                .ToListAsync();
            return result.SelectMany(x => x.TopSanPham).ToList();
        }
    }
}
