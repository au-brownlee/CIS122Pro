using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;


public class GameSettings : MonoBehaviour
{
    public GameObject player;
    public GameObject SensSlider;
    public GameObject ScaleSlider;

    public GameObject statusBars;
    public GameObject ManaBar;
    public GameObject SpellUi;

    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject Inventory;

    Vector3 oneScle = new Vector3(1, 1, 1);
    Vector3 baseScale = new Vector3(2.4f, 2.4f, 2.4f);

    public float sensitivity = 1;
    public float scale = 1;
    void Start()
    {
        Load();
        gameObject.SetActive(false);
    }

    public void OnSensSlider(float value)
    {
        sensitivity = value;
        player.GetComponent<MouseMovement>().SetSensitivity(value);
        Save();
    }
    public void onScaleSlider(float value)
    {
        scale = value;
        statusBars.GetComponent<RectTransform>().localScale = baseScale * value;
        ManaBar.GetComponent<RectTransform>().localScale = baseScale * value;
        SpellUi.GetComponent<RectTransform>().localScale = oneScle * value;
        LeftHand.GetComponent<RectTransform>().localScale = oneScle * value;
        RightHand.GetComponent<RectTransform>().localScale = oneScle * value;
        Inventory.GetComponent<RectTransform>().localScale = oneScle * value;
        Save();
    }
    public void Load()
    {
        var sr = File.ReadAllText("settings.txt").Split("\n");
        sensitivity = float.Parse(sr[0]);
        scale = float.Parse(sr[1]);

        SensSlider.GetComponent<Slider>().value = sensitivity;
        ScaleSlider.GetComponent<Slider>().value = scale;
    }

    public void Save()
    {
        var sr = File.CreateText("settings.txt");
        sr.WriteLine(sensitivity);
        sr.WriteLine(scale);
        sr.Close();
    }
}
