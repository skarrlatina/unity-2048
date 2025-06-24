using UnityEngine;
using System;
public class SwipeInput : MonoBehaviour
{
    public static event Action<Vector2> OnSwipe;

    [SerializeField] private float swipeThreshold = 50f;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            startTouchPosition = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            DetectSwipe();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startTouchPosition = touch.position;

            if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        if (swipeDelta.magnitude < swipeThreshold)
            return;

        Vector2 direction = Vector2.zero;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            direction = swipeDelta.x > 0 ? Vector2.right : Vector2.left;
        else
            direction = swipeDelta.y > 0 ? Vector2.up : Vector2.down;

        OnSwipe?.Invoke(direction);
    }
}
