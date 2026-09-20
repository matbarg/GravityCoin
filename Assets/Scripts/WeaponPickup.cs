using System;
using UnityEngine;

    public class WeaponPickup : MonoBehaviour
    {
        public WeaponType weaponType;
     
        private void OnTriggerEnter2D(Collider2D other)
        {
            WeaponHolder holder = other.GetComponent<WeaponHolder>();

            if (holder != null)
            {
                holder.EquipWeapon(weaponType);
                Destroy(gameObject);
            }
        }
    }