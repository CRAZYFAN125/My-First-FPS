using UnityEngine;

namespace Player.CustomGuns
{
    public class LaserGun : MonoBehaviour, IGun
    {
        private bool shooting = false;
        public float range = 10f;
        public float ammoTake = 0.02f;
        public float DPS = 0.1f;
        [SerializeField] LineRenderer laser;
        [SerializeField] Transform firePoint;
        Camera fpsCam;
        public void SetDatas(Camera camera)
        {
            fpsCam = camera;
        }
        public void OnShoot()
        {
            shooting = !shooting;
            laser.gameObject.SetActive(shooting);
        }

        private void Start()
        {
            laser.gameObject.SetActive(false);
            laser.useWorldSpace = true;
        }
        private void Update()
        {
            if (shooting)
            laser.SetPosition(0, firePoint.position);
        }
        private void FixedUpdate()
        {
            if (shooting == false)
            {
                GameManager.instance.canReload = true;
                return;
            }
            if (GameManager.instance.ammo - ammoTake >= 0f)
            {
                GameManager.instance.canReload = false;
                GameManager.instance.ammo -= ammoTake * Time.fixedDeltaTime;

                RaycastHit hit;
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                {
                    laser.useWorldSpace = true;
                    Target target = hit.transform.GetComponent<Target>();
                    if (target != null)
                    {
                        target.TakeDamage(DPS * Time.fixedDeltaTime);
                        Debug.Log(hit.transform.name);
                    }
                    laser.SetPosition(1, hit.point);
                }
                else
                {
                    laser.SetPosition(1, firePoint.position);
                }

            }
            else
            {
                GameManager.instance.canReload = true;
                laser.gameObject.SetActive(false);
                shooting = false;
            }

        }
    }
}