using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private Transform answerTransform;
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

    public void SetDialogue(Dialogue dialogue) //is going to be called when i click an npc
    {
        text.text = string.Empty;
        this.dialogue = dialogue;
        this.currentNode = dialogue.Nodes[0];
        StartCoroutine(RunDialogue(currentNode.Text));
    }

    private IEnumerator RunDialogue(string dialogue)
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            text.text += dialogue[i]; //for text to appear letter by letter
            yield return new WaitForSeconds(speed);
        }
        ShowAnswers();
        
    }

    private void ShowAnswers()
    {
        foreach (DialogueNode node in dialogue.Nodes)
        {
            if (node.Parent == currentNode.Name)
            {
                answers.Add(node);
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
            }
        }

    }
}
