using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.Entities
{
    public class UserRoleModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public RoleModel Role { get; set; }
        public UserModel User { get; set; }
    }
}
