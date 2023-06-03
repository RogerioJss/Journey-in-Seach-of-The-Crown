using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimento1 : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;
    [SerializeField]private int velocidade = 5;

    [SerializeField]private Transform peDoPersonagem;
    [SerializeField]private LayerMask chaoLayer;

    private bool estaNoChao;

    private Animator animator;

    private int movendoHash = Animator.StringToHash("movendo");
    private int pulandoHash = Animator.StringToHash("pulando");

    private SpriteRenderer spriteRenderer;

    public void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space) && estaNoChao){
            rb.AddForce(Vector2.up * 600); // Força do pulo
        }

        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        animator.SetBool(movendoHash, horizontalInput !=0);
        animator.SetBool(pulandoHash, !estaNoChao);

        if(horizontalInput > 0){
            spriteRenderer.flipX = false;
        }
        else if(horizontalInput < 0){
            spriteRenderer.flipX = true;
        }
    }

     private void FixedUpdate() {
        rb.velocity = new Vector2(horizontalInput * velocidade, rb.velocity.y);
    }
}
