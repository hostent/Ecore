using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Model.Collection
{
    public class Sale_ShoppingCart : IEntity
    {
        /// <summary>
        /// 商品图片ID
        /// </summary>
        [Key]
        public string Id { get; set; }

        public string MenberId { get; set; }
        public string ItemId { get; set; }
        public string ItemSkuId { get; set; }
        public int Number { get; set; }

    }


}
