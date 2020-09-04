using System;
using Assets.Scripts.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    private BuildingElementFactory _buildingElementFactory;

    private void Start()
    {
        _buildingElementFactory = FindObjectOfType<BuildingElementFactory>();
    }

    public void CreateBuildingElement(int buildingElementTypeInteger)
    {
        BuildingElementType buildingElementType;

        try
        {
            buildingElementType = (BuildingElementType)buildingElementTypeInteger;
            GameObject elementToSpawn = _buildingElementFactory.InstantiateBuildingElement(buildingElementType);
            var buildingElement = elementToSpawn.GetComponent<BuildingElement>();
            buildingElement.IsInContext = true;
        }
        catch(Exception ex)
        {
            Debug.LogError(string.Format("An error occurred whilst creating building element error [{0}]", ex.Message));
        }
    }
   
}
