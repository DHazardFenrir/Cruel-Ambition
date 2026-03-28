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

    void NextLine()
    {
        Debug.Log("currentline index" + currentLineIndex);
        currentLineIndex++;
        if(currentLineIndex >= currentNode.lines.Count-1)
        {
            currentNode = soManager.ReturnNextNode(currentNode.nextID);
            currentLineIndex = 0;
        }
        Display();
        
    }
}
