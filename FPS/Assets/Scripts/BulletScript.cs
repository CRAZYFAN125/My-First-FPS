using System.Collections;
using UnityEngine;


public class BulletScript : MonoBehaviour
{
    private Transform target;
    public float speed = 20f;
    public float DamegeForEnemy = 1.5f;
    public float DamageForPlayer = .05f;

    public void Seek(Transform _target)
    {
        target = _target;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position- transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(DamegeForEnemy);
            Destroy(gameObject);
            return;
        }
        else if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.Damage(DamageForPlayer);
            Destroy(gameObject);
            return;
        }
    }
}
