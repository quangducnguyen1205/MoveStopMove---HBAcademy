using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChange : UICanvas
{
    [SerializeField] private ScriptableObject[] weaponShopSO;
    [SerializeField] private CanvasWeaponShop canvasWeaponShop;
    private int currentIndex;
    private void Awake() {
        ChangeWeaponShop(0);
    }
    public void ChangeWeaponShop(int index) {
        currentIndex += index;
        if (currentIndex < 0) {
            currentIndex = weaponShopSO.Length - 1;
        }
        else if (currentIndex > weaponShopSO.Length - 1) {
            currentIndex = 0;
        }
        if (canvasWeaponShop != null) {
            canvasWeaponShop.DisplayWeapon((WeaponTypeSO)weaponShopSO[currentIndex]);
        }
    }
}
