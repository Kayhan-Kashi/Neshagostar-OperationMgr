using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neshagostar.DAL.DataModel.StorageRelated.MaterialRelated;

namespace Neshagostar.DAL.DataModel.CommerceRelated.MaterialRelated
{
    public class MaterialSeller
    {
        public Guid Id { get; set; }

        [Display(Name = "اسم فروشنده")]
        public string Name { get; set; }

        public List<MaterialPurchased> MaterialPurchaseds { get; set; }
    }
}
