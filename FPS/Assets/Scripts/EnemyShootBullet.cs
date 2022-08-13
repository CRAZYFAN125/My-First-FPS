using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootBullet : MonoBehaviour
{
    bool atacking = false;
    [SerializeField] float atackHeight = 20;
    Rigidbody rb;
    Material mat;
    [SerializeField] float speed = 2f;
    Transform player;

    private void Start()
    {
        rb=GetComponent<Rigidbody>();
        mat = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = mat;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x<1)
        {
            return;
        }

        
        if (transform.position.y > atackHeight)
        {
            atacking = true;
            mat.SetFloat("_FresnelPower", 0);
            mat.SetFloat("_Moment", 1);
        }
        
        if (!atacking)
        {
            rb.MovePosition(transform.position+(transform.forward*Time.deltaTime*speed));
        }

        if (atacking)
        {
            Vector3 dir = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 2f).eulerAngles;
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

            rb.MovePosition(transform.position+(dir*Time.deltaTime*(speed/4)));
        }

        if (Vector3.Distance(player.position,transform.position)<.5f)
        {
            GameManager.instance.Damage(.3f);
            Destroy(gameObject);
        }
    }
}
