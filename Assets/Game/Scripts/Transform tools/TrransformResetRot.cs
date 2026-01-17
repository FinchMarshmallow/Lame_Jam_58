using UnityEngine;

public class TrransformResetRot : MonoBehaviour
{
	[SerializeField] private Vector3 resetMask = Vector3.one;
	[SerializeField] private Transform reset;
	private void FixedUpdate()
	{
		Vector3 rot = reset.localEulerAngles;
		rot.x *= resetMask.x;
		rot.y *= resetMask.y;
		rot.z *= resetMask.z;

		reset.localEulerAngles = rot;
	}
}
