using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Weapon
{
    protected override void Update()
    {
        base.Update();
        // Xoay quanh trục ngang khi ném
        if (isShoot){
            transform.Rotate(Vector3.right * 500 * Time.deltaTime);
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        transform.position += new Vector3(0, 0.1f,0);
        // Đặt góc quay 
        transform.rotation = Quaternion.Euler(0, 90f, 0);
    }
}
