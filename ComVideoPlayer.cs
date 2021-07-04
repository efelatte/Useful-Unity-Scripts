using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ComVideoPlayer : MonoBehaviour
{

    public GameObject video;
    public GameObject reward;
    public GameObject loadingCircle;
    public VideoPlayer vp;
    public Text remainingTime;
    TimeSpan VideoUrlLength;
    string minutes;
    string seconds;
    bool once;

    public GameObject giftBoxOrButton, videoO, bg;

    public float timePassed;
    public GameObject skipVideo;

    public string[] comUrls = new string[] { "link1", "link2" };
    private void Start()
    {
        vp.loopPointReached += EndReached;
    }

    private void OnEnable()
    {
        remainingTime.text = "";
        timePassed = 0;
        skipVideo.SetActive(false);
        vp.url = comUrls[UnityEngine.Random.Range(0, comUrls.Length)];
        StartCoroutine("PlayVideoC");
        Debug.Log("Playing url : " + vp.url);
    }

    private void Update()
    {
        if (vp.isPlaying && !once)
        {
            double time = vp.frameCount / vp.frameRate;
            VideoUrlLength = TimeSpan.FromSeconds(time);
            minutes = (VideoUrlLength.Minutes).ToString("00");
            seconds = (VideoUrlLength.Seconds).ToString("00");
            remainingTime.text = minutes + ":" + seconds;
            InvokeRepeating("SubstractTime", 0, 1);
            once = true;
        }
    }

    private void SubstractTime()
    {
        timePassed++;
        VideoUrlLength = VideoUrlLength.Subtract(new TimeSpan(0, 0, 1));
        minutes = (VideoUrlLength.Minutes).ToString("00");
        seconds = (VideoUrlLength.Seconds).ToString("00");
        remainingTime.text = minutes + ":" + seconds;
    }

    IEnumerator PlayVideoC()
    {
        loadingCircle.SetActive(true);
        vp.Prepare();
        yield return new WaitUntil(() => (vp.isPrepared));
        loadingCircle.SetActive(false);
        vp.Play();
    }

    void EndReached(VideoPlayer vp)
    {
    }
}
