using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BundleDownloader : MonoBehaviour
{
    string _assetBundleURL = "https://link.com/folder/";
    public static BundleDownloader instance;
    public AssetBundle assetBundle;
    public int index;

    public List<string> LevelList;

    //[SerializeField] WWW CurrentDownload;

    public Text remainderText;
    public Image progressSlider;
    public float requiredValue;

    public Text remainderTextCheck;
    public Image progressSliderCheck;

    public GameObject downloadScreen, downloadCheck, checkCellular;

    string webVersion;

    public GameObject anErrorOccuredTextDownloadScreen, anErrorOccuredTextDownloadCheck;

    void Awake()
    {
        if (instance == null)
            instance = this;

        requiredValue = 100 / LevelList.Count;
        requiredValue /= 100;
    }

    public void CheckDownloadsQueue()
    {
        if (index > LevelList.Count - 1)
        {
            Debug.Log("Download completed");
            EncryptedPlayerPrefs.SetInt("DownloadCompleted", 1);

            if (downloadScreen != null)
                downloadScreen.SetActive(false);

            if (downloadCheck != null)
                downloadCheck.SetActive(false);

            progressSlider.fillAmount = 1;
            remainderText.text = "%100";
            StartCoroutine("SetVersion");
            return;
        }

        //EncryptedPlayerPrefs.SetInt("DownloadCompleted", 0);
        DownloadGame(LevelList[index]);
    }

    IEnumerator CheckVersion()
    {

        Debug.Log("Checking version");

        WWW checkUpdates = new WWW("versionlink.txt");

        yield return checkUpdates;
        Debug.Log(checkUpdates.text);

        if (checkUpdates.error != null) //If there's an error connecting
        {
            Debug.Log("Error trying to get game version from version.txt, user might be offline.");
        }
        else if (checkUpdates.text.Length > 6)
        {
            Debug.Log("Error trying to get game version from version.txt, user might be offline.");
        }
        else
        {
            webVersion = checkUpdates.text;

            if (EncryptedPlayerPrefs.GetString("Version", "1.0") != webVersion)
            {
                Debug.Log("Version differs from the one on the server. Will check for updates.");
                EncryptedPlayerPrefs.SetInt("DownloadCompleted", 0); //Reset download because there's a newer version.
                downloadScreen.SetActive(true); //Force open update window
            }
            else
            {
                Debug.Log("User has the latest version");
            }

        }
    }

    IEnumerator SetVersion()
    {
        WWW checkUpdates = new WWW("versionlink.txt");

        yield return checkUpdates;

        if (checkUpdates.error != null)
        {
            Debug.Log("Error updating game version from version.txt");
            yield return null;
        }
        else if (checkUpdates.text.Length > 6)
        {
            Debug.Log("Error updating game version from version.txt");
        }
        else
        {
            Debug.Log("Version set to : " + checkUpdates.text);
            EncryptedPlayerPrefs.SetString("Version", checkUpdates.text);
        }
    }

    public void DownloadGame(string _levelName)
    {
        StartCoroutine(DownloadAssetBundle(_levelName));
    }
    private IEnumerator ShowProgress(UnityWebRequest www, string _levelName)
    {
        while (!www.isDone)
        {
            Debug.Log("Downloading " + _levelName + " %" + www.downloadProgress * 100);
            yield return new WaitForSecondsRealtime(.25f);
        }
    }

    public void ErrorOccured()
    {
        downloadCheck.SetActive(false);
        downloadScreen.SetActive(false);
    }

    IEnumerator DownloadAssetBundle(string _levelName)
    {
        if (assetBundle != null)
            assetBundle.Unload(false);

        yield return new WaitUntil(() => Caching.ready);

        // get current bundle hash from server, random value added to avoid caching
        UnityWebRequest www = UnityWebRequest.Get(_assetBundleURL + _levelName + ".manifest?r=" + (Random.value * 9999999));
        Debug.Log("Loading manifest:" + _assetBundleURL + _levelName + ".manifest");

        // wait for load to finish
        yield return www.Send();

        // if received error, exit
        if (www.isNetworkError == true)
        {
            Debug.LogError("www error: " + www.error);
            www.Dispose();
            www = null;

            if (downloadScreen.activeSelf)
                anErrorOccuredTextDownloadCheck.SetActive(true);

            if (downloadCheck.activeSelf)
                anErrorOccuredTextDownloadScreen.SetActive(true);

            Invoke("ErrorOccured", 2f);
            yield break;
        }

        // create empty hash string
        Hash128 hashString = (default(Hash128));// new Hash128(0, 0, 0, 0);

        // check if received data contains 'ManifestFileVersion'
        if (www.downloadHandler.text.Contains("ManifestFileVersion"))
        {
            // extract hash string from the received data, TODO should add some error checking here
            var hashRow = www.downloadHandler.text.ToString().Split("\n".ToCharArray())[5];
            hashString = Hash128.Parse(hashRow.Split(':')[1].Trim());

            if (hashString.isValid == true)
            {
                Debug.Log("Checking, " + _assetBundleURL + _levelName + ", " + hashString);

                // we can check if there is cached version or not
                if (Caching.IsVersionCached(_assetBundleURL + _levelName, hashString) == true)
                {
                    Debug.Log("Bundle with this hash is already cached!");

                    index++;

                    progressSlider.fillAmount += requiredValue * 1.5f;
                    progressSliderCheck.fillAmount += requiredValue * 1.5f;
                    remainderText.text = "%" + (progressSlider.fillAmount * 100).ToString("F0");
                    remainderTextCheck.text = "%" + (progressSliderCheck.fillAmount * 100).ToString("F0");

                    CheckDownloadsQueue();
                }
                else
                {
                    Debug.Log("No cached version founded for this hash or there's an update! Deleting local cached file : " +
                        "" + _assetBundleURL + _levelName + " : " + Hash128.Parse(_assetBundleURL + _levelName.GetHashCode()));

                    Caching.ClearAllCachedVersions(_levelName);

                    UnityWebRequest wwwBundle = UnityWebRequestAssetBundle.GetAssetBundle(_assetBundleURL + _levelName + "?r=" + (Random.value * 9999999), hashString, 0);

                    yield return wwwBundle.Send();

                    StartCoroutine(ShowProgress(wwwBundle, _levelName));

                    yield return wwwBundle;

                    if (!string.IsNullOrEmpty(wwwBundle.error))
                    {
                        Debug.Log(wwwBundle.error);

                        Invoke("ErrorOccured", 2f);
                        yield break;
                    }
                    else
                    {
                        if (!wwwBundle.isDone)
                        {
                            assetBundle = DownloadHandlerAssetBundle.GetContent(wwwBundle);
                            assetBundle.Unload(false);
                        }
                        else
                        {
                            Debug.Log("already downloaded");
                        }

                        index++;

                        progressSlider.fillAmount += requiredValue * 1.5f;
                        progressSliderCheck.fillAmount += requiredValue * 1.5f;
                        remainderText.text = "%" + (progressSlider.fillAmount * 100).ToString("F0");
                        remainderTextCheck.text = "%" + (progressSliderCheck.fillAmount * 100).ToString("F0");

                        CheckDownloadsQueue();
                    }
                }
            }
            else
            {
                // invalid loaded hash, just try loading latest bundle
                Debug.LogError("Invalid hash:" + hashString);

                Invoke("ErrorOccured", 2f);
                yield break;
            }

        }
        else
        {
            Debug.LogError("Manifest doesn't contain string 'ManifestFileVersion': " + _assetBundleURL + _levelName + ".manifest");

            Invoke("ErrorOccured", 2f);
            yield break;
        }


    }
}
