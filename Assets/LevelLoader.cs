using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _background;

    private void Awake()
    {
        var _currentLevel = GameData.Instance.GetCurrentLevelConfig();
        if (_currentLevel == null)
        {
            Debug.LogError("Null level data");
            return;
        }
        LoadLevel(_currentLevel);
        // set background
    }

    public void LoadLevel(LevelConfig config)
    {
        // clear old level
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        // load new level
        GameObject.Instantiate(config.tileMap, Vector3.zero, Quaternion.identity, transform);

        _background.sprite = config.background;
        UIManager.Instance.ShowView(PanelName.PanelTimer);
        UIManager.Instance.GetView(PanelName.PanelTimer).GetComponent<PanelTimer>().HandleCountdown();

        SoundsManager.Instance.PlaySfx(ESoundName.StartWhistle);
        SoundsManager.Instance.PlayMusic(ESoundName.PipeDrain);
    }
}
