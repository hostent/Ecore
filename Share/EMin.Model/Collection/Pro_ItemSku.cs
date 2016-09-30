using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Model.Collection
{
    public class Pro_ItemSku : IEntity
    {
        public string Id { get; set; }

        public string ItemId { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal PromotionPrice { get; set; }

        public bool IsOnSale { get; set; }

        /// <summary>
        /// 款式
        /// </summary>
        public string Pattern { get; set; }



    }
}
