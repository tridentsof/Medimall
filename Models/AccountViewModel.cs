using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medimall.Models
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Mật khẩu")]
        [MinLength(6, ErrorMessage = "{0} phải chứa ít nhất 6 kí tự")]
        public string Password { get; set; }
    }
}