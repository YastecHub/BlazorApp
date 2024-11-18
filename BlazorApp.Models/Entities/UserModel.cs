using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.Entities
{
    public class UserModel
    {
        public UserModel()
        {
            UserRoles = new List<UserRoleModel>();
        }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public ICollection<UserRoleModel> UserRoles { get; set; }
    }
}
