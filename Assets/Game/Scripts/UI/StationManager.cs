using UnityEngine;
using System.Collections.Generic;

public class StationManager : MonoBehaviour
{
    public static StationManager Instance; // Синглтон, чтобы обращаться откуда угодно

    // Словарь: Ключ = Файл данных, Значение = Трансформ в сцене
    private Dictionary<StationData, Transform> stationMap = new Dictionary<StationData, Transform>();

    void Awake()
    {
        Instance = this;
        FindAllStations();
    }

    void FindAllStations()
    {
        StationObject[] foundStations = FindObjectsByType<StationObject>(FindObjectsSortMode.None);

        foreach (var station in foundStations)
        {
            if (station.stationData != null)
            {
                if (!stationMap.ContainsKey(station.stationData))
                {
                    stationMap.Add(station.stationData, station.transform);
                    Debug.Log($"Registered Station: {station.stationData.stationName}");
                }
            }
        }
    }

    // Метод: дай мне файл данных, я дам тебе Transform
    public Transform GetStationTransform(StationData data)
    {
        if (data != null && stationMap.ContainsKey(data))
        {
            return stationMap[data];
        }

        Debug.LogError($"Station not found in scene: {data?.name}");
        return null;
    }

    // Метод: получить вообще все станции (для Карты)
    public List<Transform> GetAllStationTransforms()
    {
        return new List<Transform>(stationMap.Values);
    }
}