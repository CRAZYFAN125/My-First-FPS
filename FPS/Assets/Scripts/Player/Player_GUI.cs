using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Player_GUI : MonoBehaviour
{
    public Text text;
    public string[] textData;
    public GameObject[] @object;
    public Behaviour[] behaviours;

    public Image HealhBar;
    public Image AmmoBar;

    GameManager GM;


    // Use this for initialization
    void Start()
    {
        GM = GameManager.instance;
        StartCoroutine(Texting());
        foreach (GameObject itemObj in @object)
        {
            itemObj.SetActive(false);
        }foreach (Behaviour itemObj in behaviours)
        {
            itemObj.enabled = false;
        }
    }

    public void ChangedDevice(PlayerInput input)
    {
        text.text = "\n Aktywowano " + input.currentControlScheme;
    }

    void Update()
    {
        if (MapGenerator.Ready)
        {
            HealhBar.fillAmount =/* Przelicz(*/GM.playerHealh/*)*/;
            AmmoBar.fillAmount = /*Przelicz(*/GM.ammo/*)*/;
        }
    }
    //float Przelicz(float data)
    //{
    //    return  (1 / data) ;
    //}

    IEnumerator Texting()
    {
        text.text = string.Empty;
        while (!MapGenerator.Ready)
        {
            foreach (string item in textData)
            {
                text.text += "\n";
                char[] vs = item.ToCharArray();
                foreach (char letter in vs)
                {
                    if (MapGenerator.Ready)
                    {
                        foreach (GameObject itemObj in @object)
                        {
                            itemObj.SetActive(true);
                        }foreach (Behaviour itemObj in behaviours)
                        {
                            itemObj.enabled =true;
                        }
                        text.text = string.Empty;
                        StopAllCoroutines();
                    }
                    else
                    {
                        text.text += letter;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(.25f);
            }
        }
        text.text = string.Empty;
        foreach (GameObject itemObj in @object)
        {
            itemObj.SetActive(true);
        }foreach (Behaviour itemObj in behaviours)
        {
            itemObj.enabled=true;
        }
    }
}
