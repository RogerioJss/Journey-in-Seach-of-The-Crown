using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    
    public GameObject CameraPlayer;
    private float lenght, startPos;
    public float SpeedParallax;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x; 
    }

    // Update is called once per frame
    void Update()
    {
      float temp = (CameraPlayer.transform.position.x * (1 - SpeedParallax));
      float dist = (CameraPlayer.transform.position.x * SpeedParallax);

      transform.position = new Vector3 (startPos + dist, transform.position.y, transform.position.z);

      if(temp > startPos + lenght)
      {
        startPos += lenght;
      }else if (temp < startPos - lenght)
      {
        startPos -= lenght;
      }
    }   
}
