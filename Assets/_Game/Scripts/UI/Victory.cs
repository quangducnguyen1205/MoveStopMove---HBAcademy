using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Victory : UICanvas
{
    [SerializeField] private TextMeshProUGUI diamondsInPlay;
    public void UpdateDiamondsInPlay(int diamonds){
        diamondsInPlay.text = diamonds.ToString();
    }
    public void MainMenuButton(){
        LevelManager.Instance.BackToMainMenu();
    }
}
