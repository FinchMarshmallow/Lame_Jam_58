using UnityEngine;

public class CameraPointRotation : MonoBehaviour
{
	[SerializeField] private Vector2 sentity;
	[SerializeField] private Transform rotX, rotY;

	private float _curentX = 0f;

	private void Update()
	{
		if (!Input.GetMouseButton(1))
			return;

		Vector2 input = Input.mousePositionDelta;

		input.x *= sentity.x;
		input.y *= sentity.y;

		_curentX += input.y;
		_curentX = Mathf.Clamp(_curentX, -89.9f, 89.9f);

		rotY.Rotate(Vector3.up, input.x);
		rotX.localEulerAngles = Vector3.zero;
		rotX.Rotate(Vector3.left, _curentX);
	}
}
