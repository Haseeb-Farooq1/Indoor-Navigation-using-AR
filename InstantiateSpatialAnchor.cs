using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InstantiateSpatialAnchor : MonoBehaviour
{
    [SerializeField] private GameObject anchorPrefab;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform xRSpaceContent;

    private List<Vector3> anchorPosition = new List<Vector3>();
    private List<Quaternion> anchorRotation = new List<Quaternion>();

    private SaveAnchorTransform saveFile;

    private string m_Filename = "content.json";

    private void Start()
    {
        LoadContents();
    }

    public void InstanciateAnchor()
    {
        GameObject anchor = Instantiate(anchorPrefab, cameraTransform.position + cameraTransform.forward, Quaternion.identity, xRSpaceContent);
        anchorPosition.Add(anchor.transform.localPosition);
        anchorRotation.Add(anchor.transform.rotation);
        SaveContents();
    }

    public void InstanciateAnchor(Vector3 pos, Quaternion rot)
    {
        GameObject anchor = Instantiate(anchorPrefab, pos, rot, xRSpaceContent);
        anchorPosition.Add(anchor.transform.localPosition);
        anchorRotation.Add(anchor.transform.rotation);
        SaveContents();
    }

    public void SaveContents()
    {
        saveFile.positions = anchorPosition;
        saveFile.rotations = anchorRotation;

        string jsonstring = JsonUtility.ToJson(saveFile, true);

        string dataPath = Path.Combine(Application.persistentDataPath, m_Filename);

        File.WriteAllText(dataPath, jsonstring);
    }

    public void LoadContents()
    {
        anchorPosition.Clear();
        anchorRotation.Clear();
        string dataPath = Path.Combine(Application.persistentDataPath, m_Filename);
        Debug.LogFormat("Trying to load file: {0}", dataPath);

        try
        {
            SaveAnchorTransform loadFile = JsonUtility.FromJson<SaveAnchorTransform>(File.ReadAllText(dataPath));

            for (int i = 0; i < loadFile.positions.Count; i++)
            {
                InstanciateAnchor(loadFile.positions[i], loadFile.rotations[i]);
            }

            Debug.Log("Successfully loaded file!");
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarningFormat("{0}\n.json file for content storage not found. Created a new file!", e.Message);
            File.WriteAllText(dataPath, "");
        }
        catch (NullReferenceException err)
        {
            Debug.LogWarningFormat("{0}\n.json file for content storage not found. Created a new file!", err.Message);
            File.WriteAllText(dataPath, "");
        }
    }
}

[System.Serializable]
public struct SaveAnchorTransform
{
    public List<Vector3> positions;
    public List<Quaternion> rotations;
}