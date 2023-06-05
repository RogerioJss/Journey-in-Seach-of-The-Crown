using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimento1 : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;
    [SerializeField] private int velocidade = 5;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;

    private bool estaNoChao;

    private Animator animator;

    private int movendoHash = Animator.StringToHash("movendo");

    private int pulandoHash = Animator.StringToHash("pulando");

    private SpriteRenderer spriteRenderer;

    [Header("Variaveis de Ataque")]
    public Transform atackCheck;
    public float radiusAttack;
    public LayerMask layerEnemy;
    float timeNextAtack;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600); // Força do pulo
        }

        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        animator.SetBool(movendoHash, horizontalInput != 0);
        animator.SetBool(pulandoHash, !estaNoChao);

        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            atackCheck.localPosition = new Vector2(-atackCheck.localPosition.x, atackCheck.localPosition.y);//problema para o lado do colisor de ataque esta aqui
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            atackCheck.localPosition = new Vector2(-atackCheck.localPosition.x, atackCheck.localPosition.y);//problema para o lado do colisor de ataque esta aqui
        }
        if (timeNextAtack <= 0)
        {
            if (Input.GetButtonDown("Fire1") && rb.velocity == Vector2.zero) // Corrected comparison
            {
                animator.SetTrigger("Atack");
                timeNextAtack = 0f;
            }
            else
            {
                timeNextAtack -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * velocidade, rb.velocity.y);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(atackCheck.position, radiusAttack);
    }

    void PlayerAttack()
    {
        Collider2D[] enemiesAttack = Physics2D.OverlapCircleAll(atackCheck.position, radiusAttack, layerEnemy);
        for (int i = 0; 1 < enemiesAttack.Length; i++)
        {
            Debug.Log (enemiesAttack [i].name);
        }
    }
}