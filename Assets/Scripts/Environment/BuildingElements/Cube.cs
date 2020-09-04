using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Environment
{
    class Cube : BuildingElement
    {
        public override BuildingElementType BuildingElementType 
        { 
            get => BuildingElementType.Cube; 
        }

        public override bool CreateManyOnDrag
        {
            get => true;
        }
    }
}
