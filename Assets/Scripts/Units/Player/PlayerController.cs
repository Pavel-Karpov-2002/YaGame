using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Unit
{
    [SerializeField] private float _timeBeforLiftingItems;
    [SerializeField] private PlayerParameters _playerParameters;
    [SerializeField] private PickUpItemController _pickUpItem;
    [SerializeField] private LookAtController _lookAtController;
    [SerializeField] private AttackController _attack;
    [SerializeField] private int _deathLayer;

    private RagdollController _ragdollController;
    private Skin _skin;
    private float _startMagnitudeRange;

    protected PlayerParameters PlayerParameters => _playerParameters;

    protected LookAtController LookAtController => _lookAtController;
    protected AttackController Attack => _attack;

    protected Skin Skin
    {
        get
        {
            if (_skin == null)
                _skin = GetComponentInChildren<Skin>();

            return _skin;
        }
    }
        
    protected override void Awake()
    {
        base.Awake();
        _startMagnitudeRange = _pickUpItem.MagniteRange;
        SetStaticSpeed(_playerParameters.MinSpeed);
        SetStaticHealth(_playerParameters.MinHealth);
        _skin = GetComponentInChildren<Skin>();
        LevelProgress.Instance.OnCompletedLevel += ChangeStaticHealth;
        LevelProgress.Instance.OnCompletedLevel += PickUpItems;
        StartGameController.Instance.OnStartGame += SetStandartMagniteRange;
        _lookAtController.OnLook += _attack.Shoot;
    }

    public override void Die()
    {
        if (Health > MinHealth)
            return;

        if (_ragdollController = Skin.RagdollController)
        {
            _ragdollController.EnablePhysics();
            SetDeathLayer();
        }

        _lookAtController.Tween.Kill();
        _lookAtController.enabled = false;
        StartCoroutine(RestartScene());
        OnDeath?.Invoke();
        OnDeath = null;
        this.enabled = false;
    }

    private void SetDeathLayer()
    {
        gameObject.layer = _deathLayer;

        foreach (var rb in _ragdollController.RagdollElements)
        {
            rb.gameObject.layer = _deathLayer;
        }
    }


    private void SetStandartMagniteRange()
    {
        _pickUpItem.MagniteRange = _startMagnitudeRange;
    }

    private void ChangeStaticHealth()
    {
        SetStaticHealth(StaticHealthParameter);
    }

    private void PickUpItems()
    {
        StartCoroutine(PickUpAllItems());
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(_playerParameters.TimeBeforRestartScene);
        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator PickUpAllItems()
    {
        yield return new WaitForSeconds(_timeBeforLiftingItems);
        _pickUpItem.MagniteRange = PickUpItemController.DefaultMagniteRange;
    }

    protected override void OnDestroy()
    {
        LevelProgress.Instance.OnCompletedLevel -= ChangeStaticHealth;
        LevelProgress.Instance.OnCompletedLevel -= PickUpItems;
        StartGameController.Instance.OnStartGame -= SetStandartMagniteRange;
        _lookAtController.OnLook -= _attack.Shoot;
        base.OnDestroy();
    }
}
