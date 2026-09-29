using UnityEngine;
using UnityEngine.UI;
public class PlayerWeaponUI : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image[] arrowIcons;

    [SerializeField] private Sprite swordIcon;
    [SerializeField] private Sprite bowIcon;
    
    [SerializeField] private Sprite normalArrowIcon;
    [SerializeField] private Sprite iceArrowIcon;
    
    [SerializeField] private GameObject ammoContainer;
    
    public void SetAmmoSlot(int index, ArrowType type)
    {
        if (index < 0 || index >= arrowIcons.Length)
        {
            return;
        }

        Image slot = arrowIcons[index];

        if (type == ArrowType.None)
        {
            slot.gameObject.SetActive(false);
        }
        else if (type == ArrowType.Normal)
        {
            slot.gameObject.SetActive(true);
            slot.sprite = normalArrowIcon;
        }
        else if (type == ArrowType.Ice)
        {
            slot.gameObject.SetActive(true);
            slot.sprite = iceArrowIcon;
        }
    }
    
    public void SetWeapon(WeaponType weapon)
    {
        if (weapon == WeaponType.Bow)
        {
            weaponIcon.sprite = bowIcon;
            ammoContainer.SetActive(true);
        }
        else
        {
            weaponIcon.sprite = swordIcon;
            ammoContainer.SetActive(false);
        }
    }
    
}