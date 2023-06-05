using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentação : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;

    [SerializeField] public float speed = 5;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;

    private bool estaNoChao;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private int movendoHash = Animator.StringToHash("movendo");
    private int pulandoHash = Animator.StringToHash("pulando");
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space) && estaNoChao){
            rb.AddForce(Vector2.up * 460);
        }

        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f,chaoLayer);

        animator.SetBool(movendoHash, horizontalInput != 0);
        animator.SetBool(pulandoHash, !estaNoChao);

        if(horizontalInput>0){
            spriteRenderer.flipX = false;
        }
        else if(horizontalInput<0){
            spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }
}
