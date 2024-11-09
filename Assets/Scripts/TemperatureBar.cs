using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureBar : MonoBehaviour
{
    private Slider slider;
    public Text healthCounter;

    public GameObject playerState;

    private int currentHealth, maxHealth;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerState.GetComponent<StatesEffects>().Temperature;
        maxHealth = playerState.GetComponent<StatesEffects>().MaxTemperature;

        float fillValue = (float)currentHealth / (float)maxHealth;  //used to calculate slider value to display health
        slider.value = fillValue;

        healthCounter.text = currentHealth + "/" + maxHealth;
    }
}
