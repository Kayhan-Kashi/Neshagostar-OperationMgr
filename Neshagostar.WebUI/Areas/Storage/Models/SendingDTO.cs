using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Storage.Models
{
    public class SendingDTO
    {

        public Guid AllocatedProductId { get; set; }
        public string Date { get; set; }
        public double SendingAmount { get; set; }
    }
}