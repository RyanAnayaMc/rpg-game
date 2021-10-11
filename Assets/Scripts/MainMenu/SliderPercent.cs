using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// A simple script that changes a text object's text to the percent of a Slider
public class SliderPercent : MonoBehaviour {
    public Slider slider;
    public TMP_Text text;
    void Update() {
        int percent = (int) (100 * slider.value / slider.maxValue);
        text.text = percent + "%";
    }
}
