using UnityEngine;
using UnityEngine.EventSystems;

public class IputEneble : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private GameObject[] offObj;
	[SerializeField] private float timeScale;

	public void Exit()
	{
		OffObj(false);
		Time.timeScale = 1f;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OffObj(true);
		Time.timeScale = timeScale;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		OffObj(false);
		Time.timeScale = 1f;
	}

	private void OffObj(bool isOff)
	{
		for (int i = 0; i < offObj.Length; i++)
			offObj[i].gameObject.SetActive(isOff);
	}
}
