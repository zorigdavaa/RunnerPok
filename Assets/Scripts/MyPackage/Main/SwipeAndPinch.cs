using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public static class SwipeAndPinch
{
    // private static InputAction clickAction;
    // public static InputAction ClickAction
    // {
    //     get {
    //         if (clickAction == null)
    //         {
    //             clickAction = InputSystem.actions.FindAction("Click");
    //         }
    //         return clickAction; }
    // }

    private static Vector2 startPos;
    private static float startTime;
    private static bool isTouching = false;

    private static float lastPinchDistance = 0f;

    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }


    private static float highestY = float.MinValue;
    private static float dragThreshold = 200f;

    public static bool DownDrag()
    {
        var pointer = Pointer.current;


        if (pointer == null)
            return false;
        float pointerY = pointer.position.ReadValue().y;
        if (highestY < pointerY)
        {
            highestY = pointerY;
        }
        if (highestY - pointerY > dragThreshold)
        {
            highestY = pointerY;
            return true;
        }

        return false;

    }
    public static void ResetHighestY()
    {
        highestY = Pointer.current.position.ReadValue().y;
    }

    public static SwipeDirection GetSwipe()
    {
        SwipeDirection direction = SwipeDirection.None;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPos = Mouse.current.position.ReadValue();
            startTime = Time.time;
            isTouching = true;
            Debug.Log("Mouse Pressed at: " + startPos);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isTouching)
        {
            Vector2 endPos = Mouse.current.position.ReadValue();
            float duration = Time.time - startTime;
            isTouching = false;
            direction = DetectSwipe(startPos, endPos, duration);
            Debug.Log($"Mouse Released at: {endPos} | Duration: {duration} | Direction: {direction}");
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Enable();

        if (Touch.activeTouches.Count == 1)
        {
            var finger = Touch.activeTouches[0];

            if (finger.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                startPos = finger.screenPosition;
                startTime = Time.time;
            }

            if (finger.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                Vector2 endPos = finger.screenPosition;
                float duration = Time.time - startTime;
                direction = DetectSwipe(startPos, endPos, duration);
            }
        }
#endif

        return direction;
    }

    private static SwipeDirection DetectSwipe(Vector2 start, Vector2 end, float duration)
    {
        float maxSwipeTime = 0.5f;
        float swipeThreshold = 100f;

        if (duration > maxSwipeTime)
        {
            Debug.Log($"Duration too long {duration}");
            return SwipeDirection.None;
        }

        Vector2 delta = end - start;
        if (delta.magnitude < swipeThreshold)
        {
            Debug.Log($"delta not Reached {delta}");
            return SwipeDirection.None;
        }

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            return delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            return delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
        }
    }

    public static float GetPinchDelta()
    {
        float pinchDelta = 0f;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                pinchDelta = scroll;
            }
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Enable();

        if (Touch.activeTouches.Count == 2)
        {
            var t1 = Touch.activeTouches[0];
            var t2 = Touch.activeTouches[1];

            float currentDistance = Vector2.Distance(t1.screenPosition, t2.screenPosition);

            if (lastPinchDistance > 0f)
            {
                pinchDelta = currentDistance - lastPinchDistance;
            }

            lastPinchDistance = currentDistance;
        }
        else
        {
            lastPinchDistance = 0f;
        }
#endif

        return pinchDelta;
    }
}
