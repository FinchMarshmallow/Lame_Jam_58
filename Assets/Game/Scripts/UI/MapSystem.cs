using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapSystem : MonoBehaviour
{
    [Header("UI Settings")]
    public RectTransform mapContainer;
    public GameObject stationIconPrefab;
    public GameObject playerIconPrefab;

    [Header("Map Settings")]
    public float mapScale = 0.5f;

    [Header("References")]
    public Transform playerShip;

    // Словарь: "Трансформ станции в мире" -> "Картинка на карте"
    private Dictionary<Transform, Image> stationIconsMap = new Dictionary<Transform, Image>();
    private RectTransform playerIconRect;

    void Start()
    {
        // Ждем долю секунды, чтобы StationManager точно успел найти все станции
        Invoke(nameof(InitMap), 0.1f);
    }

    void InitMap()
    {
        // 1. Создаем иконку Игрока
        if (playerShip != null)
        {
            GameObject pObj = Instantiate(playerIconPrefab, mapContainer);
            playerIconRect = pObj.GetComponent<RectTransform>();
        }

        // 2. Берем список станций ИЗ МЕНЕДЖЕРА
        var allStations = StationManager.Instance.GetAllStationTransforms();

        foreach (var stationTransform in allStations)
        {
            GameObject sObj = Instantiate(stationIconPrefab, mapContainer);
            Image iconImg = sObj.GetComponent<Image>();

            // Запоминаем пару: Станция -> Иконка
            stationIconsMap.Add(stationTransform, iconImg);
        }
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        // Двигаем игрока
        if (playerIconRect != null && playerShip != null)
        {
            Vector2 playerPos = new Vector2(playerShip.position.x, playerShip.position.z) * mapScale;
            playerIconRect.anchoredPosition = playerPos;

            Vector3 rot = playerIconRect.localEulerAngles;
            rot.z = -playerShip.eulerAngles.y;
            playerIconRect.localEulerAngles = rot;
        }

        // Двигаем станции
        foreach (var pair in stationIconsMap)
        {
            Transform worldObj = pair.Key;
            RectTransform iconRect = pair.Value.rectTransform;

            if (worldObj != null)
            {
                Vector2 pos = new Vector2(worldObj.position.x, worldObj.position.z) * mapScale;
                iconRect.anchoredPosition = pos;
            }
        }
    }

    // ВОТ ОН, ИСПРАВЛЕННЫЙ МЕТОД
    // Теперь он принимает Transform, как ты и передаешь из MissionSystem
    public void HighlightTarget(Transform targetStation)
    {
        // Сброс цветов
        foreach (var icon in stationIconsMap.Values)
        {
            icon.color = Color.white;
            icon.transform.localScale = Vector3.one;
        }

        // Если такая станция есть в словаре - красим
        if (targetStation != null && stationIconsMap.ContainsKey(targetStation))
        {
            Image targetIcon = stationIconsMap[targetStation];
            targetIcon.color = Color.yellow;
            targetIcon.transform.localScale = Vector3.one * 1.5f;
        }
    }
}