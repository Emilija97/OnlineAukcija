using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoDataLayer.DomainModel
{
    public class User
    {
        public String id { get; set; }
        public String name { get; set; }
        public String surname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public String email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public String password { get; set; }

        public bool role { get; set; }
    }
}
