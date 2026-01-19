using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipHUD : MonoBehaviour
{
    [Header("Bars")]
    public Image hullBar; // ���� HullSlider (Image)
    public Image fuelBar; // ���� FuelSlider (Image)

    [Header("Text")]
    public TextMeshProUGUI speedText;

    [Header("References")]
    public ShipEntity shipData; // <-- ���� �������� ������ Player/Ship
    public Rigidbody shipRb;    // <-- ���� �������� ������ Player (��� Rigidbody)

    // ������, ����� �� ������� ������������� ������ �������
    private float alertTimer = 0f;

    void Update()
    {
        if (shipData == null) return;

        // 1. ��������� ������� (Cast to float ����������, ����� ����� 0)
        // ���������� ���� ����������: CurrentHealsPoint � CurrentOil
        if (hullBar != null)
            hullBar.fillAmount = (float)shipData.CurrentHealsPoint / shipData.MaxHealsPoint;

        if (fuelBar != null)
            fuelBar.fillAmount = (float)shipData.CurrentOil / shipData.MaxOil;

        // 2. ��������� ��������
        if (shipRb != null && speedText != null)
        {
            // ����� ��������, ���������
            speedText.text = $"{Mathf.Round(shipRb.linearVelocity.magnitude)} m/s";
        }

        // 3. ������ ������� (ALERT)
        CheckStatusAlerts();
    }

    void CheckStatusAlerts()
    {
        alertTimer -= Time.deltaTime;
        if (alertTimer > 0) return; // ���� ������� �����, ������

        // �������� ������� (������ 20%)
        float fuelPercent = (float)shipData.CurrentOil / shipData.MaxOil;
        if (fuelPercent < 0.2f)
        {
            NotificationSystem.Instance.ShowAlert("WARNING: LOW FUEL", Color.red);
            alertTimer = 5f; // ��������� ����� 5 ������
            return;
        }

        // �������� �������� (������ 30%)
        float hpPercent = (float)shipData.CurrentHealsPoint / shipData.MaxHealsPoint;
        if (hpPercent < 0.3f)
        {
            NotificationSystem.Instance.ShowAlert("CRITICAL HULL DAMAGE", Color.red);
            alertTimer = 5f;
        }
    }
}