using UnityEngine;
using UnityEngine.SceneManagement;

public class HandlerSwitchScene : MonoBehaviour
{
	public void SwirchByID(int id)
	{
		SceneManager.LoadScene(id);
	}

	public void SwirchByName(string name)
	{
		SceneManager.LoadScene(name);
	}
}
