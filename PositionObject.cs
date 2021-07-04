using UnityEngine;

[ExecuteInEditMode]
public class PositionObject : MonoBehaviour
{

    public float posX, posY, posZ;

    void Update()
    {
        if (Camera.main != null)
            transform.position = Camera.main.ViewportToWorldPoint(new Vector3(posX, posY, posZ));
    }
}
