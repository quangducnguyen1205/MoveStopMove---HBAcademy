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
    [SerializeField] private Button buyButton; // Thêm biến tham chiếu đến Button

    public void DisplayWeapon(WeaponTypeSO weaponTypeSO)
    {
        weaponName.text = weaponTypeSO.weaponName;
        weaponPrice.text = weaponTypeSO.weaponPrice.ToString();
        weapon.sprite = weaponTypeSO.weaponImage;

        // Kiểm tra nếu vũ khí đã được mua
        if (GameManager.Instance.CheckWeaponPlayerHas(weaponName.text))
        {
            if (buyButton.interactable == false){
                GameManager.Instance.UpdatePlayerWeapon(weaponName.text);
            }
            // Nếu đã mua, mờ đi nút Button và vô hiệu hóa
            buyButton.interactable = false;
            Color color = buyButton.image.color;
            color.a = 0.5f; // Làm mờ đi bằng cách giảm alpha (độ trong suốt)
            buyButton.image.color = color;
        }
        else
        {
            // Nếu chưa mua, kích hoạt nút Button
            buyButton.interactable = true;
            Color color = buyButton.image.color;
            color.a = 1.0f; // Đặt alpha về 1 để nút trở nên rõ ràng
            buyButton.image.color = color;
        }
    }

    public void WeaponPriceButton()
    {
        int playerDiamond = GameManager.Instance.GetDiamonds();
        int price = int.Parse(weaponPrice.text);

        if (playerDiamond >= price && !GameManager.Instance.CheckWeaponPlayerHas(weaponName.text))
        {
            // Cập nhật vũ khí của người chơi
            GameManager.Instance.UpdatePlayerWeapon(weaponName.text);

            // Trừ kim cương
            GameManager.Instance.AddDiamond(-price);

            // Mờ đi nút sau khi mua
            buyButton.interactable = false;
            Color color = buyButton.image.color;
            color.a = 0.5f; // Làm mờ đi bằng cách giảm alpha
            buyButton.image.color = color;
        }
    }

    public void BackMainMenuButton()
    {
        Close(0);
        UIManager.Instance.GetUI<MainMenu>().UpdatePlayerDiamonds(GameManager.Instance.GetDiamonds());
    }
}
