using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Text tooltipText;
    private static UIManager instance;
    public static UIManager MyInstance
    {
        //LOGIC: When i access the UIManager through MyInstance we check if the instance is set and if is not i set it and return it
        //Because of the structure i only have one instance of the game, i always return the same instance
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Button[] actionButtons; //ref to all action buttons
    private KeyCode action1, action2; //keycodes used for executing action buttons


    private Stat healthStat; //health stat for the Unit Frame // already got one on Player and Enemy but i also need on Unit Frame
    [SerializeField]
    private Image thePortraitFrame;
    [SerializeField]
    private GameObject tooltip;
    [SerializeField]
    private Text levelTxt;

    [SerializeField]
    private CanvasGroup[] menus;

    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();//healthstat is what is going to update the healthbar in game, i want to take the children and reference it in the script. I go in targetFrame i look through all children and find bar
        action1 = KeyCode.Alpha1; //numeric key 1 keybind
        action2 = KeyCode.Alpha2; //numeric key 2 keybind
        tooltipText = tooltip.GetComponentInChildren<Text>(); //keeps a ref to the text all the time
    }
    [SerializeField]
    private GameObject targetFrame;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(action1))
        {
            ActionBtnOnClick(0);
        }
        if (Input.GetKeyDown(action2))
        {
            ActionBtnOnClick(1);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(menus[0]);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenClose(menus[1]);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScr.MyInstance.OpenClose();
        }
    }

    private void ActionBtnOnClick(int buttonIndex)
    {
        actionButtons[buttonIndex].onClick.Invoke(); //execute action like i press it with mouse
    }

    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);
        thePortraitFrame.sprite = target.ThePortraitFace;
        levelTxt.text = target.MyLevel.ToString(); //shows level
        target.healthChanged += new HealthChanged(UpdateTargetFrame); //i have an event on my target called healthChanged and i'd like to listen to this event by using the UpdateTargetFrame. So this function listens to target's healthChanged event when its triggerd by taking dmg (in Enemy script)
        target.npcRemoved += new NPCRemoved(HideTargetframe); // when removed also hode the target frame (if i dont do this, i have to deselect it to disappear, or click somewhere else)
        //level text color implementation starts here
        if (target.MyLevel >= Player.MyInstance.MyLevel + 5) //purple -- very hard
        {
            levelTxt.color = Color.magenta;
        }
        else if (target.MyLevel == Player.MyInstance.MyLevel + 3 || target.MyLevel == Player.MyInstance.MyLevel + 4) //red -- hard
        {
            levelTxt.color = Color.red;
        }
        else if (target.MyLevel >= Player.MyInstance.MyLevel - 2 && target.MyLevel <= Player.MyInstance.MyLevel + 2) //yelow - normal
        {
            levelTxt.color = Color.yellow;
        }
        else if (target.MyLevel <= Player.MyInstance.MyLevel - 3 && target.MyLevel > XPManager.CalculateGrayLevel()) //green -- easy
        {
            levelTxt.color = Color.green;
        }
        else //gray -- too easy, no XP
        {
            levelTxt.color = Color.gray;
        }

    }

    public void HideTargetframe()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float hpvalue)
    {
        healthStat.MyCurrentValue = hpvalue;
    }

    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0); //if count is 1 remove text (not icon, just text)
            clickable.MyIcon.color = Color.white; //show again
        }
        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0); //hide icon from it when empty
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void ShowTooltip(Vector3 pos, IText description)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = pos;
        tooltipText.text = description.GetDescription(); //access tooltip, get child with text in it, get ref to the actuall text in textbox and assign it to the description
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }


    //for main menu

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
    public void OpenM(CanvasGroup canvasGroup)
    {
        foreach (CanvasGroup canvas in menus)
        {
            CloseM(canvas); //close all other menus
        }
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
    public void CloseM(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
