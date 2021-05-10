using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    public float sceneWidth = 18.7f;
    Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }
    void Update()
    {

        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        _camera.orthographicSize = desiredHalfHeight;
    }
}
