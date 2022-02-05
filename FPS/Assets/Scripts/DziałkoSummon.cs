using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DziałkoSummon : MonoBehaviour
{
    public GameObject dzialko;
    public float SpawnRate = 0.3f;
    public float SpawnDelay = 10f;
    private float timeLeft;
    private float szansa;
    private MapGenerator mapGenerator;
    [HideInInspector] public bool isSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        szansa = Random.Range(0.2f, 3f);
        timeLeft = SpawnDelay*szansa;
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawned && MapGenerator.Ready)
        {
            if (timeLeft <= 0)
            {
                isSpawned = true;
                timeLeft = (SpawnDelay * SpawnRate)*szansa;
                int C = Mathf.FloorToInt(Random.Range(1, mapGenerator.vertices.Length));
                Dzialko dz = Instantiate(dzialko, mapGenerator.vertices[C] - new Vector3(mapGenerator.xSize / 2, -1f, mapGenerator.zSize / 2), Quaternion.identity, gameObject.transform).GetComponent<Dzialko>();
                szansa = Random.Range(0.2f, 3f);
                dz.summon = this;
            }
            timeLeft-=Time.deltaTime;
        }
    }
}
