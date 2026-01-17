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


    public ShipEntity playerShip;
    public GameObject Ship;

    void Start()
    {


        if (playerShip == null)
        {
            Debug.LogError("Player Ship reference is missing in ShipHUD.");
            

        }
    }

    void Update()
    {
        if (playerShip == null) return;

        hullBar.fillAmount = playerShip.CurrentHealsPoint / playerShip.MaxHealsPoint;
        fuelBar.fillAmount = playerShip.CurrentOil / playerShip.MaxOil;

        speedText.text = $"{Mathf.Round(Ship.gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude)} m/s"; 
    }

    public void SetObjective(string text)
    {
        objectiveText.text = text;
    }
}