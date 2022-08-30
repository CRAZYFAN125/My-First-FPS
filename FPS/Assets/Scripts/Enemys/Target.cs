using System.Collections;
using UnityEngine;


public class Target : MonoBehaviour
{
    public float health = 50f;
    float T = 0;
    public bool IsScore = true;
    public Enemy enemy;
    public float MaxHealth { get; private set; }

    private void Start()
    {
        MaxHealth = health;
    }

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
        if (IsScore)
        {
            GameManager.instance.Killed += 1;
        }
        Destroy(gameObject, 0.25f);
    }
    private void FixedUpdate()
    {
        if (transform.position.y<-10)
        {
            Die();
        }
        if (T > 0)
        {
            T -= Time.fixedDeltaTime;
        }
        else
        {
            T = 0;
        }
    }

    /// <summary>
    /// Heal object with 'Target' component
    /// </summary>
    /// <param name="amount">Amount of points to heal</param>
    public void Heal(float amount)
    {
        if (health+amount>MaxHealth)
        {
            health = MaxHealth;
        }
        else
        {
            health+=amount;
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