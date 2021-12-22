using System.Collections;
using UnityEngine;

public class DaySystem : MonoBehaviour
{
    public Transform Tlight;
    // Start is called before the first frame update
    void Start()
    {
        //if (PlayerPrefs.GetInt("Weather") == 1)
        //{
        InvokeRepeating("Day", 1f, .5f) ;
        //}
    }
    void Day()
    {
        
            Tlight.Rotate(2.5f, 0, 0);
    }
}
