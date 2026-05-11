using System.ComponentModel.DataAnnotations;

namespace PowerHouse.VM
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "رقم الموبايل مطلوب")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "رقم موبايل غير صحيح")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمات المرور غير متطابقة")]
        public string ConfirmPassword { get; set; } = "";

        [Required(ErrorMessage = "اختر الفرع الأساسي")]
        public int MainBranchId { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "رقم الموبايل مطلوب")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; } = "";
    }

}
