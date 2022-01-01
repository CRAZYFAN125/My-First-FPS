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

    public int Killed = 0;

    public GameObject[] guns;

    public float playerHealh = 25f;
    public float ammo = 5f;
    float startAmmo;

    int gunI = 0, gunN = 0;

    public int tableID = 94328; // Set it to 0 for main highscore table.
    string extraData = ""; // This will not be shown on the website. You can store any information.

    public void ChangeGuns(InputAction.CallbackContext context)
    {
        if (context.performed && MapGenerator.Ready)
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
        if (callback.performed&&!realoding&&MapGenerator.Ready)
        {
            guns[gunI].GetComponent<Gun>().Shoot();
        }
    }


    public void Reload()
    {
        if (ammo+.5f > 1 && MapGenerator.Ready)
        {
            ammo = 1f;
        }
        else if (ammo<startAmmo && MapGenerator.Ready)
        {
            ammo += .5f ;
        }
    }
    public void Damage(float amount)
    {
        playerHealh -= amount;
        if (playerHealh<=0)
        {
            MapGenerator.Ready = false;
            #region Scores
            if (GameJolt.API.GameJoltAPI.Instance.HasSignedInUser)
            {
                int scoreValue = Killed; // The actual score.
                string scoreText = $"{Killed} killed"; // A string representing the score to be shown on the website.
                GameJolt.API.Scores.Add(scoreValue, scoreText, tableID, extraData, (bool success) => {
                    switch (success)
                    {
                        case true:
                            Debug.Log("Sended value");
                            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                            break;
                        case false:
                            Debug.Log("Not sended value");
                            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                            break;
                    }
                });
            }
            else
            {
                int scoreValue = Killed; // The actual score.
                string scoreText = $"{Killed} killed"; // A string representing the score to be shown on the website.
                GameJolt.API.Scores.Add(scoreValue, scoreText,"Quest", tableID, extraData, (bool success) => {
                    switch (success)
                    {
                        case true:
                            Debug.Log("Sended value");
                            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                            break;
                        case false:
                            Debug.Log("Not sended value");
                            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                            break;
                    }
                });
            }
            #endregion
        }
    }

    public Transform player;
    private void Awake()
    {
        if (instance!=null)
        {
            Destroy(gameObject);
        }
        instance = this;
        startAmmo = ammo;
        
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        if (player.position.y <= -5f)
        {
            StartCoroutine(Killer());
        }
    }
    IEnumerator Killer()
    {
        while (playerHealh>=1)
        {
            Damage(0.22f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ResetGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            MapGenerator.Ready = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public void Heal(float _amount)
    {
        if (ammo <=.9f)
        {
            return;
        }
        if (playerHealh+_amount>1f)
        {
            playerHealh = 1f;
        }
        else
        {
            playerHealh += _amount;
        }
        ammo = 0;
    }
}