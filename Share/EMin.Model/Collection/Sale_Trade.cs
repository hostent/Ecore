using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Model.Collection
{
    public class Sale_Trade : IEntity, IEntityRecord
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public string Id { get; set; }

        public string BuyerId { get; set; }

        public string CreateBy { get; set; }

        public string UpdateBy { get; set; }


        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }


        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 按利率折扣
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// 按额度折扣
        /// </summary>
        public decimal DiscountAmount { get; set; }

        public decimal ActualAmount { get; set; }

        /// <summary>
        /// 10:未付款， 20：部分付款（定金），30：已付款
        /// </summary>
        public int PayState { get; set; }

        public DateTime PayTime { get; set; }

        public string Comment { get; set; }

    }
}
