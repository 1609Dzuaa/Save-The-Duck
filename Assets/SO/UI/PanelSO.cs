using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIConfig", menuName = "SO/UI")]
public class PanelSO : ScriptableObject
{
    public List<PanelUI> Panels;
}

[Serializable]
public class PanelUI
{
    public PanelName panelName;
    public GameObject prefab;
}

public enum PanelName
{
    PanelMainMenu,
    PanelOption,
    PanelForce,
    PanelTimer,
    PanelSelectLevel,

    PanelStory,
    PanelLoose,
    PanelWin,
    PanelStoryEnd,
}
