using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PowerupHolder : MonoBehaviour
{
    // Welches Powerup wird gerade gehalten (null = Slot leer).
    private PowerupType heldPowerup = PowerupType.None;
    
    [Header("Powerup-Sprites (pro Typ)")]
    public Sprite gravityFlipSprite;
    public Sprite freezeSprite;
    public Image slotIcon;

    
    // Wird vom Powerup-Objekt aufgerufen, wenn der Spieler es aufhebt.
    // Neues ersetzt immer das alte.
    public void PickUp(PowerupType type)
    {
      
        heldPowerup = type;
        UpdateIcon();
    }

    // R2-Aktion im Input
    public void UsePowerup(InputAction.CallbackContext context)
    {
        
        if (!context.performed) return;

       

        if (heldPowerup == PowerupType.None)
        {
        
            return;
        }

        Activate(heldPowerup);
        heldPowerup = PowerupType.None;
        UpdateIcon();
    }

    private void Activate(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.GravityFlip:
                PowerupEffects.GravityFlipNearestEnemy(gameObject);
                break;
            case PowerupType.Freeze:
                PowerupEffects.FreezeNearestEnemy(gameObject);
                break;
        }
    }

    private void UpdateIcon()
    {
        if (slotIcon == null) return;

        if (heldPowerup == PowerupType.None)
        {
            slotIcon.enabled = false;
            return;
        }

        slotIcon.enabled = true;

        switch (heldPowerup)
        {
            case PowerupType.GravityFlip:
                slotIcon.sprite = gravityFlipSprite;
                break;
            case PowerupType.Freeze:
                slotIcon.sprite = freezeSprite;
                break;
        }
    }
}