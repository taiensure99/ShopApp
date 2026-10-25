using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;
using ShopApp.Repositories.Interfaces;

namespace ShopApp.Services
{
    public class DatHangService : IDatHangService
    {
        private readonly IRepository<DonHang> _donHangRepo;
        private readonly IRepository<SanPham> _sanPhamRepo;
        private readonly IRepository<KhachHang> _khachHangRepo;

        public DatHangService(IUnitOfWork unitOfWork)
        {
            _donHangRepo = unitOfWork.DonHangRepository;
            _sanPhamRepo = unitOfWork.SanPhamRepository;
            _khachHangRepo = unitOfWork.KhachHangRepository;
        }

        public void DatHang(int sanPhamId, int khachHangId, int soLuong)
        {
            throw new NotImplementedException();
        }

        public async Task<DonHang> DatHangAsync(DatHangRequest request)
        {

            bool khachHangTonTai = await _khachHangRepo.Exits(request.KhachHangId);
            if (!khachHangTonTai)
            {
                throw new Exception($"Khách hàng có ID = {request.KhachHangId} không tồn tại!");
            }

            if (request.DanhSachSanPham == null || !request.DanhSachSanPham.Any())
            {
                throw new Exception("Danh sách sản phẩm đặt mua không được để trống!");
            }

            var donHangMoi = new DonHang
            {
                MaDon = $"DH-{DateTime.Now:yyyyMMddHHmmss}",
                KhachHangId = request.KhachHangId,
                NgayDat = DateTime.Now,
                TrangThai = "cho_xu_ly",
                TongTien = 0,
                ChiTietDonHangs = new List<ChiTietDonHang>()
            };

            foreach (var item in request.DanhSachSanPham)
            {
                var sanPham = await _sanPhamRepo.GetByIdAsync(item.SanPhamId);
                if (sanPham == null)
                {
                    throw new Exception($"Sản phẩm có ID = {item.SanPhamId} không tồn tại!");
                }

                if (sanPham.TonKho < item.SoLuong)
                {
                    throw new Exception($"Sản phẩm '{sanPham.Ten}' không đủ hàng (Tồn kho: {sanPham.TonKho}, Yêu cầu: {item.SoLuong})!");
                }

                sanPham.TonKho -= item.SoLuong;
                _sanPhamRepo.Update(sanPham);

                var chiTiet = new ChiTietDonHang
                {
                    SanPhamId = sanPham.Id,
                    SoLuong = item.SoLuong,
                    DonGia = sanPham.GiaBan
                };

                donHangMoi.ChiTietDonHangs.Add(chiTiet);
                donHangMoi.TongTien += sanPham.GiaBan * item.SoLuong;
            }

            await _donHangRepo.AddAsync(donHangMoi);
            await _donHangRepo.SaveChangesAsync();

            return donHangMoi;
        }

        public Task<DonHang> DatHangHoanChinh(DatHangRequest request)
        {
            throw new NotImplementedException();
        }
    }
}