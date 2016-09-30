using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Model.Collection
{
    public class Sale_TradePayment : IEntity, IEntityRecord
    {
        [Key]
        public string Id { get; set; }

        public string CreateBy { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }


        public string TradeId { get; set; }

        public decimal Amount { get; set; }

        public string MenberId { get; set; }

        public DateTime PayTime { get; set; }

    

    }
}
