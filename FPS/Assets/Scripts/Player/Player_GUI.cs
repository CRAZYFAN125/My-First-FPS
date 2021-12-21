using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Player_GUI : MonoBehaviour
{
    public Text text;
    public string[] textData;
    public GameObject[] @object;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(Texting());
        foreach (GameObject itemObj in @object)
        {
            itemObj.SetActive(false);
        }
    }

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
                        }
                        text.text = string.Empty;
                        StopAllCoroutines();
                    }
                    text.text += letter;
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(.25f);
            }
        }
        text.text = string.Empty;
        foreach (GameObject itemObj in @object)
        {
            itemObj.SetActive(true);
        }
    }
}
