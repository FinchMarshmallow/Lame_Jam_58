using UnityEngine;

public class TransformRandomRotation : MonoBehaviour
{
	[SerializeField] private float maxSpeed, maxStep;
	[SerializeField] private Vector3 randomStep;

	[SerializeField] private Vector3 startRotate;

	private Vector3 _currentStep = new(), _currentRot = new();

	private void Awake()
	{
		_currentRot = startRotate;
	}

	private void Update()
	{
		_currentStep.x = Random.Range(-randomStep.x, randomStep.x);
		_currentStep.y = Random.Range(-randomStep.y, randomStep.y);
		_currentStep.z = Random.Range(-randomStep.z, randomStep.z);


		_currentStep = Vector3.ClampMagnitude(_currentStep, maxStep);

		_currentRot += _currentStep;

		_currentRot = Vector3.ClampMagnitude( _currentRot, maxSpeed);

		transform.Rotate(_currentRot * Time.deltaTime * 60f);
	}
}
