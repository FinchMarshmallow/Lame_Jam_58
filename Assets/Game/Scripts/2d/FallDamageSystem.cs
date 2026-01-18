using UnityEngine;

public class FallDamageSystem : MonoBehaviour
{
    [Header("Настройки прочности")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Настройки повреждений")]
    [SerializeField] private float damageMultiplier = 10f;
    [SerializeField] private float minDamageVelocity = 5f; // Минимальная скорость для получения урона
    [SerializeField] private LayerMask damageLayers; // Слои, которые наносят урон при столкновении

    [Header("Визуализация")]
    [SerializeField] private bool showDebug = true;

    private Rigidbody2D rb;
    private bool isAlive = true;

    // Событие для обработки смерти объекта
    public delegate void OnDestroyedHandler(GameObject destroyedObject);
    public event OnDestroyedHandler OnDestroyed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D не найден! Добавьте Rigidbody2D для работы системы повреждений.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;

        // Проверяем, находится ли объект столкновения на нужном слое
        if (((1 << collision.gameObject.layer) & damageLayers) != 0)
        {
            CalculateDamage(collision);
        }
    }

    void CalculateDamage(Collision2D collision)
    {
        // Получаем относительную скорость столкновения
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (showDebug)
        {
            Debug.Log($"Скорость столкновения: {impactVelocity}");
        }

        // Если скорость меньше минимальной - игнорируем
        if (impactVelocity < minDamageVelocity) return;

        // Рассчитываем урон на основе скорости
        float damage = (impactVelocity - minDamageVelocity) * damageMultiplier;

        // Можно учесть угол падения (больше урона при вертикальном падении)
        Vector2 normal = collision.contacts[0].normal;
        float verticalFactor = Mathf.Abs(Vector2.Dot(-collision.relativeVelocity.normalized, Vector2.up));
        damage *= (0.5f + verticalFactor * 0.5f); // Коэффициент от 0.5 до 1

        ApplyDamage(damage);

        if (showDebug)
        {
            Debug.Log($"Получен урон: {damage}, Здоровье: {currentHealth}/{maxHealth}");
        }
    }

    void ApplyDamage(float damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;

        // Проверяем, не уничтожен ли объект
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
    }

    void Death()
    {
        //здесь должна быть смерть

        return;
    }

    // Методы для внешнего управления
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void Repair(float amount)
    {
        Heal(amount);
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}