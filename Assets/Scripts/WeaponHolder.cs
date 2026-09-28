using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private int bowMaxAmmo = 3;
        private int bowAmmo;

        public int BowAmmo => bowAmmo;
        
        public WeaponType currentWeapon = WeaponType.Sword;
        public WeaponType specialWeapon = WeaponType.None;

        

        
        private void UpdateAnimatorWeapon()
        {
            animator.SetInteger("Weapon", (int)currentWeapon);
        }
        public void EquipWeapon(WeaponType newWeapon)
        {
            specialWeapon = newWeapon;
            currentWeapon = newWeapon;

            if (newWeapon == WeaponType.Bow)
            {
                bowAmmo = bowMaxAmmo;
                Debug.Log("Bow Ammo: " + bowAmmo);
            }
            UpdateAnimatorWeapon();
        }
 

        public void SwitchWeapon(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            if (specialWeapon == WeaponType.None)
                return;

            if (currentWeapon == WeaponType.Sword)
            {
                currentWeapon = specialWeapon;
            }
            else
            {
                currentWeapon = WeaponType.Sword;
            }
            UpdateAnimatorWeapon();

            Debug.Log("Aktuelle Waffe: " + currentWeapon);
        }
        
        public bool UseBowAmmo()
        {
            if (bowAmmo <= 0)
            {
                Debug.Log("Keine Pfeile mehr");
                return false;
            }

            bowAmmo--;
            Debug.Log("Amo: " + bowAmmo);
            return true;
        }

        public bool AddBowAmmo()
        {
            if (specialWeapon != WeaponType.Bow)
            {
                return false;
            }

            if (bowAmmo >= bowMaxAmmo)
            {
                return false;
            }

            bowAmmo++;
            return true;
        }
  
    }