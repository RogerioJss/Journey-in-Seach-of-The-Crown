using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float tempoLaser;
    public Movimento1 player;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die(){
        yield return new WaitForSeconds(tempoLaser);
        Destroy(gameObject);
    }

   void OnTriggerEnter2D(Collider2D col)
{
    Movimento1 player = col.GetComponent<Movimento1>();
    if (player != null)
    {
        player.TakeDamage();
        Debug.Log("Acertou o player");
    }
}


}
