using Microsoft.AspNetCore.Http;
namespace DoAnChuyenNganh.Contract.Services.Interface
{
    public interface ICloudinaryService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
