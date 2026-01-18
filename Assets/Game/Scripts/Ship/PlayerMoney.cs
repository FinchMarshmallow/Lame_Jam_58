using UnityEngine;

// Класс для хранения денег
public class PlayerMoney : MonoBehaviour
{
    [SerializeField] private float _money = 1000f;

    public float Money => _money;

    public void AddMoney(float amount)
    {
        if (amount > 0)
            _money += amount;
    }

    public bool SpendMoney(float amount)
    {
        if (amount > 0 && _money >= amount)
        {
            _money -= amount;
            return true;
        }
        return false;
    }

    public void SetMoney(float amount)
    {
        if (amount >= 0)
            _money = amount;
    }
}