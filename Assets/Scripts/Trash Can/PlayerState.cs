using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    
    public static PlayerState Instance { get; set; }

    // ---- Player Health ---- //
    public float currentHealth;
    public float maxHealth;

    // ---- Player Calories ---- //
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody; 

    // ---- Player Hydration ---- //
    public float currentHydrationPercent;
    public float maxHydrationPercent;
    
    //public bool isHydrationActive;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }


    private void Start() {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        StartCoroutine(decreaseHydration());
    }

    IEnumerator decreaseHydration() {
        while (true) {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(10);
        }
    }


    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position; 

        if (distanceTravelled >= 5) { //player lose calories when moving
            distanceTravelled = 0;
            currentCalories -= 1;
        } 


        //testing the health bar
        if (Input.GetKeyDown(KeyCode.N)) {
            currentHealth -= 10;
        }
    }
}
