using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Enemy
{
    private const float DeafultAnimationSpeed = 1;
    private const float StopAnimationSpeed = 0;

    [SerializeField] private AIMoveTo _aIMoveTo;

    public NavMeshAgent NavMeshAgent => _aIMoveTo.NavMeshAgent;
    private bool isJump;
    private bool isAttack;

    protected override void Awake()
    {
        base.Awake();
        OnDeath += _aIMoveTo.StopScript;
    }

    private void Start()
    {
        Skin.Animator.SetFloat("Speed", NavMeshAgent.speed);
    }

    private void FixedUpdate()
    {
        if (_aIMoveTo.IsJump() != isJump)
        {
            Skin.Animator.speed = DeafultAnimationSpeed;
            Skin.Animator.SetBool("Jump", _aIMoveTo.IsJump());
            isJump = _aIMoveTo.IsJump();
            return;
        }

        if (Attack.IsAttack)
        {
            Skin.Animator.speed = DeafultAnimationSpeed;
            _aIMoveTo.StopMove();
            Skin.Animator.SetFloat("Speed", StopAnimationSpeed);
            isAttack = true;
            return;
        }

        if (isAttack)
        {
            _aIMoveTo.Move();
            isAttack = false;
            return;
        }

        if (Skin.Animator.speed != NavMeshAgent.speed)
        {
            Skin.Animator.speed = NavMeshAgent.speed;
            Skin.Animator.SetFloat("Speed", NavMeshAgent.speed);
        }
    }


    protected override void OnDestroy()
    {
        OnDeath -= _aIMoveTo.StopScript;
        base.OnDestroy();
    }
}
