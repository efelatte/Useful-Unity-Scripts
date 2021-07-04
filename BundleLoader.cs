using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BundleLoader : MonoBehaviour
{
    string _assetBundleURL = "URLHERE";
    public static BundleLoader instance;
    [SerializeField] private AssetBundle assetBundle;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGame(string _levelName)
    {
        StartCoroutine(LoadAssetBundle(_levelName));
    }
    IEnumerator LoadAssetBundle(string _levelName)
    {
        if (assetBundle != null)
        {
            assetBundle.Unload(false);
        }

        yield return new WaitUntil(() => Caching.ready);

        UnityWebRequest www = UnityWebRequest.Get(_assetBundleURL + _levelName + ".manifest?r=" + (Random.value * 9999999));

        yield return www.Send();

        Hash128 hashString = (default(Hash128));// new Hash128(0, 0, 0, 0);

        var hashRow = www.downloadHandler.text.ToString().Split("\n".ToCharArray())[5];
        hashString = Hash128.Parse(hashRow.Split(':')[1].Trim());

        UnityWebRequest wwwBundle = UnityWebRequestAssetBundle.GetAssetBundle(_assetBundleURL + _levelName + "?r=" + (Random.value * 9999999), hashString, 0);

        // wait for load to finish
        yield return wwwBundle.Send();

        if (!string.IsNullOrEmpty(wwwBundle.error))
        {
            Debug.Log(wwwBundle.error);
        }
        else
        {

            assetBundle = DownloadHandlerAssetBundle.GetContent(wwwBundle);
            Debug.Log("loaded asset bundle " + assetBundle + " " + assetBundle.name);

            if (assetBundle.isStreamedSceneAssetBundle)
            {
                string[] scenePaths = assetBundle.GetAllScenePaths();
                string _sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePaths[0]);
                Debug.Log("loading scene");
                SceneManager.LoadScene(_sceneName);
            }
            //Debug.Log(_levelName);
        }
    }
}
