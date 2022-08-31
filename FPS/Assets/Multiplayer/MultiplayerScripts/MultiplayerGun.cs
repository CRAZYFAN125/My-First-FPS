using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Crazy.Multiplayer.MultiplayerScripts
{
    public class MultiplayerGun : NetworkBehaviour
    {
        [SyncVar] public GameObject target;
        [SerializeField] float damage = 10f;
        [SerializeField] PlayerMulti Player;
        [SerializeField] GameObject Pistol;
        [SerializeField] float range = 20f;
        float MaxDamage;


        // Start is called before the first frame update
        void Start()
        {
            MaxDamage = damage;
        }


        [Client]
        // Update is called once per frame
        void Update()
        {
            if (isLocalPlayer)
            {
                if (Physics.Raycast(Pistol.transform.position, Pistol.transform.forward, out RaycastHit hit, range))
                {
                    MultiplayerTarget target = hit.transform.GetComponent<MultiplayerTarget>();
                    if (target != null)
                    {
                        target.TakeDamage(damage, hit.transform.gameObject, Player);
                        //Debug.Log(hit.transform.name);
                    }
                }
            }
        }
    }
}