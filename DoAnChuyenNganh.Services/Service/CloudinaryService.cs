using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DoAnChuyenNganh.Contract.Services.Interface;
using DoAnChuyenNganh.Core.Base;
using DoAnChuyenNganh.Core.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DoAnChuyenNganh.Services.Service
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IConfiguration configuration)
        {
            var cloudinaryConfig = configuration.GetSection("Cloudinary");
            var account = new Account(
                cloudinaryConfig["CloudName"],
                cloudinaryConfig["ApiKey"],
                cloudinaryConfig["ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file.Length == 0)
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Lỗi!!! File rỗng");

            // Lấy tên file mà không có phần mở rộng
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName);
            var publicId = fileNameWithoutExtension; // Sử dụng tên file gốc làm publicId

            using var stream = file.OpenReadStream();

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId, // Đặt publicId theo tên file
                Overwrite = true // Ghi đè nếu file đã tồn tại
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new BaseException.ErrorException(Core.Store.StatusCodes.BadRequest, ErrorCode.BadRequest, "Lỗi khi tải file lên Cloudinary.");
            }

            return uploadResult.SecureUrl.AbsoluteUri; 
        }

    }
}
