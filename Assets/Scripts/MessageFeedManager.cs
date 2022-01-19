using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageFeedManager : MonoBehaviour
{
    [SerializeField]
    private GameObject messagePrefab;
    //SINGLETON
    public static MessageFeedManager instance;
    public static MessageFeedManager MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = GameManager.FindObjectOfType<MessageFeedManager>();
            }
            return instance;
        }
    }
   
    public void WriteMessage(string msg)
    {
        GameObject go = Instantiate(messagePrefab, transform);
        go.GetComponent<Text>().text = msg;
        go.transform.SetAsFirstSibling(); //WOW! -- this reverses the way the mesasges appear, so newest update always appear on top 
        Destroy(go, 3); //remove text after 2sec
    }
}
