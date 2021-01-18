using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MutantController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    [SerializeField] float health = 100f;
    public float damage;

    NavMeshAgent agent;
    public CropManager cropManager;
    GameObject field;

    Transform target;
    Transform player;

    bool dead = false;
    bool canAttack = false;
    float attackCooldown = 2f;

    public float attackRadius = 1.5f;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject.transform;
        cropManager = FindObjectOfType<CropManager>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        FindNewTarget();
    }

    private void FindNewTarget()
    {
        if (cropManager.GetRandomField() != null)
        {
            target = cropManager.GetRandomField().GetComponent<SimpleField>().GetTransform();
        }
        else
        {
            target = player.transform;
        }
    }

    private void Update()
    { 
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
        
        if(agent.speed > 0)
        {
            GetComponent<Animator>().SetBool("Run Forward", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("Run Forward", false);
        }

        if(target.GetComponentInParent<SimpleField>())
        {
            if(!target.GetComponentInParent<SimpleField>().HasCrops())
            {
                FindNewTarget();
            }
        }

        if(2 > Vector3.Distance(this.transform.position, target.transform.position))
        {
            GetComponent<Animator>().SetBool("Run Forward", false);
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        

        if (canAttack == true && attackCooldown <= 0f)
        {
            if (target.GetComponentInParent<SimpleField>())
            {
                GetComponent<Animator>().SetBool("Smash Attack", true);
                target.parent.GetComponent<Health>().TakeDamage(damage);
            }
            else
            {
                GetComponent<Animator>().SetBool("Stab Attack", true);
                target.GetComponent<Health>().TakeDamage(damage);
                Debug.Log("BANG!");
            }
            attackCooldown = 2f;
        }
        else
        {
            GetComponent<Animator>().SetBool("Stab Attack", false);
        }

        if (canAttack)
        {
            attackCooldown -= Time.deltaTime;
        }
        else
        {
            attackCooldown = 2f;
        }
    }

    public bool IsDead()
    {
        return dead;
    }

    public void TakeDamage(float damageAmount)
    {
        if(!dead)
        {
            health -= damageAmount;
            GetComponent<Animator>().SetBool("Run Forward", false);
            GetComponent<Animator>().SetBool("Take Damage", true);

            if (health <= 0)
            {
                agent.speed = 0;
                GetComponent<Animator>().SetBool("Die", true);
                dead = true;
                Destroy(this.gameObject, 2f);
                FindObjectOfType<MutantManager>().GetComponent<MutantManager>().RemoveMutant(this.transform);
                player.GetComponent<Wallet>().AddToBalance(100);
            }
        }
    }

    public float GetHealth()
    {
        return health;
    }
}
