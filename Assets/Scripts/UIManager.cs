using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();//healthstat is what is going to update the healthbar in game, i want to take the children and reference it in the script. I go in targetFrame i look through all children and find bar
        action1 = KeyCode.Alpha1; //numeric key 1 keybind
        action2 = KeyCode.Alpha2; //numeric key 2 keybind
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
    }

    private void ActionBtnOnClick(int buttonIndex)
    {
        actionButtons[buttonIndex].onClick.Invoke(); //execute action like i press it with mouse
    }

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);
        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);
    }

    public void HideTargetframe()
    {
        targetFrame.SetActive(false);
    }
}
