using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MissionSystem : MonoBehaviour
{
    [Header("UI References")]
    public GameObject missionPanel;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI routeText;

    [Header("Navigation")]
    public CompassSystem compassSystem; 
    public Transform[] allStations;     

    [Header("Data")]
    public List<Mission> allMissions;
    public int currentMissionIndex = 0;

    private bool isPaused = false;

    void Start()
    {
        missionPanel.SetActive(false);
        UpdateMissionUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
        }
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
        Debug.Log("Mission Completed!");
        currentMissionIndex++;

        if (currentMissionIndex < allMissions.Count)
        {
            UpdateMissionUI();
        }
        else
        {
            // Конец игры
            if (compassSystem) compassSystem.ClearQuestMarkers();

            descText.text = "ALL MISSIONS COMPLETED.";
            routeText.text = "";
            headerText.text = "GAME OVER";
        }
    }

    // Главный метод обновления
    void UpdateMissionUI()
    {
        if (currentMissionIndex < allMissions.Count)
        {
            Mission m = allMissions[currentMissionIndex];

            // 1. Текст
            headerText.text = $"// MISSION LEVEL {currentMissionIndex + 1}";
            descText.text = m.description;
            routeText.text = $"PICKUP: {m.pickupStationName}\nDELIVER: {m.deliverStationName}";

            if (compassSystem != null && allStations.Length > 0)
            {
                if (m.deliverStationID >= 0 && m.deliverStationID < allStations.Length)
                {
                    Transform target = allStations[m.deliverStationID];
                    compassSystem.SetQuestMarker(target);
                }
            }
        }
    }
}

[System.Serializable]
public class Mission
{
    [TextArea(3, 5)]
    public string description;
    public string pickupStationName;
    public int pickupStationID;
    public string deliverStationName;
    public int deliverStationID;
}