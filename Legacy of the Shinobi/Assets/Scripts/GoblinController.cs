using UnityEngine;

public class GoblinController : MonoBehaviour
{
    //Atributos do goblin
    public int dano = 10; // Dano do ataque do goblin
    public int health = 50; // Vida do goblin
    public int maxHealth = 50; // Vida máxima do goblin
    public int goldDrop = 5; // Ouro que o goblin solta ao ser derrotado
    public float moveSpeedGoblin = 3.5f;
    private Vector2 goblinDirection;
    private Rigidbody2D goblinRigidBody2D;

    public DetectionController detectionArea;

    public SpriteRenderer spriteRenderer;

    public Animator goblinAnimator;

    public float maxDistanceAttack = 1.2f;

    public float attackCooldown = 1.0f;
    private float lastAttackTime;

    public float attackDuration = 3f; // Tempo igual à duração da animação
    private float attackStartTime;
    private bool isAttacking;

    private Player playerMaisProximo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goblinRigidBody2D = GetComponent<Rigidbody2D>();
        lastAttackTime = -attackCooldown;
    }


    private void FixedUpdate()
    {
        if (isAttacking)
        {
            // bloqueia movimentação e animações durante ataque
            if (Time.time >= attackStartTime + attackDuration)
            {
                Debug.Log("Terminando ataque. Tempo atual: " + Time.time + " | Tempo ataque iniciou: " + attackStartTime);

                goblinAnimator.SetInteger("movimento", 0); // volta pra Idle
                isAttacking = false;
            }

            return;
        }

        if (detectionArea.detectedObjs.Count > 0)
        {
            playerMaisProximo = detectionArea.detectedObjs[0].GetComponent<Player>();

            Vector3 targetPos = detectionArea.detectedObjs[0].transform.position;
            goblinDirection = (targetPos - transform.position).normalized;
            goblinRigidBody2D.MovePosition(goblinRigidBody2D.position + goblinDirection * moveSpeedGoblin * Time.fixedDeltaTime);
            VerificarFlipSprite(goblinDirection);
            VerificarPossibilidadeAttack(targetPos);
            goblinAnimator.SetInteger("movimento", 1);

        }
        else
        {
            goblinAnimator.SetInteger("movimento", 0);
        }
    }

    private void VerificarFlipSprite(Vector2 goblinDirection)
    {
        if (goblinDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private void VerificarPossibilidadeAttack(Vector3 targetPos)
    {

        if (playerMaisProximo && playerMaisProximo.Derrotado)
        {
            return;
        }

        if (Vector2.Distance(transform.position, targetPos) <= maxDistanceAttack)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Attack();
            }
        }
    }

    private void Attack()
    {
        Debug.Log("Goblin attack!");
        goblinAnimator.SetTrigger("atacando"); // atacando
        attackStartTime = Time.time;
        isAttacking = true;
        Debug.Log("Iniciando ataque! Tempo: " + Time.time);
    }

    public void AplicarDano()
    {
        if (playerMaisProximo != null)
        {
            playerMaisProximo.ReceberDano(dano); // Exemplo de dano, ajuste conforme necessário
        }
    }
    
    public void ReceberDano(int danoRecebido)
    {
        health -= danoRecebido;
        if (health <= 0)
        {
            goblinAnimator.SetBool("morto", true); // Inicia animação de morte
            Debug.Log("Goblin derrotado!");
            // Destroy(gameObject); // Destrói o goblin quando a vida chega a zero
        }
        else
        {
            goblinAnimator.SetTrigger("recebendo_dano"); // Inicia animação de dano
        }
    }

}
