using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimento : MonoBehaviour
{
    private float horizontalinput;
    private Rigidbody2D rb;

    [SerializeField] private int velocidade = 5;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;

    private bool estaNoChao;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private int movendoHash = Animator.StringToHash("Movendo");
    private int pulandoHash = Animator.StringToHash("Pulando");
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
   

    // Update is called once per frame
    void Update()
    {
        horizontalinput = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space ) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600);
        }

        estaNoChao =  Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);


        animator.SetBool(movendoHash, horizontalinput != 0);
        animator.SetBool(pulandoHash, !estaNoChao);   

        if(horizontalinput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalinput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontalinput * velocidade, rb.velocity.y);
    }
}
