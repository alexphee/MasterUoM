using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private string npc_type;

    [SerializeField]
    private DialogueNode[] nodes;

    public DialogueNode[] Nodes { get => nodes; set => nodes = value; }
    public string NPC_type { get => npc_type; set => npc_type = value; }
}
