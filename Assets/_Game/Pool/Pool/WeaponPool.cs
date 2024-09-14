using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponPool
{
    class Pool
    {
        Queue<Weapon> m_inactive;
        List<Weapon> m_active; // Danh sách các đối tượng đang hoạt động
        Weapon m_prefab;
        Transform m_parent;

        public Pool(Weapon prefab, int initialQty, Transform parent)
        {
            m_prefab = prefab;
            m_inactive = new Queue<Weapon>(initialQty);
            m_active = new List<Weapon>(); // Khởi tạo danh sách active
            m_parent = parent;

            // Tạo trước các đối tượng vào hàng đợi (inactive pool)
            for (int i = 0; i < initialQty; i++)
            {
                Weapon weapon = GameObject.Instantiate(prefab, m_parent);
                weapon.gameObject.SetActive(false);
                m_inactive.Enqueue(weapon);
            }
        }

        public Weapon Spawn(Vector3 pos, Quaternion rot)
        {
            Weapon weapon;

            // Nếu hàng đợi rỗng, tạo mới
            if (m_inactive.Count == 0)
            {
                weapon = GameObject.Instantiate(m_prefab, m_parent);
            }
            else
            {
                // Lấy đối tượng từ pool
                weapon = m_inactive.Dequeue();
            }

            // Đặt vị trí, xoay và kích hoạt đối tượng
            weapon.transform.SetPositionAndRotation(pos, rot);
            weapon.gameObject.SetActive(true);
            
            // Thêm vào danh sách active
            m_active.Add(weapon);
            return weapon;
        }

        public void Despawn(Weapon weapon)
        {
            if (weapon != null && weapon.gameObject.activeSelf)
            {
                // Ẩn đối tượng
                weapon.gameObject.SetActive(false);

                // Thêm trở lại vào hàng đợi inactive
                m_inactive.Enqueue(weapon);

                // Xóa khỏi danh sách active
                m_active.Remove(weapon);
            }
        }

        // Thu hồi tất cả các đối tượng đang hoạt động về pool
        public void Collect()
        {
            while (m_active.Count > 0)
            {
                Despawn(m_active[0]);  // Thu hồi đối tượng đang hoạt động
            }
        }
    }

    static Dictionary<WeaponType, Pool> pools = new Dictionary<WeaponType, Pool>();

    // Khởi tạo pool cho vũ khí cụ thể
    public static void Preload(Weapon prefab, WeaponType type, int amount, Transform parent = null)
    {
        if (!pools.ContainsKey(type))
        {
            pools.Add(type, new Pool(prefab, amount, parent));
        }
    }

    // Spawn vũ khí từ pool
    public static Weapon SpawnWeapon(WeaponType type, Vector3 pos, Quaternion rot)
    {
        return pools[type].Spawn(pos, rot);
    }

    // Thu hồi vũ khí về pool
    public static void DespawnWeapon(Weapon weapon, WeaponType type)
    {
        pools[type].Despawn(weapon);
    }

    // Thu hồi tất cả các vũ khí trong tất cả các pool
    public static void CollectAll()
    {
        foreach (var pool in pools)
        {
            pool.Value.Collect(); // Thu hồi tất cả các đối tượng trong pool
        }
    }
}
