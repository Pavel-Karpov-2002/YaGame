using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveTo : MonoBehaviour, IMove
{
    [SerializeField] private float _trackingUpdateTime = 1.5f;
    [SerializeField] private NavMeshAgent _agent;

    public NavMeshAgent NavMeshAgent => _agent;

    private Coroutine _goToCoroutine;

    private void Start()
    {
        Move();
    }

    public void Move()
    {
        _goToCoroutine = StartCoroutine(GoTo());
    }

    private IEnumerator GoTo()
    {
        if (!_agent.isOnOffMeshLink)
            _agent.SetDestination(PlayerMovement.Instance.transform.position);

        yield return new WaitForSeconds(_trackingUpdateTime);
        _goToCoroutine = StartCoroutine(GoTo());
    }

    public bool IsJump()
    {
        return _agent.isOnOffMeshLink;
    }

    public void StopMove()
    {
        StopCoroutine(_goToCoroutine);
        _agent.ResetPath();
    }

    public void StopScript()
    {
        StopMove();
        this.enabled = false;
    }
}
