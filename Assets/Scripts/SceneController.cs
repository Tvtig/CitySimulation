using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private Camera _sceneCamera = null;
    [SerializeField]
    private Camera _vehicleOverheadCamera = null; 

    private void Start()
    {
        _sceneCamera.enabled = true;
        _vehicleOverheadCamera.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _sceneCamera.enabled = !_sceneCamera.enabled;
            _vehicleOverheadCamera.enabled = !_vehicleOverheadCamera.enabled;                
        }       
    }
}
