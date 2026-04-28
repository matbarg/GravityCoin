using UnityEngine;

public class LevelGravity : MonoBehaviour
{
    [Header("Welt-Gravitation")]
    [Tooltip("Standard Erde ist -9.81. Mars ist ca. -3.7 bis -4.5.")]
    public float sceneGravity = -4.5f;

    [Header("Spieler-Anpassungen")]
    [Tooltip("Höherer Wert = Spieler fühlt sich schwerer an (weniger 'floaty').")]
    public float newGravityScale = 1.5f;
    
    [Tooltip("Höherer Wert = Mehr Luftwiderstand (weniger Rutschen in der Luft).")]
    public float newDamping = 0.5f;

    private Rigidbody2D playerRb;
    private float oldGravityScale;
    private float oldDamping;

    void Start()
    {
        Physics2D.gravity = new Vector2(0, sceneGravity);

        GameObject player = GameObject.FindWithTag("Player");
        
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            
            if (playerRb != null)
            {
                oldGravityScale = playerRb.gravityScale;
                
                oldDamping = playerRb.linearDamping; 

                playerRb.gravityScale = newGravityScale;
               
                playerRb.linearDamping = newDamping; 
            }
        }
    }

    void OnDestroy()
    {
        // 1. Welt-Schwerkraft zurück auf Standard-Erde (-9.81)
        Physics2D.gravity = new Vector2(0, -9.81f);
    // 2. Spieler-Werte nur zurücksetzen
        if (playerRb != null)
        {
            playerRb.gravityScale = oldGravityScale;
            playerRb.linearDamping = oldDamping;
            Debug.Log("Physik für Spieler zurückgesetzt.");
        }
    }
}