﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Items;

namespace ITU_Scenario.Items
{
    /// <summary>
    /// expression that splits vertically to divide a path
    /// </summary>
    public class HMSCInlineExpressionAltPar : Container
    {
        /// <summary>
        /// connectable item and has to be checked as path item
        /// </summary>
        public override bool IsPathItem => true;

        /// <summary>
        /// contains all item left of the split line
        /// </summary>
        [XmlIgnore]
        public List<BaseObject> ObjectsLeftOfLine => this.ContainingItems.Where(t => t.Locationx < this.Locationx).ToList();

        /// <summary>
        /// contains all items right of the split line
        /// </summary>
        [XmlIgnore]
        public List<BaseObject> ObjectsRightOfLine => this.ContainingItems.Where(t => t.Locationx > this.Locationx).ToList();

    }
}
