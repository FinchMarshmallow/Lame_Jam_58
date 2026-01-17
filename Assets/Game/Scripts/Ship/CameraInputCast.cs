using UnityEngine;

public class CameraInputCast : MonoBehaviour
{
	[SerializeField] private float distance;
	[SerializeField] private LayerMask layer;

	private Camera _camera;
	private ICastHandler _currentHandler;

	private void Awake()
	{
		_camera = Camera.main;
	}

	private void Update()
	{
		Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, distance, layer, QueryTriggerInteraction.Ignore) &&
			hit.transform.TryGetComponent(out ICastHandler handler))
		{
			if (!ReferenceEquals(handler, _currentHandler))
			{
				if (_currentHandler != null)
					_currentHandler.AimExit(hit.point);

				_currentHandler = handler;
				_currentHandler.AimEnter(hit.point);

			}

			handler.AimStay(hit.point);
		}
		else if(_currentHandler != null)
		{
			_currentHandler.AimExit(hit.point);
			_currentHandler = null;
		}
	}
}

public interface ICastHandler
{
	void AimEnter(Vector3 pos);
	void AimStay(Vector3 pos);
	void AimExit(Vector3 pos);
}


