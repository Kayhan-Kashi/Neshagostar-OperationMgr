using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.StorageRelated.WastageRelated
{
    public class ProductIssue
    {
        public Guid Id { get; set; }

        [Display(Name = "عنوان ایراد")]
        public string Title { get; set; }

        public List<ImportedUnStandardProduct> ImportedUnStandardProducts { get; set; }
    }
}
