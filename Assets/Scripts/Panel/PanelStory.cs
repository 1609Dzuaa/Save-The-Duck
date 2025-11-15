using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelStory : MonoBehaviour
{
    [SerializeField]
    private float moveDuration;

    [SerializeField]
    private float fadeDuration;

    [SerializeField]
    private Image story1;

    [SerializeField]
    private Image story2;

    [SerializeField]
    private Image story2Faucet;

    [SerializeField]
    private Image story3;

    private bool _isFirstTime = true;

    private void OnEnable()
    {
        if (_isFirstTime)
        {
            _isFirstTime = false;
            return;
        }

        Debug.Log("start story 1 " + Time.time);
        DoAnimStory1(() =>
        {
            DoAnimStory2(() =>
            {
                DoAnimStory3(() =>
                {
                    SceneManager.LoadScene("GamePlayScene");
                    //UIManager.Instance.ShowView(PanelName.PanelForce);
                });
            });
        });
    }

    private void DoAnimStory1(Action callBack = null)
    {
        story1
            .transform.DOMove(Vector3.zero, moveDuration)
            .OnComplete(() =>
            {
                Debug.Log("done story 1 " + Time.time);
                DOVirtual.DelayedCall(
                    1f,
                    () =>
                    {
                        story1
                            .DOColor(new Color32(50, 50, 50, 255), fadeDuration)
                            .OnComplete(() => callBack?.Invoke());
                    }
                );
            });
    }

    private void DoAnimStory2(Action callBack = null)
    {
        Debug.Log("start story 2 " + Time.time);
        story2
            .transform.DOMove(Vector3.zero, moveDuration)
            .OnComplete(() =>
            {
                Debug.Log("done story 2 " + Time.time);
                story2Faucet
                    .transform.DOScale(Vector3.one, 1)
                    .OnComplete(() =>
                    {
                        DOVirtual.DelayedCall(
                            1f,
                            () =>
                            {
                                story2Faucet.DOColor(new Color32(50, 50, 50, 255), fadeDuration);
                                story2
                                    .DOColor(new Color32(50, 50, 50, 255), fadeDuration)
                                    .OnComplete(() => callBack?.Invoke());
                            }
                        );
                    });
            });
    }

    private void DoAnimStory3(Action callBack = null)
    {
        Debug.Log("start story 3 " + Time.time);

        story3
            .transform.DOMove(Vector3.zero, moveDuration)
            .OnComplete(() =>
            {
                Debug.Log("done story 3 " + Time.time);
                DOVirtual.DelayedCall(3f, () => callBack?.Invoke());
            });
    }
}
