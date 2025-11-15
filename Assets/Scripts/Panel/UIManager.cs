using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PanelSO _config;
    private Dictionary<PanelName, GameObject> dictionary = new();
    private static UIManager _instance;
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        InitUI();
    }

    private void Start()
    {
        EventsManager.Subcribe(EventID.OnGameEnds, (o => ShowView(PanelName.PanelStoryEnd)));
    }

    private void OnDestroy()
    {
        EventsManager.Unsubcribe(EventID.OnGameEnds, (o => ShowView(PanelName.PanelStoryEnd)));
    }

    public bool IsWinPanelActive()
    {
        return dictionary[PanelName.PanelWin].activeSelf;
    }

    public GameObject GetView(PanelName panelName)
    {
        var result = dictionary[panelName];
        return result;
    }

    private void InitUI()
    {
        foreach (var item in _config.Panels)
        {
            var newGameObject = Instantiate(item.prefab, this.transform);
            newGameObject.SetActive(false);
            dictionary.Add(item.panelName, newGameObject);
        }
    }

    public void ShowView(PanelName panelName)
    {
        var result = dictionary[panelName];
        if (result != null)
            result.SetActive(true);
    }

    public void HideView(PanelName panelName)
    {
        var result = dictionary[panelName];
        if (result != null)
            result.SetActive(false);
    }
}
