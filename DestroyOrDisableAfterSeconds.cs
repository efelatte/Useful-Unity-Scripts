using System.Collections;
using UnityEngine;

public class DestroyOrDisableAfterSeconds : MonoBehaviour
{

    enum Action
    {

        Destroy,
        Disable

    }

    [SerializeField] Action currentAction;
    [Space(10)]
    [SerializeField] bool InstantiateBeforeDeactivation;
    [SerializeField] GameObject ObjectToInstantiate;
    [SerializeField] float ExtraSecondsToWaitAfterInstantiate;
    [Space(10)]
    [SerializeField] float SecondsToWait;


    void Start()
    {
        
        StartCoroutine("Begin");

    }

    IEnumerator Begin()
    {
        yield return new WaitForSecondsRealtime(SecondsToWait);

        if (InstantiateBeforeDeactivation)
            Instantiate(ObjectToInstantiate, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

        if (ExtraSecondsToWaitAfterInstantiate > 0.1f)
            yield return new WaitForSeconds(ExtraSecondsToWaitAfterInstantiate);

        if (currentAction == Action.Destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

}
