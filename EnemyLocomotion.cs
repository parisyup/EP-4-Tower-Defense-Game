using UnityEngine;
using UnityEngine.AI;
public class EnemyLocomotion : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject target;
    public float turnSpeed = 7.5f;
    Animator animator;
    EnemyController enemyController;

    bool _won = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        if (_won) return;
        if (!target.activeInHierarchy) return;
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        agent.destination = target.transform.position;
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (distanceToTarget <= agent.stoppingDistance)
        {
            FaceTarget();

            enemyController.withinAttackRange = true;
        }
        else enemyController.withinAttackRange = false;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    public void attackAnimation()
    {
        animator.ResetTrigger("Slash");
        animator.SetTrigger("Slash");
    }


    public void won()
    {
        agent.destination = transform.position;
        animator.SetTrigger("Celebrate");
        _won = true;
    }

    public void setTarget(GameObject t) { target = t;}
}
