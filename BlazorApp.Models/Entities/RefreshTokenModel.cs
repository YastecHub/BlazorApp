using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.Entities
{
    public class RefreshTokenModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string RefreshToken { get; set; }
        public UserModel UserModel { get; set; }
    }
}
