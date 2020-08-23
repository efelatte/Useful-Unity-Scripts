using UnityEngine;

public class PositionToMiddle : MonoBehaviour
{
    void Start()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane + 1f));
    }
}
