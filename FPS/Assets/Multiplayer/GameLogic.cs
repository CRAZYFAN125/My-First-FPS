using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _singleton;
    public static GameLogic Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null) _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameLogic)} istnieje, usuwanie duplikacji!");
                Destroy(value);
            }
        }
    }
    public GameObject PlayerPrefab => playerPrefab;
    public GameObject LocalPlayerPrefab => localPlayerPrefab;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject localPlayerPrefab;

    private void Awake()
    {
        Singleton = this;
    }


}