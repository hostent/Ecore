using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Model.Collection
{
    public class Sale_TradeRecord : IEntity, IEntityRecord
    {
        public string Id { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string RecordLog { get; set; }

    }
}
