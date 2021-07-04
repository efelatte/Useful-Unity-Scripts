using UnityEngine;

public class SmoothDrag : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    float startPosX, startPosY, startPosZ;

    public float speed = 10;
    bool dragging;

    private void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        startPosZ = transform.position.z;
    }

    void OnMouseDrag()
    {
        dragging = true;
    }

    private void FixedUpdate()
    {
        if (!dragging)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(startPosX, startPosY, startPosZ), speed * Time.deltaTime);
        }
    }

    private void Update()
    {
        if (dragging)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = Vector2.Lerp(transform.position, curPosition, Time.deltaTime * 10);
        }
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}

