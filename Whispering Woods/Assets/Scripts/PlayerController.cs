using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int curHp;
    public int maxHp;
    public float moveSpeed;

    [Header("Movement Details")]
    [SerializeField] private Tilemap tm;
    [SerializeField] private GridTile startingPos;
    public LayerMask movementLayerMask;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 facingDirection;

    private void Start()
    {

        //Vector3Int cellPos = tm.WorldToCell(startingPos.gameObject.transform.position);
        //gameObject.transform.position = tm.GetCellCenterWorld(cellPos);
        //transform.position = startingPos.gameObject.GetComponent<GridTile>().cellInWorldPos;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        TryMovement();
    }

    private void TryMovement()
    {
        RaycastHit2D hit;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            hit = Physics2D.Raycast((Vector2)transform.position + Vector2.up, Vector3.up, 1f, movementLayerMask);
            Debug.DrawRay((Vector2)transform.position + Vector2.up, Vector3.up, Color.red, 1f);

            MovePlayer(hit);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            hit = Physics2D.Raycast((Vector2)transform.position + Vector2.down, Vector3.down, 1f, movementLayerMask);
            Debug.DrawRay((Vector2)transform.position + Vector2.down, Vector3.down, Color.red, 1f);

            MovePlayer(hit);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            hit = Physics2D.Raycast((Vector2)transform.position + Vector2.left, Vector3.left, 1f, movementLayerMask);
            Debug.DrawRay((Vector2)transform.position + Vector2.left, Vector3.left, Color.red, 1f);

            MovePlayer(hit);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            hit = Physics2D.Raycast((Vector2)transform.position + Vector2.right, Vector3.right, 1f, movementLayerMask);
            Debug.DrawRay((Vector2)transform.position + Vector2.right, Vector3.right, Color.red, 1f);

            MovePlayer(hit);
        }
    }

    private void MovePlayer(RaycastHit2D hit)
    {
        if (hit.collider == null)
            return;

        //gameObject.transform.position = hit.collider.gameObject.GetComponent<GridTile>().gridCenter.position;
        gameObject.transform.position = hit.collider.gameObject.GetComponent<GridTile>().cellInWorldPos;

        Debug.Log(hit.collider.gameObject.transform.position);

        //gameObject.transform.position = tm.GetCellCenterWorld(new Vector3Int((int)hit.collider.gameObject.transform.position.x, (int)hit.collider.gameObject.transform.position.y, 0));

        

        //hit.collider.gameObject.SetActive(false);
        //hit.collider.gameObject.SetActive(true);
    }

}
