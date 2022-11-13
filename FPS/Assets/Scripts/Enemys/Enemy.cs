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
    float xSize = 0, zSize = 0;

    [Header("If its \"Golerk\"")]
    [SerializeField] bool isGolerk = false;
    public bool IsGolerk_ { get => isGolerk; }
    Vector3 spawnPoint;

    bool isPlayerInRange = false;
    [SerializeField] float FindingRange = 15f;
    [SerializeField] Animator animator;
    [SerializeField] float gSpeedf = 3f;
    bool hide = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        target = GetComponent<Target>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (!isGolerk)
        {
            material = new Material(GetComponent<MeshRenderer>().material);
            material.SetFloat("_MLife", target.MaxHealth);
            GetComponent<MeshRenderer>().material = material;
        }
        //else
        //    material = new Material(GetComponent<MeshRenderer>().materials[1]);

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
        if (isGolerk)
        {
            spawnPoint = transform.position;
        }
    }


    void FixedUpdate()
    {
        if (!MapGenerator.Ready)
        {
            return;
        }
        if (!isGolerk)
        {
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
                    if (transform.position.x - point[selected].x < 2 && transform.position.z - point[selected].z < 2)
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
        }
        else
        {
            if (!hide)
            {
                if (Vector3.Distance(transform.position, player.position) <= FindingRange)
                {
                    isPlayerInRange = true;
                }
                else
                {
                    isPlayerInRange = false;
                }

                bool isAtacking = false;
                if (Vector3.Distance(transform.position, player.position) <= 5f)
                {
                    animator.SetBool("Atack", true);
                    isAtacking = true;
                }

                if (isPlayerInRange && !isAtacking)
                {
                    agent.SetDestination(player.position);
                    animator.SetBool("Go", true);
                    agent.speed = gSpeedf;
                }
                else
                {
                    if (!isAtacking)
                        animator.SetBool("Atack", false);
                    animator.SetBool("Go", false);
                    agent.speed = 0;
                }

            }
            else
            {
                agent.SetDestination(spawnPoint);
                animator.SetBool("Go", true);
                agent.speed = gSpeedf;
                if (Vector3.Distance(transform.position, spawnPoint) <= 5f)
                {
                    hide = false;
                    agent.SetDestination(player.position);
                    animator.SetBool("Go", false);
                }
            }
        }
        if (!isGolerk)
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
        Destroy(gameObject, .5f);
    }

    public void Atack()
    {
        hide = true;
        GameManager.instance.Damage(Force);

        animator.SetBool("Atack", false);
    }

}