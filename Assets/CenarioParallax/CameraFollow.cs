using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public void FixedUpdate(){
            transform.position = new Vector3(Mathf.Clamp(transform.position.x,-43.5f,42.91535f), transform.position.y, transform.position.z);
    }

}
