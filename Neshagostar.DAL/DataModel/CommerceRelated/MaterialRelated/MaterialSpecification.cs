using Neshagostar.DAL.DataModel.StorageRelated.MaterialRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CommerceRelated.MaterialRelated
{
    public class MaterialSpecification
    {
        public Guid Id { get; set; }

        [Display(Name = "نوع مواد")]
        public string Specification { get; set; }

        public List<MaterialPurchased> MaterialPurchaseds { get; set; }
    }
}
