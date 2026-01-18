using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MissionSystem : MonoBehaviour
{
    [Header("UI")]
    public GameObject missionPanel;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI routeText;

    [Header("Systems")]
    public CompassSystem compassSystem;
    public MapSystem mapSystem;

    [Header("Data")]
    public List<Mission> allMissions;
    public int currentMissionIndex = 0;

    private bool isPaused = false;

    void Start()
    {
        missionPanel.SetActive(false);
        // Небольшая задержка, чтобы StationManager успел проинициализироваться
        Invoke(nameof(UpdateMissionUI), 0.1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        missionPanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void CompleteCurrentMission()
    {
        currentMissionIndex++;
        if (currentMissionIndex < allMissions.Count) UpdateMissionUI();
        else
        {
            if (compassSystem) compassSystem.ClearQuestMarkers();
            headerText.text = "GAME OVER";
            descText.text = "ALL JOBS DONE";
        }
    }

    void UpdateMissionUI()
    {
        if (currentMissionIndex >= allMissions.Count) return;

        Mission m = allMissions[currentMissionIndex];

        // 1. Текст
        headerText.text = $"// JOB {currentMissionIndex + 1}";
        descText.text = m.description;

        // Берем имена прямо из Data
        string pickName = m.pickupStation ? m.pickupStation.stationName : "???";
        string delName = m.deliverStation ? m.deliverStation.stationName : "???";
        routeText.text = $"PICKUP: {pickName}\nDELIVER: {delName}";

        // 2. Получаем Transform цели через Менеджер
        Transform targetTransform = StationManager.Instance.GetStationTransform(m.deliverStation);

        if (targetTransform != null)
        {
            // Обновляем Компас
            if (compassSystem) compassSystem.SetQuestMarker(targetTransform);

            // Обновляем Карту (Надо будет чуть обновить MapSystem, чтобы он принимал Transform, а не ID)
            if (mapSystem) mapSystem.HighlightTarget(targetTransform);
        }
    }
}

[System.Serializable]
public class Mission
{
    [TextArea] public string description;

    // ТЕПЕРЬ МЫ ИСПОЛЬЗУЕМ ФАЙЛЫ, А НЕ INT
    public StationData pickupStation;
    public StationData deliverStation;
}