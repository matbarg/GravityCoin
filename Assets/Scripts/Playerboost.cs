using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// Speed-Boost mit Tank (Ladebalken).
// L2 halten (Gamepad) bzw. eine Taste (Tastatur) -> schneller, Tank leert sich.
// Laedt sich mit der Zeit wieder auf: langsam beim Laufen, schneller im Stehen.
// An-/Ausschalten zentral ueber den GameLevelSpawner (enableBoost).
public class PlayerBoost : MonoBehaviour
{
    [Header("An/Aus")]
    public bool boostEnabled = true;

    [Header("Boost")]
    [Tooltip("Tempo-Faktor beim Boosten (1.5 = 50% schneller).")]
    public float boostMultiplier = 1.5f;
    [Tooltip("Wie lange ein voller Tank reicht (Sekunden).")]
    public float boostDuration = 3f;

    [Header("Aufladen (pro Sekunde, 1 = voller Tank)")]
    [Tooltip("Aufladen waehrend man laeuft.")]
    public float rechargeMoving = 0.15f;
    [Tooltip("Aufladen im Stehen (schneller).")]
    public float rechargeStanding = 0.4f;
    [Tooltip("Ab welchem Tempo man als 'laufend' gilt.")]
    public float standingThreshold = 0.5f;

    [Header("Tastatur-Boost-Tasten")]
    public Key keyboardLeftBoostKey = Key.LeftShift;
    public Key keyboardRightBoostKey = Key.RightShift;

    [Header("UI")]
    public Image boostBar;   // Fill-Image, wird meist vom Spawner zugewiesen

    [Header("Sprint-Effekt")]
    [Tooltip("Trail Renderer, der beim Sprinten sichtbar wird (am Spieler).")]
    public TrailRenderer sprintTrail;

    private float tank = 1f;              // 0..1
    private bool wasBoosting = false;
    private PlayerMovement movement;
    private PlayerInput playerInput;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();

        // Trail beim Start aus
        if (sprintTrail != null) sprintTrail.emitting = false;
    }

    void Update()
    {
        if (!boostEnabled)
        {
            if (wasBoosting) { movement.SetSpeedMultiplier(1f); wasBoosting = false; }
            if (boostBar != null) boostBar.fillAmount = 0f;
            SetTrail(false);
            return;
        }

        bool wantBoost = ReadBoostInput();
        bool boosting = wantBoost && tank > 0f;

        if (boosting)
        {
            tank -= (1f / boostDuration) * Time.deltaTime;
            if (tank < 0f) tank = 0f;

            movement.SetSpeedMultiplier(boostMultiplier);
            wasBoosting = true;
        }
        else
        {
            // Boost gerade beendet -> Tempo zuruecksetzen (nur einmal)
            if (wasBoosting) { movement.SetSpeedMultiplier(1f); wasBoosting = false; }

            // Aufladen
            float rate = IsStanding() ? rechargeStanding : rechargeMoving;
            tank += rate * Time.deltaTime;
            if (tank > 1f) tank = 1f;
        }

        // Trail nur waehrend des Sprints sichtbar
        SetTrail(boosting);

        if (boostBar != null) boostBar.fillAmount = tank;
    }

    private void SetTrail(bool on)
    {
        if (sprintTrail == null) return;
        if (sprintTrail.emitting != on)
            sprintTrail.emitting = on;
    }

    private bool IsStanding()
    {
        if (movement == null || movement.rb == null) return true;
        return Mathf.Abs(movement.rb.linearVelocity.x) < standingThreshold;
    }

    // Liest die Boost-Taste direkt vom gekoppelten Geraet des Spielers.
    private bool ReadBoostInput()
    {
        if (playerInput == null) return false;

        // Gamepad? -> L2 (linker Trigger)
        foreach (var device in playerInput.devices)
        {
            if (device is Gamepad gp)
                return gp.leftTrigger.isPressed;
        }

        // Tastatur -> je nach Schema die passende Taste
        var kb = Keyboard.current;
        if (kb == null) return false;

        string scheme = playerInput.currentControlScheme;
        if (scheme == "KeyBoardLeft")  return kb[keyboardLeftBoostKey].isPressed;
        if (scheme == "KeyBoardRight") return kb[keyboardRightBoostKey].isPressed;

        return false;
    }

    // Fuer spaeter: Auflade-Coins / Power-Ups koennen den Tank fuellen.
    public void AddCharge(float amount)
    {
        tank = Mathf.Clamp01(tank + amount);
    }
}