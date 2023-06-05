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
    private float moveDirection = 1f;
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

    private bool isAttacking = false;
    public float attackDuration = 1.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        rb2D = GetComponent<Rigidbody2D>();
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
            else if (distance <= 10f) // Substitua 10f pela distância de visão desejada
            {
                bool isPlayerAhead = player.transform.position.x > transform.position.x;

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

                if (isGrounded && !isAttacking)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    isWalking = true;
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
    }

    public void TakeDamage()
    {
        if (!isTakingDamage)
        {
            currentHealth -= 10;

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

    private IEnumerator Knockback()
    {
        const float knockbackDuration = 0.2f;
        const float knockbackForce = 10f;

        Vector2 knockbackDirection = (transform.position - player.transform.position).normalized;
        rb2D.velocity = knockbackDirection * knockbackForce;

        yield return new WaitForSeconds(knockbackDuration);

        rb2D.velocity = Vector2.zero;
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
}
