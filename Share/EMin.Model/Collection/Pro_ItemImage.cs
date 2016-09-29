
using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EMin.Model.Collection
{
    public class Pro_ItemImage
    {
        /// <summary>
        /// 商品图片ID
        /// </summary>
        [Key]
        public string Id { get; set; }

        public string ItemId { get; set; }

        public string Url { get; set; }


    }
}
