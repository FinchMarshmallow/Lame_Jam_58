using UnityEngine;
using UnityEngine.Events;

public class UndokingTermenal : MonoBehaviour
{
	[SerializeField] private DockingRealization docking;
	[SerializeField] private UnityEvent interact, deInteract;


	private bool _isInteract = false;

	private void Update()
	{
		if (!_isInteract)
			return;

		if (Input.GetKeyDown(KeyCode.E))
			docking.Undock();
	}

	private void OnTriggerEnter(Collider other)
	{
		_isInteract = true;
		interact?.Invoke();
	}

	private void OnTriggerExit(Collider other)
	{
		_isInteract = false;
		deInteract?.Invoke();
	}
}
