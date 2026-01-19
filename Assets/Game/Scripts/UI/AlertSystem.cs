using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem Instance; // Чтобы вызывать отовсюду
    public TextMeshProUGUI alertText;

    void Awake()
    {
        Instance = this;
        if (alertText != null) alertText.text = ""; // Скрываем при старте
    }

    public void ShowAlert(string message, Color color)
    {
        if (alertText == null) return;

        StopAllCoroutines(); // Сбрасываем старую анимацию, если она была
        StartCoroutine(AnimateText(message, color));
    }

    IEnumerator AnimateText(string msg, Color col)
    {
        alertText.text = msg;
        alertText.color = col;
        alertText.alpha = 1f;

        // Текст висит 2 секунды
        yield return new WaitForSeconds(2f);

        // Плавно исчезает
        float duration = 1f;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            alertText.alpha = Mathf.Lerp(1f, 0f, timer / duration);
            yield return null;
        }
        alertText.text = "";
    }
}