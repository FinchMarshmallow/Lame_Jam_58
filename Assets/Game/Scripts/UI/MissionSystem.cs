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
        // Если миссии кончились - выходим
        if (currentMissionIndex >= allMissions.Count) return;

        Mission m = allMissions[currentMissionIndex];

        // ==================================================
        // ЧАСТЬ 1: ТЕКСТ (Работает всегда, даже без станций)
        // ==================================================

        // 1. Название Миссии (В заголовке)
        if (headerText != null)
        {
            headerText.text = m.missionName;
        }



        // 3. Описание (Что везем и детали)
        if (descText != null)
        {
            descText.text = m.description;
        }


        // ==================================================
        // ЧАСТЬ 2: КОМПАС И КАРТА
        // ==================================================

        // Проверяем, есть ли менеджер и знаем ли мы, куда лететь
        if (StationManager.Instance != null && m.deliverStation != null)
        {
            // Пытаемся найти эту станцию в 3D мире
            Transform targetTransform = StationManager.Instance.GetStationTransform(m.deliverStation);

            if (targetTransform != null)
            {
                // УРА! Станция найдена. Включаем компас и карту.
                if (compassSystem) compassSystem.SetQuestMarker(targetTransform);
                if (mapSystem) mapSystem.HighlightTarget(targetTransform);
            }
        }
    }
}

    [System.Serializable]
public class Mission
{
    [TextArea] public string description;
    [TextArea] public string missionName;

    // ТЕПЕРЬ МЫ ИСПОЛЬЗУЕМ ФАЙЛЫ, А НЕ INT
    public StationData pickupStation;
    public StationData deliverStation;
}