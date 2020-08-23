using UnityEngine;

public class BringYourFriend : MonoBehaviour
{

    [SerializeField] GameObject friend;


    private void OnEnable()
    {

        friend.SetActive(true);

    }

    private void OnDisable()
    {

        friend.SetActive(false);

    }


}
