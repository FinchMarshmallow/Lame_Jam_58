using UnityEngine;
using UpdatesIntarfaces;

public class TransformCopyRotation : MonoBehaviour
{
	[SerializeField] private Transform origin, copyist;
	[SerializeField] private float interpolation;
	[SerializeField] private bool isSlerp;
	[SerializeField] private TypeUpdate typeUpdate;
	[SerializeField] private Vector3 offset;

	private void Update()
	{
		if ((typeUpdate & TypeUpdate.Update) == 0)
			return;

		CopyPosition(Time.deltaTime);
	}

	private void LateUpdate()
	{
		if ((typeUpdate & TypeUpdate.LateUpdate) == 0)
			return;

		CopyPosition(Time.deltaTime);
	}

	private void FixedUpdate()
	{
		if ((typeUpdate & TypeUpdate.FixedUpdate) == 0)
			return;

		CopyPosition(Time.fixedDeltaTime);
	}

	private void CopyPosition(float frameDelta)
	{
		if (interpolation == 1)
		{
			copyist.rotation = origin.rotation;
			copyist.Rotate(offset);
		}
		else if (isSlerp)
		{
			copyist.rotation = Quaternion.Slerp(copyist.rotation, origin.rotation, interpolation / frameDelta);
			copyist.Rotate(offset);
		}
		else
		{
			copyist.rotation = Quaternion.Lerp(copyist.rotation, origin.rotation, interpolation * frameDelta);
			copyist.Rotate(offset);
		}
	}
	
}
