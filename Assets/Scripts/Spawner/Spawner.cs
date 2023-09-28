using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private SpawnerParameters _spawnerParameters;

    public SpawnerParameters SpawnerParameters => _spawnerParameters;

    protected Coroutine _spawnerCoroutine;
    private int _minPercent = 0;
    private int _maxPercent = 100;

    private void Start()
    {
        LevelProgress.Instance.OnCompletedLevel += StopSpawner;
    }

    public virtual void StartSpawner()
    {
        int countObj = Random.Range(_spawnerParameters.MinCountObjects, _spawnerParameters.MaxCountObjects);

        if (_spawnerCoroutine == null)
            _spawnerCoroutine = StartCoroutine(StartSpawnCoroutine(countObj, GetTime()));
    }

    public virtual void StopSpawner()
    {
        if (_spawnerCoroutine != null)
            StopCoroutine(_spawnerCoroutine);

        _spawnerCoroutine = null;
    }

    private IEnumerator StartSpawnCoroutine(int countObjects, float reloadingTime)
    {
        CreateObjects(countObjects, GetTime());
        yield return new WaitForSeconds(reloadingTime);
        _spawnerCoroutine = StartCoroutine(StartSpawnCoroutine(countObjects, GetTime()));
    }

    protected void CreateObjects(int countObjects, float reloadingTime)
    {
        Vector2 z = new Vector2(_collider.bounds.min.z, _collider.bounds.max.z);
        Vector2 x = new Vector2(_collider.bounds.min.x, _collider.bounds.max.x);

        for (int i = 0; i < countObjects; i++)
        {
            foreach (var spawn in _spawnerParameters.SpawnPrefabs)
            {
                if (Random.Range(_minPercent, _maxPercent) <= spawn.Chance)
                {
                    Unit spawnObject = Instantiate(spawn.SpawnPrefab);
                    Vector3 newPos = new Vector3(Random.Range(x.x, x.y), transform.position.y, Random.Range(z.x, z.y));

                    if (spawnObject is EnemyAI)
                    {
                        ((EnemyAI)spawnObject).NavMeshAgent.Warp(newPos);
                    }
                    else
                    {
                        spawnObject.transform.position = newPos;
                    }

                    break;
                }
            }
        }
    }

    private float GetTime()
    {
        return Random.Range(_spawnerParameters.MinReloadingTime, _spawnerParameters.MaxReloadingTime) - (Mathf.Pow(GameInformation.Instance.Information.PassedLevel, _spawnerParameters.ReduceReloadingTime));
    }

    private void OnDestroy()
    {
        LevelProgress.Instance.OnCompletedLevel -= StopSpawner;
    }
}
