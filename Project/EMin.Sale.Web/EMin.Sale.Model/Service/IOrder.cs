using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Sale.Model.Service
{
    public interface IOrder
    {
        Result CreateTrade(TradeCreatePar par);

    }

    public class TradeCreatePar
    {
        public string MenberId { get; set; }

        public List<ItemPar> ItemParList { get; set; }

        public string Comment { get; set; }
    } 

    public class ItemPar
    {
        public string ItemId { get; set; }

        public string ItemSkuId { get; set; }

        public int Count { get; set; }

        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 按利率折扣
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// 按额度折扣
        /// </summary>
        public decimal DiscountAmount { get; set; }

        public decimal ActualAmount { get; set; }
    }
}
