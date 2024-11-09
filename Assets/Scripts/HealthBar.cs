using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelathBar : MonoBehaviour
{
    private Slider slider;
    public Text healthCounter;

    public GameObject playerState;

    private float currentHealth, maxHealth;
    void Awake() {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update() {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;  //used to calculate slider value to display health
        slider.value = fillValue;

        healthCounter.text = currentHealth + "/" + maxHealth;
    }
}
