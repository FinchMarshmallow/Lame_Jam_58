using UnityEngine;

public class MagneticGrab : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 0.5f;
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private string boxTag = "box";
    [SerializeField] private bool alignRotationWithMagnet = false; // ����� �����

    private GameObject currentBox;
    private bool isMagnetActive = false;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    void Update()
    {
        // ��������� ������� ��� ������������ �������
        if (Input.GetMouseButtonDown(0))
        {
            if (!isMagnetActive)
            {
                // �������� �������� ����
                TryGrabBox();
            }
            else
            {
                // ��������� ����
                ReleaseBox();
            }
        }
    }

    private void TryGrabBox()
    {
        // ������� ��� ����� � �������
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRadius, boxLayer);
        GameObject closestBox = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(boxTag))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBox = collider.gameObject;
                }
            }
        }

        // ���� ����� ���� - ������� ���
        if (closestBox != null)
        {
            currentBox = closestBox;
            isMagnetActive = true;

            // ��������� ������������ �������� � �������
            originalRotation = currentBox.transform.rotation;
            originalScale = currentBox.transform.localScale;

            // ������ ���� �������� ��������
            currentBox.transform.SetParent(transform);

            // ���������� ��������� ���������� ��� ��������� ����������
            currentBox.transform.localPosition = Vector3.zero;

            // ���� ����� ��������� ���� � ��������
            if (alignRotationWithMagnet)
            {
                // ����������� �������� � ��������
                currentBox.transform.localRotation = Quaternion.identity;
            }
            else
            {
                // ��������� ������������ �������� (� ������ ��������)
                currentBox.transform.localRotation = Quaternion.Inverse(transform.rotation) * originalRotation;
            }

            // ��������� ������������ �������
            currentBox.transform.localScale = originalScale;

            // ��������� ������ �� ����� �������
            Rigidbody2D rb = currentBox.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = false;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // ��������� ��������� ��� ������ ��� ���������, ����� �������� ������������
            Collider2D boxCollider = currentBox.GetComponent<Collider2D>();
            if (boxCollider != null)
            {
                boxCollider.isTrigger = true;
            }

            Debug.Log($"Magnet ON: Grabbed box: {currentBox.name}");
        }
    }

    private void ReleaseBox()
    {
        if (currentBox != null)
        {
            // ��������� ������� ���������� �������� ����� �������������
            Quaternion currentGlobalRotation = currentBox.transform.rotation;

            // ����������� �� ��������
            currentBox.transform.SetParent(null);

            // ��������������� ����������� ���������� ��������
            currentBox.transform.rotation = currentGlobalRotation;

            // ��������������� ������������ �������
            currentBox.transform.localScale = originalScale;

            // �������� ������ �������
            Rigidbody2D rb = currentBox.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;
            }

            // �������� ��������� �������
            Collider2D boxCollider = currentBox.GetComponent<Collider2D>();
            if (boxCollider != null)
            {
                boxCollider.isTrigger = false;
            }

            Debug.Log($"Magnet OFF: Released box: {currentBox.name}");
        }

        currentBox = null;
        isMagnetActive = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isMagnetActive ? Color.blue : Color.green;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}