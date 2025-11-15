using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerAnim _animation;

    [SerializeField]
    private BubbleGunController _bubbleGunController;

    [SerializeField]
    private PlayerMover _playerMover;

    [SerializeField]
    private float offsetOnPlayer = 0.5f;

    private Vector3 _mousePosition;
    private bool _isInCoroutine = false;
    private bool _isPreparing = false;

    private void Start()
    {
        EventsManager.Subcribe(EventID.OnLevelFailed, ResetState);
        EventsManager.Subcribe(EventID.OnLevelPassed, ResetState);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            _isInCoroutine = false;
            return;
        }

        if (!_bubbleGunController.HasSpawn)
        {
            HandleNoBubbleState();
        }
        else
        {
            HandleBubbleSpawnedState();
        }
    }

    private void HandleNoBubbleState()
    {
        if (Input.GetMouseButton(0) && !_isInCoroutine)
        {
            StartCoroutine(HandleTouchHold());
            return;
        }

        if (!IsMouseOverPlayerX() && !_isPreparing)
        {
            _animation.SetAnim(AnimationName.Move);
            _playerMover.Move(_mousePosition);
        }
        else if (!_isPreparing)
        {
            _animation.SetAnim(AnimationName.Idle);
        }
    }

    private void HandleBubbleSpawnedState()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleRaycast();
        }

        if (!IsMouseOverPlayerX())
        {
            _animation.SetAnim(AnimationName.Move);
            _playerMover.Move(_mousePosition);
        }
        else
        {
            _animation.SetAnim(AnimationName.Idle);
        }
    }

    private void HandleRaycast()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hits = new List<RaycastHit2D>();
        hits = Physics2D
            .RaycastAll(mousePosition, Vector2.zero)
            .Where(hit => hit.collider.GetComponent<IClickable>() is not null)
            .ToList();
        if (hits.Count == 0)
            return;

        foreach (var clickable in hits.Select(hit => hit.collider.GetComponent<IClickable>()))
        {
            clickable.HandleClick();
        }
    }

    private bool IsMouseOverPlayerX()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _mousePosition.x > transform.position.x - offsetOnPlayer
            && _mousePosition.x < transform.position.x + offsetOnPlayer;
    }

    private IEnumerator HandleTouchHold()
    {
        _isInCoroutine = true;
        yield return new WaitForSeconds(1f);
        if (Input.GetMouseButton(0))
        {
            _playerMover.StopMove();
            _isPreparing = true;
            _playerMover.StopMove();
            _animation.SetAnim(AnimationName.Blow);
        }

        _isInCoroutine = false;
    }

    private void ResetState(object obj)
    {
        _isInCoroutine = false;
        _isPreparing = false;
    }
}
