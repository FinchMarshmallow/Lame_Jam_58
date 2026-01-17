using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MissionSystem : MonoBehaviour
{
    [Header("UI References")]
    public GameObject missionPanel;      // Сама панель которую включаем/выключаем
    public TextMeshProUGUI headerText;   
    public TextMeshProUGUI descText;     // Описание миссии
    public TextMeshProUGUI routeText;    // Откуда -> Куда

    [Header("Data")]
    public List<Mission> allMissions;    // Список всех миссий в игре
    public int currentMissionIndex = 0;  // На каком мы сейчас уровне

    private bool isPaused = false;

    void Start()
    {
        missionPanel.SetActive(false);
        UpdateMissionUI();
    }

    void Update()
    {
        // Логика нажатия TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            missionPanel.SetActive(true);
            Time.timeScale = 0f; 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            missionPanel.SetActive(false);
            Time.timeScale = 1f; 
        }
    }

    // Метод вызываем, когда миссия пройдена
    public void CompleteCurrentMission()
    {
        Debug.Log("Mission Completed!");

        currentMissionIndex++;

        if (currentMissionIndex < allMissions.Count)
        {
            UpdateMissionUI();
            // Можно проиграть звук успеха тут
        }
        else
        {
            descText.text = "ALL MISSIONS COMPLETED. YOU ARE FREE.";
            routeText.text = "Relax pilot.";
            headerText.text = "GAME OVER";
        }
    }

    void UpdateMissionUI()
    {
        if (currentMissionIndex < allMissions.Count)
        {
            Mission m = allMissions[currentMissionIndex];
            headerText.text = $"// MISSION LEVEL {currentMissionIndex + 1}";
            descText.text = m.description;
            routeText.text = $"PICKUP: {m.pickupStationName}\nDELIVER: {m.deliverStationName}";
        }
    }
}

[System.Serializable]
public class Mission
{
    [TextArea(3, 5)] 
    public string description;
    public string pickupStationName;
    public string deliverStationName; 
}