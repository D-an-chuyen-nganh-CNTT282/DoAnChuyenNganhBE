
using System.ComponentModel.DataAnnotations;

public class ChangePasswordModel
{
    [Required(ErrorMessage = "Mật khẩu hiện tại là bắt buộc")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,16}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 số và 1 kí tự đặc biệt")]
    public string OldPassword { get; set; }
    [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,16}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 số và 1 kí tự đặc biệt")]
    public string NewPassword { get; set; }
}
