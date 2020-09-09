using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Environment
{
    class House : BuildingElement
    {
        public override BuildingElementType BuildingElementType
        {
            get => BuildingElementType.House;
        }

        public override bool CreateManyOnDrag
        {
            get => false;
        }

        public override bool CanRotate
        {
            get => true;
        }
    }
}
