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

    [Header("UI")]
    public Image boostBar;  

    [Header("Sprint-Effekt")]
    [Tooltip("Trail Renderer, der beim Sprinten sichtbar wird (am Spieler).")]
    public TrailRenderer sprintTrail;
    [Tooltip("Nach einem komplett leeren Tank muss dieser Wert erreicht werden, bevor erneut geboostet werden kann.")]
    [Range(0f, 1f)]
    public float minimumRechargeToBoost = 0.2f;
    

    private float tank = 1f;        
    private bool wasBoosting = false;
    private PlayerMovement movement;
    private bool boostLocked = false;
    private bool wantBoost = false;
    void Awake()
    {
        movement = GetComponent<PlayerMovement>();

        // Trail beim Start aus
        if (sprintTrail != null) sprintTrail.emitting = false;
    }

    void Update()
    {
        if (!boostEnabled)
        {
            if (wasBoosting)
            {
                movement.SetSpeedMultiplier(1f);
                wasBoosting = false;
            }

            if (boostBar != null)
                boostBar.fillAmount = 0f;

            SetTrail(false);
            return;
        }
        

        // Wenn der Tank wieder mindestens 20 % erreicht hat,
        // wird der Boost erneut freigeschaltet.
        if (boostLocked && tank >= minimumRechargeToBoost)
        {
            boostLocked = false;
        }

        bool boosting = wantBoost && !boostLocked && tank > 0f;

        if (boosting)
        {
            tank -= (1f / Mathf.Max(boostDuration, 0.01f)) * Time.deltaTime;
            tank = Mathf.Clamp01(tank);

            movement.SetSpeedMultiplier(boostMultiplier);
            wasBoosting = true;

            // Tank vollständig leer: Boost bis zur Mindestladung sperren.
            if (tank <= 0f)
            {
                tank = 0f;
                boostLocked = true;

                movement.SetSpeedMultiplier(1f);
                wasBoosting = false;
                boosting = false;
            }
        }
        else
        {
            if (wasBoosting)
            {
                movement.SetSpeedMultiplier(1f);
                wasBoosting = false;
            }

            float rate = IsStanding() ? rechargeStanding : rechargeMoving;
            tank += rate * Time.deltaTime;
            tank = Mathf.Clamp01(tank);
        }

        SetTrail(boosting);

        if (boostBar != null)
            boostBar.fillAmount = tank;
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

    public void Boost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            wantBoost = true;
        }

        if (context.canceled)
        {
            wantBoost = false;
        }
    }

    // Fuer spaeter: Auflade-Coins / Power-Ups koennen den Tank fuellen.
    public void AddCharge(float amount)
    {
        tank = Mathf.Clamp01(tank + amount);
    }
}