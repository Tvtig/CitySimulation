using Assets.Scripts.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCurve : BuildingElement
{
    public override BuildingElementType BuildingElementType
    {
        get => BuildingElementType.RoadCurve;
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
