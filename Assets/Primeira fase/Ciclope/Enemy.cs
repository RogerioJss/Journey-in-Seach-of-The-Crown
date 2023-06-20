using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Movimento1 player;
    public  int maxHealth;
    public float currentHealth;

    public virtual void SetPlayer(Movimento1 playerDefine){
        if(player)return;
        player = playerDefine;

    }
}
