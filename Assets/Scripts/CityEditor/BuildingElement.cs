using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BuildingElement : MonoBehaviour
{    
    [SerializeField]
    private GroundManager _groundManager = null;
    [SerializeField]
    private float _tileMaxRayCastDistance = 10f;
    [SerializeField]
    private float _tileSnapDistance = 0.2f;

    private Vector3 _screenPoint = Vector3.zero;
    private Vector3 _offset = Vector3.zero;
    private Rigidbody _rb = null;
    private bool _isAcnhored = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Input.GetMouseButton(0);

        if (!_isAcnhored)
        {
            foreach (Vector3 tileCenter in _groundManager.TileCenterPoints)
            {
                Ray ray = new Ray(transform.position, tileCenter - transform.position);

                if (Physics.Raycast(ray, out RaycastHit hitInfo, _tileMaxRayCastDistance))
                {
                    if (hitInfo.distance <= _tileSnapDistance)
                    {
                        transform.position = tileCenter;
                        _isAcnhored = true;
                        _rb.velocity = Vector3.zero;
                    }
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (!_isAcnhored)
        {
            _screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            _offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
        }
    }

    private void OnMouseDrag()
    {
        if (!_isAcnhored)
        {
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + _offset;

            transform.position = cursorPosition;
        }       
    }
}
