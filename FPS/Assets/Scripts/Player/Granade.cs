using System.Collections;
using UnityEngine;


public class Granade : MonoBehaviour
{
    public float AmmoToTake = 0.85f;
    
    public float delay = 1.5f;



    float radius = 3f;

    public float ThrowForce =20f;
    public GameObject explodionEffect;

    float countdown;
    bool hasExploaded = false;
    
    public void GranadeThrow(Vector3 _force)
    {
        if (GameManager.instance.ammo - AmmoToTake <= 0f)
        {
            Destroy(gameObject);
            return;
        }
        GameManager.instance.ammo -= AmmoToTake;
        gameObject.GetComponent<Rigidbody>().AddForce(_force*ThrowForce, ForceMode.VelocityChange);
    }


    // Use this for initialization
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown-=Time.deltaTime;
        if (countdown <= 0 && !hasExploaded)
        {
            hasExploaded = true;
            Explode();
        }
    }
    void Explode()
    {
        if (explodionEffect != null) { 
            
            GameObject g =Instantiate(explodionEffect, transform.position, transform.rotation);
            g.transform.localScale = new Vector3(radius, radius, radius);
            Destroy(g,0.5f);
        
        }

        Collider[] colliders= Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            Target target = collider.GetComponent<Target>();
            if (target !=null)
            {
                target.TakeDamage(target.MaxHealth / 3);
            }
            else if (collider.tag=="Player")
            {
                GameManager.instance.Damage(0.25f);
            }
        }

        Destroy(gameObject);
    }
}