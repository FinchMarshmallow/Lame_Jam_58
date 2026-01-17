using UnityEngine;

public class DistanceActivator : MonoBehaviour
{
    public GameObject uiCanvas; 
    public Transform player;    
    public float showDistance = 500f; 

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < showDistance)
        {
            if (!uiCanvas.activeSelf) uiCanvas.SetActive(true);

            uiCanvas.transform.LookAt(uiCanvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
        else
        {
            if (uiCanvas.activeSelf) uiCanvas.SetActive(false);
        }
    }
}