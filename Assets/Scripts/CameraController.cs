using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = .125f;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = target.position - transform.position;    
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 desiredPos = target.position - offset;

        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);

        if(target.position.y < .8f)
        {
            smoothPos.y = transform.position.y;
        }
        transform.position = smoothPos;
    }
}
