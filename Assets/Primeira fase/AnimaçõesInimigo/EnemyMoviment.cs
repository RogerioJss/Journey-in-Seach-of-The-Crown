using UnityEngine;
using System.Collections;

public class EnemyMoviment : MonoBehaviour
{
   
    public Movimento1 player;

    public Transform target;
    public float speed = 5f;
    public float attackDistance = 2f;
    private bool isWalking = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public int maxHealth = 20;
    private int currentHealth;
    public float damageDuration = 0.5f;
    private bool isTakingDamage = false;

    private Rigidbody2D rb2D;
    private bool isGrounded = false;
    private float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public int danoPlayer = 10;    

    private bool isAttacking = false;
    public float attackDuration = 1.0f;

    private bool isKnockBack;

    [Header("Hud")]
    public Transform healthBar;         // barra verde
    public GameObject healthBarObject;  // objeto pai das barras
    private Vector3 healthBarScale;     //tamanho da barra
    private float healthPercent;       //percentual de vida para o calculo do tamanho da barra



    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        rb2D = GetComponent<Rigidbody2D>();
        healthBarScale = healthBar.localScale;
        healthPercent = healthBarScale.x / maxHealth; 
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= attackDistance)
            {
                if (!isAttacking)
                {
                    StartCoroutine(Attack());
                }
            }
            else if (distance <= 10f && isGrounded && !isAttacking && !isKnockBack) // Substitua 10f pela distância de visão desejada
            {
                bool isPlayerAhead = player.transform.position.x > transform.position.x;
                isWalking = true;

                if (isPlayerAhead)
                {
                    // Player está à frente do inimigo
                    if (!spriteRenderer.flipX)
                    {
                        spriteRenderer.flipX = true; // Inverte a sprite horizontalmente
                    }
                }
                else
                {
                    // Player está atrás do inimigo
                    if (spriteRenderer.flipX)
                    {
                        spriteRenderer.flipX = false; // Reseta a sprite horizontalmente
                    }
                }

            }
            else
            {
                isWalking = false;
            }
        }

        animator.SetBool("IsWalking", isWalking);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isWalking)
        {
            float playerDir = Mathf.Sign(player.transform.position.x - transform.position.x);
            rb2D.velocity = new Vector2( playerDir * speed, rb2D.velocity.y);
        }
    }

    public void TakeDamage()
    {
        if (!isTakingDamage)
        {
            currentHealth -= danoPlayer;
            UpdateHealthBar();

            StartCoroutine(ShowDamageEffect());

            // Empurrão
            StartCoroutine(Knockback());

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ShowDamageEffect()
    {
        isTakingDamage = true;

        // Alterar a cor do inimigo para vermelho (ou qualquer outra cor que você desejar)
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(damageDuration);

        // Voltar à cor normal do inimigo
        spriteRenderer.color = Color.white;

        isTakingDamage = false;
    }

    public IEnumerator Knockback()
{
    isKnockBack = true;
    const float knockbackDuration = 0.3f;
    const float knockbackForce = 10f;

    float playerDir = Mathf.Sign(player.transform.position.x - transform.position.x);
    float elapsedTime = 0f;
    while (elapsedTime < knockbackDuration)
    {
        rb2D.velocity = new Vector2(-playerDir * knockbackForce, rb2D.velocity.y);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    rb2D.velocity = Vector2.zero;

    isKnockBack = false;
}


    private IEnumerator Attack()
    {
        isAttacking = true;

        animator.SetTrigger("Attack");
        // Adicione aqui qualquer lógica adicional relacionada ao ataque

        player.TakeDamage();

        yield return new WaitForSeconds(attackDuration);



        isAttacking = false;
    }


    void UpdateHealthBar(){
        float newHealthBarScaleX = (float)currentHealth / maxHealth * healthBarScale.x;
        healthBar.localScale = new Vector3(newHealthBarScaleX, healthBarScale.y, healthBarScale.z);
    }
}
