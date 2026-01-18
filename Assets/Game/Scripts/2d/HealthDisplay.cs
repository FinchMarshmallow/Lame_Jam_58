using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private FallDamageSystem damageSystem;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient healthGradient;

    void Start()
    {
        if (damageSystem == null)
        {
            damageSystem = GetComponent<FallDamageSystem>();
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = 100f;
            healthSlider.minValue = 0f;
        }
    }

    void Update()
    {
        if (damageSystem != null && healthSlider != null)
        {
            float healthPercent = damageSystem.GetHealthPercentage() * 100f;
            healthSlider.value = healthPercent;

            if (fillImage != null)
            {
                fillImage.color = healthGradient.Evaluate(damageSystem.GetHealthPercentage());
            }
        }
    }
}