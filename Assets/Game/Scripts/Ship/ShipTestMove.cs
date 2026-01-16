using UnityEngine;

public class ShipTestMove : MonoBehaviour
{
	[SerializeField] private float forceRotation;
	[SerializeField] private Vector3 forceLinear;

	private Rigidbody _rb;

	/*[HideInInspector]*/ public Vector3 inputLinear;
	/*[HideInInspector]*/ public Vector2 InputRot;

	private void Awake()
	{
		TryGetComponent(out _rb);
	}

	private void FixedUpdate()
	{
		Vector3 linearForce = new();

		linearForce.x = inputLinear.x * forceLinear.x;
		linearForce.y = inputLinear.y * forceLinear.y;
		linearForce.z = inputLinear.z * forceLinear.z;

		_rb.AddRelativeForce(linearForce);

		_rb.AddRelativeTorque(Vector3.left * InputRot.y * forceRotation);
		_rb.AddTorque(Vector3.up * InputRot.x * forceRotation);
	}
}
