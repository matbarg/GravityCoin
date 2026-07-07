using UnityEngine;

public class KingOfCoinPlayer : MonoBehaviour
{
    
    public bool isCarrier { get; private set; }
 
    
    [HideInInspector] public float accumulatedTime = 0f;
 
    [Tooltip("Tempo-Faktor, solange man den Coin trägt (0.6 = 60% Speed).")]
    public float carrierSpeedMultiplier = 0.7f;
 
    private PlayerMovement movement;
 
    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }
 
    public void SetCarrier(bool value)
    {
        isCarrier = value;
 
        // Träger wird langsamer, sonst normal
        if (movement != null)
        {
            movement.SetSpeedMultiplier(value ? carrierSpeedMultiplier : 1f);
        }
    }
}