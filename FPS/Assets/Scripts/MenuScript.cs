using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class MenuScript : MonoBehaviour
{
    public Toggle toggle;
    public Slider slider;
    public Toggle Time;
    public Text sliderText;
    public Text ControlsMenu;
    public Text ControlsGame;
    [Header("Sensivity:")]
    public Text SensivityText;
    public Slider SensivitySlider;

    private void Start()
    {
        PlayerPrefs.SetInt("CONF", 0);
        sliderText.text = string.Empty;
        PlayerPrefs.SetInt("Wether", 1);
        float value = PlayerPrefs.GetFloat("Sensi");
        SensivityText.text = value.ToString();
        SensivitySlider.value = value;

        if (Gamepad.current!=null)
        {
            ControlsMenu.text = string.Empty;
            ControlsMenu.text += Gamepad.current.displayName;
            Gamepad gp = Gamepad.current;
            ControlsMenu.text += "\n\nGamepad Buttons\n\nMenu\n\nActive Custom Map: North Button\n\nSet Custom Map value: Right Stick\n\nDeactive Time: East Button\n\nStart: Start";
            ControlsGame.text += "\n\nGamepad Buttons\n\nGame\n\nJump: South Button\n\nMove: Left Stick\n\nLook: Right Stick\n\nShoot: Right Trigger\n\nChange Weapon: D-Pad Y\n\nReload: Right Bumper/Shoulder";
        }
        else
        {
            ControlsMenu.text = string.Empty;
            ControlsMenu.text += "\nMenu\n\nActive Custom Map: Space\n\nSet Custom Map value: Scroll\n\nDeactive Time: Control\n\nStart: Enter";
            ControlsGame.text += "\nGame\n\nJump: Space\n\nMove: WSAD\n\nLook: Mouse Move\n\nShoot: Right Mouse Button\n\nChange Weapon: Scroll\n\nReload: Left Mouse Button";
        }
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
                slider.interactable = false;
                break;
        }
    }

    public void SetSensivity(float value)
    {
        value = Mathf.FloorToInt(value);
        PlayerPrefs.SetFloat("Sensi", value);
        SensivityText.text = value.ToString();
    }

    public void SetDayTime(bool x)
    {
        switch (x)
        {
            case true:
                PlayerPrefs.SetInt("Wether", 1);
                break;
            case false:
                PlayerPrefs.SetInt("Wether", 0);
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


    #region InputSystem

    public void ActiveCustom(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            toggle.isOn= !toggle.isOn;
            ActiveReConfig(toggle.isOn);
        }
    }
    public void SetCustom(InputAction.CallbackContext context)
    {
        if (context.performed && toggle.isOn)
        {
            float x =Mathf.Clamp( context.ReadValue<float>(),-.75f,.75f);
            x = slider.value + x;
            if (x<slider.minValue||x>slider.maxValue)
            {
                return;
            }
            slider.value = x;
            ReConfigGenerator(x);
        }
    }
    //public void SetSensivityR(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        float x =Mathf.Clamp( context.ReadValue<float>(),-.55f,.55f);
    //        x = slider.value + x;
    //        if (x<SensivitySlider.minValue||x>SensivitySlider.maxValue)
    //        {
    //            return;
    //        }
    //        SensivitySlider.value = x;
    //        SetSensivity(x);
    //    }
    //}
    public void DeactiveTime(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Time.isOn= !Time.isOn;
            SetDayTime(Time.isOn);
        }
    }
    public void StartIt(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Lunch();
        }
    }

    #endregion

}