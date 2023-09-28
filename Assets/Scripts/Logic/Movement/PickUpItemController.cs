using DG.Tweening;
using UnityEngine;

public class PickUpItemController : MonoBehaviour
{
    public const int DefaultLayer = 0;
    public const float DefaultMagniteRange = 100f;

    [SerializeField] private LayerMask _itemLayers;
    [SerializeField] private float _magniteRange;
    [SerializeField] private float _itemLiftingTime;
    [SerializeField] private float _upItemRange;

    public float MagniteRange { get { return _magniteRange; } set { _magniteRange = value; } }

    private float _clearMagniteRange = 0;

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _magniteRange, _itemLayers);

        foreach (var collider in colliders)
        {
            Item item = collider.gameObject.GetComponent<Item>();
            item.gameObject.layer = DefaultLayer;
            item.Tween.SetAutoKill(false);
            item.Tween = item.transform.DOMove(transform.position, _itemLiftingTime)
                .OnUpdate(() =>
                {

                    if ((item.transform.position - transform.position).sqrMagnitude <= _upItemRange)
                    {
                        item.Active();
                        Destroy(item.gameObject);
                        return;
                    }

                    item.Tween.ChangeEndValue(transform.position, true);
                });
        }

        if (colliders.Length == 0 && _magniteRange >= DefaultMagniteRange)
        {
            _magniteRange = _clearMagniteRange;
            GameInformation.OnInformationChange?.Invoke();
        }
    }
}
