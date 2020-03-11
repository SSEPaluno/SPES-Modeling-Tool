using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CREST_FunctionNetwork
{
    public class ExtendedFunctionNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<String> { "SMT_eFN.vssx", "SMT_IA.vssx", "SMT_DCM.vssx"};
        protected override Type MappingListType => null;
        public override string ModelName => "Extended Function Network";
        public ExtendedFunctionNetwork(Application pApplication) : base(pApplication)
        {
        }
        
        //public override string ToString()
        //{
        //    return "extended Function Network";
        //}
    }
}