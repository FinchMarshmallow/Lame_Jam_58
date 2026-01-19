using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShipInputSlider : MonoBehaviour, IDragHandler, IDropHandler, IPointerExitHandler, ICastHandler
{
	[SerializeField] private Vector2 minMax;
	[SerializeField] private Transform viewPoint, viewSelect, start, end;
	[SerializeField] private float round;
	[SerializeField] private GameObject[] offObj;

	[SerializeField] private UnityEvent pressEvent;

	public float Value;

	private bool _isDrag;
	private float _distance;

	private void Awake()
	{
		_distance = (start.position - end.position).magnitude;
	}

	public void OnDrag(PointerEventData eventData)
	{
		
	}

	public void OnDrop(PointerEventData eventData)
	{
		_isDrag = false;
		OffObj(true);
		viewPoint.gameObject.SetActive(true);
		viewSelect.gameObject.SetActive(false);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_isDrag = false;
		OffObj(true);
		viewPoint.gameObject.SetActive(true);
		viewSelect.gameObject.SetActive(false);
	}

	public void AimEnter(Vector3 pos)
	{
		if (Input.GetMouseButton(1) || !Input.GetMouseButton(0))
			return;

		if (!_isDrag)
		{
			pressEvent?.Invoke();
			OffObj(false);
			_isDrag = true;
			viewPoint.gameObject.SetActive(false);
			viewSelect.gameObject.SetActive(true);
		}
	}

	public void AimStay(Vector3 pos)
	{
		if (!_isDrag && Input.GetMouseButton(0) && !Input.GetMouseButton(1))
		{
			pressEvent?.Invoke();
			OffObj(false);
			_isDrag = true;
			viewPoint.gameObject.SetActive(false);
			viewSelect.gameObject.SetActive(true);
		}

		if (!_isDrag || Input.GetMouseButton(1))
		{
			OffObj(true);
			return;
		}

		Vector3 normPos;

		if ((pos - start.position).magnitude> _distance)
		{
			normPos = end.position;
		}
		if((pos - end.position).magnitude > _distance)
		{
			normPos = start.position;
		}
		else
		{
			normPos = start.position + ((pos - start.position).magnitude * (end.position - start.position).normalized);
		}

		float
			min = 0.2f,
			value = ((normPos - start.position).magnitude / _distance) * (minMax.y - minMax.x) + minMax.x;

		if (-min < value && min > value)
		{
			value = 0f;
			normPos = start.position + ((end.position - start.position) * 0.5f);
		}

		viewPoint.position = normPos;
		viewSelect.position = normPos;

		this.Value = value;
	}

	public void AimExit(Vector3 pos)
	{
		OffObj(true);
		_isDrag = false;
		viewPoint.gameObject.SetActive(true);
		viewSelect.gameObject.SetActive(false);
	}

	private void Update()
	{
		if(_isDrag && !Input.GetMouseButton(0))
		{
			_isDrag = false;
			viewPoint.gameObject.SetActive(true);
			viewSelect.gameObject.SetActive(false);
		}
	}

	private void OffObj(bool isOff)
	{
		for(int i = 0; i < offObj.Length; i++)
			offObj[i].gameObject.SetActive(isOff);
	}

	public void ResetValue()
	{
		Value = 0f;
		
		Vector3 normPos = start.position + ((end.position - start.position) * 0.5f);

		viewPoint.position = normPos;
		viewSelect.position = normPos;
	}
}
