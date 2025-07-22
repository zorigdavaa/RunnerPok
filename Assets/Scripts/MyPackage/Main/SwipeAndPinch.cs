using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SwipeAndPinch : MonoBehaviour
{
    private Vector2 startPos;
    private float startTime;
    private bool isTouching = false;

    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;
    public float maxSwipeTime = 0.5f;

    [Header("Pinch Settings")]
    public float pinchSensitivity = 5f;
    private float lastPinchDistance = 0f;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
    }

    void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
#if UNITY_EDITOR
        HandleMouseSwipe();
        HandleMousePinch();
#endif

#if UNITY_ANDROID || UNITY_IOS
        HandleTouchPinch();
#endif
    }

    // ---------------- Touch: Swipe ----------------
    void OnFingerDown(Finger finger)
    {
        if (Touch.activeTouches.Count == 1)
        {
            startPos = finger.screenPosition;
            startTime = Time.time;
        }
    }

    void OnFingerUp(Finger finger)
    {
        if (Touch.activeTouches.Count == 1)
        {
            Vector2 endPos = finger.screenPosition;
            float duration = Time.time - startTime;
            DetectSwipe(startPos, endPos, duration);
        }
    }

    void DetectSwipe(Vector2 start, Vector2 end, float duration)
    {
        if (duration > maxSwipeTime) return;

        Vector2 delta = end - start;
        if (delta.magnitude < swipeThreshold) return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0) OnSwipeRight();
            else OnSwipeLeft();
        }
        else
        {
            if (delta.y > 0) OnSwipeUp();
            else OnSwipeDown();
        }
    }

    // ---------------- Editor: Swipe ----------------
    void HandleMouseSwipe()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPos = Mouse.current.position.ReadValue();
            startTime = Time.time;
            isTouching = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isTouching)
        {
            Vector2 endPos = Mouse.current.position.ReadValue();
            float duration = Time.time - startTime;
            isTouching = false;
            DetectSwipe(startPos, endPos, duration);
        }
    }

    // ---------------- Mobile: Pinch ----------------
    void HandleTouchPinch()
    {
        if (Touch.activeTouches.Count == 2)
        {
            var t1 = Touch.activeTouches[0];
            var t2 = Touch.activeTouches[1];

            float currentDistance = Vector2.Distance(t1.screenPosition, t2.screenPosition);

            if (lastPinchDistance != 0f)
            {
                float delta = currentDistance - lastPinchDistance;

                if (Mathf.Abs(delta) > pinchSensitivity)
                {
                    if (delta > 0)
                        OnPinchOut();
                    else
                        OnPinchIn();
                }
            }

            lastPinchDistance = currentDistance;
        }
        else
        {
            lastPinchDistance = 0f;
        }
    }

    // ---------------- Editor: Pinch (Mouse Wheel) ----------------
    void HandleMousePinch()
    {
        float scrollDelta = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scrollDelta) > 0.1f)
        {
            if (scrollDelta > 0)
                OnPinchOut();
            else
                OnPinchIn();
        }
    }

    // ---------------- Callbacks ----------------
    void OnSwipeUp() => Debug.Log("Swipe Up");
    void OnSwipeDown() => Debug.Log("Swipe Down");
    void OnSwipeLeft() => Debug.Log("Swipe Left");
    void OnSwipeRight() => Debug.Log("Swipe Right");
    void OnPinchIn() => Debug.Log("Pinch In (Zoom Out)");
    void OnPinchOut() => Debug.Log("Pinch Out (Zoom In)");
}
