using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public GameObject statusBars;
    public GameObject ManaBar;
    public GameObject SpellUi;

    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject Inventory;

    Vector3 oneScle = new Vector3(1, 1, 1);
    Vector3 baseScale = new Vector3(2.4f, 2.4f, 2.4f);
    public void onScaleSlider(float scale)
    {
        statusBars.GetComponent<RectTransform>().localScale = baseScale * scale;
        ManaBar.GetComponent<RectTransform>().localScale = baseScale * scale;
        SpellUi.GetComponent<RectTransform>().localScale = oneScle * scale;
        LeftHand.GetComponent<RectTransform>().localScale = oneScle * scale;
        RightHand.GetComponent<RectTransform>().localScale = oneScle * scale;
        Inventory.GetComponent<RectTransform>().localScale = oneScle * scale;
    }
}
