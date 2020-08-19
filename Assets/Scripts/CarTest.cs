using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTest : MonoBehaviour
{
    [SerializeField]
    private List<AxleInfo> _axleInfos = null;
    [SerializeField]
    private float _maxMotorTorque = 800;
    [SerializeField]
    private float _maxSteeringAngle = 30;

    private void FixedUpdate()
    {      
        float motor = _maxMotorTorque * Input.GetAxis("Vertical");
        float steering = _maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in _axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }


            //ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            //ApplyLocalPositionToVisuals(axleInfo.rightWheel);

        }
    }
}
