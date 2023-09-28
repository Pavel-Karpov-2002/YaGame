using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class LookAtController : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private float _directionTime;
    [SerializeField] private float _minTriggerDistance;
    [SerializeField] private Vector3 _offset;

    private GameObject _triggerObject;
    private Tween _tween;
    private Vector3 _lookAt;

    public Tween Tween => _tween;
    public event Action OnLook;

    private void Awake()
    {
        _lookAt = Vector3.zero;
        _tween.SetAutoKill(false);
        _sphereCollider.radius = _minTriggerDistance;
        _sphereCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!this.enabled)
            return;

        if ((1 << other.gameObject.layer) != _layerMask.value)
            return;

        if (_lookAt == Vector3.zero)
            _lookAt = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);

        if (_triggerObject != null && _tween != null && _tween.active)
        {
            _lookAt = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(_lookAt - transform.position);
        _tween = transform.DORotateQuaternion(targetRotation * Quaternion.Euler(_offset), _directionTime);
        _triggerObject = other.gameObject;
        OnLook?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (_triggerObject != null && _triggerObject.Equals(other.gameObject))
            _triggerObject = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _minTriggerDistance);
    }

    private void OnDisable()
    {
        _tween.Kill();
    }

    private void OnDestroy()
    {
        _tween.Kill();
    }
}
