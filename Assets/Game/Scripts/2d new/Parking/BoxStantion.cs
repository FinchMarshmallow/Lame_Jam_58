using UnityEngine;

public class BoxStantion : MonoBehaviour
{
	[SerializeField] private Transform box;

	private void OnTriggerEnter(Collider other)
	{
		box.SetParent(other.transform);
	}
}
