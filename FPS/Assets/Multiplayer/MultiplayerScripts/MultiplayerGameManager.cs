using Mirror;
using UnityEngine;
using Crazy.Multiplayer.MultiplayerScripts;


public class MultiplayerGameManager : NetworkBehaviour
{
    [Header("Enemy spawning Settings:")]
    [SerializeField] GameObject Enemy;
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] float timeToSpawn = 10f;
    float timeToNextSpawn;
    [SerializeField] int EnemysAmount = 2;
    bool isRunning = false;

    [Header("Other thinks:")]
    [SerializeField] GameObject startCamera;

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        

        Application.targetFrameRate = 60;
        timeToNextSpawn = timeToSpawn;
        isRunning = true;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        startCamera.SetActive(false);
    }

    [ServerCallback]
    void Update()
    {
        if (!isRunning) { return; }
        if (GameObject.FindGameObjectsWithTag("Player").Length==0)
        {
            return;
        }


        if (timeToNextSpawn <= 0)
        {
            for (int i = 0; i < EnemysAmount; i++)
            {
                Instantiate(Enemy, SpawnPoints[Random.Range(0, SpawnPoints.Length - 1)].position, Quaternion.identity);
            }
            timeToSpawn += timeToSpawn / 2;
            timeToNextSpawn = timeToSpawn;
            EnemysAmount++;
        }

        timeToNextSpawn -= Time.deltaTime;
    }
}
