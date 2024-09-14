using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Weapon
{
    private bool isReturn = false;
    protected override void Update(){
        if (isShoot && owner != null){
            float distanceToOwner = Vector3.Distance(owner.transform.position, transform.position);
            if (distanceToOwner > owner.attackRange){
                isReturn = true;
                //rb.velocity = -rb.velocity; // Đảo ngược vận tốc để boomerang quay lại
            }
        }
        if (isReturn){
            SetDirection((owner.transform.position - transform.position).normalized);
            rb.velocity = attackDirection * attackSpped;
            if (Vector3.Distance(owner.transform.position, transform.position) < 0.01f){
                isReturn = false;
                OnDespawn();
            }
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        isBoomerang = true;
        transform.rotation = Quaternion.Euler(0, 90f, 0);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (!isShoot){
            return;
        }
        Character character = other.GetComponent<Character>();
        if (character != null && character != owner){
            character.OnDeath(owner);

            ParticlePool.Play(ParticleType.Destruction_air_blue, transform.position, Quaternion.identity);
        }
    }
}
