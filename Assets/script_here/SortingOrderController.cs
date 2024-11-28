using System.Collections;
using System.Collections.Generic;
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



        if (playerPos != null)
        {
            playerY = playerPos.position.y;


            if (playerY > treeY)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, backZ);

            }
            // Condition 4: Both below the tree - Tree in front of both
            else if (playerY <= treeY)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, frontZ);

            }
        }

        if (monsterPos != null)
        {
            monsterY = monsterPos.position.y;
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

        }



    }

    #region<OnTriggerEnter/Exit>
    private void OnTreeTriggerEnter(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerPos == null)
        {
            playerPos = collision.transform;
            //Debug.Log("Player Position Updated"+playerPos.position);
        }

        if (collision.CompareTag("Enemy") && monsterPos == null)
        {
            monsterPos = collision.transform;
            //Debug.Log("Monster Position Updated"+monsterPos.position);
        }
    }

    private void OnTreeTriggerExited(Collider2D collision)
    {
        monsterPos = null;
        playerPos = null;
    }
    #endregion
}
