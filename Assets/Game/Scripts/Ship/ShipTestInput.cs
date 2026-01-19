using UnityEngine;

public class ShipTestInput : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private ShipInputSlider forward;
    [SerializeField] private ShipInputSlider left;
    [SerializeField] private ShipInputSlider up;

    [Header("Visuals")]
    [SerializeField] private GameObject offObj;
    [SerializeField] private GameObject onObj;

    [Header("Audio")]
    public AudioSource mainEngineSource; // Drag 'звук работы маршевого двигателя' here
    public AudioSource rcsSource;        // Drag 'маневры' here

    [Header("Reference")]
    [SerializeField] private ShipTestMove _move;

    private void Awake()
    {
        if (_move == null) TryGetComponent(out _move);

        // Setup Audio settings automatically
        if (mainEngineSource) mainEngineSource.loop = true;
        if (rcsSource) rcsSource.loop = true;
    }

    private void Update()
    {
        // ... (Boost Logic - Same as before) ...
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetSliders();
            _move.isFullForward = true;
            if (offObj) offObj.SetActive(false);
            if (onObj) onObj.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _move.isFullForward = false;
            if (offObj) offObj.SetActive(true);
            if (onObj) onObj.SetActive(false);
        }

        HandleRotationInput();
        HandleLinearInput();
        HandleAudio(); // New Audio Logic
    }

    private void HandleAudio()
    {
        // 1. Main Engine Sound (Forward/Back)
        bool isThrusting = _move.inputLinear.z != 0 || _move.isFullForward;

        if (mainEngineSource)
        {
            if (isThrusting && !mainEngineSource.isPlaying) mainEngineSource.Play();
            else if (!isThrusting && mainEngineSource.isPlaying) mainEngineSource.Stop();
        }

        // 2. RCS/Maneuver Sound (Rotation or Strafing)
        bool isManeuvering = _move.InputRot != Vector2.zero || _move.inputLinear.x != 0 || _move.inputLinear.y != 0;

        if (rcsSource)
        {
            if (isManeuvering && !rcsSource.isPlaying) rcsSource.Play();
            else if (!isManeuvering && rcsSource.isPlaying) rcsSource.Stop();
        }
    }

    // ... (Keep HandleRotationInput, HandleLinearInput, ResetSliders EXACTLY as they were) ...
    // Paste the previous HandleRotationInput and HandleLinearInput here...

    private void HandleRotationInput()
    {
        Vector2 rotInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) rotInput.y += 1f;
        if (Input.GetKey(KeyCode.S)) rotInput.y -= 1f;
        if (Input.GetKey(KeyCode.D)) rotInput.x += 1f;
        if (Input.GetKey(KeyCode.A)) rotInput.x -= 1f;
        if (Input.GetKey(KeyCode.Keypad4)) rotInput.x -= 1f;
        if (Input.GetKey(KeyCode.Keypad6)) rotInput.x += 1f;
        if (Input.GetKey(KeyCode.Keypad8)) rotInput.y += 1f;
        if (Input.GetKey(KeyCode.Keypad2)) rotInput.y -= 1f;
        _move.InputRot = rotInput;
    }

    private void HandleLinearInput()
    {
        Vector3 moveInput = Vector3.zero;
        if (forward != null) moveInput.z += forward.Value;
        if (left != null) moveInput.x += left.Value;
        if (up != null) moveInput.y += up.Value;
        if (Input.GetKey(KeyCode.UpArrow)) moveInput.z += 1f;
        if (Input.GetKey(KeyCode.DownArrow)) moveInput.z -= 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveInput.x -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveInput.x += 1f;
        if (Input.GetKey(KeyCode.RightShift)) moveInput.y += 1f;
        if (Input.GetKey(KeyCode.RightControl)) moveInput.y -= 1f;
        _move.inputLinear = moveInput;
    }

    private void ResetSliders()
    {
        if (forward != null) forward.ResetValue();
        if (left != null) left.ResetValue();
        if (up != null) up.ResetValue();
    }
}