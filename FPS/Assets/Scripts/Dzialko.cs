using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dzialko : MonoBehaviour
{
    [HideInInspector] public Dzia³koSummon summon;

    private Transform target;
    [Header("Atrybuty")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float range = 15f;
    public float fireRate = .5f;
    private float fireCoundown = 0;
    [SerializeField] private int Ammo = 10;

    [Header("SetUp")]
    public Transform partToRotate;
    public float rotationSpeed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, .5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform neareastEnemy = null;


        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (shortestDistance > distanceToEnemy)
            {
                shortestDistance = distanceToEnemy;
                neareastEnemy = enemy.transform;
            }
        }
        if (neareastEnemy != null && shortestDistance <= range)
        {
            target = neareastEnemy.transform;
        }
        else
        {
            target=null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Ammo <= 0)
        {
            summon.isSpawned = false;
            Destroy(gameObject, 0.01f);
            return;
        }

        if (target==null)
            return;
        
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime*rotationSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

        if (fireCoundown <= 0f)
        {
            Shoot();
            fireCoundown = 1f / fireRate;
        }
        fireCoundown-=Time.deltaTime;
    }

    void Shoot()
    {
        GameObject g = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        g.GetComponent<BulletScript>().Seek(target);
        Ammo--;
    }
    

    private void OnDrawGizmosSelected()
    {
        if (range>0f)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
