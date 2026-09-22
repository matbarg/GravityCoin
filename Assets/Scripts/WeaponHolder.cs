using System;
using UnityEngine;
using UnityEngine.InputSystem;
    public class WeaponHolder : MonoBehaviour
    {

        public WeaponType currentWeapon = WeaponType.Sword;
        public WeaponType specialWeapon = WeaponType.None;
        public void EquipWeapon(WeaponType newWeapon)
        {
            specialWeapon = newWeapon;
            currentWeapon = newWeapon;
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

            Debug.Log("Aktuelle Waffe: " + currentWeapon);
        }
    }