using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    private Transform player;
    public NavMeshAgent agent;
    public float Force = 0.2f;
    Material material;
    Target target;


    [Header("If is Shooting")]
    [SerializeField] bool isShooting = false;
    [SerializeField] GameObject VFX_Shoot;
    [SerializeField] Transform[] ShootingPoints;
    [SerializeField] float TimeForLoadShoot = .5f;
    Vector3[] point;
    int selected = 0;
    public bool canMove { get; set; } = true;
    Rigidbody rb;
    float xSize=0, zSize = 0;
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
        GetComponent<MeshRenderer>().material = material;
        if (isShooting)
        {
            VFX_Shoot.gameObject.SetActive(false);
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "SampleScene")
            {
                point = new Vector3[ShootingPoints.Length];
                for (int i = 0; i < ShootingPoints.Length; i++)
                {
                    point[i] = ShootingPoints[i].position;
                }
            }
            else
            {
                MapGenerator mg = FindObjectOfType<MapGenerator>();
                point = mg.GetFirstAndLastPoint();
                xSize = mg.xSize;
                zSize = mg.zSize;
            }

            rb = GetComponent<Rigidbody>();

        }
    }


    void FixedUpdate()
    {
        if (!MapGenerator.Ready)
        {
            return;
        }
        if (!isShooting)
        {
            agent.SetDestination(player.position);
            if (gameObject.transform.position.x - player.position.x <= 10 && gameObject.transform.position.z - player.position.z <= 10)
            {
                agent.speed = 2.5f;
            }
            else
            {
                agent.speed = 1;
            }
        }
        else
        {
            if (canMove)
            {
                if (transform.position.x- point[selected].x < 2 && transform.position.z - point[selected].z<2)
                {
                    if (Vector3.Distance(transform.position, player.position) < 5)
                    {
                        SelectedAdd();
                    }
                    else
                    {
                        StartCoroutine(Shooting());
                    }
                }
                else
                {
                    Vector3 dir = point[selected] - new Vector3(xSize / 3, 0, zSize / 3) - transform.position;
                    rb.MovePosition(transform.position + (dir * Time.fixedDeltaTime * 1.2f));
                    print(transform.position + dir);
                }
            }
        }
        material.SetFloat("_life", target.health);

    }

    void SelectedAdd()
    {
        if (point.Length - 1 != selected)
        {
            selected++;
        }
        else
        {
            selected = 0;
        }
    }

    IEnumerator Shooting()
    {
        canMove = false;
        float procent = 0;
        VFX_Shoot.gameObject.SetActive(true);
        VFXController vfx = VFX_Shoot.GetComponent<VFXController>();
        vfx.SourceScript = this;

        while (!canMove)
        {
            Vector3 dir = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, /*Time.deltaTime * */2f).eulerAngles;
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

            vfx.procent = procent;
            procent += TimeForLoadShoot;
            yield return new WaitForSeconds(1);
        }

        SelectedAdd();
        Destroy(gameObject,.5f);
    }



}