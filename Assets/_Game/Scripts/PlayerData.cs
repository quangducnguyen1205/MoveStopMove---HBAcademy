using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int diamonds; // Số kim cương người chơi có
    public string currentWeapon; // Vũ khí người chơi đang sử dụng
    public List<string> weapons; // Danh sách vũ khí mà người chơi đã sở hữu

    // Constructor mặc định
    public PlayerData(){
        diamonds = 0;
        currentWeapon = "Stone";
        weapons = new List<string> { "Stone" }; // Khởi tạo danh sách vũ khí với vũ khí mặc định
    }

    // Constructor với tham số
    public PlayerData(int diamonds, string currentWeapon){
        this.diamonds = diamonds;
        this.currentWeapon = currentWeapon;
        weapons = new List<string>(); // Khởi tạo danh sách vũ khí

        // Kiểm tra và thêm vũ khí vào danh sách nếu chưa có
        if (!weapons.Contains(currentWeapon)){
            weapons.Add(currentWeapon);
        }
    }

    // Lưu dữ liệu người chơi vào PlayerPrefs dưới dạng chuỗi JSON
    public void SavePlayerData(PlayerData playerData){
        string jsonData = JsonUtility.ToJson(playerData); // Chuyển đổi dữ liệu thành chuỗi JSON
        PlayerPrefs.SetString("PLAYER_DATA", jsonData);   // Lưu vào PlayerPrefs với khóa "PLAYER_DATA"
        PlayerPrefs.Save();                               // Gọi hàm Save để đảm bảo dữ liệu được lưu
    }

    // Tải dữ liệu người chơi từ PlayerPrefs
    public PlayerData LoadPlayerData(){
        string jsonData = PlayerPrefs.GetString("PLAYER_DATA", ""); // Lấy chuỗi JSON từ PlayerPrefs
        if (string.IsNullOrEmpty(jsonData))
        {
            return new PlayerData(); // Nếu không có dữ liệu, trả về PlayerData mặc định
        }
        return JsonUtility.FromJson<PlayerData>(jsonData); // Chuyển đổi chuỗi JSON thành đối tượng PlayerData
    }
}
