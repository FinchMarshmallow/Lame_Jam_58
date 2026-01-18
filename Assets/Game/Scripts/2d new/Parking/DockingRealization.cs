using System.Collections;
using UnityEngine;

public class DockingRealization : MonoBehaviour
{
	[SerializeField] private GameObject shipUp;
	[SerializeField] private Transform cam, ship;
	[SerializeField] private MonoBehaviour[] offing, oning;
	[SerializeField] private float interpolationPos, interpolationRot;

	private DockingStantionHandler _stationHandler;
	private DockingShipHandler _shipHandler;

	private Vector3 _camStartPos;
	private Quaternion _camStartRot;

	public void Dock(DockingStantionHandler station, DockingShipHandler shipH)
	{
		_stationHandler = station;
		_shipHandler = shipH;

		_camStartPos = cam.transform.localPosition;
		_camStartRot = cam.transform.localRotation;

		ship.position = station.ShipPoint.position;
		ship.rotation = station.ShipPoint.rotation;

		Switch(true);
		
		StopAllCoroutines();
		StartCoroutine(Docking());
		
		station.Dock();
	}

	public void Undock()
	{
		_stationHandler.Undock();
		_shipHandler.Undock();

		Switch(false);

		StopAllCoroutines();
		StartCoroutine(Undocking());
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
		while((cam.position - _stationHandler.CmeraPoint.position).sqrMagnitude > 0.1f)
		{
			cam.position = Vector3.Lerp(cam.position, _stationHandler.CmeraPoint.position, interpolationPos);
			cam.rotation = Quaternion.Lerp(cam.rotation, _stationHandler.CmeraPoint.rotation, interpolationRot);

			yield return null;
		}

		cam.position = _stationHandler.CmeraPoint.position;
		cam.rotation = _stationHandler.CmeraPoint.rotation;
	}

	private IEnumerator Undocking()
	{
		while ((cam.localPosition - _camStartPos).sqrMagnitude > 0.1f)
		{
			cam.localPosition = Vector3.Lerp(cam.localPosition, _camStartPos, interpolationPos);
			cam.localRotation = Quaternion.Lerp(cam.localRotation, _camStartRot, interpolationRot);

			yield return null;
		}

		cam.localPosition = _camStartPos;
		cam.localRotation = _camStartRot;
	}
}
