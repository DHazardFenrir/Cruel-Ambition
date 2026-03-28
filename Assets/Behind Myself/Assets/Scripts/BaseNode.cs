using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue", order = 1)]
public class BaseNode : ScriptableObject
{
    public int ID;
    public int parentID;
    public List<string> lines;
    public Tags tag;
    public TypeSpeaker type;
    public string[] choices;
    public int nextID;
    public int price; 
}
