using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueWindow : Window
{
    Dictionary<int, bool> teacherIDcheck = new Dictionary<int, bool>();
    
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

    //private bool flag = true;
    private int maxCheck;
    private int currentCheck;
    private Player player;
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
        maxCheck = 0;
        currentCheck = 0;
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
            for (int i = 0; i < node.Parent.Length; i++)
            {
              if (node.Parent[i] == currentNode.Name)
              {
                answers.Add(node);//run through nodes and if the parent of the node im looking is the same as current node then its a child and a possible answer
              }
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
                maxCheck += node.Check;
                
            }
        }
        else //if there are no answers left give option to close dialogue
        {
            if (dialogue.NPC_type == "simple")
            {
                answerTransform.gameObject.SetActive(true);
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                if (dialogue.IsGreek)
                {
                    go.GetComponentInChildren<TextMeshProUGUI>().text = "Τέλος";
                }
                else {
                    go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
                }
                
                go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
                
            }
            else if (dialogue.NPC_type == "first_encounter")
            {
                answerTransform.gameObject.SetActive(true);
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                if (dialogue.IsGreek)
                {
                    go.GetComponentInChildren<TextMeshProUGUI>().text = "Τέλος";
                }
                else
                {
                    go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
                }
                Item qi = Instantiate(InventoryScr.MyInstance.items[5]);
                InventoryScr.MyInstance.AddItem(qi);
                go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
            }
            else if (dialogue.NPC_type == "second_encounter")
            {
                answerTransform.gameObject.SetActive(true);
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                if (dialogue.IsGreek)
                {
                    go.GetComponentInChildren<TextMeshProUGUI>().text = "Τέλος";
                }
                else
                {
                    go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
                }
                Item qi = Instantiate(InventoryScr.MyInstance.items[10]);
                InventoryScr.MyInstance.AddItem(qi);
                go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
            }
            
            else if (dialogue.NPC_type == "teacher")
            {
                
                if (currentCheck == maxCheck)
                {
                    if (!teacherIDcheck.ContainsKey(dialogue.TeacherID))
                    {
                        teacherIDcheck.Add(dialogue.TeacherID, false);
                    }
                    
                    //Debug.Log("CORRECT");
                    if (teacherIDcheck[dialogue.TeacherID] == false)
                    {
                        Item qi = Instantiate(InventoryScr.MyInstance.items[4]);
                        InventoryScr.MyInstance.AddItem(qi);
                        teacherIDcheck[dialogue.TeacherID] = true;
                        answerTransform.gameObject.SetActive(true);
                        GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                        buttons.Add(go);
                        if (dialogue.IsGreek)
                        {
                            go.GetComponentInChildren<TextMeshProUGUI>().text = "Όλα σωστά!\nΔες στην τσάντα σου!";
                        }
                        else {
                            go.GetComponentInChildren<TextMeshProUGUI>().text = "All correct!\nCheck your bag!";
                        }
                        
                        go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
                    }
                    
                }
                else if (currentCheck <= maxCheck)
                {
                    //Debug.Log(maxCheck - currentCheck + "WRONG ANSWERS");
                    answerTransform.gameObject.SetActive(true);
                    GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                    buttons.Add(go);
                    if (dialogue.IsGreek)
                    {
                        go.GetComponentInChildren<TextMeshProUGUI>().text = "Σωστά: " + currentCheck + "/" + maxCheck + " Προσπάθησε ξανά.";
                    }
                    else
                    {
                        go.GetComponentInChildren<TextMeshProUGUI>().text = "Correct: " + currentCheck + "/" + maxCheck + " Try again.";
                    }
                    go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
                }
               
            }
            else if (dialogue.NPC_type == "scene1")
            {
                answerTransform.gameObject.SetActive(true);
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
                go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
                LoadScene("Scene01");
            }
            else if (dialogue.NPC_type == "scene1gr")
            {
                answerTransform.gameObject.SetActive(true);
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = "Τέλος";
                go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
                LoadScene("Scene01gr");
            }
            else if (dialogue.NPC_type == "scene2")
            {
                answerTransform.gameObject.SetActive(true);
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
                go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
                LoadScene("Scene02");
            }
            else if (dialogue.NPC_type == "scene2gr")
            {
                answerTransform.gameObject.SetActive(true);
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = "Τέλος";
                go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
                LoadScene("Scene02gr");
            }
            else if (dialogue.NPC_type == "scene3")
            {
                answerTransform.gameObject.SetActive(true);
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
                go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
                LoadScene("Scene03");
            }
            else if (dialogue.NPC_type == "scene3gr")
            {
                answerTransform.gameObject.SetActive(true);
                GameObject go = Instantiate(answerButtonPrefab, answerTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = "Τέλος";
                go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialogue(); });
                LoadScene("Scene03gr");
            }



        }
    }

     private void PickAnswer(DialogueNode node)
    {
        if (node.Check == 1)
        {
            currentCheck += 1;
        }
        this.currentNode = node; //change the node to the current
        Clear(); //clear previous text, it stacks with the new one without this 
        StartCoroutine(RunDialogue(currentNode.Text)); //run dialogue based on answer picked

    }

    public void CloseDialogue()
    {
        Debug.Log("closed");
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

    public void LoadScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }
}
