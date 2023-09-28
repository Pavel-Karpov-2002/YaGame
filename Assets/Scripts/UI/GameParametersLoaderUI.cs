using UnityEngine;
using UnityEngine.UI;

public class GameParametersLoaderUI : Singleton<GameParametersLoaderUI>
{
    private const int DefaultMinStack = 0;
    private const int DefaultPointOnStack = 1;

    [SerializeField] private Image _loaderImage;
    
    private int _stackLoader;

    public void Loading(bool isLoad)
    {
        _loaderImage.gameObject.SetActive(isLoad);
    }

    public void AddStack()
    {
        _stackLoader += DefaultPointOnStack;
        Loading(true);
    }

    public void TakeStack()
    {
        if (_stackLoader > DefaultMinStack)
            _stackLoader -= DefaultPointOnStack;

        if (_stackLoader == DefaultMinStack)
            Loading(false);
    }
}
