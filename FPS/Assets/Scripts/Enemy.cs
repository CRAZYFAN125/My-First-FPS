using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    private Transform player;
    public NavMeshAgent agent;
    public float Force=0.2f;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    float T = 0;
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

        if (T>0)
        {
            T -= Time.deltaTime;
        }
        else
        {
            T = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player"&&T==0)
        {
            GameManager.instance.Damage(Force);
            T = 5f;
        }
    }
}