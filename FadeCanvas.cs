using UnityEngine;

public class FadeCanvas : MonoBehaviour
{

    CanvasGroup canvasGroup;

    float lerpTime = 0.2f;
    float currentLerpTime;
    public bool Opening;

    private void Start()
    {

        canvasGroup = GetComponent<CanvasGroup>();

    }

    public void Update()
    {

        if (Opening)
        {
            //increment timer once per frame
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp!
            float perc = currentLerpTime / lerpTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, perc);

            if (canvasGroup.alpha == 1)
            {
                Opening = false;
            }

        }
    }
}
