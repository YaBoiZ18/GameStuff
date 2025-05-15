using UnityEngine;

public class PlayerAttak : MonoBehaviour
{
    private GameObject attackArea = default;
    private Animator anim;
    private bool attacking = false;
   [SerializeField] private AudioClip attackClip;

    private float attackTime = 0.25f;
    private float timer = 0f;

    [SerializeField] private PlayerMovement playerController; // Reference to the PlayerController script

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        attackArea = transform.GetChild(0).gameObject;

        // Ensure PlayerController is assigned
        if (playerController == null)
        {
            playerController = GetComponent<PlayerMovement>();
        }
    }

    bool canAttack;

    // Update is called once per frame
    private void Update()
    {
        // Check if the player is grounded and not moving
        canAttack = playerController.isGrounded() && Mathf.Abs(playerController.GetHorizontalInput()) < 0.01f;

        if (Input.GetKeyDown(KeyCode.J) && canAttack)
        {
            Attack();
        }

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= attackTime)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    private void Attack()
    {       
        anim.SetTrigger("Attack");
        attacking = true;
        attackArea.SetActive(attacking);
        SoundManager.instance.PlaySound(attackClip);
    }
}
