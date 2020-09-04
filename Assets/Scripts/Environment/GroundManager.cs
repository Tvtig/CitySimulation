using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _groundTile = null;
    [SerializeField]
    private bool _instantiateGroundTiles = false;

    private int _groundScale = 0;
    private int _groundTileScale = 0;
    private Vector3 _groundTileStartingPosition;
    private List<Vector3> _groundTilePositions = null;
    private Plane _groundPlane;

    public List<Vector3> GroundTilePositions
    {
        get
        {
            return _groundTilePositions;
        }
    }

    public Plane GroundPlane
    {
        get
        {
            return _groundPlane;
        }
    }

    void Start()
    {
        _groundPlane = new Plane(Vector3.up, Vector3.zero);

        _groundTilePositions = new List<Vector3>();
        _groundTileScale = (int)_groundTile.transform.localScale.x;
        _groundScale = (int)transform.localScale.x;

        //Determine the number of ground tiles we can place based on the size of the ground tile object
        int numGroundTiles = _groundScale / _groundTileScale;


        //Based on the formula: StartingXPoint = -((GroundScale * 5) - (GroundTileScale/2)); StartingZPoint = -StartingXPoint;
        int tileStartingX = -((_groundScale * 5) - (_groundTileScale / 2));
        int tileStartingZ = -tileStartingX;
        _groundTileStartingPosition = new Vector3(tileStartingX, _groundTile.transform.localPosition.y, tileStartingZ);

        Vector3 groundTileCurrentPosition = _groundTileStartingPosition;

        //Instantiate the rest of the ground tiles
        for (int z = 0; z < _groundScale; z++)
        {
            for (int x = 0; x < _groundScale; x++)
            {                
                //Add to the ground tile position list
                _groundTilePositions.Add(groundTileCurrentPosition);

                //No need to instantate the ground tiles unless you wanna see the gizmos in the inspector, we just need their positions so that we can snap objects to them based on ray casts
                if (_instantiateGroundTiles)
                {
                    Instantiate(_groundTile, groundTileCurrentPosition, Quaternion.identity);
                }

                groundTileCurrentPosition = new Vector3(groundTileCurrentPosition.x + _groundTileScale, groundTileCurrentPosition.y, groundTileCurrentPosition.z);
            }

            groundTileCurrentPosition = new Vector3(_groundTileStartingPosition.x, groundTileCurrentPosition.y, groundTileCurrentPosition.z - _groundTileScale);
        }
    }

    /// <summary>
    /// Get the closes tile position based on the input
    /// </summary>
    /// <param name="inputPosition"></param>
    /// <returns></returns>
    public Vector3 GetClosestTilePosition(Vector3 inputPosition)
    {
        Vector3 closestTile = Vector3.zero;
        float shortestDistance = 0f;
        bool isFirstLoop = true;
        float currentDistance;

        foreach (Vector3 tilePosition in _groundTilePositions)
        {
            if (isFirstLoop)
            {
                shortestDistance = Vector3.Distance(inputPosition, tilePosition);
                closestTile = tilePosition;
                isFirstLoop = false;
            }
            else
            {
                currentDistance = Vector3.Distance(inputPosition, tilePosition);
                if (currentDistance < shortestDistance)
                {
                    shortestDistance = currentDistance;
                    closestTile = tilePosition;
                }
            }
        }

        return closestTile;
    }
}
