using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI playerDiamonds;
    public void UpdatePlayerDiamonds(int diamonds){
        playerDiamonds.text = diamonds.ToString();
    }
    public void PlayButton(){
        Close(0);
        UIManager.Instance.OpenUI<GamePlay>();
        UIManager.Instance.OpenUI<CanvasJoystick>();
        LevelManager.Instance.OnStartGame();
    }
    public void SettingsButton(){
        UIManager.Instance.OpenUI<Settings>();
    }
    public void WeaponButton(){
        UIManager.Instance.OpenUI<CanvasWeaponShop>();
    }
    public void SkinButton(){

    }
}
