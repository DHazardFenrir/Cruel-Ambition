using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScriptableObjectsManager : MonoBehaviour
{
    public Dictionary<int, BaseNode> dialogueDict;
    public Dictionary<int, EffectBaseSO> effectDict;
    private const string pathDialogueNodes = "ScriptableNodes/Nodes/Dialogues";
    private const string pathEffectNodes = "ScriptableNodes/Nodes/Effects";


    private void Awake()
    {

        LoadAllDialogue();
        LoadAEffects();
    }
    public Dictionary<int, BaseNode> GetAllDialogue()
    {
        return dialogueDict;
    }

    public BaseNode ReturnNextNode(int nextID)
    {
        Debug.Log("Buscando nextID: " + nextID);
        Debug.Log("Keys en dict: " + string.Join(", ", dialogueDict.Keys));

        if (dialogueDict.ContainsKey(nextID))
        {
            return dialogueDict[nextID];
        }

        Debug.LogError("No existe el nodo con ID: " + nextID);
        return null;
    }

    public Dictionary<int, EffectBaseSO> GetEffect()
    {
        return effectDict;
    }

    public void LoadAllDialogue()
    {
        dialogueDict = new Dictionary<int, BaseNode>();

        BaseNode[] allDialogues = Resources.LoadAll<BaseNode>("");
        Debug.Log("Sin path, nodos encontrados: " + allDialogues.Length);
        foreach (var node in allDialogues)
            Debug.Log("Encontrado: " + node.name + " en: " + node.ID);

        foreach (var node in allDialogues)
        {
            try
            {
                Debug.Log("Intentando nodo: " + node.ID + " | " + node.name);
                if (!dialogueDict.ContainsKey(node.ID))
                {
                    dialogueDict.Add(node.ID, node);
                    Debug.Log("Agregado OK: " + node.ID);
                }
                else
                {
                    Debug.LogWarning("ID duplicado ignorado: " + node.ID);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error en nodo " + node.name + ": " + e.Message);
            }
        }

        Debug.Log("Dict final, keys: " + string.Join(", ", dialogueDict.Keys));
    }



    void LoadAEffects()
    {
        effectDict = new Dictionary<int, EffectBaseSO>();
       EffectBaseSO[] allEffects= Resources.LoadAll<EffectBaseSO>(pathEffectNodes);
        foreach (var nodeEff in allEffects)
        {
            if (!effectDict.ContainsKey(nodeEff.effectID))
            {
                SpriteLoader.GetSprite(nodeEff.spriteKey);
                effectDict.Add(nodeEff.effectID, nodeEff);
            }
        }

    }
}
