using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWeaponShop : UICanvas
{   
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI weaponPrice;
    [SerializeField] private Image weapon;
    public void DisplayWeapon(WeaponTypeSO weaponTypeSO){
        weaponName.text = weaponTypeSO.weaponName;
        weaponPrice.text = weaponTypeSO.weaponPrice.ToString();
        weapon.sprite = weaponTypeSO.weaponImage;
    }
    public void WeaponPriceButton(){
        int playerDiamond = GameManager.Instance.GetDiamonds();
        int price = int.Parse(weaponPrice.text);

        if (playerDiamond >= price && !GameManager.Instance.CheckWeaponPlayerHas(weaponName.text)){
            // Cập nhật vũ khí của người chơi
            GameManager.Instance.UpdatePlayerWeapon(weaponName.text);

            // Trừ kim cương
            GameManager.Instance.AddDiamond(-price);
        }
    }
    public void BackMainMenuButton(){
        Close(0);
        UIManager.Instance.GetUI<MainMenu>().UpdatePlayerDiamonds(GameManager.Instance.GetDiamonds());
    }
}
