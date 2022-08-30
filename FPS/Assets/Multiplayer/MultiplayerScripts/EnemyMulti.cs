using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMulti : NetworkBehaviour
{
    [SyncVar] public float Health;
    float MaxHealth;
    Transform player;
    public Rigidbody rb;

    [Server]
    private void Start()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        float smallestDistance = int.MaxValue;
        if (Players.Length>1)
        {
            foreach (var item in Players)
            {
                if (Vector3.Distance(item.transform.position, transform.position) < smallestDistance)
                {
                    smallestDistance = Vector3.Distance(item.transform.position, transform.position);
                    player = item.transform;
                }
            }
        }
        else
        {
            player = Players[0].transform;
        }
    }

    [Server]
    private void Update()
    {
        rb.MovePosition(transform.position+(player.position-transform.position) * (Time.deltaTime * .5f));
    }

}
