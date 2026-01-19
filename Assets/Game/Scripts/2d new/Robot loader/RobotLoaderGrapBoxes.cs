using UnityEngine;

public class RobotLoaderGrapBoxes : MonoBehaviour
{
	[SerializeField] private Transform pointMagnite;
	[SerializeField] private float force, radius;

	private Rigidbody _boxRb;
	private Transform _boxTr;
	private Transform _parentBox;
	private void OnTriggerEnter(Collider other)
	{
		_boxRb = other.gameObject.GetComponent<Rigidbody>();
		_boxTr = other.transform;

		_parentBox = other.transform.parent;

		_boxTr.transform.SetParent(pointMagnite);
	}

	private void OnTriggerExit(Collider other)
	{
		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

		if (rb != null && _boxRb == rb)
		{
			_boxTr.transform.SetParent(_parentBox);
			_boxRb = null;
		}
	}

	private void Update()
	{
		if(_boxRb == null || !Input.GetMouseButton(0))
			return;

		Vector3 
			direct = (pointMagnite.position - _boxRb.transform.position),
			forceV = direct * (Mathf.Clamp((direct.magnitude / radius) * force, 0, force) / Time.fixedDeltaTime);

		_boxRb.AddForce(forceV);	
	}
}
