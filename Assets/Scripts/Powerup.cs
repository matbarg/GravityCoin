using System;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Tooltip("Welches Powerup diese Kugel gibt.")]
    public PowerupType type = PowerupType.GravityFlip;

    [Header("Aussehen pro Typ")]
    public Sprite gravityFlipSprite;
    public Sprite freezeSprite;
    public Sprite impactSprite;


    [Header("Audio")]
    public AudioClip collectSound;
    public PowerupSpawner spawner;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Wird vom Spawner aufgerufen, um Typ UND Aussehen zu setzen.
    public void SetType(PowerupType newType)
    {
        type = newType;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        switch (type)
        {
            case PowerupType.GravityFlip:
                sr.sprite = gravityFlipSprite;
                break;
            case PowerupType.Freeze:
                sr.sprite = freezeSprite;
                break;
            case PowerupType.Impact:
                sr.sprite = impactSprite;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerupHolder holder = other.GetComponent<PowerupHolder>();
        if (holder != null)
        {
            holder.PickUp(type);

            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position, 3.0f);

            if (spawner != null)
                spawner.OnPowerupCollected();

            Destroy(gameObject);
        }
    }
}