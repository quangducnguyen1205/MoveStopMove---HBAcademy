using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    protected override void Update()
    {
        if (isShoot && owner != null)
        {
            // Đảm bảo rằng Spear luôn giữ góc quay x = 90 độ
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(attackDirection.x, 0, attackDirection.z));
            targetRotation = Quaternion.Euler(90f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            transform.rotation = targetRotation;

            // Kiểm tra khoảng cách để hủy vũ khí nếu cần
            float distanceToOwner = Vector3.Distance(owner.transform.position, transform.position);
            if (!isBoomerang && distanceToOwner > owner.attackRange)
            {
                OnDespawn();
            }
        }
    }
}
