using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    protected Vector3 ownerInitalPosition; // position when player shoot
    internal Vector3 attackDirection;
    public float attackSpped;
    public float timeInvokeOnDespawn;
    public WeaponType weaponType;
    public Rigidbody rb;
    public ParticleSystem hitVFX;
    public bool isShoot = false;
    public Character owner; // Biến nhận diện chủ sở hữu
    public bool isBoomerang = false;
    public Collider weaponCollider;
    
    protected virtual void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Kiểm tra nếu vũ khí đã bắn ra và nếu nó vượt quá tầm bắn
        if (isShoot && owner != null){
            float distanceToOwner = Vector3.Distance(ownerInitalPosition, transform.position);
            if (!isBoomerang){
                if (distanceToOwner > owner.attackRange){
                    OnDespawn();
                }
            }
        }
    }
    public virtual void OnInit(){
        
    }
    public void Shoot(){
        isShoot = true;
        ownerInitalPosition = owner.transform.position;
        // Kiểm tra nếu hướng đã được đặt
        if (attackDirection != Vector3.zero)
        {
            rb.velocity = attackDirection * attackSpped; // Đảm bảo rằng vận tốc đã được đặt chính xác
            Invoke(nameof(OnDespawn), timeInvokeOnDespawn); // Tự hủy sau 5 giây
        }
    }
    public virtual void OnDespawn(){
        //Destroy(gameObject);
        WeaponPool.DespawnWeapon(this, weaponType);
    }
    public void SetDirection(Vector3 direchtion){
        attackDirection = direchtion;
    }
    protected virtual void OnTriggerEnter(Collider other) {
        // Chỉ xử lý va chạm khi vũ khí đã được bắn ra
        if (!isShoot){
            return;
        }
        // Kiểm tra nếu vũ khí không va chạm với chính chủ sở hữu của nó
        Character character = Cache.GetCharacter(other); // use Cache
        if (character != null && character != owner){
            character.OnDeath(owner);
            OnDespawn();

            ParticlePool.Play(ParticleType.Destruction_air_blue, transform.position, Quaternion.identity);
        }
    }
}
