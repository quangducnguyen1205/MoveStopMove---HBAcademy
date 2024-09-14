using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 rotationAngle = new Vector3(60f, 0, 0);
    private Transform target;
    void Start()
    {
        transform.rotation = Quaternion.Euler(rotationAngle);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null){
            transform.position = Vector3.Lerp(transform.position, target.position + offSet, Time.deltaTime * speed);
        }
    }
    public void SetTarget(Transform newTarget){
        target = newTarget;
    }
}
