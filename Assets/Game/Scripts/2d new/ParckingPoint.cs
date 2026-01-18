using UnityEngine;
using UnityEngine.Events;

public class ParckingPoint : MonoBehaviour
{
	[SerializeField] private UnityEvent good, fall;
	
	private bool _state = true, _isInit = false;

	public void Good()
	{
		if (_state && !_isInit)
			return;

		_state = true;
		_isInit = true;

		good?.Invoke();
	}

	public void Fall()
	{
		if (!_state && !_isInit)
			return;

		_state = false;
		_isInit = true;

		fall?.Invoke();
	}
}
