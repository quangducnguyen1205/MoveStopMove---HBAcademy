using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "WeaponTypeSO", menuName = "ScriptableObjects/WeaponTypeSO", order = 1)]
public class WeaponTypeSO : ScriptableObject
{
    public int weaponIndex;
    public int weaponPrice;
    public Weapon weapon;
    public string weaponName;
    public Sprite weaponImage;
    public object sceneToLoad;
}

