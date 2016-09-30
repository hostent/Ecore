using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EMin.Model.Collection
{
    public class Sale_Menber : IEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public string Id { get; set; }
    }
}
