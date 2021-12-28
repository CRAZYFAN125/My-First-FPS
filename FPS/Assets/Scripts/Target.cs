using System.Collections;
using UnityEngine;


public class Target : MonoBehaviour
{
    public float health = 50f;
    float T = 0;
    public Enemy enemy;


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health<=0f)
        {
            Die();
        }
    }
    void Die()
    {
        GameManager.instance.Reload();
        Destroy(gameObject, 0.25f);
    }
    private void FixedUpdate()
    {

        if (T > 0)
        {
            T -= Time.fixedDeltaTime;
        }
        else
        {
            T = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (enemy == null)
        {
            return;
        }
        if (other.tag == "Player" && T == 0)
        {
            GameManager.instance.Damage(enemy.Force);
            T = 5f;
        }
    }
}