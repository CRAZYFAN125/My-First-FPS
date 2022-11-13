using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    float time;
    void Update()
    {
        if (time>=5)
        {
            Destroy(gameObject);
        }

        time += Time.deltaTime;
    }
}
