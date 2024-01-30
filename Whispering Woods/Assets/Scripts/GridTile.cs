using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Transform gridCenter;
    public Vector3 cellInWorldPos;

    private SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        Grid grid = transform.parent.gameObject.transform.parent.GetComponent<Grid>();
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        cellInWorldPos = grid.GetCellCenterWorld(cellPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
