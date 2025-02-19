using UnityEngine;
using UnityEngine.UI;

public class UI_Statbar : MonoBehaviour
{
    private Slider staminaSlider;
    //Secondary bar behind the main bar to show how much health or stamina you lose every hit

    public void Awake()
    {
        staminaSlider = GetComponent<Slider>();   
    }

    public void SetStat()
    {

    }

    public void SetMaxStat()
    {
        
    }



}
