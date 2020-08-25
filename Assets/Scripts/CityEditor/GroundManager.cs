using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    private GameObject[] _groundTiles = null;
    private Vector3[] _tileCenterPoints = null;

    public Vector3[] TileCenterPoints
    {
        get
        {
            return _tileCenterPoints;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _groundTiles = GameObject.FindGameObjectsWithTag("GroundTile");
        _tileCenterPoints = new Vector3[_groundTiles.Length];

        for(int i = 0; i < _groundTiles.Length; i++)
        {
            _tileCenterPoints[i] = _groundTiles[i].GetComponent<MeshRenderer>().bounds.center;
        }
    }
}
