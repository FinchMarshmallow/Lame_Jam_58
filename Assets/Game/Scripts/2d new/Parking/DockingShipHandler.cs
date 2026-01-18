using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class DockingShipHandler : MonoBehaviour
{
	[SerializeField] private UnityEvent dock, undock;

	[SerializeField] private Transform ship;

	[SerializeField] private ParckingPoint[] pointsDocking;
	[SerializeField] private float radiusPoints;
	[SerializeField] private LayerMask maskPoints;

	[SerializeField] private KeyCode keyDocking = KeyCode.E;

	[SerializeField] private DockingRealization dockingRealization;

	[SerializeField] private DockingStantionHandler _targetStation;

	private bool _isProcessDocking = false, _isCanDocking = false, _isWeDocked = false;

	private Rigidbody _rb;

	// gizmo
	private int _fallIndex = -1;

	private void Awake()
	{
		ship.TryGetComponent(out _rb);
	}

	private void Update()
	{
		if (!_isProcessDocking)
			return;

		bool isCan = true;

		for(int i = 0; i < pointsDocking.Length; i++)
		{
			Collider[] colliders = Physics.OverlapSphere(pointsDocking[i].transform.position, radiusPoints, maskPoints);
			

			if (colliders.Length <= 0)
			{
				isCan = false;
				_fallIndex = i;
				pointsDocking[i].Fall();
			}
			else
			{
				pointsDocking[i].Good();
			}
		}

		_isCanDocking = isCan;

		if (isCan)
			_fallIndex = -1;

		if(_isCanDocking && !_isWeDocked && Input.GetKeyDown(keyDocking))
		{
			_isWeDocked = true;
			_rb.isKinematic = true;
			dock?.Invoke();
			dockingRealization.Dock(_targetStation, this);
		}
	}

	public void Undock()
	{
		_rb.isKinematic = false;
		_isWeDocked = false;
		undock?.Invoke();
	}

	private void OnTriggerEnter(Collider other)
	{
		_isProcessDocking = true;
		if (other.TryGetComponent(out DockingStantionHandler sh))
		{
			_targetStation = sh;
		}

		for(int i = 0;i < pointsDocking.Length; i++)
		{
			pointsDocking[i].gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		_isProcessDocking = false;

		for (int i = 0; i < pointsDocking.Length; i++)
		{
			pointsDocking[i].gameObject.SetActive(false);
		}
	}

	private void OnDrawGizmos()
	{
		if (!_isProcessDocking)
			return;

		Color
			good = new Color (0.1f, 0.9f, 0.1f),
			fall = new Color (0.9f, 0.1f, 0.1f);

		for(int i = 0; i < pointsDocking.Length; i++)
		{
			if (i != _fallIndex)
				Gizmos.color = good;
			else
				Gizmos.color = fall;

			Gizmos.DrawWireSphere(pointsDocking[i].transform.position, radiusPoints);
		}
	}
}