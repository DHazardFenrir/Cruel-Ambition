using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayText : MonoBehaviour
{
    [SerializeField] TMP_Text displayText;
    [SerializeField] BaseNode currentNode;
    [SerializeField] Button buttonNext;
    private int currentLineIndex = 0;
    private ScriptableObjectsManager soManager;

    private void Awake()
    {
        soManager = GetComponent<ScriptableObjectsManager>();
        soManager.LoadAllDialogue(); 
    }

    private void Start()
    {
       
        Display();
        buttonNext.onClick.AddListener(NextLine);
    }


    // Update is called once per frame
    void Update()
    {
      
    }

    void TypeWriterEffect(string text)
    {

    }

    void Display()
    {

        if (currentNode != null && currentNode.lines.Count > 0)
        {
            displayText.text = currentNode.lines[currentLineIndex];
        }



    }

    private bool MeetCondition(BaseNode node)
    {
        switch (node.condition)
        {
            case Condition.NONE:
                return true;
                break;

            case Condition.HAS_STRETCH:
                return GameManager.Instance.GetPlayerState().HasEffect(Condition.HAS_STRETCH);
                break;

            case Condition.HAS_TRANSFORM:
                return GameManager.Instance.GetPlayerState().HasEffect(Condition.HAS_TRANSFORM);
                break;

            case Condition.HAS_TRANSPARENCY:
                return GameManager.Instance.GetPlayerState().HasEffect(Condition.HAS_TRANSPARENCY);
                break;

            case Condition.HAS_WEAPON:
                return GameManager.Instance.GetPlayerState().HasEffect(Condition.HAS_WEAPON);
                break;

            case Condition.IS_MARRIED:
                return GameManager.Instance.GetPlayerState().HasEffect(Condition.IS_MARRIED);
                break;


            default:
                return true;


        }
    }

    void NextLine()
    {
        Debug.Log("currentline index" + currentLineIndex);
        currentLineIndex++;
        if (currentLineIndex >= currentNode.lines.Count)
        {
            BaseNode nextNode = soManager.ReturnNextNode(currentNode.nextID);

            while (nextNode != null && !MeetCondition(nextNode))
            {
                nextNode = soManager.ReturnNextNode(nextNode.nextID);
            }

            currentNode = nextNode;
            currentLineIndex = 0;
        }
        Display();

    }
}
