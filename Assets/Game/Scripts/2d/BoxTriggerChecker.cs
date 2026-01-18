using System.Collections.Generic;
using UnityEngine;

public class BoxTriggerChecker : MonoBehaviour
{
    public enum TriggerMode
    {
        AllBoxesNoPlayer,    // Вариант 1: все коробки в триггере, игрока нет
        EmptyTrigger         // Вариант 2: в триггере нет ни коробок, ни игроков
    }

    [Header("Настройки")]
    [SerializeField] private TriggerMode mode = TriggerMode.AllBoxesNoPlayer;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string boxTag = "Box";

    [Header("События")]
    public UnityEngine.Events.UnityEvent OnConditionMet;      // Основное событие (для обоих режимов)
    public UnityEngine.Events.UnityEvent OnTriggerEmpty;      // Дополнительное событие для EmptyTrigger режима

    [Header("Настройки режимов")]
    [Tooltip("Отключить триггер после срабатывания события")]
    [SerializeField] private bool disableAfterTrigger = true;
    [Tooltip("Автоматически проверять условие при выходе игрока/коробки")]
    [SerializeField] private bool autoCheckOnExit = true;

    [Header("Настройки зеркала")]
    [Tooltip("По оси X (горизонтальное зеркало)")]
    [SerializeField] private bool mirrorX = true;
    [Tooltip("По оси Y (вертикальное зеркало)")]
    [SerializeField] private bool mirrorY = false;
    [Tooltip("Точка отражения (центр сцены)")]
    [SerializeField] private Vector2 mirrorPoint = Vector2.zero;
    [Tooltip("Объекты, которые не должны отражаться")]
    [SerializeField] private string[] excludeFromMirrorTags = { "Player", "MainCamera" };

    private List<GameObject> boxesInTrigger = new List<GameObject>();
    private List<GameObject> allBoxesInScene = new List<GameObject>();
    private bool playerInTrigger = false;
    private bool conditionMet = false;
    private int playerCount = 0;

    // Структура для сохранения позиций объектов
    private class ObjectPositionData
    {
        public GameObject obj;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;
        public bool isActive;
    }

    private List<ObjectPositionData> savedPositions = new List<ObjectPositionData>();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Находим все объекты с тегом Box на сцене
        GameObject[] boxes = GameObject.FindGameObjectsWithTag(boxTag);
        allBoxesInScene.Clear();
        allBoxesInScene.AddRange(boxes);

        Debug.Log($"Найдено коробок на сцене: {allBoxesInScene.Count}");
        Debug.Log($"Режим триггера: {mode}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (conditionMet && disableAfterTrigger) return;

        if (other.CompareTag(boxTag))
        {
            if (!boxesInTrigger.Contains(other.gameObject))
            {
                boxesInTrigger.Add(other.gameObject);
            }
            CheckCondition();
        }
        else if (other.CompareTag(playerTag))
        {
            playerInTrigger = true;
            playerCount++;
            CheckCondition();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (conditionMet && disableAfterTrigger) return;

        if (other.CompareTag(boxTag))
        {
            boxesInTrigger.Remove(other.gameObject);

            if (autoCheckOnExit || mode == TriggerMode.EmptyTrigger)
            {
                CheckCondition();
            }
        }
        else if (other.CompareTag(playerTag))
        {
            playerCount--;
            if (playerCount <= 0)
            {
                playerInTrigger = false;
                playerCount = 0;
            }

            if (autoCheckOnExit || mode == TriggerMode.EmptyTrigger)
            {
                CheckCondition();
            }
        }
    }

    private void CheckCondition()
    {
        if (conditionMet && disableAfterTrigger) return;

        switch (mode)
        {
            case TriggerMode.AllBoxesNoPlayer:
                CheckAllBoxesNoPlayer();
                break;

            case TriggerMode.EmptyTrigger:
                CheckEmptyTrigger();
                break;
        }
    }

    private void CheckAllBoxesNoPlayer()
    {
        // Проверяем, все ли коробки из сцены находятся в триггере
        bool allBoxesInside = allBoxesInScene.Count > 0; // Если коробок нет, считаем условие выполненным

        foreach (GameObject box in allBoxesInScene)
        {
            if (box == null) continue;
            if (!boxesInTrigger.Contains(box))
            {
                allBoxesInside = false;
                break;
            }
        }

        // Условие: все коробки в триггере И игрок НЕ в триггере
        if (allBoxesInside && !playerInTrigger && !conditionMet)
        {
            conditionMet = true;
            OnConditionMet?.Invoke();
            Debug.Log($"Условие выполнено! Все коробки в триггере ({boxesInTrigger.Count}/{allBoxesInScene.Count}), игрока нет.");

            if (disableAfterTrigger)
            {
                DisableTrigger();
            }

            //переход в 3д
        }
    }

    private void CheckEmptyTrigger()
    {
        // Проверяем, пуст ли триггер (нет коробок и игроков)
        bool isEmptyTrigger = boxesInTrigger.Count == 0 && !playerInTrigger;

        if (isEmptyTrigger && !conditionMet)
        {
            conditionMet = true;
            OnConditionMet?.Invoke();
            Debug.Log($"Триггер пуст! Коробок: {boxesInTrigger.Count}, Игроков: {playerCount}");

            // Вызываем дополнительное событие для режима EmptyTrigger
            OnTriggerEmpty?.Invoke();

            if (disableAfterTrigger)
            {
                DisableTrigger();
                //конец уровня
            }
        }
        else if (!isEmptyTrigger && conditionMet)
        {
            // Сброс состояния, если кто-то снова вошел в триггер
            conditionMet = false;
            Debug.Log("Триггер снова занят, состояние сброшено");
        }
    }

    private void DisableTrigger()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        enabled = false;
        Debug.Log("Триггер отключен");
    }

    public void SaveAllObjectsPositions()
    {
        savedPositions.Clear();

        // Получаем все активные игровые объекты
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Пропускаем объекты, которые не должны сохраняться
            if (obj == null || obj.transform.parent != null)
                continue;

            ObjectPositionData data = new ObjectPositionData
            {
                obj = obj,
                position = obj.transform.position,
                rotation = obj.transform.rotation,
                localScale = obj.transform.localScale,
                isActive = obj.activeSelf
            };

            savedPositions.Add(data);
        }

        Debug.Log($"Сохранено позиций объектов: {savedPositions.Count}");
    }

    public void RestoreAllObjectsPositions()
    {
        if (savedPositions.Count == 0)
        {
            Debug.LogWarning("Нет сохраненных позиций для восстановления");
            return;
        }

        int restoredCount = 0;

        foreach (ObjectPositionData data in savedPositions)
        {
            if (data.obj == null) continue;

            data.obj.transform.position = data.position;
            data.obj.transform.rotation = data.rotation;
            data.obj.transform.localScale = data.localScale;
            data.obj.SetActive(data.isActive);
            restoredCount++;
        }

        Debug.Log($"Восстановлено позиций объектов: {restoredCount}");
    }

    public void MirrorScene()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int mirroredCount = 0;

        foreach (GameObject obj in allObjects)
        {
            Vector3 newPosition = obj.transform.position;

            if (mirrorX)
            {
                // Отражение по оси X относительно точки mirrorPoint
                newPosition.x = 2 * mirrorPoint.x - newPosition.x;
            }

            if (mirrorY)
            {
                // Отражение по оси Y относительно точки mirrorPoint
                newPosition.y = 2 * mirrorPoint.y - newPosition.y;
            }

            // Применяем новую позицию
            obj.transform.position = newPosition;

            // Для 2D объектов можно также отразить по оси X
            if (mirrorX)
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = !spriteRenderer.flipX;
                }

                // Отражаем локальный масштаб по оси X
                Vector3 newScale = obj.transform.localScale;
                newScale.x = -newScale.x;
                obj.transform.localScale = newScale;
            }

            mirroredCount++;
        }

        Debug.Log($"Зеркально отражено объектов: {mirroredCount}");
    }
}