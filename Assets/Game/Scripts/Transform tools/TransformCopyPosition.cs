using UnityEngine;
using UpdatesIntarfaces;

public class TransformCopyPosition : MonoBehaviour
{
	[SerializeField] private Transform origin, copyist;
	[SerializeField] private bool isSmoot;
	[SerializeField] private float interpolation;
	[SerializeField] private TypeUpdate typeUpdate;
	[SerializeField] private Vector3 offset;

	private Vector3[] _oldPos = new Vector3[3];

	private void Update()
	{
		if ((typeUpdate & TypeUpdate.Update) == 0)
			return;

		CopyPosition(0, Time.deltaTime);
		_oldPos[0] = transform.position;
	}

	private void LateUpdate()
	{
		if ((typeUpdate & TypeUpdate.LateUpdate) == 0)
			return;

		CopyPosition(1, Time.deltaTime);
		_oldPos[1] = transform.position;
	}

	private void FixedUpdate()
	{
		if ((typeUpdate & TypeUpdate.FixedUpdate) == 0)
			return;

		CopyPosition(2, Time.fixedDeltaTime);
		_oldPos[2] = transform.position;
	}

	private void CopyPosition(int surce, float frameDelta)
	{
		if (interpolation == 1)
		{
			copyist.position = origin.position + offset;
		}
		else if (isSmoot)
		{
			Vector3 speed = _oldPos[surce] - transform.position;
			copyist.position = Vector3.SmoothDamp(copyist.position, origin.position + offset, ref speed, interpolation * frameDelta, float.MaxValue);
		}
		else
		{
			copyist.position = Vector3.Lerp(copyist.position, origin.position + offset, interpolation * frameDelta);
		}
	}
}
