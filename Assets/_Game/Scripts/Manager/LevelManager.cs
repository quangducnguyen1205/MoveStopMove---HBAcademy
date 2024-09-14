using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private PlayerController playerPrefab;
    public PatrolConfigSO patrolConfig;
    public Level levelPrefabs;
    public int botsPerWave; // Số lượng bot mỗi lượt
    public int totalBots = 50; // Tổng số lượng bot mỗi màn
    public float spawnInterval; // Thời gian giữa các lượt sinh bot
    private int botsSpawned = 0; // Số bot đã sinh ra
    private List<BotController> bots = new List<BotController>(); // Danh sách các bot đã sinh ra
    private Level currentLevel;
    private Coroutine spwanCoroutine;
    private PlayerController playerInstance;
    private int diamondsinPlay; //Number of diamonds the player earns in a game
    private int botAliveCount;
    private void Awake()
    {
        // Khởi tạo level hoặc các thiết lập cần thiết
    }

    private void Start()
    {
        currentLevel = Instantiate(levelPrefabs);
    }

    public void OnInit()
    {
        spwanCoroutine = StartCoroutine(SpawnBotWaves());

        // In ra màn hình tổng số lượng bot
        SetInitalBotAlive(totalBots);

        // Khởi tạo Player và các thông số cần thiết
        playerInstance = Instantiate(playerPrefab, currentLevel.playerPoint.position, Quaternion.identity);
        // Gán lại vị trí instantiate cho WeaponManager
        WeaponManager.Instance.instantiatePoint = playerInstance.instantiateWeapon;

        playerInstance.joystick = UIManager.Instance.GetUI<CanvasJoystick>().dynamicJoystick;

        // Lấy dữ liệu vũ khí từ PlayerData
        string currentWeapon = GameManager.Instance.GetCurrentWeapon();
        WeaponType weaponType;
        if (System.Enum.TryParse(currentWeapon, out weaponType)){
            playerInstance.ChangeWeapon(weaponType);
        }

        playerInstance.OnInit();

        // Cập nhật target cho CameraFollow
        Camera.main.GetComponent<CameraFollow>().SetTarget(playerInstance.transform);
    }

    public void OnStartGame()
    {
        diamondsinPlay = 0;
        OnInit();
        // Bắt đầu trò chơi, kích hoạt bot di chuyển hoặc tấn công
        for (int i = 0; i < bots.Count; i++){
            bots[i].ChangeState(new PatrolState(patrolConfig));
        }
    }

    public void OnFinishGameFail()
    {
        AudioManager.Instance.PlayPlayerDieSound();
        UIManager.Instance.GetUI<Fail>().UpdateRanking(botAliveCount);
        UIManager.Instance.GetUI<Fail>().UpdateDiamondsInPlay(diamondsinPlay);
        CleanLevel();      
    }
    public void OnFinishGameWin(){
        UIManager.Instance.GetUI<Victory>().UpdateDiamondsInPlay(diamondsinPlay);
        CleanLevel();
    }
    public void OnReset()
    {
        // Đặt lại trạng thái trò chơi
        WeaponPool.CollectAll();
        ParticlePool.CollectAll();
        SimplePool.CollectAll();
        bots.Clear();
        botsSpawned = 0;
    }
    public void OnRetry()
    {
        // Khi người chơi chọn thử lại
        OnReset();
        OnInit();
        for (int i = 0; i < bots.Count; i++){
            bots[i].ChangeState(new PatrolState(patrolConfig));
        }
        GameManager.Instance.AddDiamond(diamondsinPlay);
        diamondsinPlay = 0;
    }
    public void CleanLevel(){
        // Kết thúc trò chơi, dừng tất cả hoạt động của bot 
        if (spwanCoroutine != null){
            StopCoroutine(spwanCoroutine);
            spwanCoroutine = null;
        }
        OnReset();
        // Hủy Player nếu có
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
        }
    }
    public void SetInitalBotAlive(int initalBotCount){
        botAliveCount = initalBotCount;
        UpdateBotAliveUI();
    }
    public void DecreaseBotCount(){
        botAliveCount--;
        UpdateBotAliveUI();
        if (botAliveCount <= 0){
            GameManager.Instance.OnWinGame();
        }
    }
    public void BackToMainMenu(){
        GameManager.Instance.AddDiamond(diamondsinPlay);
        UIManager.Instance.GetUI<MainMenu>().UpdatePlayerDiamonds(GameManager.Instance.GetDiamonds());
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<MainMenu>();
    }
    private void UpdateBotAliveUI(){
        UIManager.Instance.GetUI<GamePlay>().UpdateBotAlive(botAliveCount);
    }
    private IEnumerator SpawnBotWaves()
    {
        while (botsSpawned < totalBots){
            int botsToSpawn = Mathf.Min(botsPerWave, totalBots - botsSpawned);
            for (int i = 0; i < botsToSpawn; i++){
                Vector3 spawnPosition = GetRandomSpawnPosition();
                BotController bot = SimplePool.Spawn<BotController>(PoolType.Bot, spawnPosition, Quaternion.identity);
                bot.OnInit();
                bots.Add(bot);
                botsSpawned++;
            } 
            // Đợi một khoảng thời gian trước khi sinh thêm bot
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    public void IncreaseDiamondsInPlay(){
        diamondsinPlay++;
    }
    private Vector3 GetRandomSpawnPosition(){
        // Trục x có thể nằm trong vùng (-a, -b) hoặc (c, d)
        float x = Random.Range(0, 2) == 0 ? Random.Range(-10f, -4f) : Random.Range(4f, 10f);
        
        // Trục z có thể nằm trong vùng (-a, -b) hoặc (c, d)
        float z = Random.Range(0, 2) == 0 ? Random.Range(-10f, -4f) : Random.Range(4f, 10f);

        return new Vector3(x, 0, z); // y = 0 để spawn trên mặt đất
    }
}
