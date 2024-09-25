using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import thư viện UI để làm việc với Slider

public class Settings : UICanvas
{
    [SerializeField] private Slider volumeSlider; // Tham chiếu đến Slider từ Unity Editor

    public override void Open()
    {
        Time.timeScale = 0;
        base.Open();

        // Khởi tạo giá trị cho Slider khi mở Settings
        volumeSlider.value = Cache.GetCameraAudioSource().volume; // Lấy giá trị volume hiện tại của AudioSource
    }

    public override void Close(float delayTime)
    {
        Time.timeScale = 1;
        base.Close(delayTime);
    }

    public void CloseButton()
    {
        Close(0);
    }

    public void ContinueButton()
    {
        Close(0);
    }

    // Phương thức được gọi khi Slider thay đổi giá trị
    public void SetVolume()
    {
        float value = volumeSlider.value;

        Cache.GetCameraAudioSource().volume = value; // Đặt âm lượng cho AudioSource
    }
}
