using UnityEngine;

public class MouseFollower2D : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool smoothMovement = true;
    [SerializeField] private float smoothSpeed = 0.1f;

    [Header("Ограничения")]
    [SerializeField] private bool clampToScreen = true;
    [SerializeField] private float minX = 0f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 10f;

    [Header("Ограничение радиуса")]
    [SerializeField] private bool useRadiusLimit = true;
    [SerializeField] private float maxRadius = 3f;
    [SerializeField] private bool drawRadiusGizmo = true;
    [SerializeField] private Color radiusGizmoColor = Color.yellow;

    [Header("Поворот на курсор")]
    [SerializeField] private bool rotateToMouse = false;
    [SerializeField] private Transform lookPoint; // Точка, которая должна смотреть на курсор
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private bool smoothRotation = true;

    [Header("Ограничения поворота")]
    [SerializeField] private bool clampRotation = false;
    [SerializeField] private float minAngle = -180f;
    [SerializeField] private float maxAngle = 180f;

    [Header("Смещение цели")]
    [SerializeField] private Vector2 lookOffset = Vector2.zero;

    private Camera mainCamera;
    private Vector3 targetPosition;
    private Vector3 mouseWorldPosition;
    private Transform parentTransform;
    private Vector3 parentOffset; // Смещение от родителя в локальных координатах

    void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position;

        // Получаем родительский объект
        parentTransform = transform.parent;

        if (parentTransform != null)
        {
            // Сохраняем начальное смещение от родителя
            parentOffset = transform.localPosition;
        }
        else
        {
            Debug.LogWarning("Объект не имеет родителя. Ограничение радиуса не будет работать.");
            useRadiusLimit = false;
        }

        // Автоматически назначаем lookPoint, если не задан
        if (rotateToMouse && lookPoint == null)
        {
            // Ищем дочерний объект с тегом "LookPoint" или первый дочерний
            GameObject lookPointObj = GameObject.FindGameObjectWithTag("LookPoint");
            if (lookPointObj != null)
            {
                lookPoint = lookPointObj.transform;
            }
            else if (transform.childCount > 0)
            {
                lookPoint = transform.GetChild(0);
                Debug.Log($"Автоназначение: lookPoint = {lookPoint.name}");
            }
            else
            {
                Debug.LogWarning("LookPoint не назначен и нет дочерних объектов. Поворот отключен.");
                rotateToMouse = false;
            }
        }
    }

    void Update()
    {
        UpdateMousePosition();
        FollowMouse();

        if (rotateToMouse && lookPoint != null)
        {
            RotateToMouse();
        }
    }

    void UpdateMousePosition()
    {
        // Получаем позицию мыши в мировых координатах
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

    void FollowMouse()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3 desiredPosition = mouseWorldPos;

        // Применяем ограничение радиуса, если оно включено
        if (useRadiusLimit && parentTransform != null)
        {
            desiredPosition = ApplyRadiusLimit(desiredPosition);
        }

        if (smoothMovement)
        {
            targetPosition = desiredPosition;
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime * 60);
            transform.position = ClampPosition(newPosition);
        }
        else
        {
            transform.position = ClampPosition(desiredPosition);
        }
    }

    Vector3 ApplyRadiusLimit(Vector3 desiredPosition)
    {
        // Получаем позицию родителя в мировых координатах
        Vector3 parentWorldPos = parentTransform.position;

        // Вычисляем вектор от родителя к желаемой позиции
        Vector3 toDesiredPos = desiredPosition - parentWorldPos;

        // Ограничиваем длину вектора максимальным радиусом
        if (toDesiredPos.magnitude > maxRadius)
        {
            toDesiredPos = toDesiredPos.normalized * maxRadius;
        }

        // Возвращаем ограниченную позицию
        return parentWorldPos + toDesiredPos;
    }

    void RotateToMouse()
    {
        // Вычисляем направление от lookPoint к курсору
        Vector3 directionToMouse = mouseWorldPosition - lookPoint.position;
        directionToMouse += new Vector3(lookOffset.x, lookOffset.y, 0);
        directionToMouse.z = 0;

        // Вычисляем угол поворота
        float targetAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg - 90f;

        // Применяем ограничения угла, если включены
        if (clampRotation)
        {
            targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);
        }

        if (smoothRotation)
        {
            // Плавный поворот
            float currentAngle = transform.rotation.eulerAngles.z;
            float smoothedAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
        }
        else
        {
            // Мгновенный поворот
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        }
    }

    Vector3 ClampPosition(Vector3 position)
    {
        if (!clampToScreen)
            return position;

        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector3(clampedX, clampedY, position.z);
    }

    // Метод для привязки к родителю с учетом радиуса
    public void SnapToParentRadius()
    {
        if (parentTransform == null || !useRadiusLimit) return;

        Vector3 toParent = transform.position - parentTransform.position;
        if (toParent.magnitude > maxRadius)
        {
            transform.position = parentTransform.position + toParent.normalized * maxRadius;
        }
    }

    // Метод для получения текущего расстояния до родителя
    public float GetDistanceToParent()
    {
        if (parentTransform == null) return 0f;
        return Vector3.Distance(transform.position, parentTransform.position);
    }

    // Метод для получения нормализованного расстояния (0-1)
    public float GetNormalizedDistanceToParent()
    {
        if (parentTransform == null || !useRadiusLimit) return 0f;
        return Mathf.Clamp01(GetDistanceToParent() / maxRadius);
    }

    public void SetScreenBounds()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;
    }

    // Метод для ручной настройки поворота
    public void SetLookPoint(Transform newLookPoint)
    {
        lookPoint = newLookPoint;
        rotateToMouse = (newLookPoint != null);
    }

    // Метод для включения/выключения поворота
    public void SetRotationEnabled(bool enabled)
    {
        rotateToMouse = enabled;
    }

    // Метод для изменения радиуса
    public void SetMaxRadius(float newRadius)
    {
        maxRadius = Mathf.Max(0.1f, newRadius);
    }

    // Метод для получения текущего угла к курсору
    public float GetAngleToMouse()
    {
        if (lookPoint == null) return 0f;

        Vector3 direction = mouseWorldPosition - lookPoint.position;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void OnDrawGizmosSelected()
    {
        if (!clampToScreen) return;

        Gizmos.color = Color.green;
        Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0.1f);
        Gizmos.DrawWireCube(center, size);

        // Визуализация направления к курсору
        if (Application.isPlaying && rotateToMouse && lookPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(lookPoint.position, mouseWorldPosition);
            Gizmos.DrawWireSphere(mouseWorldPosition, 0.2f);
        }

        // Визуализация радиуса ограничения
        if (drawRadiusGizmo && useRadiusLimit)
        {
            Transform currentParent = parentTransform != null ? parentTransform : transform.parent;

            if (currentParent != null)
            {
                Gizmos.color = radiusGizmoColor;
                Vector3 parentPos = Application.isPlaying ? currentParent.position : currentParent.transform.position;
                Gizmos.DrawWireSphere(parentPos, maxRadius);

                // Линия от родителя к объекту
                Gizmos.color = Color.cyan;
                Vector3 objPos = Application.isPlaying ? transform.position : transform.position;
                Gizmos.DrawLine(parentPos, objPos);

                // Отображение текущего расстояния
#if UNITY_EDITOR
                float distance = Vector3.Distance(parentPos, objPos);
                UnityEditor.Handles.Label(
                    (parentPos + objPos) / 2,
                    $"Distance: {distance:F2}/{maxRadius:F2}"
                );
#endif
            }
        }
    }
}