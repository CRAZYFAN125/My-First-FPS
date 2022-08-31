using System.Collections;
using UnityEngine;
using Mirror;

namespace Crazy.Multiplayer.MultiplayerScripts
{
    public class MultiplayerTarget : NetworkBehaviour
    {
        [Command]
        public void TakeDamage(float damage,GameObject target,PlayerMulti shootingPlayer)
        {
            bool killed;
            switch (target.tag)
            {
                case "Player":
                    killed=target.GetComponent<PlayerMulti>().GetDamage(damage);
                    if (killed)
                    {
                        shootingPlayer.PlayersKilled++;
                    }
                    print($"{target.name} was atacked");
                    break;
                case "Enemy":
                    killed=target.GetComponent<EnemyMulti>().Hurt(damage);
                    if (killed)
                    {
                        shootingPlayer.EnemyKilled++;
                    }
                    print($"{target.name} was atacked");
                    break;
                default:
                    print("Error");
                    break;
            }
        }

    }
}