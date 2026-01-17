using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipHUD : MonoBehaviour
{
    [Header("Bars")]
    public Image hullBar; 
    public Image fuelBar; 

    [Header("Text")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI objectiveText;


    //public ShipController playerShip;

    //void Update()
    //{
    //    if (playerShip == null) return;

    //    // ќбновл€ем бары (значени€ должны быть нормализованы 0..1)
    //    hullBar.fillAmount = playerShip.currentHealth / playerShip.maxHealth;
    //    fuelBar.fillAmount = playerShip.currentFuel / playerShip.maxFuel;

    //    // ќбновл€ем скорость (округл€ем до целого)
    //    speedText.text = $"{Mathf.Round(playerShip.rb.velocity.magnitude)} m/s";
    //}

    public void SetObjective(string text)
    {
        objectiveText.text = text;
    }
}