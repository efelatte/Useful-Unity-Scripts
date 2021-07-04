using UnityEngine;
using UnityEngine.EventSystems;

public class OnDragRotateMenuBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float rotationSpeed;
    public float rotationDamping;

    public float RotationVelocity;
    public bool dragging;

    public static OnDragRotateMenuBar instance;
    private Vector3 theSpeed;
    private Vector3 avgSpeed;

    private void Awake()
    {
        instance = this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!UIManager.instance.rotate)
            dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UIManager.instance.blocker.activeSelf)
            return;

        if (!UIManager.instance.rotate)
        {
            //RotationVelocity = -eventData.delta.x + eventData.delta.y * rotationSpeed;
            //transform.Rotate(Vector3.back, -RotationVelocity, Space.Self);

            theSpeed = new Vector3(-Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            avgSpeed = Vector3.Lerp(avgSpeed, theSpeed, Time.deltaTime * 5);
            transform.Rotate(Vector3.forward, theSpeed.x * 10, Space.Self);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    private void Update()
    {
        if (!dragging && !Mathf.Approximately(RotationVelocity, 0))
        {
            float deltaVelocity = Mathf.Min(Mathf.Sign(RotationVelocity) * Time.deltaTime * rotationDamping, Mathf.Sign(RotationVelocity) * RotationVelocity);
            RotationVelocity -= deltaVelocity;
            transform.Rotate(Vector3.back, -RotationVelocity, Space.Self);
        }
    }
}