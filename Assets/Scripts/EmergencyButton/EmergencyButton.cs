using System;
using UnityEngine;

public class EmergencyButton : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Level passed");
        EventsManager.Notify(EventID.OnLevelPassed);
        SoundsManager.Instance.PlaySfx(ESoundName.Win);
    }
}
