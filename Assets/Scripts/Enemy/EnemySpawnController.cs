using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public class EnemySpawnController : MonoBehaviour
{
    [System.Serializable]
    public class Enemy
    {
        public GameObject enemyPrefab;
        [Tooltip("Düşmanın spawn olma şansını belirler")]
        public float weight = 1f;
        [Tooltip("Düşman tipini belirleyen benzersiz isim")]
        public string enemyType;
    }
    [System.Serializable]
    public class SpawnPoint
    {
        [Tooltip("Spawn noktasının pozisyonu")]
        public Transform point;
        [Tooltip("Bu noktada spawn olma şansını belirler")]
        public float weight = 1f;
        [Tooltip("Spawn noktasının görsel çapı (sadece editor'da)")]
        public float visualRadius = 1f;
        [Tooltip("Bu noktada spawn olabilecek düşman tipleri")]
        public List<string> allowedEnemyTypes = new List<string>();
        [Tooltip("Eğer true ise, sadece izin verilen düşmanlar spawn olabilir")]
        public bool restrictEnemyTypes = false;
    }
    public event Action<GameObject> OnEnemySpawned;
    [Header("Düşman Ayarları")]
    [Tooltip("Spawn edilebilecek düşmanların listesi")]
    public List<Enemy> enemies = new List<Enemy>();
    [Header("Spawn Noktası Ayarları")]
    [Tooltip("Düşmanların spawn olabileceği noktalar")]
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    [Header("Debug")]
    public bool showSpawnPoints = true;
    public Color spawnPointColor = Color.red;
    public Color restrictedSpawnPointColor = Color.yellow;
    private Transform playerTransform;
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnEnemy();
        }
    }
    public void SpawnEnemy()
    {
        var validSpawnPoints = GetValidSpawnPoints();
        if (validSpawnPoints.Count == 0)
        {
            Debug.LogWarning("Uygun spawn noktası bulunamadı!");
            return;
        }
        SpawnPoint selectedPoint = GetRandomSpawnPoint(validSpawnPoints);
        if (selectedPoint == null) return;
        GameObject enemyPrefab = GetRandomEnemyForSpawnPoint(selectedPoint);
        if (enemyPrefab != null)
        {
            // Karaktere doğru rotasyonu hesapla
            Vector3 directionToPlayer = playerTransform.position - selectedPoint.point.position;
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            GameObject spawnedEnemy = Instantiate(enemyPrefab, selectedPoint.point.position, rotation);
            OnEnemySpawned?.Invoke(spawnedEnemy);
        }
        else
        {
            Debug.Log("Bu spawn noktası için uygun düşman bulunamadı!");
        }
    }
    public void SpawnSpecificEnemy(string enemyType)
    {
        var validSpawnPoints = GetValidSpawnPointsForEnemyType(enemyType);
        if (validSpawnPoints.Count == 0)
        {
            Debug.LogWarning($"{enemyType} tipi için uygun spawn noktası bulunamadı!");
            return;
        }
        SpawnPoint selectedPoint = GetRandomSpawnPoint(validSpawnPoints);
        if (selectedPoint == null) return;
        Enemy enemyToSpawn = enemies.Find(e => e.enemyType == enemyType);
        if (enemyToSpawn != null)
        {
            Vector3 directionToPlayer = playerTransform.position - selectedPoint.point.position;
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            GameObject spawnedEnemy = Instantiate(enemyToSpawn.enemyPrefab, selectedPoint.point.position, rotation);
            OnEnemySpawned?.Invoke(spawnedEnemy);
        }
    }
    private List<SpawnPoint> GetValidSpawnPoints()
    {
        return spawnPoints.Where(sp => sp.point != null).ToList();
    }
    private List<SpawnPoint> GetValidSpawnPointsForEnemyType(string enemyType)
    {
        return spawnPoints.Where(sp =>
            sp.point != null &&
            (!sp.restrictEnemyTypes || sp.allowedEnemyTypes.Contains(enemyType)))
            .ToList();
    }
    private SpawnPoint GetRandomSpawnPoint(List<SpawnPoint> validPoints)
    {
        if (validPoints.Count == 0) return null;
        float totalWeight = validPoints.Sum(p => p.weight);
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        foreach (SpawnPoint point in validPoints)
        {
            currentWeight += point.weight;
            if (randomValue <= currentWeight)
            {
                return point;
            }
        }
        return validPoints[0];
    }
    private GameObject GetRandomEnemyForSpawnPoint(SpawnPoint spawnPoint)
    {
        List<Enemy> validEnemies;
        if (spawnPoint.restrictEnemyTypes)
        {
            validEnemies = enemies.Where(e =>
                spawnPoint.allowedEnemyTypes.Contains(e.enemyType)).ToList();
        }
        else
        {
            validEnemies = enemies;
        }
        if (validEnemies.Count == 0) return null;
        float totalWeight = validEnemies.Sum(e => e.weight);
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        foreach (Enemy enemy in validEnemies)
        {
            currentWeight += enemy.weight;
            if (randomValue <= currentWeight)
            {
                return enemy.enemyPrefab;
            }
        }
        return validEnemies[0].enemyPrefab;
    }
    private void OnDrawGizmos()
    {
        if (!showSpawnPoints) return;
        foreach (SpawnPoint point in spawnPoints)
        {
            if (point.point != null)
            {
                // Kısıtlamalı spawn noktalarını farklı renkte göster
                Gizmos.color = point.restrictEnemyTypes ? restrictedSpawnPointColor : spawnPointColor;
                Gizmos.DrawWireSphere(point.point.position, point.visualRadius);
                // İzin verilen düşman tiplerini göster
                if (point.restrictEnemyTypes && point.allowedEnemyTypes.Count > 0)
                {
                    string allowedTypes = string.Join(", ", point.allowedEnemyTypes);
                    UnityEditor.Handles.Label(point.point.position + Vector3.up * point.visualRadius,
                        $"Allowed: {allowedTypes}");
                }
            }
        }
    }
}