using UnityEngine;

public class TrainingPanelScriptUI : MonoBehaviour
{
    [SerializeField] private TextTranslator _textKey;

    private void Start()
    {
        StartGameController.Instance.OnStartGame += ClosePanel;

        if (PlayerPrefs.HasKey(_textKey.key))
            gameObject.SetActive(false);
    }

    public void SaveIsClosedPanel()
    {
        PlayerPrefs.SetInt(_textKey.key, 1);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StartGameController.Instance.OnStartGame -= ClosePanel;
    }
}
