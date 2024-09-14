using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlay : UICanvas
{
    [SerializeField] TextMeshProUGUI botAliveText;
    public void UpdateBotAlive (int botAlive){
        botAliveText.text = botAlive.ToString();
    }
    public void SettingsButton(){
        UIManager.Instance.OpenUI<Settings>();
    }
    
}
