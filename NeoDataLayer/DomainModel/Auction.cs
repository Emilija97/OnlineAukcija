using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoDataLayer.DomainModel
{
    public class Auction
    {
        public string title { get; set; }
        public string type { get; set; }

        [DataType(DataType.Date)]
        public DateTime datePublished { get; set; }

        [DataType(DataType.Date)]
        public DateTime duration { get; set; }

    }
}
