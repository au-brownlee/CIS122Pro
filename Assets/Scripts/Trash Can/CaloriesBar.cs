using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaloriesBar : MonoBehaviour
{
    private Slider slider;
    public Text caloriesCounter;

    public GameObject playerState;

    private float currentCalories, maxCalories;
    void Awake() {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update() {
        currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerState>().maxCalories;

        float fillValue = currentCalories / maxCalories;  //used to calculate slider value to display health
        slider.value = fillValue;

        caloriesCounter.text = currentCalories + "/" + maxCalories;
    }
}
