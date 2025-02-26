using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmsModels.BaseModels
{
    // Represents a type of goods consignment (GC)
    public class GcTypeModel
    {
        public short? GcTypeID { get; set; } // Unique identifier for the GC type (nullable)
        public string GcTypeName { get; set; }        
    }
}
