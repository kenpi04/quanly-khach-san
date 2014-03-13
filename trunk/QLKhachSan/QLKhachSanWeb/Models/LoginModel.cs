using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLKhachSanWeb.Models
{
    public class LoginModel
    {
        public LoginModel() { }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int PermissonId { get; set; }


    }

    public class UserModel
    {
        public UserModel() { }

        public int Id { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }


        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Name { get; set; }
        public string CreateDate { get; set; }
        public string LastUpdateDate { get; set; }
        public int IsActive { get; set; }
        public int PermissonId { get; set; }

      
    }

}