using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button[] actionButtons;
    private KeyCode action1, action2;
    // Start is called before the first frame update
    void Start()
    {
        action1 = KeyCode.Alpha1; //numeric key 1 keybind
        action2 = KeyCode.Alpha2; //numeric key 2 keybind
    }

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
}
