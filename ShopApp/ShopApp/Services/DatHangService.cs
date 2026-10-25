using ShopApp.Interfaces;
using ShopApp.Models;
using ShopApp.Models.DataModels;
using ShopApp.Repositories.Interfaces;

namespace ShopApp.Services
{
    public class DatHangService : IDatHangService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatHangService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DatHang(int sanPhamId, int khachHangId, int soLuong)
        {
            throw new NotImplementedException();
        }

        public async Task<DonHang> DatHangAsync(DatHangRequest request)
        {

            bool khachHangTonTai = await _unitOfWork.KhachHangRepository.Exits(request.KhachHangId);
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
                var sanPham = await _unitOfWork.SanPhamRepository.GetByIdAsync(item.SanPhamId);
                if (sanPham == null)
                {
                    throw new Exception($"Sản phẩm có ID = {item.SanPhamId} không tồn tại!");
                }

                if (sanPham.TonKho < item.SoLuong)
                {
                    throw new Exception($"Sản phẩm '{sanPham.Ten}' không đủ hàng (Tồn kho: {sanPham.TonKho}, Yêu cầu: {item.SoLuong})!");
                }

                sanPham.TonKho -= item.SoLuong;
                _unitOfWork.SanPhamRepository.Update(sanPham);

                var chiTiet = new ChiTietDonHang
                {
                    SanPhamId = sanPham.Id,
                    SoLuong = item.SoLuong,
                    DonGia = sanPham.GiaBan
                };

                donHangMoi.ChiTietDonHangs.Add(chiTiet);
                donHangMoi.TongTien += sanPham.GiaBan * item.SoLuong;
            }

            await _unitOfWork.DonHangRepository.AddAsync(donHangMoi);
            await _unitOfWork.SaveChangesAsync();

            return donHangMoi;
        }

        public Task<DonHang> DatHangHoanChinh(DatHangRequest request)
        {
            throw new NotImplementedException();
        }
    }
}