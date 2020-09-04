using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class BuildingElementFactory : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _buildingElementTypes = null;

        private void Start()
        {
            var buildingElementFactories = FindObjectsOfType<BuildingElementFactory>();

            if(buildingElementFactories.Length > 1)
            {
                throw new Exception("Cannot have multiple building element factories in the scene");
            }

            if(_buildingElementTypes == null || _buildingElementTypes.Length == 0)
            {
                throw new Exception("No building elements specified");
            }
        }

        public GameObject InstantiateBuildingElement(BuildingElementType buildingElementType)
        {
            return Instantiate(_buildingElementTypes[(int)buildingElementType]);
        }
    }
}
