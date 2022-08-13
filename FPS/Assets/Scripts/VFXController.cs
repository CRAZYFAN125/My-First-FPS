using System.Collections.Generic;
using UnityEngine;


public class VFXController : MonoBehaviour
{
    public float procent = 0;

    [System.Serializable]
    public class VFXSpawner
    {
        public float MinProcent = .5f;
        public GameObject VFX;
        public bool isActive = false;
    }

    public VFXSpawner[] values;
    public GameObject Shoot;
    public Transform shootPoint;
    GameObject ShootClone;
    public Enemy SourceScript;

    float Power;

    private void Start()
    {
        foreach (VFXSpawner item in values)
        {
            item.VFX.SetActive(false);
        }
    }

    private void Update()
    {
        foreach (VFXSpawner item in values)
        {
            if (!item.isActive)
            {
                if (procent>=item.MinProcent)
                {
                    item.VFX.SetActive(true);
                    item.isActive = true;
                }
            }
        }
        if(procent >= 100) {
            if (Power <=0)
            {
                ShootClone = Instantiate(Shoot, shootPoint.position, Quaternion.Euler(-90, 0, 0), transform.parent.parent);
                ShootClone.transform.localScale = new Vector3(0, 0, 0);
                Power+=Time.deltaTime;
            }
            else
            {
                ShootClone.transform.localScale = new Vector3(Mathf.Clamp01(Power),Mathf.Clamp01(Power),Mathf.Clamp01(Power));
                Power += Time.deltaTime;
            }
        }

        if (Power>=1)
        {
            ShootClone.transform.localScale = new Vector3(1, 1, 1);
            SourceScript.canMove = true;
            foreach (VFXSpawner item in values)
            {
                item.VFX.SetActive(false);
                item.isActive = false;
            }
            gameObject.SetActive(false);
        }
    }
}
