using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    private Slider slider;
    public Text counterText;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void SetSlider(float currentValue, float maxValue)
    {
        float fillValue = currentValue / maxValue;  //used to calculate slider value to display health
        slider.value = fillValue;
    }

    public void SetSlider(float currentValue, float maxValue, bool showText)
    {
        this.SetSlider(currentValue, maxValue);
        if (showText)
        {
            counterText.text = (int)currentValue + "/" + (int)maxValue;
        }
    }
}
