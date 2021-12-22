using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [System.Serializable]
    public class Item
    {
        public GameObject prefab;
        public int amount = 1;
    }

    public GameObject[] guns;

    public float playerHealh = 25f;
    public float ammo = 5f;
    float startAmmo;

    int gunI = 0, gunN = 0;

    public void ChangeGuns(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.ReadValue<float>()>0)
            {
                if (gunI + 1 >= guns.Length)
                {
                    gunI = 0;
                }
                else
                {
                    gunI += 1;
                }
            }
            else
            {
                if (gunI - 1 <= -1)
                {
                    gunI = guns.Length - 1;
                }
                else
                {
                    gunI -= 1;
                }
            }
            Debug.Log(gunI);
            guns[gunN].SetActive(false);
            guns[gunI].SetActive(true);
            gunN = gunI;
        }
    }
    bool realoding = false;
    public void Reloads(InputAction.CallbackContext context)
    {
        if (!realoding && context.performed)
        {
            realoding = true;
            StartCoroutine(Reloader());
        }
    }
    IEnumerator Reloader()
    {
        while (ammo < startAmmo)
        {
            ammo += 0.10f;
            yield return new WaitForSeconds(0.5f);
        }
        realoding = false;
    }

    public void Shoot(InputAction.CallbackContext callback)
    {
        if (callback.performed&&!realoding)
        {
            guns[gunI].GetComponent<Gun>().Shoot();
        }
    }


    public void Reload()
    {
        if (ammo<5)
        {
            ammo += 1;
        }
    }
    public void Damage(float amount)
    {
        playerHealh -= amount;
        if (playerHealh<=0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }


    private void Awake()
    {
        if (instance!=null)
        {
            Destroy(gameObject);
        }
        instance = this;
        startAmmo = ammo;
    }
}