using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache
{
    // Cache cho Character
    private static Dictionary<Collider, Character> characters = new Dictionary<Collider, Character>();
    
    // Cache cho AudioSource (đối với Camera chính)
    private static AudioSource cachedCameraAudioSource;

    public static Character GetCharacter(Collider collider)
    {
        if (collider == null) return null; // Kiểm tra null để tránh lỗi

        if (!characters.ContainsKey(collider))
        {
            // Nếu collider chưa tồn tại trong cache, thêm nó vào
            Character character = collider.GetComponent<Character>();
            if (character != null) // Kiểm tra nếu collider thực sự có Component Character
            {
                characters.Add(collider, character);
            }
        }

        return characters.ContainsKey(collider) ? characters[collider] : null;
    }

    // Phương thức để lấy AudioSource từ Camera
    public static AudioSource GetCameraAudioSource()
    {
        if (cachedCameraAudioSource == null) // Kiểm tra nếu AudioSource chưa được cache
        {
            cachedCameraAudioSource = Camera.main.GetComponent<AudioSource>();
        }

        return cachedCameraAudioSource;
    }

    // Hàm này dùng để clear cache khi cần thiết, ví dụ khi load level mới hoặc reset game
    public static void ClearCache()
    {
        characters.Clear();
        cachedCameraAudioSource = null;
    }
}

