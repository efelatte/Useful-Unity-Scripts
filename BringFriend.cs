using UnityEngine;

public class BringYourFriend : MonoBehaviour
{

    [SerializeField] GameObject friend;


    private void OnEnable()
    {

        friend.setActive(true);

    }

    private void OnDisable()
    {

        friendBar.SetActive(false);

    }


}
