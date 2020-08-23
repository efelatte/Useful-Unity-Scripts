using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMouse : MonoBehaviour
{

    void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5f);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = objPosition;
    }

}
