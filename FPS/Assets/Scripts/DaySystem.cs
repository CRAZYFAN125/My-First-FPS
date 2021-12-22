using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaySystem : MonoBehaviour
{
    public Transform light;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Weather") == 1){
		StartCoroutine(Day());
	}
    }
    IEnumerator Day()
    {
        while (true)
        {
            light.Rotate(2.5f, 0,0);
            yield return new WaitForSeconds(1f);
        }
    }
}
