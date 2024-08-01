using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADizErp
{
    public class UserProfileImage
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ProfilePic { get; set; }
    }
}
