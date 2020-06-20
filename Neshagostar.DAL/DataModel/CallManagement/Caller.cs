using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neshagostar.DAL.DataModel.CallManagement
{
    public class Caller
    {

        public Guid Id { get; set; }
   
        [Display(Name = "نام تماس گیرنده")]
        public string Name { get; set; }
        [Display(Name = "شماره تماس")]
        public string Tel { get; set; }
        [Display(Name = "توضیح در مورد تماس گیرنده")]
        public string Comments { get; set; }
        public List<RecievedCall> RecievedCalls { get; set; }
    }
}
