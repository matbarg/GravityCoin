using System;
using UnityEngine;
    public class WeaponHolder : MonoBehaviour
    {

        public WeaponType currentWeapon = WeaponType.Sword;

        public void EquipWeapon(WeaponType newWeapon)
        {
            currentWeapon = newWeapon;
        }
    }