using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExpScroller : MonoBehaviour
{
    [Tooltip("Spacing between experiences")]
    public float spacing = 1f;  // Spacing between experiences
    [Tooltip("Speed of scrolling")]
    public float smoothTime = 1f;  // Speed of scrolling
    [Tooltip("Index of the experience to display by default")]
    public int defaultIndex = 0;

    private List<GameObject> scrollerItems;  // List of the different scoller items

    private Vector3 targetPosition;  // Target position for scrolling
    private int currentIndex = 0;  // Index of the currently displayed experience
    private Vector3 velocity;  // Current velocity (used by SmoothDamp)

    private void OnValidate()
    {
        if (scrollerItems != null)
        {
            Start();
            currentIndex = defaultIndex;
            Scroll(0);
            transform.localPosition = targetPosition;
        }
    }

    private void Start()
    {
        currentIndex = defaultIndex;

        // get all children of the scroller
        scrollerItems = new List<GameObject>();
        foreach (Transform child in transform) if (child.gameObject.activeSelf) scrollerItems.Add(child.gameObject); // scrolleritems must be active at start

        // Initialize experiences' positions
        for (int i = 0; i < scrollerItems.Count; i++)
        {
            scrollerItems[i].transform.localPosition = new Vector3(i * spacing, 0, 0);
        }

        // Initialize scroller's target position
        targetPosition = transform.localPosition;
    }

    private void Update()
    {
        // Scroll left or right based on arrow key input
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Scroll(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Scroll(1);
        }

        // Smoothly move scroller to target position
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, smoothTime);
    }

    private void Scroll(int direction)
    {
        // Update current index and ensure it wraps around within the list's bounds
        currentIndex += direction;
        currentIndex = (currentIndex + scrollerItems.Count) % scrollerItems.Count;
        //Debug.Log(currentIndex);

        // Update target position based on current index and spacing
        targetPosition = new Vector3(-currentIndex * spacing, 0, 0);
        //Debug.Log(targetPosition);
    }

}
