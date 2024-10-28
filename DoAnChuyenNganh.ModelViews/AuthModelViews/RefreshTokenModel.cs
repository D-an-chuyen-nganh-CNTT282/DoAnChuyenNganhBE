using System.ComponentModel.DataAnnotations;

namespace DoAnChuyenNganh.ModelViews.AuthModelViews
{
    public class RefreshTokenModel
    {
        [Required(ErrorMessage = "RefreshToken không được để trống")]
        public string refreshToken { get; set; }
    }
}
