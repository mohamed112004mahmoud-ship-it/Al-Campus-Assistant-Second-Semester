using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Al_Campus_Assistant.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int Id { get; set; }

    public string Name
    {
        get { return $"{FirstName} {LastName}".Trim(); }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                var parts = value.Split(' ', 2);
                FirstName = parts.Length > 0 ? parts[0] : "";
                LastName = parts.Length > 1 ? parts[1] : "";
            }
        }
    }

    [Required(ErrorMessage = "الاسم الأول مطلوب")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "الاسم الأخير مطلوب")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Student";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool AgreeToTerms { get; set; }
    public bool IsAbove18 { get; set; }

    public ICollection<Filter> Filters { get; set; } = new List<Filter>();
}

public class LoginRequest
{
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    public string Password { get; set; } = string.Empty;
}

public class RegisterModel
{
    [Required(ErrorMessage = "الاسم الأول مطلوب")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "الاسم الأخير مطلوب")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "يجب الموافقة على الشروط والأحكام")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "يجب الموافقة على الشروط والأحكام")]
    public bool AgreeToTerms { get; set; }

    [Required(ErrorMessage = "يجب أن يكون عمرك 18 سنة على الأقل")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "يجب أن يكون عمرك 18 سنة على الأقل")]
    public bool IsAbove18 { get; set; }
}

public class ForgotPasswordModel
{
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordModel
{
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز التحقق مطلوب")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
    [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [Compare("NewPassword", ErrorMessage = "كلمتا المرور غير متطابقتين")]
    public string ConfirmPassword { get; set; } = string.Empty;
}