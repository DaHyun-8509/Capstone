using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PirodoBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxpirodo(int pirodo)
    {
        slider.maxValue = pirodo;
        slider.value = pirodo;

    }
    public void Setpirodo(int pirodo)
    {
        slider.value = pirodo;
    }
}
