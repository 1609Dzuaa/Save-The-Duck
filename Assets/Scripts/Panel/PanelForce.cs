using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelForce : MonoBehaviour
{
    [SerializeField]
    Image _imageForce;

    private void Awake()
    {
        EventsManager.Subcribe(EventID.OnSendSliderForce, HandleImage);
        EventsManager.Subcribe(EventID.OnLevelPassed, (o => _imageForce.fillAmount = 0f));
        EventsManager.Subcribe(EventID.OnLevelFailed, (o => _imageForce.fillAmount = 0f));
    }

    private void OnDestroy()
    {
        EventsManager.Unsubcribe(EventID.OnSendSliderForce, HandleImage);
    }

    private void HandleImage(object obj)
    {
        float value = (float)obj;
        _imageForce.fillAmount = value;
    }
}
