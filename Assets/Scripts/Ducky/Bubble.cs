using System;
using UnityEngine;

public class Bubble : MonoBehaviour, IClickable
{
    [SerializeField]
    private float scaleForce = 1f;

    [SerializeField]
    private float smallScalePop = 0.03f;

    [SerializeField]
    private float forceMagnitude = 4f;

    [SerializeField]
    private float maxLifeTime = 40f;

    [SerializeField]
    private float coolDown = 0.5f;

    [SerializeField]
    private Bubble bubblePrefab;

    private Rigidbody2D _rigidbody2D;
    private CircleBorder _circleBorder;

    private float _elapsedTime = 0f;
    private readonly int _popAnimationHash = Animator.StringToHash("pop");

    public bool IsRealeased { get; set; } = false;
    private bool _isPopped = false;
    private bool _isInitialized = false;
    private float _timeSinceLastClick = 0f;// Mathf.Infinity;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleBorder = GetComponent<CircleBorder>();
    }

    private void OnMouseDown()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleClick();
#endif
    }

    private void OnEnable()
    {
        EventsManager.Subcribe(EventID.OnLevelFailed, PopTheBubble);
        EventsManager.Subcribe(EventID.OnLevelPassed, PopTheBubble);
    }

    private void OnDisable()
    {
        EventsManager.Unsubcribe(EventID.OnLevelFailed, PopTheBubble);
        EventsManager.Unsubcribe(EventID.OnLevelPassed, PopTheBubble);
    }

    private void PopTheBubble(object obj) => PopBubble(this);

    private void Update()
    {
        _timeSinceLastClick += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!IsRealeased || _isPopped)
        {
            PreventMoving();
            return;
        }
        else if (!_isInitialized)
        {
            Initialize();
        }

        var upForce = new Vector2(0, (1 * scaleForce * Time.fixedDeltaTime));

        if (_rigidbody2D.velocity.y <= 1.5f)
            _rigidbody2D.AddForce(upForce, ForceMode2D.Impulse);

        _elapsedTime += Time.fixedDeltaTime;

        if (_elapsedTime >= maxLifeTime)
        {
            PopBubble(this);
        }
    }

    private void PreventMoving()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        _rigidbody2D.gravityScale = 0f;
    }

    private void Initialize()
    {
        _rigidbody2D.gravityScale = 1f;
        _rigidbody2D.mass = Mathf.Clamp(transform.localScale.x, 0.5f, 1.5f);

        maxLifeTime *= transform.localScale.x;
        _isInitialized = true;
    }

    public void SeparateBubble()
    {
        if (_isPopped)
            return;

        if (_timeSinceLastClick < coolDown)
            return;

        _timeSinceLastClick = 0f;

        // Check if the bubble is too small to separate
        var newScale = transform.localScale / 2;
        if (newScale.x < smallScalePop)
        {
            PopBubble(this);
            return;
        }

        // Get positions for the two bubbles
        var bubble1Pos = _circleBorder.GetPositionOnCircle(0) * 0.8f; // Slightly inward
        var bubble2Pos = _circleBorder.GetPositionOnCircle(180) * 0.8f; // Slightly inward

        // Instantiate new bubbles
        var bubble1 = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
        var bubble2 = Instantiate(bubblePrefab, transform.position, Quaternion.identity);

        // Release the new bubbles
        bubble1.IsRealeased = true;
        bubble2.IsRealeased = true;

        // Scale the new bubbles
        bubble1.transform.localScale = newScale;
        bubble2.transform.localScale = newScale;

        // Add a small separation force
        var direction1 = (bubble1Pos - transform.position).normalized;
        var direction2 = (bubble2Pos - transform.position).normalized;

        bubble1
            .GetComponent<Rigidbody2D>()
            .AddForce(direction1 * forceMagnitude, ForceMode2D.Impulse);
        bubble2
            .GetComponent<Rigidbody2D>()
            .AddForce(direction2 * forceMagnitude, ForceMode2D.Impulse);

        PopBubble(this);
    }

    public void PopBubble(Bubble bubble)
    {
        if (_isPopped)
            return;

        bubble.GetComponentInChildren<Animator>().SetTrigger(_popAnimationHash);
        bubble.GetComponent<Collider2D>().enabled = false;
        SoundsManager.Instance.PlaySfx(ESoundName.BubblePop);
        _isPopped = true;

        Destroy(bubble.gameObject, 2f);
    }

    public void HandleClick()
    {
        SeparateBubble();
    }
}
