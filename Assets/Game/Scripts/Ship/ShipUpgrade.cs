using UnityEngine;

// Класс прокачки
public class ShipUpgrade : MonoBehaviour
{
    [Header("Базовая стоимость")]
    [SerializeField] private float cost = 100f;
    [SerializeField] private float costMultiplier = 1.5f;

    [Header("Уровни прокачки HP")]
    [SerializeField] private int maxHpLevel = 10;
    [SerializeField] private int currentHpLevel = 1;
    [SerializeField] private float hpMultiplier = 1.2f;

    [Header("Уровни прокачки топлива")]
    [SerializeField] private int maxOilLevel = 10;
    [SerializeField] private int currentOilLevel = 1;
    [SerializeField] private float oilMultiplier = 1.2f;

    [Header("Уровни прокачки скорости")]
    [SerializeField] private int maxSpeedLevel = 10;
    [SerializeField] private int currentSpeedLevel = 1;
    [SerializeField] private float speedMultiplier = 1.1f;

    [Header("Ссылки")]
    [SerializeField] private ShipEntity shipEntity;
    [SerializeField] private ShipTestMove shipMovement;
    [SerializeField] private PlayerMoney playerMoney;

    // Метод прокачки HP
    public void UpgradeHP()
    {
        if (playerMoney.SpendMoney(cost) && currentHpLevel < maxHpLevel)
        {
            shipEntity.MaxHealsPoint = Mathf.RoundToInt(shipEntity.MaxHealsPoint * hpMultiplier);
            cost *= costMultiplier;
            currentHpLevel++;
        }
    }

    // Метод прокачки топлива
    public void UpgradeOil()
    {
        if (playerMoney.SpendMoney(cost) && currentOilLevel < maxOilLevel)
        {
            shipEntity.MaxOil = Mathf.RoundToInt(shipEntity.MaxOil * oilMultiplier);
            cost *= costMultiplier;
            currentOilLevel++;
        }
    }

    // Метод прокачки скорости
    public void UpgradeSpeed()
    {
        if (playerMoney.SpendMoney(cost) && currentSpeedLevel < maxSpeedLevel)
        {
            shipMovement.forceLinear *= speedMultiplier;
            shipMovement.forceFullForward *= speedMultiplier;
            cost *= costMultiplier;
            currentSpeedLevel++;
        }
    }

    // Геттеры для UI
    public float GetCurrentCost() => cost;
    public int GetCurrentHpLevel() => currentHpLevel;
    public int GetMaxHpLevel() => maxHpLevel;
    public int GetCurrentOilLevel() => currentOilLevel;
    public int GetMaxOilLevel() => maxOilLevel;
    public int GetCurrentSpeedLevel() => currentSpeedLevel;
    public int GetMaxSpeedLevel() => maxSpeedLevel;
}