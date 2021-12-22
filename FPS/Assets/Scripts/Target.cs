using System.Collections;
using UnityEngine;


public class Target : MonoBehaviour
{
    public float health = 50f;

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
}