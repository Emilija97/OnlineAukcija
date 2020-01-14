using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoDataLayer.DomainModel
{
    public class Subject
    {
        [Required(ErrorMessage = "Name is required.")]
        public string name { get; set; }
        public int sellingPrice { get; set; }

        [Required(ErrorMessage = "Starting price is required.")]
        public int startingPrice { get; set; }
        public int offerPrice { get; set; }
        public string description { get; set; }
        public string auctionName { get; set; }
    }
}
