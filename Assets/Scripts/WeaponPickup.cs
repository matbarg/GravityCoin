using System.Collections;
using UnityEngine;

    public class WeaponPickup : MonoBehaviour
    {
        public WeaponType weaponType;
        private float respawnTime = 15f;
        private Collider2D pickupCollider;
        private SpriteRenderer spriteRenderer;
        
        private void Awake()
        {
            pickupCollider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            WeaponHolder holder =
                other.GetComponentInParent<WeaponHolder>();

            if (holder != null)
            {
                if (holder.specialWeapon != WeaponType.None)
                {
                    return;
                }

                holder.EquipWeapon(weaponType);
                StartCoroutine(RespawnPickup()); 
            }
        }
        private IEnumerator RespawnPickup()
        {
            pickupCollider.enabled = false;
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(respawnTime);

            spriteRenderer.enabled = true;
            pickupCollider.enabled = true;
        }
    }