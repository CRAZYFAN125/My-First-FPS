using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 40f;
    public Camera fpsCam;
    public float ammoWep = 0.25f;


    //public float fireRate = 15f;
    //float WaitTime = 0;

    public GameObject particles;
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
        
            if (GameManager.instance.ammo<=0&&GameManager.instance.ammo - ammoWep <0)
            {
                return;
            }
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
