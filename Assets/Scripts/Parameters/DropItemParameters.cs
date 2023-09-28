using UnityEngine;

[CreateAssetMenu(fileName = "DropItemParameters", menuName = "CustomParameters/DropItemParameters")]
public class DropItemParameters : ScriptableObject
{
    [SerializeField] private Item _itemPrefab;
    [SerializeField] private int _minDrops;
    [SerializeField] private int _maxDrops;

    public Item ItemPrefab => _itemPrefab;
    public int MinDrops => _minDrops;
    public int MaxDrops => _maxDrops;
}
