using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Transform target;
    Vector3 velocity = Vector3.zero;

    [Range(0, 1)]
    public float smoothTime;
    public Vector3 positionOffSet;
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + positionOffSet;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

}
