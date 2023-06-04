using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LoadDataAsset : MonoBehaviour
{
    
    public List<Sprite> spriteList;
    public string session;
    // Start is called before the first frame update
    void Start()
    {
        SpawnDataAsset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SpawnDataAsset()
    {
        foreach (Sprite sprite in spriteList)
        {
            Data vocabulary = ScriptableObject.CreateInstance<Data>();

            vocabulary.word = sprite.name;
            vocabulary.session = session;
            vocabulary.image = sprite;

            string path = "Assets/Data/" + sprite.name + ".asset";
           
            AssetDatabase.CreateAsset(vocabulary, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();           
        }
    }
}
