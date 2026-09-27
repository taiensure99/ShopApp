using Microsoft.EntityFrameworkCore;
using ShopApp.Interfaces;
using ShopApp.Models;

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
    }
}
