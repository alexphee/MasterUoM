using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
        //Debug.Log(LayerMask.GetMask("Clickable"));
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0))
            if (!EventSystem.current.IsPointerOverGameObject()) //check if mouse is hovering a UI element. The code bellow will execute only if my mouse is NOT over a UI element 
            {
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
                    if (hit.collider != null)
                    { //if not null i hit sth
                        if (hit.collider.tag == "Enemy") ;
                        {
                            player.MyTarget = hit.transform; //αν πετύχω κάτι τότε set τον στόχο MyTarget 
                        }
                    }
                    else
                    { //leave target
                        player.MyTarget = null;
                    }
                }
            }
       
    }
}
