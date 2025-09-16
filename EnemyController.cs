using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool withinAttackRange;
    public float timeBetweenAttacks = 2f;
    public float attackCoolDown = 0f;
    stats myStats;

    public GameObject target;

    public event Action<EnemyController> Died;
    bool wasAlive;

    EnemyLocomotion locomotion;
    void Start()
    {
        locomotion = GetComponent<EnemyLocomotion>();
        myStats = GetComponent<stats>();
    }

    private void OnEnable()
    {
        wasAlive = true;
    }

    void Update()
    {
        attackCheck();
        checkIfTargetIsVaild();

        if(myStats.health <= 0) gameObject.SetActive(false);

        if(target.activeInHierarchy)locomotion.setTarget(target);
    }

    void attackCheck()
    {
        if (attackCoolDown <= 0 && withinAttackRange && target != null)
        {
            locomotion.attackAnimation();
            attackCoolDown = timeBetweenAttacks;

            stats targetStats = target.GetComponent<stats>();
            if (targetStats != null)
            {
                targetStats.takeDamage(myStats.damage);
            }


        }
        else if (attackCoolDown > 0) attackCoolDown -= Time.deltaTime;
    }

    void checkIfTargetIsVaild()
    {
        if (!target.activeInHierarchy)
        {
            locomotion.won();
        }

    }

    private void OnDisable()
    {
        if (wasAlive)
        {
            wasAlive = false;
            Died?.Invoke(this);
        }
    }
}
