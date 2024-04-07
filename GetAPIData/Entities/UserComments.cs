using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAPIData.Entities
{
    public class UserComments
    {
        [Key]
        public int slno { get; set; }
        public int postId { get; set; } = 0;
        public int id { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;
    }
}
