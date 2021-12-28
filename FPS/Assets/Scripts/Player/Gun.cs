using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public enum GunType
    {
        Normal,
        Bomb,
        Medicine
    }
    public GunType type;
    public Camera fpsCam;
    //Weapon normal
    [HideInInspector] public float damage = 10f;
    [HideInInspector] public float range = 40f;
    [HideInInspector] public float ammoWep = 0.25f;
    [HideInInspector] public GameObject particles;

    //Heal capsule
    [HideInInspector] public float Heal;
    [HideInInspector] public Animator animator;
    Quaternion MedRot;

    //Granade
    [HideInInspector]public GameObject Spawn;

    //public float fireRate = 15f;
    //float WaitTime = 0;

    
    //public Transform pPoint;
    //public Transform Player;

    //public ParticleSystem particleS;

    //bool Shooted = false;
    //bool set = false;
    // Update is called once per frame
    void Update()
    {
        //if (Shooted&&set)
        //{
        //    set = true;
        //    WaitTime += 10 + 1 / fireRate;
        //}
        //if (WaitTime <= 0)
        //{
        //    Shooted = false;
        //    set = false;
        //    WaitTime = 0;
        //}
        //else
        //{
        //    WaitTime -= Time.deltaTime;
        //}
    }
    public void Shoot()
    {
        switch (type)
        {
            case GunType.Normal:
                if (/*GameManager.instance.ammo<=0f&&*/GameManager.instance.ammo - ammoWep >= 0f)
                {



                    GameManager.instance.ammo -= ammoWep;
                    StartCoroutine(Particle());
                    //Shooted = true;

                    //particleS.gameObject.SetActive(true);
                    //particleS.Play();
                    //Instantiate(particles, pPoint.position, Player.localRotation);

                    RaycastHit hit;
                    if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                    {
                        Target target = hit.transform.GetComponent<Target>();
                        if (target != null)
                        {
                            target.TakeDamage(damage);
                            Debug.Log(hit.transform.name);
                        }
                    }

                }
                break;
            case GunType.Bomb:
                Debug.Log("Bomb!");
                break;
            case GunType.Medicine:
                GameManager.instance.Heal(Heal);
                if (animator!=null) animator.SetTrigger("Heal");
                break;
        }
    }
    private void Start()
    {
        MedRot = gameObject.transform.rotation;
    }
    private void OnEnable()
    {
        if (type == GunType.Medicine)
        {
            gameObject.transform.rotation = MedRot;
        }
    }
    IEnumerator Particle()
    {
        int i = 0;
        particles.SetActive(true);
        if (i==0)
        {
            i++;
            yield return new WaitForSeconds(0.01f);
        }
            particles.SetActive(false);
    }
}
