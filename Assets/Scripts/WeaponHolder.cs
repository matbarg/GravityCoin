using UnityEngine;
using UnityEngine.InputSystem;
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        public WeaponType currentWeapon = WeaponType.Sword;
        public WeaponType specialWeapon = WeaponType.None;
        public PlayerWeaponUI weaponUI;
        private ArrowType[] bowAmmoSlots = new ArrowType[3];

        
        private void UpdateAnimatorWeapon()
        {
            animator.SetInteger("Weapon", (int)currentWeapon);
            
            if (weaponUI != null)
            {
                weaponUI.SetWeapon(currentWeapon);
            }
 
            
        }
        private void UpdateAmmoUI()
        {
            if (weaponUI == null)
                return;

            for (int i = 0; i < bowAmmoSlots.Length; i++)
            {
                weaponUI.SetAmmoSlot(i, bowAmmoSlots[i]);
            }
        }
        public void EquipWeapon(WeaponType newWeapon)
        {
            specialWeapon = newWeapon;
            currentWeapon = newWeapon;

            if (newWeapon == WeaponType.Bow)
            {
                for (int i = 0; i < bowAmmoSlots.Length; i++)
                {
                    bowAmmoSlots[i] = ArrowType.Normal;
                }
                
            }
            UpdateAnimatorWeapon();
            UpdateAmmoUI(); 
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
        
        public ArrowType UseBowAmmo()
        {
            for (int i = 0; i < bowAmmoSlots.Length; i++)
            {
                if (bowAmmoSlots[i] != ArrowType.None)
                {
                    ArrowType usedArrow = bowAmmoSlots[i];

                    bowAmmoSlots[i] = ArrowType.None;
                    UpdateAmmoUI();
                    return usedArrow;
                }
            }

            return ArrowType.None;
        }

        public bool AddBowAmmo(ArrowType type)
        {
            if (specialWeapon != WeaponType.Bow)
            {
                return false;
            }

            for (int i = 0; i < bowAmmoSlots.Length; i++)
            {
                if (bowAmmoSlots[i] == ArrowType.None)
                {
                    bowAmmoSlots[i] = type;
                    UpdateAmmoUI();
                    return true;
                }
            }

            return false;
        }
  
    }