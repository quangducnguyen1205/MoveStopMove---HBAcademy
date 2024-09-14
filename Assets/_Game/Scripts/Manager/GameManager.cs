using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Finish, Revive, Setting }

public class GameManager : Singleton<GameManager>
{
    private static GameState gameState;
    private PlayerData playerData;

    public static void ChangeState(GameState state)
    {
        gameState = state;
    }

    public static bool IsState(GameState state) => gameState == state;

    private void Awake()
    {
        // Khởi tạo dữ liệu người chơi từ PlayerPrefs
        playerData = new PlayerData();
        playerData = playerData.LoadPlayerData();

        //tranh viec nguoi choi cham da diem vao man hinh
        Input.multiTouchEnabled = false;
        //target frame rate ve 60 fps
        Application.targetFrameRate = 60;
        //tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //xu tai tho
        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    private void Start()
    {
        UIManager.Instance.OpenUI<MainMenu>();
        UIManager.Instance.GetUI<MainMenu>().UpdatePlayerDiamonds(GetDiamonds());
    }
    public bool CheckWeaponPlayerHas(string newWeapon){
        return playerData.weapons.Contains(newWeapon);
    }
    // Hàm để cập nhật vũ khí khi người chơi mua vũ khí mới
    public void UpdatePlayerWeapon(string newWeapon){
        playerData.currentWeapon = newWeapon; // Cập nhật vũ khí mới
        playerData.weapons.Add(newWeapon); 
        playerData.SavePlayerData(playerData); // Lưu lại dữ liệu
    }
    // Hàm để cập nhật số kim cương khi người chơi thu thập
    public void AddDiamond(int diamondsCount){
        playerData.diamonds += diamondsCount; // Cộng thêm kim cương
        playerData.SavePlayerData(playerData);
    }
    // Hàm để trả về số kim cương hiện tại của người chơi
    public int GetDiamonds(){
        return playerData.diamonds;
    }
    public void OnLoseGame(){
        LevelManager.Instance.OnFinishGameFail();
        UIManager.Instance.CloseUI<GamePlay>();
        UIManager.Instance.OpenUI<Fail>();
    }
    public void OnWinGame(){
        LevelManager.Instance.OnFinishGameWin();
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<Victory>();
    }
    public string GetCurrentWeapon(){
        return playerData.currentWeapon;
    }
}