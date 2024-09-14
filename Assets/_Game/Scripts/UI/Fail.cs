using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fail : UICanvas
{
    [SerializeField] private TextMeshProUGUI intRanking;
    [SerializeField] private TextMeshProUGUI diamondsInPlay;
    public void UpdateDiamondsInPlay(int diamonds){
        diamondsInPlay.text = diamonds.ToString();
    }
    public void UpdateRanking(int rank){
        intRanking.text = rank.ToString();
    }
    public void RetryButton(){
        Close(0);
        LevelManager.Instance.OnRetry();
    }
    public void MainMenuButton(){
        LevelManager.Instance.BackToMainMenu();
    }
}
