using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CompassSystem : MonoBehaviour
{
    [Header("UI Settings")]
    public RectTransform compassContainer;
    public GameObject iconPrefab; 
    public GameObject textPrefab;

    public float compassWidth = 800f;
    public float viewAngle = 180f;

    [Header("References")]
    public Transform cameraTransform;

    // Списки маркеров
    private List<CompassMarker> worldDirections = new List<CompassMarker>();
    private CompassMarker questMarker = null;

    private class CompassMarker
    {
        public Vector3 worldPosition;
        public Transform targetTransform;
        public RectTransform uiElement;
        public Image imageComponent;
        public TextMeshProUGUI textComponent;
    }

    void Start()
    {
        if (cameraTransform == null) cameraTransform = Camera.main.transform;

        CreateDirectionMarker(Vector3.forward * 10000, "N");
        CreateDirectionMarker(Vector3.right * 10000, "E");
        CreateDirectionMarker(Vector3.back * 10000, "S");
        CreateDirectionMarker(Vector3.left * 10000, "W");
    }

    void CreateDirectionMarker(Vector3 pos, string label)
    {
        if (textPrefab == null) return;

        GameObject obj = Instantiate(textPrefab, compassContainer);
        var marker = new CompassMarker();
        marker.worldPosition = pos;
        marker.uiElement = obj.GetComponent<RectTransform>();
        marker.textComponent = obj.GetComponent<TextMeshProUGUI>();

        if (marker.textComponent != null) marker.textComponent.text = label;

        worldDirections.Add(marker);
    }

    public void SetQuestMarker(Transform target)
    {
        ClearQuestMarkers(); 

        GameObject obj = Instantiate(iconPrefab, compassContainer);
        questMarker = new CompassMarker();
        questMarker.targetTransform = target;
        questMarker.uiElement = obj.GetComponent<RectTransform>();
        questMarker.imageComponent = obj.GetComponent<Image>();
        questMarker.imageComponent.color = Color.yellow;
    }

    public void ClearQuestMarkers()
    {
        if (questMarker != null && questMarker.uiElement != null)
        {
            Destroy(questMarker.uiElement.gameObject);
        }
        questMarker = null;
    }

    void Update()
    {
        if (cameraTransform == null) return;

        foreach (var dir in worldDirections)
        {
            UpdateMarkerPosition(dir, dir.worldPosition);
        }

        if (questMarker != null && questMarker.targetTransform != null)
        {
            UpdateMarkerPosition(questMarker, questMarker.targetTransform.position);
        }
    }

    void UpdateMarkerPosition(CompassMarker marker, Vector3 targetPos)
    {
        Vector3 camFwd = cameraTransform.forward;
        camFwd.y = 0;

        Vector3 dirToTarget = targetPos - cameraTransform.position;
        dirToTarget.y = 0;

        float angle = Vector3.SignedAngle(camFwd, dirToTarget, Vector3.up);
        float posX = (angle / viewAngle) * compassWidth;

        marker.uiElement.anchoredPosition = new Vector2(posX, 0);

        float alpha = Mathf.Abs(angle) > (viewAngle / 2) ? 0f : 1f;

        if (marker.imageComponent != null)
        {
            var c = marker.imageComponent.color;
            c.a = alpha;
            marker.imageComponent.color = c;
        }
        if (marker.textComponent != null)
        {
            marker.textComponent.alpha = alpha;
        }
    }
}