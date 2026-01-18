using UnityEngine;

// Это создаст пункт в меню Unity, чтобы создавать новые станции как файлы
[CreateAssetMenu(fileName = "New Station Data", menuName = "Game/Station Data")]
public class StationData : ScriptableObject
{
    public string stationName; // Имя (отображается в UI)
    [TextArea] public string description; // Описание (если нужно)

    // Сюда можно добавить иконку станции для карты
    public Sprite mapIcon;
}