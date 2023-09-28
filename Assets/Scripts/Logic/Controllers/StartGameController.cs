using System;
using System.Collections.Generic;
using UnityEngine;

public class StartGameController : Singleton<StartGameController>
{
    private int StartLevelNum = 1;
    private int DeafaultWeaponNum = 0;
    private int DeafaultSkinNum = 0;
    private const int ElementaryLevel = 1;
    private const int StarterPassedLevel = 1;
    private const int StarterSkin = 0;
    private const int StarterWeapon = 0;

    [SerializeField] private List<Shop> _shops;
    [SerializeField] private List<EnemySpawner> _enemySpawners;
    [SerializeField] private List<Spawner> _spawners;
    [SerializeField] private PointsTimer _pointsTimer;
    [SerializeField] private MoveTo _startTerrain;
    [SerializeField] private UpgradeCanvas _upgradeCanvas;

    public event Action OnStartGame;

    private void Start()
    {
        _startTerrain.DisableScript();
        OnStartGame += _startTerrain.EnableScript;
    }

    public void SetGameParams()
    {
        ChangeLanguageController.Instance.SetLanguage();

        if (GameInformation.Instance.Information.PassedLevel == 0)
            GameInformation.Instance.Information.PassedLevel = StartLevelNum;

        if (GameInformation.Instance.Information.SkinsBought == null)
        {
            GameInformation.Instance.Information.SkinsBought = new List<int> { DeafaultSkinNum };
        }

        _shops[ElementaryLevel].UnlockItems(GameInformation.Instance.Information.SkinsBought);
        _shops[StarterPassedLevel].Equip(GameInformation.Instance.Information.SkinEquip);

        if (GameInformation.Instance.Information.WeaponsBought == null)
        {
            GameInformation.Instance.Information.WeaponsBought = new List<int> { DeafaultWeaponNum };
        }

        _shops[StarterSkin].UnlockItems(GameInformation.Instance.Information.WeaponsBought);
        _shops[StarterWeapon].Equip(GameInformation.Instance.Information.WeaponEquip);
        LevelProgressUI.Instance.UpdateLevelNumText(GameInformation.Instance.Information.PassedLevel);
        _upgradeCanvas.gameObject.SetActive(true);
    }

    public void StartLevel()
    {
        int countKillsOnLevel = 0;
        float time = 0;
        float additionalTime = 1;

        foreach (var spawner in _spawners)
            spawner.StartSpawner();

        foreach (var enemySpawner in _enemySpawners)
            enemySpawner.StartSpawner();

        foreach (var enemySpawner in _enemySpawners)
        {
            SpawnerEnemiesParameters spawnerParameters = (SpawnerEnemiesParameters)enemySpawner.SpawnerParameters;
            countKillsOnLevel = enemySpawner.ObjectsCount * enemySpawner.WavesToPassed; 
            time = (spawnerParameters.TimeToKillInSeconds * countKillsOnLevel + 
                (enemySpawner.WavesToPassed * enemySpawner.EnemiesReloadingTime + additionalTime)) * spawnerParameters.AdditionalTime;
        }

        LevelProgress.Instance.RequiredNumberOfKills = countKillsOnLevel;

        if (_pointsTimer != null)
            _pointsTimer.StartTimer(time);

        OnStartGame?.Invoke();
    }

    private void OnDestroy()
    {
        OnStartGame -= _startTerrain.EnableScript;
    }
}
