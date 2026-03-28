using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Text;
public class ReadNCreate : MonoBehaviour
{
    public string TSVURL = "";
    public string EffectsURL = "";
   
    private bool isTheFirstCall = false;
    private ScriptableObjectsManager soManager;

    private void Awake()
    {
        soManager = GetComponent<ScriptableObjectsManager>();
        TSVLoader(TSVURL, EffectsURL);
    }

    void TSVLoader(string url, string url2)
    {
        url = TSVURL;
        url2 = EffectsURL;
        StartCoroutine(LoadDialogueSheet(url));
        StartCoroutine(LoadEffectSheet(url2));
    }
    public IEnumerator LoadDialogueSheet(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error Loading tsv from url");

            }
            else
            {
                string tsvData = www.downloadHandler.text;
                string downloadedHash = CompareDataHash(tsvData);
                string savedHash = PlayerPrefs.GetString("last hash", "");

                if(savedHash != downloadedHash)
                {
                    PlayerPrefs.SetString("last hash", downloadedHash);
                    ProcessTSVDialogue(tsvData);
                }
                else
                {
                    Debug.Log("TSV Sin cambios");
                }




            }
        }
    }

    public IEnumerator LoadEffectSheet(string effectUrl)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(effectUrl))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error Loading effect tsv from url");

            }
            else
            {
                
                string tsvData = www.downloadHandler.text;
                string downloadedHash = CompareDataHash(tsvData);
                string savedHash = PlayerPrefs.GetString("effect_last hash", "");

                if (savedHash != downloadedHash)
                {
                    PlayerPrefs.SetString("effect_last hash", downloadedHash);
                    ProcessTSVEffects(tsvData);
                }
                else
                {
                    Debug.Log("TSV de Efectos sin cambios");
                }


            }
        }
    }


    public void ProcessTSVEffects(string tsvData)
    {
        string[] lines = tsvData.Split('\n');
        int numRows = lines.Length;

      
        Debug.Log("Header: " + lines[0]);
        Debug.Log("Primera data: " + lines[1]);
        for (int i = 1; i < numRows; i++)
        {
            string line = lines[i].Trim();
            CreateNewEffect(lines[i]);
            if (string.IsNullOrEmpty(line))
            {
                Debug.LogWarning("Empty line in line " + i);
                continue;
            }
            else
            {
                UpdateEffect(lines);
            }
           
        }
    }

    public void ProcessTSVDialogue(string csvData)
    {
        string[] lines = csvData.Split('\n');
        int numRows = lines.Length;
        if (!isTheFirstCall)
        {
            Debug.Log("Header: " + lines[0]);
            Debug.Log("Primera data: " + lines[1]);
            for (int i = 1; i < numRows; i++)
            {
                string line = lines[i].Trim();
                CreateNewDialogue(line);
                if (string.IsNullOrEmpty(line))
                {
                    Debug.LogWarning("Línea vacía en la línea " + i);
                    continue;
                }
               
            }
        }
        else
        {
            UpdateDialogue(lines);
        }
    }


    void CreateNewDialogue(string line)
    {
        string[] values = line.Split('\t');

        BaseNode currentNode = null;

        if (values.Length >= 8)
        {
            if (int.TryParse(values[0], out int nodeID))
            {
                BaseNode asset = ScriptableObject.CreateInstance<BaseNode>();
                currentNode = asset;
                asset.ID = nodeID;
                asset.parentID = int.TryParse(values[1], out int parent) ? parent : 0;
                asset.name = values[2];
                asset.lines = new List<string>();
                asset.lines.Add(values[4]);

                if (Enum.TryParse(values[3], out TypeSpeaker speakerType))
                {
                    asset.type = speakerType;
                }
                if (Enum.TryParse(values[5], out Tags tagType))
                {
                    asset.tag = tagType;
                }
                if (values[6].Split('|').Length > 1)
                {
                    asset.choices = values[6].Split('|');
                }

                asset.price = int.TryParse(values[7], out int priceNode) ? priceNode : 0;

                string path = "Assets/Resources/ScriptableNodes/Nodes/Dialogues";


                if (!AssetDatabase.IsValidFolder("Assets/Resources/ScriptableNodes"))
                    AssetDatabase.CreateFolder("Assets/Resources", "ScriptableNodes");
                if (!AssetDatabase.IsValidFolder("Assets/Resources/ScriptableNodes/Nodes"))
                    AssetDatabase.CreateFolder("Assets/Resources/ScriptableNodes", "Nodes");
                if (!AssetDatabase.IsValidFolder("Assets/Resources/ScriptableNodes/Nodes/Dialogues"))
                    AssetDatabase.CreateFolder("Assets/Resources/ScriptableNodes/Nodes", "Dialogues");

                AssetDatabase.Refresh();

                string assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(BaseNode).ToString() + ".asset");
                AssetDatabase.CreateAsset(asset, assetPath);
                asset.nextID = int.TryParse(values[8], out int nextNode) ? nextNode : 0;


            }
            else if (currentNode != null && !string.IsNullOrEmpty(values[1]))
            {
                currentNode.lines.Add(values[4]);
                EditorUtility.SetDirty(currentNode);
                AssetDatabase.SaveAssetIfDirty(currentNode);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                isTheFirstCall = true;
            }
            else
            {
                Debug.LogError("Error al analizar el  NodeID " + nodeID );

            }
        }
    }
    void UpdateDialogue(string[] lines)
    {
        string[] guids = AssetDatabase.FindAssets("t:BaseNode");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            BaseNode node = AssetDatabase.LoadAssetAtPath<BaseNode>(path);
        }
        Dictionary<int, string> tsvLines = new Dictionary<int, string>();
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split('\t');
            if (values.Length >= 8 && int.TryParse(values[0], out int id))
            {
                tsvLines[id] = lines[i];
            }
        }

        foreach (var kvp in tsvLines)
        {
            string savedHash = PlayerPrefs.GetString("node_ " + kvp.Key, "");
            string currentHash = GetLineDataHash(kvp.Value);


            if(savedHash != currentHash)
            {
                BaseNode node = soManager.dialogueDict[kvp.Key];
                PlayerPrefs.SetString("node_" + kvp.Key, currentHash);
                EditorUtility.SetDirty(node);
                AssetDatabase.SaveAssetIfDirty(node);
            }
            else
            {
                CreateNewDialogue(kvp.Value);
            }


        }
        AssetDatabase.SaveAssets();
    }

        void UpdateEffect(string[] lines)
        {
        string[] guids = AssetDatabase.FindAssets("t:EffecBaseSO");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EffectBaseSO effect = AssetDatabase.LoadAssetAtPath<EffectBaseSO>(path);
        }
        Dictionary<int, string> tsvLines = new Dictionary<int, string>();
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split('\t');
            if (values.Length >= 4 && int.TryParse(values[0], out int id))
            {
                tsvLines[id] = lines[i];
            }
        }

        foreach (var kvp in tsvLines)
        {
            string savedHash = PlayerPrefs.GetString("effect_ " + kvp.Key, "");
            string currentHash = GetLineDataHash(kvp.Value);


            if (savedHash != currentHash)
            {
                EffectBaseSO effect = soManager.effectDict[kvp.Key];
                PlayerPrefs.SetString("effect_" + kvp.Key, currentHash);
                EditorUtility.SetDirty(effect);
                AssetDatabase.SaveAssetIfDirty(effect);
            }
            else
            {
                CreateNewEffect(kvp.Value);
            }


        }
        AssetDatabase.SaveAssets();
    }

        void CreateNewEffect(string line)
        {
        EffectBaseSO currentEffect = null;
        string[] values = line.Split('\t');
        if (values.Length >= 4)
        {
            if (int.TryParse(values[0], out int effectID))
            {
                EffectBaseSO effectAsset = ScriptableObject.CreateInstance<EffectBaseSO>();
                currentEffect = effectAsset;
                currentEffect.effectID = effectID;
                currentEffect.displayName = values[1];
                if (Enum.TryParse(values[2], out BoonType boonType))
                {
                    currentEffect.boonType = boonType;
                }
                if (Enum.TryParse(values[3], out EffectType type))
                {
                    currentEffect.effectType = type;
                }
                currentEffect.spriteKey = values[4];
                string path = "Assets/Resources/ScriptableNodes/Nodes/Effects";

                if (!AssetDatabase.IsValidFolder("Assets/Resources/ScriptableNodes"))
                    AssetDatabase.CreateFolder("Assets/Resources", "ScriptableNodes");
                if (!AssetDatabase.IsValidFolder("Assets/Resources/ScriptableNodes/Nodes"))
                    AssetDatabase.CreateFolder("Assets/Resources/ScriptableNodes", "Nodes");
                if (!AssetDatabase.IsValidFolder("Assets/Resources/ScriptableNodes/Nodes/Effects"))
                    AssetDatabase.CreateFolder("Assets/Resources/ScriptableNodes/Nodes", "Effects");

                AssetDatabase.Refresh();
                string assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(EffectBaseSO).ToString() + ".asset");

                AssetDatabase.CreateAsset(currentEffect, assetPath);
                EditorUtility.SetDirty(currentEffect);
                AssetDatabase.SaveAssetIfDirty(currentEffect);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Error al analizar el  EfffectID en del SOEffect " + effectID);
            }
        }
        else
        {
            Debug.LogError("Valores insuficientes en la línea " + line+ ". Se esperaban 4 campos, pero se encontraron " + values.Length);
        }

    }


    public string CompareDataHash(string tsvData)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(tsvData);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
               
                return sb.ToString();


            }

        }

        string GetLineDataHash(string line)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(line);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();


            }

        }
    }

