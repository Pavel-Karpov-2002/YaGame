using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private float _speed = 1;

    private bool _isRotationEnable;

    private void Start()
    {
        StartGameController.Instance.OnStartGame += EnableRotate;
        LevelProgress.Instance.OnCompletedLevel += DisableRotate;
        DisableRotate();
    }

    private void Update()
    {
        if (_isRotationEnable)
            RotateWheel();
    }

    public void RotateWheel()
    {
        transform.RotateAround(transform.position, Vector3.left, _speed);
    }

    public void EnableRotate()
    {
        _isRotationEnable = true;
    }

    public void DisableRotate()
    {
        _isRotationEnable = false;
    }

    private void OnDestroy()
    {
        StartGameController.Instance.OnStartGame -= EnableRotate;
        LevelProgress.Instance.OnCompletedLevel -= DisableRotate;
    }
}
