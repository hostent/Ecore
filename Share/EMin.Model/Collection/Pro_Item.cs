using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EMin.Model.Collection
{
    public class Pro_Item
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        [Key]
        public string Id { get; set; }

        public string Title { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal PromotionPrice { get; set; }

        public string Detail { get; set; }

        public string MainImageId { get; set; }

        public bool IsOnSale { get; set; }

    }
}
