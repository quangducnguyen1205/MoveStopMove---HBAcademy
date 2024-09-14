using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PoolControler : MonoBehaviour
{
    [Space]
    [Header("Pool")]
    public List<GameUnit> PoolNoneRoot;

    [Header("Pool")]
    public List<PoolAmount> PoolWithRoot;

    [Header("Particle")]
    public ParticleAmount[] Particle;
    [Header("Weapons")]
    public WeaponAmount[] Weapon; // Thêm các vũ khí khác nếu cần


    public void Awake()
    {
        for (int i = 0; i < PoolNoneRoot.Count; i++)
        {
            SimplePool.Preload(PoolNoneRoot[i], 0, transform);
        }       
        
        for (int i = 0; i < PoolWithRoot.Count; i++)
        {
            SimplePool.Preload(PoolWithRoot[i].prefab, PoolWithRoot[i].amount, PoolWithRoot[i].root);
        }

        for (int i = 0; i < Particle.Length; i++)
        {
            ParticlePool.Preload(Particle[i].prefab, Particle[i].amount, Particle[i].root);
            ParticlePool.Shortcut(Particle[i].particleType, Particle[i].prefab);
        }
        for (int i = 0; i < Weapon.Length; i++)
        {
            WeaponPool.Preload(Weapon[i].prefab, Weapon[i].weaponType, Weapon[i].amount, Weapon[i].root);
        }
    }
}

[System.Serializable]
public class PoolAmount
{
    [Header("-- Pool Amount --")]
    public Transform root;
    public GameUnit prefab;
    public int amount;

    public PoolAmount (Transform root, GameUnit prefab, int amount)
    {
        this.root = root;
        this.prefab = prefab;
        this.amount = amount;
    }
}


[System.Serializable]
public class ParticleAmount
{
    public Transform root;
    public ParticleType particleType;
    public ParticleSystem prefab;
    public int amount;
}


[System.Serializable]
public class WeaponAmount
{
    public Transform root;
    public Weapon prefab;
    public int amount;
    public WeaponType weaponType;
}


public enum ParticleType
{
    Destruction_air_blue = 0,
}

public enum PoolType
{
    None = 0,
    Bot = 1,
}

