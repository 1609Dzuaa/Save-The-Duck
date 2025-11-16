using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    private static GameData _instance;
    public static GameData Instance;

    [SerializeField]
    private LevelSO _configLevel;
    private LevelConfig _currentLevel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public bool SetLevel(int level)
    {
        if (_currentLevel == _configLevel.levelConfigs.Last())
        {
            EventsManager.Notify(EventID.OnGameEnds);
            return false;
        }

        _currentLevel = _configLevel.levelConfigs.FirstOrDefault(p => p.level == level);

        return true;
    }

    public LevelConfig GetCurrentLevelConfig()
    {
        return _currentLevel;
    }

    public LevelConfig GetLevelConfig(int level)
    {
        if (level > _configLevel.levelConfigs.Count)
        {
            Debug.LogError("Try get a Level out of range " + level);
            return null;
        }
        Debug.Log("Get level config for level " + level);
        return _configLevel.levelConfigs.FirstOrDefault(p => p.level == level);
    }
}
