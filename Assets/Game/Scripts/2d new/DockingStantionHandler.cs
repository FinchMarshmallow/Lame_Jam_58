using UnityEngine;
using UnityEngine.Events;

public class DockingStantionHandler : MonoBehaviour
{
	public Transform ShipPoint, CmeraPoint;

	[SerializeField] private UnityEvent dock, undock;

	public void Dock()
	{
		dock?.Invoke();
	}

	public void Undock()
	{
		undock?.Invoke();
	}
}
