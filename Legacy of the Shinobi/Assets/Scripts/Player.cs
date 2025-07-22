using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Atributos do jogador
    public Slider healthSlider;
    public int maxHealth = 100;
    public int currentHealth;
    private float initialSpeed = 8f;

    // Movimentação
    public float moveSpeed = 0;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isAttack = false;

    // Objetos
    private Camera mainCamera;
    private PlayerInputActions inputActions;

    private Animator playerAnimator;
    private Text healthText;

    public GoblinController goblinTeste;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        currentHealth = maxHealth; // Inicializa a vida do jogador
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Click.performed += OnClickPerformed;
        inputActions.Player.Attack.started += ctx => StartAttack();
        inputActions.Player.Attack.canceled += ctx => EndAttack();
    }

    private void OnDisable()
    {
        inputActions.Player.Click.performed -= OnClickPerformed;
        inputActions.Disable();
    }

    private void Start()
    {
        moveSpeed = initialSpeed;
        mainCamera = Camera.main;
        targetPosition = transform.position;
        playerAnimator = GetComponent<Animator>();

        // Configura o slider de vida
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText = healthSlider.GetComponentInChildren<Text>();

    }

    private void Update()
    {
        HealthUpdate();
        if (Derrotado) return;
        if (isAttack)
        {
            playerAnimator.SetInteger("Movement", 2);
            return;
        }
        else
        {
            playerAnimator.SetInteger("Movement", 0);

        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            Vector2 moveDirection = targetPosition - transform.position;
            Vector2 normalizedDirection = moveDirection.normalized;

            playerAnimator.SetInteger("Movement", 1);

            playerAnimator.SetFloat("AxisX", normalizedDirection.x);
            playerAnimator.SetFloat("AxisY", normalizedDirection.y);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                transform.position = targetPosition;
                isMoving = false;
                playerAnimator.SetInteger("Movement", 0);

            }
        }


    }

    private void HealthUpdate()
    {
        healthSlider.value = currentHealth;
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;
        targetPosition = worldPos;
        isMoving = true;
    }

    void StartAttack()
    {
        isAttack = true;
        moveSpeed = 0f;
        goblinTeste.ReceberDano(10);
    }

    void EndAttack()
    {
        isAttack = false;
        moveSpeed = initialSpeed;
    }

    public void ReceberDano(int damage)
    {
        currentHealth -= damage;
        playerAnimator.SetTrigger("RecebendoDano");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        // Lógica de morte do jogador
        Debug.Log("Player morreu.");
        playerAnimator.SetBool("Morto", true);
        // Aqui você pode adicionar lógica para reiniciar o jogo, carregar uma cena, etc.
    }

    public bool Derrotado
    {
        get { return currentHealth <= 0; }
    }
}
