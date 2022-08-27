using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.CustomGuns
{
    [RequireComponent(typeof(Gun))]
    public class ShieldActivator : MonoBehaviour, IGun
    {
        [Range(.01f, 1)] public float AmmoTaking=.15f;
        [Range(.01f, 1)] public float AmmoFightTaking=.25f;
        Transform Cam;
        Material matAct;
        GameManager gameManager;
        bool isWorking = false;
        [SerializeField]float distance=2f;
        [SerializeField] float odrzut = 6f;
        [SerializeField]float damage=5f;
        [SerializeField] GameObject OnlineParticle;

        public void OnShoot()
        {
            if (!isWorking && gameManager.ammo >= gameManager.StartAmmo)
            {
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
                List<GameObject> enemysToMove = new List<GameObject>();

                foreach (GameObject item in enemys)
                {
                    if (Vector3.Distance(Cam.position, item.transform.position) <= distance)
                    {
                        enemysToMove.Add(item);
                    }
                }

                foreach (GameObject item in enemysToMove)
                {
                    item.GetComponent<Target>().TakeDamage(damage);
                    item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, (item.transform.forward.z * -1) - odrzut);
                }
                gameManager.ammo -= AmmoFightTaking;
            }
            isWorking = !isWorking;
            if (isWorking)
            {
                gameManager.Damage(.2f);
                OnlineParticle.SetActive(true);
                gameManager.isShieldOnline = true;
            }
        }

        public void SetDatas(Camera fpsCam)
        {
            Cam = fpsCam.GetComponent<Transform>();
        }

        // Use this for initialization
        void Start()
        {
            matAct = GetComponent<Renderer>().materials[0];
            gameManager = GameManager.instance;
        }


        void FixedUpdate()
        {
            matAct.SetFloat("_Power", gameManager.ammo);
            if (gameManager.ammo<=0&&isWorking)
            {
                isWorking = false;
            }
            if (isWorking&&gameManager.ammo<=gameManager.StartAmmo)
            {
                gameManager.canReload = false;
                if (gameManager.ammo-AmmoTaking*Time.fixedDeltaTime<=0)
                {
                    isWorking = false;
                    return;
                }
                gameManager.ammo -= AmmoTaking * Time.fixedDeltaTime;

            }
            else
            {
                OnlineParticle.SetActive(false);
                gameManager.isShieldOnline = false;
                gameManager.canReload = true;
            }
        }
    }
}