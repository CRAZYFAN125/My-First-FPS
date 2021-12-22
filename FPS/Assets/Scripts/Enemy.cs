using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    private Transform player;
    public NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.SetDestination(player.position);
        if (gameObject.transform.position.x - player.position.x<=10&& gameObject.transform.position.z - player.position.z <= 10)
        {
            agent.speed = 2.5f;
        }
        else
        {
            agent.speed = 1;
        }
    }
}