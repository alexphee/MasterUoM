using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : Window
{
    [SerializeField]
    private TextMeshProUGUI text;

    private Dialogue dialogue;
    private DialogueNode currentNode;
    private List<DialogueNode> answers = new List<DialogueNode>();
    private List<GameObject> buttons = new List<GameObject>(); //so i can delete buttons after
    [SerializeField]
    private GameObject answerButtonPrefab;
    [SerializeField]
    private Transform answerTransform; //so the answer goes under this, as child
    [SerializeField]
    private float speed; //text speed

    private static DialogueWindow instance;
    public static DialogueWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueWindow>();
            }
            return instance;
        }
    }

    public void SetDialogue(Dialogue dialogue)
    {
        text.text = string.Empty;
        this.dialogue = dialogue;
        currentNode = dialogue.Nodes[0];
        StartCoroutine(RunDialogue(currentNode.Text));
    }


    private IEnumerator RunDialogue(string dialogue)
    {
        for (int i = 0; i < dialogue.Length; i++) //this runs through the dialogue text 1 by 1 letter
        {
            text.text += dialogue[i];
            yield return new WaitForSeconds(speed);
        }
        ShowAnswers();
    }

    
    private void ShowAnswers()
    {
        answers.Clear(); //solves errors
        foreach (DialogueNode node in dialogue.Nodes)
        {
            if (node.Parent == currentNode.Name)
            {
                answers.Add(node);//run through nodes and if the parent of the node im looking is the same as current node then its a child and a possible answer
            }
        }


        if (answers.Count > 0)
        {
            answerTransform.gameObject.SetActive(true);

            foreach (DialogueNode node in answers)
            {
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = node.Answer;
                go.GetComponent<Button>().onClick.AddListener(delegate { PickAnswer(node); }); //assigne buttons to the method below

            }
        }
        else //if there are no answers left give option to close dialogue
        {
            answerTransform.gameObject.SetActive(true);
            GameObject go = Instantiate(answerButtonPrefab, answerTransform);
            buttons.Add(go);
            go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
            go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
        }
    }

     private void PickAnswer(DialogueNode node)
    {
        this.currentNode = node; //change the node to the current
        Clear(); //clear previous text, it stacks with the new one without this 
        StartCoroutine(RunDialogue(currentNode.Text)); //run dialogue based on answer picked

    }

    public void CloseDialogue()
    {
        Close(); //the Window.scr method
        Clear();
    }
    private void Clear()
    {
        text.text = string.Empty;
        answerTransform.gameObject.SetActive(false);
        foreach (GameObject gameObject in buttons) //clear buttons
        {
            Destroy(gameObject);
        }
        buttons.Clear();//remove any references to the destroyed objects
    }
}
