using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class SortingOrderController : MonoBehaviour
{
    #region<Variables>
    private Transform playerPos;
    private Transform monsterPos;

    private float frontZ = -1;
    private float backZ = -3;
    private float midZ = -2;

    private float playerY;
    private float monsterY;
    private float treeY;

    private float treeYOffset = 1.44f;

    #endregion

    #region<CustomTrigger>
    public CustomTrigger tree;

    private void Awake()
    {
        tree.EnteredTrigger += OnTreeTriggerEnter;
        tree.ExitedTrigger += OnTreeTriggerExited;
    }

    #endregion

    private void Start()
    {
        
    }

    private void Update()
    {
        treeY = transform.position.y + treeYOffset;

        switch ((monsterPos != null, playerPos != null))
        {
            case (true, false): // Monster is not null, Player is null
                Debug.Log("MonsterOnly");
                monsterY = monsterPos.position.y;


                if (monsterY > treeY)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, midZ);
                    monsterPos.position = new Vector3(monsterPos.position.x, monsterPos.position.y, backZ);
                    

                }
                // Condition 4: Both below the tree - Tree in front of both
                else if (monsterY <= treeY)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, midZ);
                    monsterPos.position = new Vector3(monsterPos.position.x, monsterPos.position.y, frontZ);

                }
                break;

            case (false, true): // Monster is null, Player is not null

                playerY = playerPos.position.y;
                Debug.Log("player is not null, monster is null");

                if (playerY > treeY)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, midZ);
                    playerPos.position = new Vector3(playerPos.position.x, playerPos.position.y, frontZ);
                    Debug.Log("player forntz");

                }
                // Condition 4: Both below the tree - Tree in front of both
                else if (playerY <= treeY)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, midZ);
                    playerPos.position = new Vector3(playerPos.position.x, playerPos.position.y, backZ);
                    Debug.Log("player backz");

                }
                break;

            case (true, true): // Both Monster and Player are not null
                monsterY = monsterPos.position.y;
                playerY = playerPos.position.y;
                if (playerY > treeY && monsterY <= treeY)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, midZ);
                    playerPos.position = new Vector3(playerPos.position.x, playerPos.position.y, frontZ);
                    monsterPos.position = new Vector3(monsterPos.position.x, monsterPos.position.y, backZ);

                }
                // Condition 2: Monster is above the tree, player is below
                else if (monsterY > treeY && playerY <= treeY)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, midZ);
                    playerPos.position = new Vector3(playerPos.position.x, playerPos.position.y, backZ);
                    monsterPos.position = new Vector3(monsterPos.position.x, monsterPos.position.y, frontZ);

                }

                break;

            case (false, false): // Both Monster and Player are null
                Debug.Log("Neither Monster nor Player exists.");
                break;
        }

        
        
       
    }

    #region<OnTriggerEnter/Exit>
    private void OnTreeTriggerEnter(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerPos == null) 
        {
            playerPos = collision.transform;
            Debug.Log("Player Position Updated"+playerPos.position);
        }

        if (collision.CompareTag("Enemy") && monsterPos == null)
        {
            monsterPos = collision.transform;
            Debug.Log("Monster Position Updated"+monsterPos.position);
        }
    }

    private void OnTreeTriggerExited(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            monsterPos = null;

        }

        if (collision.CompareTag("Enemy"))
        {
            playerPos = null;
        }
    }
    #endregion
}
