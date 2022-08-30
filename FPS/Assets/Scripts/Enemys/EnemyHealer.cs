using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyHealer : MonoBehaviour
{
    [SerializeField] VisualEffect vfx;
    [SerializeField] float Healing = 15f;
    [SerializeField] float HealingCooldown = 15f;
    float HealingCooldownRemaining;


    private void Start()
    {
        HealingCooldownRemaining = HealingCooldown;
    }


    void FixedUpdate()
    {
        if (HealingCooldownRemaining<=0)
        {
            vfx.SendEvent("OnHeal");
            HealingCooldownRemaining = HealingCooldown;

            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var item in enemys)
            {
                item.GetComponent<Target>().Heal(Healing);
            }
        }

        HealingCooldownRemaining -= Time.fixedDeltaTime;
    }
}
