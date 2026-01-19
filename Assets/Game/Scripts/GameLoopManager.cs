using UnityEngine;
using System.Collections.Generic;

public class GameLoopManager : MonoBehaviour
{
    [Header("Managers")]
    public MissionSystem missionSystem;

    [Header("Game Modes")]
    public GameObject shipHUD;          // The UI for flying
    public GameObject minigameRoot;     // The "Ship -> view -> 2d" object

    [Header("3D Visuals to Hide")]
    // DRAG "up" and "down" HERE
    public List<GameObject> objectsToHide;

    [Header("Ship Systems")]
    public DockingRealization dockingSystem;

    private void Start()
    {
        // 1. Ensure 2D game is OFF
        if (minigameRoot) minigameRoot.SetActive(false);

        // 2. Ensure Ship HUD is ON
        if (shipHUD) shipHUD.SetActive(true);

        // 3. Ensure 3D Visuals are ON
        Set3DVisuals(true);
    }

    // Called from DockingShipHandler -> OnDock
    public void StartCargoPhase()
    {
        Debug.Log("Docking Complete. Switching to 2D Mode.");

        // Hide Ship UI
        if (shipHUD) shipHUD.SetActive(false);

        // Hide 3D Ship Parts (up/down) so we can see the robot
        Set3DVisuals(false);

        // Show 2D Robot
        if (minigameRoot) minigameRoot.SetActive(true);
    }

    // Called from BoxTriggerChecker -> OnConditionMet
    public void CompleteCargoPhase()
    {
        Debug.Log("Cargo Loaded. Undocking.");

        // Update Mission
        if (missionSystem != null) missionSystem.CompleteCurrentMission();

        // Undock Ship
        if (dockingSystem != null) dockingSystem.Undock();

        // Hide 2D Robot
        if (minigameRoot) minigameRoot.SetActive(false);

        // Show Ship UI
        if (shipHUD) shipHUD.SetActive(true);

        // Show 3D Ship Parts again
        Set3DVisuals(true);
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
        Set3DVisuals(true);
        if (shipHUD) shipHUD.SetActive(true);
        if (minigameRoot) minigameRoot.SetActive(false);
    }
}
