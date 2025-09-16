using System.Collections.Generic;
using UnityEngine;

public class waveConfig : MonoBehaviour
{
    [Header("Spawn Setup")]
    public List<GameObject> enemyPrefab = new();
    public List<int> totalToSpawn = new();
    public float intervalSeconds = 0.5f;
    public float spawnRadius = 0f;
    public GameObject target;

    private void OnDrawGizmosSelected()
    {
        if (spawnRadius > 0f) Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
