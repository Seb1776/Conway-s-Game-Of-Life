using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive;
    public float rayLength;
    public float rayOffset;
    public Color originalColor;
    public bool[] aliveNeighbors = new bool[8];
    public SpriteRenderer[] renderers;

    CellSpawn cellSpawn;
    BoxCollider2D box2d;

    void Start()
    {
        box2d = GetComponent<BoxCollider2D>();
        cellSpawn = GameObject.FindGameObjectWithTag("Spawn").GetComponent<CellSpawn>();

        CheckForAlive();
    }

    void Update()
    {
        AddNeighbors();
    }

    public void KillCell()
    {   
        if (isAlive)
        {
            isAlive = false;
            CheckForAlive();
        }
    }

    public void ReviveCell()
    {   
        if (!isAlive)
        {
            isAlive = true;
            CheckForAlive();
        }
    }

    public void AddNeighbors()
    {
        RaycastHit2D hitL = Physics2D.Raycast(new Vector2(box2d.bounds.min.x + rayOffset, box2d.bounds.center.y), Vector2.left, rayLength);
        RaycastHit2D hitR = Physics2D.Raycast(new Vector2(box2d.bounds.max.x - rayOffset, box2d.bounds.center.y), Vector2.right, rayLength);
        RaycastHit2D hitU = Physics2D.Raycast(new Vector2(box2d.bounds.center.x, box2d.bounds.max.y - rayOffset), Vector2.up, rayLength);
        RaycastHit2D hitD = Physics2D.Raycast(new Vector2(box2d.bounds.center.x, box2d.bounds.min.y + rayOffset), Vector2.down, rayLength);
        RaycastHit2D hitRU = Physics2D.Raycast(new Vector2(box2d.bounds.max.x - rayOffset, box2d.bounds.max.y - rayOffset), (Vector2.right + Vector2.up), rayLength);
        RaycastHit2D hitLU = Physics2D.Raycast(new Vector2(box2d.bounds.min.x + rayOffset, box2d.bounds.max.y - rayOffset), (Vector2.left + Vector2.up), rayLength);
        RaycastHit2D hitRD = Physics2D.Raycast(new Vector2(box2d.bounds.max.x - rayOffset, box2d.bounds.min.y - rayOffset), (Vector2.right + Vector2.down), rayLength);
        RaycastHit2D hitLD = Physics2D.Raycast(new Vector2(box2d.bounds.min.x + rayOffset, box2d.bounds.min.y - rayOffset), (Vector2.left + Vector2.down), rayLength);

        if (hitL || hitR || hitU || hitD || hitRU || hitLU || hitRD || hitLD)
        {   
            if (hitL.collider != null)
                if (hitL.transform.GetComponent<Cell>().isAlive)
                    aliveNeighbors[0] = true;
                else
                    aliveNeighbors[0] = false;
            
            if (hitR.collider != null)
                if (hitR.transform.GetComponent<Cell>().isAlive)
                    aliveNeighbors[1] = true;
                else
                    aliveNeighbors[1] = false;
            
            if (hitU.collider != null)
                if (hitU.transform.GetComponent<Cell>().isAlive)
                    aliveNeighbors[2] = true;
                else
                    aliveNeighbors[2] = false;
            
            if (hitD.collider != null)
                if (hitD.transform.GetComponent<Cell>().isAlive)
                    aliveNeighbors[3] = true;
                else
                    aliveNeighbors[3] = false;

            if (hitRU.collider != null)
                if (hitRU.transform.GetComponent<Cell>().isAlive)
                    aliveNeighbors[4] = true;
                else
                    aliveNeighbors[4] = false;
            
            if (hitLU.collider != null)
                if (hitLU.transform.GetComponent<Cell>().isAlive)
                    aliveNeighbors[5] = true;
                else
                    aliveNeighbors[5] = false;
            
            if (hitRD.collider != null)
                if (hitRD.transform.GetComponent<Cell>().isAlive)
                    aliveNeighbors[6] = true;
                else
                    aliveNeighbors[6] = false;
            
            if (hitLD.collider != null)
                if (hitLD.transform.GetComponent<Cell>().isAlive)
                    aliveNeighbors[7] = true;
                else
                    aliveNeighbors[7] = false;
        }
    }

    void CheckForAlive()
    {
        if (!isAlive)
            renderers[0].color = renderers[1].color = Color.black;

        else
        {
            renderers[0].color = Color.white;
            renderers[1].color = originalColor;
        }
    }

    public bool SetState(bool random, float chanceOfLive, bool _isAlive = false)
    {
        if (random)
            if (Random.value >= chanceOfLive)
                return true;
        else
            return _isAlive;
        
        return false;
    }

    void OnMouseOver()
    {   
        if (Input.GetMouseButtonDown(0)) 
            if (isAlive)
                KillCell();
            else
                ReviveCell();
    }

    void OnDrawGizmos() 
    {
        box2d = GetComponent<BoxCollider2D>();

        Gizmos.color = Color.red;

        Gizmos.DrawRay(new Vector2(box2d.bounds.min.x + rayOffset, box2d.bounds.center.y), Vector2.left * rayLength);
        Gizmos.DrawRay(new Vector2(box2d.bounds.max.x - rayOffset, box2d.bounds.center.y), Vector2.right * rayLength);
        Gizmos.DrawRay(new Vector2(box2d.bounds.center.x, box2d.bounds.max.y - rayOffset), Vector2.up * rayLength);
        Gizmos.DrawRay(new Vector2(box2d.bounds.center.x, box2d.bounds.min.y + rayOffset), Vector2.down * rayLength);
        Gizmos.DrawRay(new Vector2(box2d.bounds.max.x - rayOffset, box2d.bounds.max.y - rayOffset), (Vector2.right + Vector2.up) * rayLength);
        Gizmos.DrawRay(new Vector2(box2d.bounds.min.x + rayOffset, box2d.bounds.max.y - rayOffset), (Vector2.left + Vector2.up) * rayLength);
        Gizmos.DrawRay(new Vector2(box2d.bounds.max.x - rayOffset, box2d.bounds.min.y - rayOffset), (Vector2.right + Vector2.down) * rayLength);
        Gizmos.DrawRay(new Vector2(box2d.bounds.min.x + rayOffset, box2d.bounds.min.y - rayOffset), (Vector2.left + Vector2.down) * rayLength);
    }
}
