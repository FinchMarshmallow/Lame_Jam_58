using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject dockingPanel; // Ссылка на панель стыковки

    void Awake()
    {
        Instance = this;
        if (dockingPanel != null) dockingPanel.SetActive(false); // Скрываем при старте
    }

    // Вызывай этот метод из любого скрипта: UIManager.Instance.SetDocking(true);
    public void SetDocking(bool isActive)
    {
        if (dockingPanel != null)
        {
            dockingPanel.SetActive(isActive);
        }
    }
}