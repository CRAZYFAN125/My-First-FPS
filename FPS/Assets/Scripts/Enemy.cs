using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    private Transform player;
    public NavMeshAgent agent;
    public float Force = 0.2f;
    Material material;
    Target target;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        target = GetComponent<Target>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        material = new Material(GetComponent<MeshRenderer>().material);
        material.SetFloat("_MLife", target.MaxHealth);
        GetComponent<MeshRenderer>().material=material;
    }
    

    void FixedUpdate()
    {
        if (!MapGenerator.Ready)
        {
            return;
        }
        agent.SetDestination(player.position);
        if (gameObject.transform.position.x - player.position.x <= 10 && gameObject.transform.position.z - player.position.z <= 10)
        {
            agent.speed = 2.5f;
        }
        else
        {
            agent.speed = 1;
        }
        material.SetFloat("_life", target.health);
    }

    
}