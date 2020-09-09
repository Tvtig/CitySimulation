using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _panSpeed = 20f;
    [SerializeField]
    private float _scrollSpeed = 2f;
    [SerializeField]
    private float _panBorderThickness = 10f;
    [SerializeField]
    private float _xPanLimit = 200f;
    [SerializeField]
    private float _zPanLimit = 200f;
    [SerializeField]
    private float _minY = 20f;
    [SerializeField]
    private float _maxY = 200f;
    [SerializeField]
    private bool _panningEnabled = false;
    
    private float _dragSpeed;
    private Vector3 _dragOrigin = Vector3.zero;
    private bool _isDragging = false;
    private float _scrollPosition = 0f;
    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
        _dragSpeed = _panSpeed * 4;
        _scrollSpeed *= 5000;
    }

    void LateUpdate()
    {
        //Reset Camera
        if(Input.GetKey(KeyCode.C))
        {
            transform.position = _initialPosition;
        }

        if (_panningEnabled)
        {
            Vector3 position = Vector3.zero;

            if (Input.GetMouseButtonUp(2))
            {
                _isDragging = false;
            }

            if (Input.GetMouseButtonDown(2))
            {
                _dragOrigin = Input.mousePosition;
                _isDragging = true;
                return;
            }
            else if (_isDragging)
            {
                Vector3 mousePositionWithDragOffset = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);

                //Moving right
                if (mousePositionWithDragOffset.x < 0 && transform.position.x < _xPanLimit)
                {
                    position.x = -mousePositionWithDragOffset.x * _dragSpeed;
                }

                //Moving left
                if (mousePositionWithDragOffset.x > 0 && transform.position.x > -_xPanLimit)
                {
                    position.x = -mousePositionWithDragOffset.x * _dragSpeed;
                }

                if (mousePositionWithDragOffset.y < 0 && transform.position.z < _zPanLimit)
                {
                    position.z = -mousePositionWithDragOffset.y * _dragSpeed;
                }

                if (mousePositionWithDragOffset.y > 0 && transform.position.z > -_zPanLimit)
                {
                    position.z = -mousePositionWithDragOffset.y * _dragSpeed;
                }
            }
            else
            {
                position = Vector3.zero;

                if ((Input.GetKey("w") || Input.mousePosition.y >= Screen.height - _panBorderThickness) && transform.position.z < _zPanLimit)
                {
                    position.z += _panSpeed;
                }

                if ((Input.GetKey("s") || Input.mousePosition.y <= _panBorderThickness) && transform.position.z > -_zPanLimit)
                {
                    position.z -= _panSpeed;
                }

                if ((Input.GetKey("d") || Input.mousePosition.x >= Screen.width - _panBorderThickness) && transform.position.x < _xPanLimit)
                {
                    position.x += _panSpeed;
                }

                if ((Input.GetKey("a") || Input.mousePosition.x <= _panBorderThickness) && transform.position.x > -_xPanLimit)
                {
                    position.x -= _panSpeed;
                }

                float scroll = Input.GetAxis("Mouse ScrollWheel");

                if (scroll != 0)
                {
                    if (scroll > 0 && transform.position.y > _minY)
                    {
                        _scrollPosition = transform.position.y - (scroll * _scrollSpeed);
                        position.y = Mathf.Lerp(position.y, _scrollPosition, 0.05f);
                    }
                    else if (scroll < 0 && transform.position.y < _maxY)
                    {
                        _scrollPosition = transform.position.y - (scroll * _scrollSpeed);
                        position.y = Mathf.Lerp(position.y, _scrollPosition, 0.05f);
                    }
                }
            }

            transform.Translate(position * Time.deltaTime, Space.World);
        }
    }
}
