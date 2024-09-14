using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponType{
    Stone = 0,
    Spear = 1,
    Boomerang = 2,
}
public class WeaponManager : Singleton<WeaponManager>
{
    [SerializeField] public WeaponTypeSO[] weaponTypeSO;
    public Transform instantiatePoint;

    // Phương thức này nhận vào WeaponType và spawn vũ khí tương ứng
    public Weapon SpawnWeapon(WeaponType weaponType)
    {
        int index = (int)weaponType;
        Weapon weaponPrefab = weaponTypeSO[index].weapon;
        Weapon weaponInstance = Instantiate(weaponPrefab, instantiatePoint);
        return weaponInstance;
    }
}
