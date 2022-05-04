using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMulti : MonoBehaviour
{
    public static Dictionary<ushort, PlayerMulti> list = new Dictionary<ushort, PlayerMulti>();

    public ushort Id { get; private set; }
    public bool IsLocal { get; private set; }
    public string username;

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        PlayerMulti player;
        if (id == NetworkManager.Singleton.Client.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<PlayerMulti>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<PlayerMulti>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";

        list.Add(id, player);
    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(),message.GetString(),message.GetVector3());
    }
}
