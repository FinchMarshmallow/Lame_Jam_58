using UnityEngine;

public class RobotLoaderMove : MonoBehaviour
{
	[SerializeField] private float linerForce, rotationForce;

	[SerializeField] private Transform compass;

	[SerializeField] private LayerMask layerMask;

	[SerializeField] private float distance;

	[SerializeField] private Vector3 offsetLookAt;

	[SerializeField] private Vector3 _input;

	private Rigidbody _rb;

	private Camera _cam;

	private void OnEnable()
	{
		_cam = Camera.main;
	}

	private void Awake()
	{
		TryGetComponent(out _rb);
	}

	private void Update()
	{
		GetInput();
		//GetRot();
		
		Move();
	}

	private void Move()
	{
		_rb.AddRelativeForce(new Vector3(0f, 0f, _input.x) * linerForce);
		_rb.AddTorque(Vector3.up * _input.y * rotationForce);
	}

	private void GetInput()
	{
		_input.x = 0;
		_input.y = 0;

		if (Input.GetKey(KeyCode.W)) _input.x++;
		if (Input.GetKey(KeyCode.S)) _input.x--;

		if (Input.GetKey(KeyCode.A)) _input.y--;
		if (Input.GetKey(KeyCode.D)) _input.y++;
	}

	private void GetRot()
	{
		Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

		if (!Physics.Raycast(ray, out RaycastHit hit, distance, layerMask))
			return;

		

		compass.transform.LookAt(hit.point + ((hit.point - transform.position).normalized) * 10f);
		compass.Rotate(offsetLookAt);
		
		transform.rotation = Quaternion.Lerp(compass.rotation, transform.rotation, 0.2f);
		
		Vector3 angles = transform.localEulerAngles;
		angles.x = 0f;
		angles.z = 0f;
		
		transform.localEulerAngles = angles;
	}
}
