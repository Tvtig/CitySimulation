using Assets.Scripts;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class DriverController : Agent
{
    private const float POSITION_CHANGE_WATCHDOG = 120;

    [SerializeField]
    private List<AxleInfo> _axleInfos = null;
    [SerializeField]
    private float _maxMotorTorque = 800;
    [SerializeField]
    private float _maxSteeringAngle = 30;
    [SerializeField]
    private GameObject _trainingArea = null;

    public float LocalVelocity;
    public float Horizontal;
    public float Vertical;

    private Rigidbody _rb;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private List<GameObject> _deactivatedRewards;
    private int _numRewardCollected;

    public override void Initialize()
    {
        _rb = transform.GetComponent<Rigidbody>();
        _deactivatedRewards = new List<GameObject>();
    }    

    private void Start()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;        
    }

    public override void OnEpisodeBegin()
    {
        StartCoroutine(PositionChangeWathdog());
        _rb.velocity = Vector3.zero;
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
        ResetCheckpoints();
        _deactivatedRewards.Clear();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var localVelocity = transform.InverseTransformDirection(_rb.velocity);
        sensor.AddObservation(localVelocity.x);
        sensor.AddObservation(localVelocity.z);
        sensor.AddObservation(_rb.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            SetReward(-10f);
            EndEpisode();
        }
    }

    private void Update()
    {
        //Manually End Episode
        if (Input.GetKeyDown(KeyCode.R))
        {
            EndEpisode();
        }

        LocalVelocity = transform.InverseTransformDirection(_rb.velocity).z;

        //If its going backwards
        if (LocalVelocity < 0)
        {
            SetReward(-1f);
        }

        //If its colliding with the ground
        foreach(AxleInfo wheel in _axleInfos)
        {
            if(wheel.leftWheel.GetGroundHit(out WheelHit hit))
            {               
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    SetReward(-1f);
                }
            }

        }
    }

    private void ResetCheckpoints()
    {
        foreach (GameObject reward in _deactivatedRewards)
        {
            reward.SetActive(true);
        }
    }

    private IEnumerator PositionChangeWathdog()
    {
        Vector3 positionBefore = transform.position;

        yield return new WaitForSeconds(POSITION_CHANGE_WATCHDOG);

        float distance = Vector3.Distance(positionBefore, transform.position);
        if (distance < 0.5f)
        {

            EndEpisode();
        }
        else
        {
            StartCoroutine(PositionChangeWathdog());
        }
        
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
        Move(actionsOut);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reward"))
        {
            _deactivatedRewards.Add(other.gameObject);
            other.gameObject.SetActive(false);
            SetReward(1f);
            _numRewardCollected++;
        }

        if((_numRewardCollected % 5) == 0)
        {
            ResetCheckpoints();
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {              
        Move(vectorAction);
    }

    private void Move(float[] actions)
    {
        float motor = _maxMotorTorque * actions[0];
        float steering = _maxSteeringAngle * actions[1];

        Vertical = motor;
        Horizontal = steering;

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

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }    
}
