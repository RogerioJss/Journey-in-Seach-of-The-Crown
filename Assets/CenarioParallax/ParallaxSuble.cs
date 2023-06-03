using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSuble : MonoBehaviour
{
    [Header("Needed objects")]
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _subject;

    [Header("Config")]

    [SerializeField] private bool lockY = false;
    [SerializeField] private bool lockX = false;
    public float velocidade;

     

    private Vector3 _startingPos;

    private float travelX => this._camera.transform.position.x - this._startingPos.x;
    private float travelY => this._camera.transform.position.y - this._startingPos.y;

    private float distanceFromSubject =>_startingPos.z - this._subject.transform.position.z;
    private float clipPlane =>_camera.transform.position.z + (this.distanceFromSubject > 0 ? this._camera.farClipPlane : -this._camera.nearClipPlane);
    private float parallaxFactor => Mathf.Abs(this.distanceFromSubject) /this.clipPlane;
    private float newX => this.lockX ? this._startingPos.x : this._startingPos.x + (this.travelX * this.parallaxFactor * velocidade );
    private float newY => this.lockY ? this._startingPos.y : this._startingPos.y + (this.travelY * this.parallaxFactor * velocidade );

    private void Start(){
        this._startingPos= transform.position;
    }
    private void FixedUpdate(){
        this.transform.position = new Vector3(this.newX, this.newY, this._startingPos.z);
    }
}
