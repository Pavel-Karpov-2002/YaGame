using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Vector3 _triggerSize;
    [SerializeField] private Transform _triggerPos;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private Hand _hand;

    public bool IsAttack { get; private set; }

    private Coroutine _attackCoroutine;
    private Collider[] _enemyColldiers;
    private int _minTriggerColliders = 0;

    private void Awake()
    {
        IsAttack = false;
    }

    public void Shoot()
    {
        if (_attackCoroutine != null)
            return;

        _enemyColldiers = Physics.OverlapBox(_triggerPos.position, _triggerSize, Quaternion.identity, _enemyMask);

        if (_enemyColldiers.Length > _minTriggerColliders)
            _attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        IsAttack = true;
        yield return new WaitForSeconds(_hand.Weapon.TimerBulletDelay);

        if (_hand.Weapon is WeaponHands)
            ((WeaponHands)_hand.Weapon).EnemyColliders = _enemyColldiers;

        _hand.Weapon.Attack();
        IsAttack = false;
        _attackCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_triggerPos.position, _triggerSize);
    }
}
