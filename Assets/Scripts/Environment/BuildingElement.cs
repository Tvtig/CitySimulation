using Assets.Scripts.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class BuildingElement : MonoBehaviour
{    
    [SerializeField]
    private GroundManager _groundManager = null;

    private bool _isInContext = false;

    public bool IsInContext
    {
        get
        {
            return _isInContext;
        }
        set
        {
            _isInContext = value;
        }
    }

    public abstract BuildingElementType BuildingElementType
    {
        get;
    }

    public abstract bool CreateManyOnDrag 
    {
        get;
    }

    public abstract bool CanRotate
    {
        get;
    }

    public void Awake()
    {
        _groundManager = FindObjectOfType<GroundManager>();
    }

    private void Update()
    {
        if(_isInContext && Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _isInContext = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && _isInContext)
        {
            if (CanRotate)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 90, 0);
            }
        }

        if (_isInContext)
        {
            MoveToMouse();
        }
    }

    private void OnMouseDown()
    {
        _isInContext = !_isInContext;
    }

    private void MoveToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (_groundManager.GroundPlane.Raycast(ray, out distance))
        {
            Vector3 rayPoint = ray.GetPoint(distance);
            Vector3 closestTilePosition = _groundManager.GetClosestTilePosition(rayPoint);

            transform.position = new Vector3(closestTilePosition.x, transform.position.y, closestTilePosition.z);
        }
    }
}
