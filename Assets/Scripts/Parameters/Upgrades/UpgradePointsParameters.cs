using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UpgradePointsParameters", menuName = "CustomParameters/UpgradesParameters/UpgradePointsParameters")]
public class UpgradePointsParameters : ScriptableObject
{
    [Header("How long does it take to complete a level to get a gem?")]
    [SerializeField][Range(0, 1)] private List<float> _timesToGem;
    [Header("how many gems per time?")]
    [SerializeField][Min(0)] private int _numOfGems;

    public List<float> TimesToGem => _timesToGem;
    public int NumOfGems => _numOfGems;
}