﻿
using System.ComponentModel.DataAnnotations;

public class ChangePasswordModel
{
    [Required(ErrorMessage = "Mật khẩu hiện tại là bắt buộc")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,16}$", ErrorMessage = "Password must be at least 8 characters, 1 uppercase, 1 lowercase, 1 number and 1 special character")]
    public string OldPassword { get; set; }
    [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,16}$", ErrorMessage = "Password must be at least 8 characters, 1 uppercase, 1 lowercase, 1 number and 1 special character")]
    public string NewPassword { get; set; }
}
