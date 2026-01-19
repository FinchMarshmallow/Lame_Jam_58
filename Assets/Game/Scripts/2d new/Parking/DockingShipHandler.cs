using UnityEngine;
using UnityEngine.Events;

public class DockingShipHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private KeyCode keyDocking = KeyCode.E;
    [SerializeField] private float radiusPoints = 1f;
    [SerializeField] private LayerMask maskPoints;

    [Header("References")]
    [SerializeField] private Transform shipTransform;
    [SerializeField] private Rigidbody shipRb;
    [SerializeField] private DockingRealization dockingRealization;
    [SerializeField] private GameObject dockPromptUI; // <--- Drag your "Press E" UI here!


    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dockSound;   // Drag 'звук стыковки'
    public AudioClip undockSound; // Drag 'звук отстыковки'

    [Header("Docking Points")]
    [SerializeField] private ParckingPoint[] pointsDocking;

    // Events
    public UnityEvent onDock;
    public UnityEvent onUndock;

    private DockingStantionHandler _targetStation;
    private bool _isProcessDocking = false; // Are we inside the trigger?
    private bool _isCanDocking = false;     // Are we aligned correctly?
    private bool _isWeDocked = false;       // Are we currently docked?

    private void Awake()
    {
        if (shipTransform == null) shipTransform = transform;
        if (shipRb == null) shipRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 1. Handle UI Visibility
        if (dockPromptUI != null)
        {
            // Show UI only if we CAN dock and are NOT yet docked
            dockPromptUI.SetActive(_isCanDocking && !_isWeDocked);
        }

        if (!_isProcessDocking || _isWeDocked) return;

        // 2. Check Alignment Points
        bool allPointsGood = true;
        for (int i = 0; i < pointsDocking.Length; i++)
        {
            // Check if point is inside a station docking zone
            Collider[] colliders = Physics.OverlapSphere(pointsDocking[i].transform.position, radiusPoints, maskPoints);

            if (colliders.Length > 0)
                pointsDocking[i].Good();
            else
            {
                pointsDocking[i].Fall();
                allPointsGood = false;
            }
        }

        _isCanDocking = allPointsGood;

        // 3. Handle Input
        if (_isCanDocking && Input.GetKeyDown(keyDocking))
        {
            PerformDocking();
        }
    }

    private void PerformDocking()
    {
        _isWeDocked = true;
        shipRb.isKinematic = true; // Freeze ship physics

        // Hide the prompt immediately
        if (dockPromptUI) dockPromptUI.SetActive(false);

        // Notify GameLoopManager (via Inspector Event)
        onDock?.Invoke();

        // Start Camera Animation
        // ADD THIS LINE:
        if (audioSource && dockSound) audioSource.PlayOneShot(dockSound);

        onDock?.Invoke();

        if (dockingRealization) dockingRealization.Dock(_targetStation, this);
    }

    public void Undock()
    {
        shipRb.isKinematic = false; // Unfreeze physics
        _isWeDocked = false;

        if (audioSource && undockSound) audioSource.PlayOneShot(undockSound);


        onUndock?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DockingStantionHandler sh))
        {
            _isProcessDocking = true;
            _targetStation = sh;
            TogglePoints(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out DockingStantionHandler sh))
        {
            _isProcessDocking = false;
            _targetStation = null;
            _isCanDocking = false;
            TogglePoints(false);
        }
    }

    private void TogglePoints(bool state)
    {
        foreach (var p in pointsDocking) p.gameObject.SetActive(state);
    }
}