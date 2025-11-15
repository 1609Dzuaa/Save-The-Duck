using DG.Tweening;
using TMPro;
using UnityEngine;

public class PanelTimer : MonoBehaviour
{
    [SerializeField]
    GameObject blueImage;

    [SerializeField]
    TMPro.TextMeshProUGUI blueImageText;

    [SerializeField]
    GameObject redImage;

    [SerializeField]
    TextMeshProUGUI redImageText;

    private TextMeshProUGUI timerText;
    private float duration;
    private float timeRemaining;
    private Tween tween;

    private void Start()
    {
        blueImage.SetActive(true);
        redImage.SetActive(false);

        EventsManager.Subcribe(EventID.OnLevelPassed, KillTween);
        EventsManager.Subcribe(EventID.OnLevelFailed, KillTween);
    }

    private void KillTween(object obj)
    {
        tween.Kill();
    }

    private void SetUp()
    {
        duration = GameData
            .Instance.GetLevelConfig(GameData.Instance.GetCurrentLevelConfig().level)
            .limitedTime;
        timeRemaining = duration;
    }

    public void HandleCountdown()
    {
        SetUp();
        float elapsedTime = 0f;
        Debug.Log("b4 tween");

        tween?.Kill();

        tween = DOTween
            .To(() => timeRemaining, x => timeRemaining = x, 0f, duration)
            .OnUpdate(() =>
            {
                // Update elapsed time with deltaTime
                elapsedTime += Time.deltaTime;
                timeRemaining = Mathf.Max(0, duration - elapsedTime);
                var formattedTime = string.Empty;

                // Update the timer text
                if (timeRemaining >= 60)
                {
                    formattedTime =
                        Mathf.FloorToInt(timeRemaining / 60).ToString("00")
                        + ":"
                        + Mathf.CeilToInt(timeRemaining % 60).ToString("00");
                }
                else
                {
                    formattedTime = Mathf.CeilToInt(timeRemaining).ToString();
                }

                if (timeRemaining / duration > 0.3f)
                {
                    blueImage.SetActive(true);
                    redImage.SetActive(false);
                    blueImageText.text = formattedTime;
                }
                else
                {
                    blueImage.SetActive(false);
                    redImage.SetActive(true);
                    redImageText.text = formattedTime;
                }
            })
            .OnComplete(() =>
            {
                UIManager.Instance.HideView(PanelName.PanelTimer);
                UIManager.Instance.HideView(PanelName.PanelForce);
                EventsManager.Notify(EventID.OnLevelFailed);
            });
    }
}
