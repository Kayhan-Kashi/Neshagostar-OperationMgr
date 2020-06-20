using Neshagostar.DAL.DataModel.StorageRelated.OrdersRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neshagostar.WebUI.Areas.Storage.Models
{
    public class OrderItemStorageInfo
    {
        public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        
        public double AmountSent { get; set; }
        public double AmountNotSent { get; set; }
        public bool IsSendingCompleted { get; set; }
        public double AmountReadyToSend { get; set; }

        public double AmountDispatched { get; set; }

        public double AmountAllocated { get; set; }
        public double AmountUnAllocated { get; set; }
        public bool IsProductionCompleted { get; set; }


        public string OrderDate { get; set; }
        public string DispatchDate { get; set; }

        public string LastSendingDate { get; set; }

        public decimal PageNumbers { get; set; }
        public int Page { get; set; }

        public List<OrderItemSendingDetailDTO> OrderItemSendingDetails { get; set; }
    }
}