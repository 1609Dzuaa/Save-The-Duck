using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelMainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _btnStart;

    [SerializeField]
    private Button _btnOption;

    [SerializeField]
    private Button _btnExit;

    private void OnEnable()
    {
        _btnStart.onClick.AddListener(OnClickedStart);
        _btnOption.onClick.AddListener(OnClickedOption);
        _btnExit.onClick.AddListener(OnClickedExit);
    }

    private void OnDisable()
    {
        _btnStart.onClick.RemoveAllListeners();
        _btnOption.onClick.RemoveAllListeners();
        _btnExit.onClick.RemoveAllListeners();
    }

    private void OnClickedStart()
    {
        GameData.Instance.SetLevel(1); //Jusg to test, set level in select level
        UIManager.Instance.ShowView(PanelName.PanelStory);
    }

    private void OnClickedOption() { }

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void ReloadPage();
#endif

    public void OnClickedExit()
    {
#if UNITY_WEBGL
        ReloadPage();
#else
        Application.Quit();
#endif
    }
}
