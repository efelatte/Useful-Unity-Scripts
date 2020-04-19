using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 2;

    //Camera bounds
    private float Left;
    private float Right;
    private float Top;
    private float Bottom;
    private float MaxZoom;
    private float MinZoom;

    //Sprite details
    private Sprite backgroundSprite;
    private float PixelUnits;
    private Vector2 Size;
    private Vector2 Offset;

    //The background image to use
    Transform backgroundTransform;

    private void Start()
    {
        backgroundTransform = GameObject.FindGameObjectWithTag("Background").transform;
        backgroundSprite = backgroundTransform.transform.GetComponent<SpriteRenderer>().sprite;

        CalculatePixelUnits();
        CalculateSize();
        Refresh();
    }

    private void CalculatePixelUnits()
    {
        PixelUnits = backgroundSprite.rect.width / backgroundSprite.bounds.size.x;
    }

    private void CalculateSize()
    {
        Size = new Vector2(backgroundTransform.transform.localScale.x * backgroundSprite.texture.width / PixelUnits,
        backgroundTransform.transform.localScale.y * backgroundSprite.texture.height / PixelUnits);
        Offset = backgroundTransform.transform.position;
    }

    private void Refresh()
    {
        //calculate current screen ratio
        float w = Screen.width / Size.x;
        float h = Screen.height / Size.y;
        float ratio = w / h;
        float ratio2 = h / w;
        if (ratio2 > ratio)
        {
            MaxZoom = 7f;
        }
        else
        {
            MaxZoom = 7f;
            MaxZoom /= ratio;
        }

        MinZoom = 4;
        Camera.main.orthographicSize = MaxZoom;

        RefreshBounds();
    }

    private void RefreshBounds()
    {
        var vertExtent = Camera.main.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        Left = horzExtent - Size.x / 2.0f + Offset.x;
        Right = Size.x / 2.0f - horzExtent + Offset.x;
        Bottom = vertExtent - Size.y / 2.0f + Offset.y;
        Top = Size.y / 2.0f - vertExtent + Offset.y;
    }

    void Update()
    {

        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {

            pos.y += panSpeed * Time.deltaTime;

        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {

            pos.y -= panSpeed * Time.deltaTime;

        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {

            pos.x += panSpeed * Time.deltaTime;

        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {

            pos.x -= panSpeed * Time.deltaTime;

        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Zoom(scroll * scrollSpeed);

        pos.x = Mathf.Clamp(pos.x, Left, Right);
        pos.y = Mathf.Clamp(pos.y, Bottom, Top);

        transform.position = pos;

    }

    public void Zoom(float value)
    {
        Camera.main.orthographicSize -= value;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, MinZoom, MaxZoom);

        RefreshBounds();

    }
}
