using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CREST_Uncertainty
{
    public class CrestUncertaintyNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<String> { "SMT_OUM.vssx" };
        protected override Type MappingListType => null;
        public override string ModelName => "Orthogonal Uncertainty Modeling";

        public CrestUncertaintyNetwork(Application pApplication) : base(pApplication)
        {
        }
        

    }
}