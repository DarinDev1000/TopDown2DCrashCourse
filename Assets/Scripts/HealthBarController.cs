using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Gradient gradient;

    private Slider slider;
    private Image fill;

    private void Awake()
    {
        // Start method was happening before slider existed
        slider = gameObject.GetComponent<Slider>();
        fill = transform.Find("Fill").GetComponent<Image>();
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
