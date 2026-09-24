using System;
using UnityEngine;
using UnityEngine.InputSystem;
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Animator animator; 
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

  
    }