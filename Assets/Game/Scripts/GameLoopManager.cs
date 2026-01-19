using UnityEngine;
using System.Collections.Generic;

public class GameLoopManager : MonoBehaviour
{
    [Header("Managers")]
    public MissionSystem missionSystem;

    [Header("Game Modes")]
    public GameObject shipHUD;          // The UI for flying

    [Header("3D Visuals to Hide")]
    // DRAG "up" and "down" HERE
    public List<GameObject> objectsToHide;

    [Header("Ship Systems")]
    public DockingRealization dockingSystem;

    private void Start()
    {
        // 2. Ensure Ship HUD is ON
        if (shipHUD) shipHUD.SetActive(true);

    }

    // Called from DockingShipHandler -> OnDock
    public void StartCargoPhase()
    {
        Debug.Log("Docking Complete. Switching to 2D Mode.");

        // Hide Ship UI
        if (shipHUD) shipHUD.SetActive(false);

    }

    // Called from BoxTriggerChecker -> OnConditionMet
    public void CompleteCargoPhase()
    {
        Debug.Log("Cargo Loaded. Undocking.");

        // Update Mission
        if (missionSystem != null) missionSystem.CompleteCurrentMission();

        // Undock Ship
        if (dockingSystem != null) dockingSystem.Undock();

        // Show Ship UI
        if (shipHUD) shipHUD.SetActive(true);

    }

    private void Set3DVisuals(bool isActive)
    {
        foreach (var obj in objectsToHide)
        {
            if (obj != null) obj.SetActive(isActive);
        }
 
    
    }


    public void RestoreShipVisuals()
    {
        if (shipHUD) shipHUD.SetActive(true);
    }
}
