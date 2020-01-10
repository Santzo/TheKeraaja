using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThumbStickHandler : MonoBehaviour, IUIHandler
{
    private bool leftPress, rightPress;
    private float top, bottom,  leftSize;
    private float left, right,  rightSize;
    private Transform leftThumb, rightThumb;
    private Camera cam;
    private Vector2 leftThumbStart, leftThumbEnd, rightThumbStart, rightThumbEnd;
    private Vector3 leftCenter, rightCenter;
    public void Awake()
    {
        top = transform.Find("Top").position.y;
        bottom = transform.Find("Bottom").position.y;
        leftThumb = transform.Find("LeftThumbCircle");
        leftCenter = leftThumb.position;
        leftSize = top - bottom;

        RectTransform leftRect = leftThumb.GetComponent<RectTransform>();
        leftThumbStart = new Vector2(leftThumb.transform.position.x - leftRect.sizeDelta.x / 2, leftThumb.transform.position.y - leftRect.sizeDelta.y / 2);
        leftThumbEnd = new Vector2(leftThumb.transform.position.x + leftRect.sizeDelta.x / 2, leftThumb.transform.position.y + leftRect.sizeDelta.y / 2);

        rightThumb = transform.Find("RightThumbCircle");
        left = transform.Find("Left").position.x;
        right = transform.Find("Right").position.x;
        rightCenter = rightThumb.position;
        rightSize = right - left;

        RectTransform rightRect = rightThumb.GetComponent<RectTransform>();
        rightThumbStart = new Vector2(rightThumb.transform.position.x - rightRect.sizeDelta.x / 2, rightThumb.transform.position.y - rightRect.sizeDelta.y / 2);
        rightThumbEnd = new Vector2(rightThumb.transform.position.x + rightRect.sizeDelta.x / 2, rightThumb.transform.position.y + rightRect.sizeDelta.y / 2);

    }
    void Start()
    {
        cam = Camera.main;
    }

    public void Update()
    {
        leftPress = rightPress = false;
        Vector2 force = Vector2.zero;
        int touchCount = Input.touchCount;
        if (touchCount > 0)
        {
            bool leftPressSet = false, rightPressSet = false;
            for (int i = 0; i < touchCount; i++)
            {
                Vector2 pos = Input.GetTouch(i).position;
                if (!leftPressSet)
                    leftPress = pos.x > leftThumbStart.x && pos.x < leftThumbEnd.x && pos.y > leftThumbStart.y && pos.y < leftThumbEnd.y;
                if (!rightPressSet)
                    rightPress = pos.x > rightThumbStart.x && pos.x < rightThumbEnd.x && pos.y > rightThumbStart.y && pos.y < rightThumbEnd.y;

                if (leftPress && !leftPressSet)
                {
                    leftPressSet = true;
                    float touchPos = Mathf.Clamp(pos.y, bottom, top);
                    leftThumb.transform.position = new Vector3(leftThumb.transform.position.x, touchPos, 0f);
                    force.y = Mathf.Abs(bottom - leftThumb.transform.position.y) / leftSize * 2f - 1f;
                }
                if (rightPress && !rightPressSet)
                {
                    rightPressSet = true;
                    float touchPos = Mathf.Clamp(pos.x, left, right);
                    rightThumb.transform.position = new Vector3(touchPos, rightThumb.transform.position.y, 0f);
                    force.x = Mathf.Abs(left - rightThumb.transform.position.x) / rightSize * 2f - 1f;
                    Debug.Log(force.x);
                }
            }
        }
        if (!leftPress) leftThumb.transform.position = leftCenter;
        if (!rightPress) rightThumb.transform.position = rightCenter;
        Events.applyForce(force);
    }
    public void OnDown(Transform trans)
    {
      
    }

    public void OnEnter(Transform trans)
    {
    
    }

    public void OnExit(Transform trans)
    {
      
    }

    public void OnUp(Transform trans)
    {
      
    }
}
