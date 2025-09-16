using UnityEngine;
using UnityEngine.InputSystem;

public class turretController : MonoBehaviour
{
    public bool controlledByPlayer = false;
    public float smoothTime = 0.06f;
    Vector3 _vel;
    public GameObject turretCamera;

    public GameObject aimAt;
    public GameObject currentTarget;
    public float range = 10f;
    public float leaveRange = 15f;
    public float aimAtSpeed = 100f;
    public float fireRate = 1f;
    TurretProjectileModule turretProjectileModule;
    stats myStats;

    public LayerMask ignoreLayer;

    float countDownForNextBullet = 0f;
    private void Start()
    {
        turretProjectileModule = GetComponent<TurretProjectileModule>();
        myStats = GetComponent<stats>();
    }

    void Update()
    {
        findClosestEnemy();
        rechamberBullet();

        if (currentTarget != null && !controlledByPlayer && currentTarget.activeInHierarchy)
        {
            aimAtEnemy();
            checkIfTargetIsVaild();
            fire();
        }

        if (myStats.health <= 0) gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (controlledByPlayer)
        {
            playerControl();
        }
    }

    void aimAtEnemy()
    {
        aimAt.transform.position = Vector3.Lerp(aimAt.transform.position, new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y + 1, currentTarget.transform.position.z), aimAtSpeed * Time.deltaTime);
    }

    void findClosestEnemy()
    {
        if(currentTarget != null && currentTarget.activeInHierarchy) return;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance && distance <= range && enemy.activeInHierarchy)
            {
                minDistance = distance;
                currentTarget = enemy;
            }
        }
    }

    void checkIfTargetIsVaild()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        
        if(distanceToTarget > leaveRange) currentTarget = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere (transform.position, leaveRange);
    }

    void rechamberBullet()
    {
        if(countDownForNextBullet > 0) countDownForNextBullet -= Time.deltaTime;
    }
    
    void fire()
    {
        if(countDownForNextBullet <= 0) 
        { 
            turretProjectileModule.Fire();
            countDownForNextBullet = 1 / fireRate;
        }
    }

    void playerControl()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Vector3 lookPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, ~ignoreLayer))
        {
            lookPoint = hit.point;
        }
        else
        {
            lookPoint = Camera.main.transform.position + Camera.main.transform.forward * 100f;
        }

        aimAt.transform.position = Vector3.SmoothDamp(aimAt.transform.position, lookPoint, ref _vel, smoothTime);


        if (Mouse.current.leftButton.isPressed) fire();
    }
}
