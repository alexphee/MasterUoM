using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirm(Character character);
public class GameManager : MonoBehaviour
{
    private Camera mainCamera;
    public event KillConfirm killConfirmEvent; //will trigger anytime i kill sth (anything)

    private static GameManager instance;
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    [SerializeField]
    private Player player;
    private Enemy currentTarget;
    private int targetIndex;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
        NextTarget();
        //Debug.Log(LayerMask.GetMask("Clickable"));
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) //check if mouse is hovering a UI element. The code bellow will execute only if my mouse is NOT over a UI element 
            {
                RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
                if (hit.collider != null) //Logic: IF i click sth, i check IF i already have target and if yes i deselect current and pick new
                {
                    /*if (currentTarget != null) //check if currentTarget is not null, if i already have a target i need to deselect this target. eg i have 2 enemies and i select no1, if i want to select no2 i first need to deselect no1
                    {
                        currentTarget.Deselect(); //deselect current target
                    }*/
                    DeselectTarget();
                    SelectTarget(hit.collider.GetComponent<Enemy>());
                    /*currentTarget = hit.collider.GetComponent<Enemy>(); //select new target

                    player.MyTarget = currentTarget.Select(); //i can do this because the actual select function in NPC script returns a Transform, so it returns hitBox. So i can set it equal to hitBox and then it throws this to MyTarget in GameManager (where i call it)


                    UIManager.MyInstance.ShowTargetFrame(currentTarget);*/

                }
                else //if i dont click sth
                {
                    UIManager.MyInstance.HideTargetframe(); //if i deselect, also hide target frame
                    /*if (currentTarget != null)
                    {
                        currentTarget.Deselect();
                    }*/
                    DeselectTarget();

                    currentTarget = null;//remove refs to target
                    player.MyTarget = null;
                }

            }

        }
        else if (Input.GetMouseButtonDown(1)) //right click on enemy to loot
        {
            if (!EventSystem.current.IsPointerOverGameObject()) //same as above, dont hover UI
            {
                RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
                if (hit.collider != null && (hit.collider.CompareTag("thyEnemy") || hit.collider.CompareTag("Interactable")))
                {
                    //hit.collider.GetComponent<NPC>().Interact();
                    player.Interact();
                }
            }

        }
    }

    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeselectTarget();
            if (Player.MyInstance.MyAttackers.Count > 0) //if there are attackers
            {
                if (targetIndex < Player.MyInstance.MyAttackers.Count)
                {
                    SelectTarget(Player.MyInstance.MyAttackers[targetIndex]);
                    targetIndex++;
                    if (targetIndex >= Player.MyInstance.MyAttackers.Count)
                    {
                        targetIndex = 0;
                    }
                }
                else
                {
                    targetIndex = 0; //this is for a bug fix with attacks
                }
                
            }
        }
    }
    private void SelectTarget(Enemy enemy)
    {
        currentTarget = enemy;
        player.MyTarget = currentTarget.Select();
        UIManager.MyInstance.ShowTargetFrame(currentTarget);
    }
    private void DeselectTarget()
    {
        if (currentTarget!=null)
        {
            currentTarget.Deselect();
        }
    }
    public void OnKillConfirmed(Character character)
    {
        if (killConfirmEvent != null)
        {
            killConfirmEvent(character);
        }
    }
}


/*{ //if not null i hit sth
                        if (hit.collider.CompareTag("thyEnemy"))
                        {
                            player.MyTarget = hit.transform.GetChild(0); //áí ðåôý÷ù êÜôé ôüôå set ôïí óôü÷ï MyTarget 
                        }
                    }
                    else
                    { //leave target
                        player.MyTarget = null;
                    }*/