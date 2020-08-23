using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flickering2DLight : MonoBehaviour
{

    [SerializeField] bool FlorasanFlicker, PortalFlicker, OnAndOff;

    Light2D light;

    int NextUpdate;

    AudioSource audio;

    void Start()
    {

        audio = GetComponent<AudioSource>();
        light = GetComponent<Light2D>();

    }

    void Update()
    {
        if (PortalFlicker)
        {
            light.intensity = Mathf.Lerp(light.intensity, Random.Range(3f, 5f), 1f);
            light.shadowIntensity = light.intensity;
        }

        if (FlorasanFlicker)
        {
            light.intensity = Mathf.Lerp(light.intensity, Random.Range(0.4f, 1f), 0.1f);
            light.shadowIntensity = light.intensity;
        }

        if (Time.time >= NextUpdate && onAndOff)
        {
            NextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEverySecond();
        }
    }


    void UpdateEverySecond()
    {

        if (Random.Range(1,100) < 40)
        {

            return;

        }

        if (light.intensity == 0)
        {
            audio.Play();
            light.intensity = Random.Range(0.8f, 1f);
        } else
        {
            audio.Stop();
            light.intensity = 0;
        }
    }
}
