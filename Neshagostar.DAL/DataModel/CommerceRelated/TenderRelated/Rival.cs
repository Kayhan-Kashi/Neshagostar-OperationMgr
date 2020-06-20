using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.TenderRelated
{
    public class Rival
    {
        public Guid Id { get; set; }
        [Display(Name="نام")]
        public string Name { get; set; }
        [Display(Name = "شهر")]
        public string City { get; set; }


        public List<RivalPrice> RivalPrices { get; set; }
    }
}
