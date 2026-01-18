using System.Collections;
using UnityEngine;

public class DockingRealization : MonoBehaviour
{
	[SerializeField] private GameObject shipUp;
	[SerializeField] private Transform cam, ship;
	[SerializeField] private MonoBehaviour[] offing, oning;
	[SerializeField] private float interpolationPos, interpolationRot;

	private DockingStantionHandler _station;

	private Vector3 _camStartPos;
	private Quaternion _camStartRot;

	public void Dock(DockingStantionHandler station)
	{
		_station = station;

		_camStartPos = cam.transform.localPosition;
		_camStartRot = cam.transform.localRotation;

		ship.position = station.ShipPoint.position;
		ship.rotation = station.ShipPoint.rotation;

		Switch(true);
		
		StopAllCoroutines();
		StartCoroutine(Docking());
		
		station.Dock();
	}

	public void Undock(DockingStantionHandler station)
	{
		_station = null;

		Switch(false);

		StopAllCoroutines();
		StartCoroutine(Undocking());

		station.Undock();
	}

	private void Switch(bool _isDock)
	{
		for (int i = 0; i < offing.Length; i++)
		{
			offing[i].enabled = !_isDock;
		}

		for (int i = 0; i < oning.Length; i++)
		{
			oning[i].enabled = _isDock;
		}
	}

	private IEnumerator Docking()
	{
		while((cam.position - _station.CmeraPoint.position).sqrMagnitude > 0.1f)
		{
			cam.position = Vector3.Lerp(cam.position, _station.CmeraPoint.position, interpolationPos);
			cam.rotation = Quaternion.Lerp(cam.rotation, _station.CmeraPoint.rotation, interpolationRot);

			yield return null;
		}

		cam.position = _station.CmeraPoint.position;
		cam.rotation = _station.CmeraPoint.rotation;
	}

	private IEnumerator Undocking()
	{
		while ((cam.localPosition - _camStartPos).sqrMagnitude > 0.1f)
		{
			cam.localPosition = Vector3.Lerp(cam.position, _camStartPos, interpolationPos);
			cam.localRotation = Quaternion.Lerp(cam.rotation, _camStartRot, interpolationRot);

			yield return null;
		}

		cam.localPosition = _camStartPos;
		cam.localRotation = _camStartRot;
	}
}
