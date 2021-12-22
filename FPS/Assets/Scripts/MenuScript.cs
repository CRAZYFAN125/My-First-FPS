using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour
{
    
    public Slider slider;
    public Text sliderText;
    private void Start()
    {
        PlayerPrefs.SetInt("CONF", 0);
        sliderText.text = string.Empty;
	PlayerPrefs.SetInt("Wether",0);
    }
    public void ActiveReConfig(bool x)
    {
        slider.enabled = x;
        switch (x)
        {
            case true:
                PlayerPrefs.SetInt("CONF", 1);
                slider.interactable = true;
                break;
            case false:
                PlayerPrefs.SetInt("CONF", 0);
                slider.interactable =false;
                break;
        }
    }

public void SetDayTime(bool x){
switch(x){
	case true:
		PlayerPrefs.SetInt("Wether",1);
		break;
	case false:
		PlayerPrefs.SetInt("Wether",0);
		break;
}
}

    public void ReConfigGenerator(float _i)
    {
        int i = Mathf.FloorToInt(_i);
        PlayerPrefs.SetInt("Gen", i);
        sliderText.text = i.ToString();
    }
    public void Lunch()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}