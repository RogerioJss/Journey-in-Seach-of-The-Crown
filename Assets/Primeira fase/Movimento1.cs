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
    private float checkLocalX;

    [Header("Variaveis de Ataque")]
    public Transform atackCheck;
    public float radiusAttack;
    public LayerMask layerEnemy;
    float timeNextAtack;

    [Header("Variaveis de combate")]
    public int maxHealth = 100;
    private int currentHealth;
    public float damageDuration = 0.5f;
    private bool isTakingDamage = false;

    private EnemyMoviment currentEnemy;


    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        checkLocalX = atackCheck.localPosition.x;
        currentHealth = maxHealth;
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
            atackCheck.localPosition = new Vector2(checkLocalX, atackCheck.localPosition.y);//problema para o lado do colisor de ataque esta aqui
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            atackCheck.localPosition = new Vector2(-checkLocalX, atackCheck.localPosition.y);//problema para o lado do colisor de ataque esta aqui
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

        UpdateEnemy();
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
        for (int i = 0; i < enemiesAttack.Length; i++)
        {
            //Debug.Log (enemiesAttack [i].name); mostra o nome dos inimigos que o jogador ataca
             EnemyMoviment enemy = enemiesAttack[i].GetComponent<EnemyMoviment>();
            if (enemy != null)
        {
            enemy.TakeDamage(); // Chame o método TakeDamage() do inimigo para aplicar dano
        }
        }
    }


     void UpdateEnemy(){
        Camera cam = Camera.main;
        float height = cam.orthographicSize * 2;
        float width = height * cam.aspect;
        Collider2D[] enemies = Physics2D.OverlapAreaAll(
            new Vector2 (cam.transform.position.x - (width / 2 ), cam.transform.position.y + (height / 2 ) ),
            new Vector2 (cam.transform.position.x + (width / 2 ), cam.transform.position.y - (height / 2 ) ),
            layerEnemy
            
        );
        for (int i = 0; i < enemies.Length; i++)
        {
            //Debug.Log (enemies [i].name); mostra os inimigos que estao na tela

            if(currentEnemy == null){
                currentEnemy = enemies[i].GetComponent<EnemyMoviment>();
                currentEnemy.player = this;
            }
        }
     }

     public void TakeDamage(){
        if (!isTakingDamage)
        {
            currentHealth -= 10;

            StartCoroutine(ShowDamageEffect());


            if (currentHealth <= 0)
            {
                // Colocar aqui a tela e a animação de morte
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
    
}