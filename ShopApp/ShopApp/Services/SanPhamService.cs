using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;
using ShopApp.Repositories.Interfaces;
using System.Data;

namespace ShopApp.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly IRepository<SanPham> _sanPhamRepo;

        public SanPhamService(IUnitOfWork unitOfWork)
        {
            _sanPhamRepo = unitOfWork.SanPhamRepository;
        }


        public async Task<bool> KiemTraTonTai(int id)
        {
            bool tonTai = await _sanPhamRepo.Exits(id);
            return tonTai;
        }
        public async Task<SanPham> GetById(int id) //async await
        {
            var sanPham = await _sanPhamRepo.FromSqlRaw($"EXECUTE sp_LaySanPhamTheoId @SanPhamId={id}")
                .FirstOrDefaultAsync();
            return sanPham;
        }


        public async Task<decimal> TinhTongTien(int sanPhamId, int soLuong)
        {
            var sanPham = await _sanPhamRepo.GetByIdAsync(sanPhamId); //async await => 1 luồng khác hoàng toàn ko chung luông với thằng này

            if (sanPham == null)
            {
                throw new ArgumentException($"Sản phẩm với Id {sanPhamId} không tồn tại.");
            }
            return decimal.Multiply((decimal)sanPham.GiaBan, soLuong);
        }

        public async Task<List<SanPham>> GetAllSanPham()
        {
            return await _sanPhamRepo.GetAllAsync();
        }

        public async Task<bool> CreateSanPham(SanPham sanPham)
        {
            await _sanPhamRepo.AddAsync(sanPham);
            var kq = await _sanPhamRepo.SaveChangesAsync(); //cập nhất xuống db thành công thì kq >0 ngược lại thất bại kq = 0
            return kq > 0; //trả true false > 0 => thêm thành công  ngược thêm thất bại
        }

        public async Task<bool> UpdateSanPham(int id, SanPham sanPham)
        {
            var existingSanPham = await _sanPhamRepo.GetByIdAsync(id);
            if (existingSanPham == null)
            {
                throw new ArgumentException($"Không tìm thấy sản phẩm Id={id}");
            }
            existingSanPham.Ten = sanPham.Ten;
            existingSanPham.GiaBan = sanPham.GiaBan;
            existingSanPham.TonKho = sanPham.TonKho;
            existingSanPham.DanhMuc = sanPham.DanhMuc;
            existingSanPham.IsActive = sanPham.IsActive;
            _sanPhamRepo.Update(existingSanPham);
            var kq = await _sanPhamRepo.SaveChangesAsync(); //cập nhất xuống db thành công thì kq >0 ngược lại thất bại kq = 0
            return kq > 0; //trả true false > 0 => thêm thành công  ngược thêm thất bại
        }

        public async Task<bool> DeleteSanPham(int id)
        {
            var existingSanPham = await _sanPhamRepo.GetByIdAsync(id);
            if (existingSanPham == null)
            {
                throw new ArgumentException($"Không tìm thấy sản phẩm Id={id}");
            }
            _sanPhamRepo.Remove(existingSanPham);
            var kq = await _sanPhamRepo.SaveChangesAsync(); //cập nhất xuống db thành công thì kq >0 ngược lại thất bại kq = 0
            return kq > 0; //trả true false > 0 => thêm thành công  ngược thêm thất bại
        }

        public async Task<List<SanPham>> LocAsync(string danhMuc, decimal? giaToiDa)
        {
            var query = _sanPhamRepo.Get();
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
            var result = await _sanPhamRepo.Get()
                .GroupBy(sp => sp.DanhMuc)
                .Select(g => new { DanhMuc = g.Key, SoLuong = g.Count() })
                .ToDictionaryAsync(x => x.DanhMuc, x => x.SoLuong);
            return result;
        }

        //public async Task<bool> ChuyenSanPhamGiuaDanhMucAsync(int spId1, int spId2)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var sp1 = await _context.SanPhams.FindAsync(spId1);
        //        var sp2 = await _context.SanPhams.FindAsync(spId2);
        //        if (sp1 == null || sp2 == null)
        //        {
        //            throw new ArgumentException("Một trong hai sản phẩm không tồn tại.");
        //        }
        //        var tempDanhMuc = sp1.DanhMuc;
        //        sp1.DanhMuc = sp2.DanhMuc;
        //        sp2.DanhMuc = tempDanhMuc;
        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        throw; 
        //    }
        //}

        public async Task<List<SanPham>> LocGiaAsync(decimal giamin, decimal giamax)
        {
            var query = _sanPhamRepo.Get();
            query = query.Where(sp => sp.GiaBan >= giamin && sp.GiaBan <= giamax).OrderBy(sp => sp.GiaBan);
            return await query.ToListAsync();
            //var List = await _context.SanPhams.Where(sp => sp.GiaBan >= giamin && sp.GiaBan <= giamax).OrderBy(sp => sp.GiaBan).ToListAsync();
            //return List;
        }

        public async Task<List<SanPham>> DanhSachRutGonAsync()
        {
            return await _sanPhamRepo.Get().Select(sp => new SanPham
            {
                Id = sp.Id,
                Ten = sp.Ten,
                GiaBan = sp.GiaBan
            }).ToListAsync();
        }
        public async Task<DenSoDonHang> DemTheoDanhMucAsync()
        {
            var result = await _sanPhamRepo.Get()
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
            var result = await _sanPhamRepo.Get()
                .GroupBy(sp => sp.DanhMuc)
                .Select(g => new { DanhMuc = g.Key, TopSanPham = g.OrderByDescending(sp => sp.GiaBan).Take(1) })
                .ToListAsync();
            return result.SelectMany(x => x.TopSanPham).ToList();
        }

        public async Task<SanPham> GetByStoreUseId(int id)
        {
            var ketqua = await  _sanPhamRepo.FromSqlRaw($"EXECUTE sp_LaySanPhamTheoId @SanPhamId={id}").ToListAsync();
            return ketqua.FirstOrDefault();
        }

        public async Task<List<SanPham>> TimSanPhamTheoTen(string ten)
        {
            return await _sanPhamRepo
                .FromSqlRaw("EXECUTE sp_TimSanPhamTheoTen @Ten = {0}", ten)
                .ToListAsync();

        
        }


        //public async Task<List<SanPhamTimKiemDto>> TimKiemNangCaoAsync(TimKiemSanPhamQuery query)
        //{
        //    var danhSachThamSo = new List<string>();
        //    var sqlParams = new List<SqlParameter>();

        //    if (!string.IsNullOrWhiteSpace(query.TuKhoa))
        //    {
        //        danhSachThamSo.Add("@TuKhoa = @TuKhoa");
        //        sqlParams.Add(new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = query.TuKhoa });
        //    }

        //    if (!string.IsNullOrWhiteSpace(query.DanhMuc))
        //    {
        //        danhSachThamSo.Add("@DanhMuc = @DanhMuc");
        //        sqlParams.Add(new SqlParameter("@DanhMuc", SqlDbType.NVarChar, 100) { Value = query.DanhMuc });
        //    }

        //    if (query.GiaTu.HasValue)
        //    {
        //        danhSachThamSo.Add("@GiaTu = @GiaTu");
        //        sqlParams.Add(new SqlParameter("@GiaTu", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = query.GiaTu.Value });
        //    }

        //    if (query.GiaDen.HasValue)
        //    {
        //        danhSachThamSo.Add("@GiaDen = @GiaDen");
        //        sqlParams.Add(new SqlParameter("@GiaDen", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = query.GiaDen.Value });
        //    }

        //    if (query.ChiConHang.HasValue)
        //    {
        //        danhSachThamSo.Add("@ChiConHang = @ChiConHang");
        //        sqlParams.Add(new SqlParameter("@ChiConHang", SqlDbType.Bit) { Value = query.ChiConHang.Value });
        //    }

        //    if (!string.IsNullOrWhiteSpace(query.SapXepTheo))
        //    {
        //        danhSachThamSo.Add("@SapXepTheo = @SapXepTheo");
        //        sqlParams.Add(new SqlParameter("@SapXepTheo", SqlDbType.NVarChar, 50) { Value = query.SapXepTheo });
        //    }

        //    if (query.ChieuTang.HasValue)
        //    {
        //        danhSachThamSo.Add("@ChieuTang = @ChieuTang");
        //        sqlParams.Add(new SqlParameter("@ChieuTang", SqlDbType.Bit) { Value = query.ChieuTang.Value });
        //    }

        //    string sql = "EXEC dbo.sp_TimKiemSanPhamNangCao";
        //    if (danhSachThamSo.Count > 0)
        //    {
        //        sql += " " + string.Join(", ", danhSachThamSo);
        //    }

        //    var ketQua = await _context.Database
        //        .SqlQueryRaw<SanPhamTimKiemDto>(sql, sqlParams.ToArray())
        //        .ToListAsync();

        //    return ketQua;
        //}
    }
    
}
