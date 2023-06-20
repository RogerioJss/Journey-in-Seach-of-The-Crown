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
    public AudioSource jumpAudio;

    private SpriteRenderer spriteRenderer;
    private float checkLocalX;

    [Header("Variaveis de Ataque")]
    public Transform atackCheck;
    public float radiusAttack;
    public LayerMask layerEnemy;
    float timeNextAtack;
    public AudioSource AtackAudio;

    [Header("Variaveis de combate")]
    public int maxHealth = 100;
    private int currentHealth;
    public int danoInimigo = 10;
    public float damageDuration = 0.5f;
    private bool isTakingDamage = false;
    private bool defesa = false;
    public bool estaVivo = true;
    public AudioSource DefesaAudio;
  

    [Header("Hud")]
    public Transform healthBar;         // barra verde
    public GameObject healthBarObject;  // objeto pai das barras
    private Vector3 healthBarScale;     //tamanho da barra
    private float healthPercent;       //percentual de vida para o calculo do tamanho da barra
    public GameController gameController;


    


    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jumpAudio = GameObject.Find("jumpSound").GetComponent<AudioSource>();
        AtackAudio = GameObject.Find("AtackAudio").GetComponent<AudioSource>();
        DefesaAudio = GameObject.Find("defesaAudio").GetComponent<AudioSource>();
        checkLocalX = atackCheck.localPosition.x;
        currentHealth = maxHealth;
        healthBarScale = healthBar.localScale;
        healthPercent = healthBarScale.x / maxHealth;
    }


    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600); // Força do pulo
            jumpAudio.Play();
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
                DefesaAudio.Play();
                timeNextAtack = 0f;
            }
            else
            {
                timeNextAtack -= Time.deltaTime;
            }
        }
        if(Input.GetButton("Fire2") && rb.velocity == Vector2.zero)
        {
            if(!defesa)
            {
                animator.SetTrigger("Defesa");
                defesa = true;
            }
            else if(defesa)
            {
                animator.Play("");
                defesa = false;
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
        Collider2D[] bossAttack = Physics2D.OverlapCircleAll(atackCheck.position, radiusAttack, layerEnemy);
        Collider2D[] enemiesAttack = Physics2D.OverlapCircleAll(atackCheck.position, radiusAttack, layerEnemy);
        for (int i = 0; i < enemiesAttack.Length; i++)
        {
             EnemyMoviment enemy = enemiesAttack[i].GetComponent<EnemyMoviment>();
            Boss boss = bossAttack[i]?.GetComponent<Boss>(); // Adicione o operador de verificação de nulo "?"
            if (enemy != null)
        {
                enemy.TakeDamage(); // Chame o método TakeDamage() do inimigo para aplicar dano
        }
            if (boss != null) // Adicione a verificação para o boss
        {
                boss.TakeDamage(); // Chame o método TakeDamage() do boss para aplicar dano
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
            Enemy currentEnemy = enemies[i].GetComponent<Enemy>();
            if(currentEnemy!=null){
                currentEnemy.SetPlayer(this);
            }
            
        
        }
     }

     public void TakeDamage(){
        if (!isTakingDamage)
        {
            if(defesa == true){
                danoInimigo = danoInimigo - 7;
                currentHealth -= danoInimigo;
            }else if(defesa == false){
            danoInimigo = 10;
            currentHealth -= danoInimigo;
            }
            UpdateHealthBar();
            StartCoroutine(ShowDamageEffect());
    
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                gameController.GameOver();
                // Colocar aqui a tela e a animação de morte
                
            }
        }
    }
     private IEnumerator ShowDamageEffect()
    {
        isTakingDamage = true;

        // Alterar a cor do inimigo para vermelho (ou qualquer outra cor que você desejar)
        spriteRenderer.color = Color.red;
        AtackAudio.Play();

        yield return new WaitForSeconds(damageDuration);

        // Voltar à cor normal do inimigo
        spriteRenderer.color = Color.white;

        isTakingDamage = false;
    }

     void UpdateHealthBar(){
        float newHealthBarScaleX = (float)currentHealth / maxHealth * healthBarScale.x;
        healthBar.localScale = new Vector3(newHealthBarScaleX, healthBarScale.y, healthBarScale.z);
    }
    
}