using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class Information
{
    [SerializeField] private int _golds;
    [SerializeField] private int _gems;
    [SerializeField] private int _passedLevel;
    [SerializeField] private int _upgradePoints;
    [SerializeField] private int _weaponEquip;
    [SerializeField] private int _skinEquip;
    [SerializeField] private List<int> _weaponsBought = new List<int>() { 0 };
    [SerializeField] private List<int> _skinsBought = new List<int>() { 0 };
    [SerializeField] private List<int> _upgradesLevel = new List<int>() { 0, 0, 0 };

    public int Golds
    {
        get { return _golds; }
        set
        {
            _golds = value;
            GameInformation.Instance.ChangeGolds?.Invoke(_golds);
        }
    }

    public int Gems
    {
        get { return _gems; }
        set
        {
            _gems = value;
            GameInformation.Instance.ChangeGems?.Invoke(_gems);
        }
    }

    public int PassedLevel
    {
        get { return _passedLevel; }
        set
        {
            _passedLevel = value;
            GameInformation.Instance.ChangePassedLevel?.Invoke(_passedLevel);
        }
    }

    public int UpgradePoints
    {
        get { return _upgradePoints; }
        set
        {
            _upgradePoints = value;
            GameInformation.Instance.ChangeUpgradePoints?.Invoke(_upgradePoints);
        }
    }

    public int WeaponEquip
    {
        get { return _weaponEquip; }
        set
        {
            _weaponEquip = value;
            GameInformation.Instance.ChangeWeaponEquip?.Invoke(_weaponEquip);
        }
    }

    public int SkinEquip
    {
        get { return _skinEquip; }
        set
        {
            _skinEquip = value;
            GameInformation.Instance.ChangeSkinEquip?.Invoke(_skinEquip);
        }
    }

    public List<int> WeaponsBought
    {
        get { return _weaponsBought; }
        set
        {
            _weaponsBought = value;
        }
    }

    public List<int> SkinsBought
    {
        get { return _skinsBought; }
        set
        {
            _skinsBought = value;
        }
    }

    public List<int> UpgradesLevel
    {
        get { return _upgradesLevel; }
        set
        {
            _upgradesLevel = value;
        }
    }

    public void AllInvoke()
    {
        GameInformation.Instance.ChangeGolds?.Invoke(_golds);
        GameInformation.Instance.ChangeGems?.Invoke(_gems);
        GameInformation.Instance.ChangePassedLevel?.Invoke(_passedLevel);
        GameInformation.Instance.ChangeUpgradePoints?.Invoke(_upgradePoints);
        GameInformation.Instance.ChangeWeaponEquip?.Invoke(_weaponEquip);
        GameInformation.Instance.ChangeSkinEquip?.Invoke(_skinEquip);
    }

    public override string ToString()
    {
        return "Golds: " + _golds + "\n" +
            "_gems: " + _gems + "\n" +
            "_passedLevel: " + _passedLevel + "\n" +
            "_upgradePoints: " + _upgradePoints + "\n" +
            "_weaponEquip: " + _weaponEquip + "\n" +
            "_skinEquip: " + _skinEquip + "\n" +
            "_weaponsBought count: " + _weaponsBought.Count + "\n" +
            "_skinsBought count: " + _skinsBought.Count + "\n" +
            "_upgradesLevel count: " + _upgradesLevel.Count + "\n";
    }
}

public class GameInformation : Singleton<GameInformation>
{
    private Information _information;

    public Information Information
    {
        get { return _information; }
        private set
        {
            _information = value;
        }
    }

    [DllImport("__Internal")]
    public static extern void SaveExtern(string data);
    [DllImport("__Internal")]
    public static extern void LoadExtern();

    public static Action OnInformationChange;

    public Action<int> ChangeGolds;
    public Action<int> ChangeGems;
    public Action<int> ChangePassedLevel;
    public Action<int> ChangeUpgradePoints;
    public Action<int> ChangeWeaponEquip;
    public Action<int> ChangeSkinEquip;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetInformationFromJSON(string info)
    {
        _information = JsonUtility.FromJson<Information>(info);
        StartGameController.Instance.SetGameParams();
        _information.AllInvoke();
    }

    public void Save()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (MainMenuController.isOnline)
        {
            GameParametersLoaderUI.Instance.AddStack();
            string json = JsonUtility.ToJson(_information);
            SaveExtern(json);
        }
        else
        {
            string json = JsonUtility.ToJson(_information);
            PlayerPrefs.SetString("information", json);
            PlayerPrefs.Save();
        }
#elif UNITY_2019_1_OR_NEWER
        string json = JsonUtility.ToJson(_information);
        PlayerPrefs.SetString("information", json);
        PlayerPrefs.Save();
#endif
    }

    private void OnDestroy()
    {
        OnInformationChange -= Save;
    }
}
