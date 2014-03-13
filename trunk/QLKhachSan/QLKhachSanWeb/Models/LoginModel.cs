using System;
using System.Collections.Generic;
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
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string CreateDate { get; set; }
        public string LastUpdateDate { get; set; }
        public int IsActive { get; set; }
        public int PermissonId { get; set; }

      
    }

}