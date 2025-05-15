using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool IsDead => dead;

    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("Iframs")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Componets")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;


    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;



    private void Awake()
    {
        currentHealth = startingHealth; 
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetBool("Grounded", true);
                anim.SetTrigger("Die");

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

    public void AddHealth(float _value) 
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        // Invunerability duration
        for (int i = 0; i < numberOfFlashes; i++) 
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(8, 9, false);

    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Respawn()
    {       
        AddHealth(startingHealth);
        anim.ResetTrigger("Die");
        anim.Play("Idle");
        StartCoroutine(Invunerability());
        dead = false;

        //Activate all attached component classes
        foreach (Behaviour componet in components) 
        {
            componet.enabled = true;
        }
    }
}
