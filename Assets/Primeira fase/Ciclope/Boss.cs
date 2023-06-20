using UnityEngine;
using System.Collections;

public class Boss : Enemy
{
    public GameObject laser;
    public float distanciaAtaque;
    public Transform cabecaInimigo;
    public float tempoAtaque;
    private Animator animator;
    private bool isTakingDamage = false;

    [Header("Hud")]
    public Transform healthBar;         // barra verde
    public GameObject healthBarObject;  // objeto pai das barras
    private Vector3 healthBarScale;     //tamanho da barra
    private float healthPercent;       //percentual de vida para o calculo do tamanho da barra
    private SpriteRenderer spriteRenderer;
    public float damageDuration = 0.5f;


    void Start(){
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBarScale = healthBar.localScale;
        healthPercent = healthBarScale.x / maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void SetPlayer(Movimento1 playerDefine){
        if(player)return;
        player = playerDefine;
        StartCoroutine(Ataque());
    }
    IEnumerator Ataque(){
        while(player.estaVivo){
            yield return new WaitUntil(() => Vector3.Distance(player.transform.position, cabecaInimigo.position ) < distanciaAtaque );
            animator.SetTrigger("Attack");
            Quaternion lookRotation = Quaternion.LookRotation(player.transform.position - cabecaInimigo.position, Vector3.up);
            Instantiate(laser, cabecaInimigo.position, new Quaternion(0,0,lookRotation.x, lookRotation.w));
            yield return new WaitForSeconds(tempoAtaque);
        }
    }

      public void TakeDamage(){
        if (!isTakingDamage)
        {
            currentHealth -= 10;
            UpdateHealthBar();
            StartCoroutine(ShowDamageEffect());
    
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                // Colocar aqui a tela e a animação de morte
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
    void UpdateHealthBar(){
        float newHealthBarScaleX = (float)currentHealth / maxHealth * healthBarScale.x;
        healthBar.localScale = new Vector3(newHealthBarScaleX, healthBarScale.y, healthBarScale.z);
    }
}
