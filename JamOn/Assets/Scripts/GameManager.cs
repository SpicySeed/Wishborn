using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] int numLevels = 2;
    [SerializeField] Text collectableText; //TODO: mover esto al manager de la interfaz del nivel

    private int currentLevel = 0;
    private int[] collectablesCollected;   

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
            return;
        }
        Destroy(gameObject);
    }

    public void Init()
    {
        collectablesCollected = new int[numLevels];
    }

    public void ObjectCollected()
    {
        collectablesCollected[currentLevel]++;
        collectableText.text = collectablesCollected[currentLevel].ToString();
    }

    public int GetObjectsCollected()
    {
        return collectablesCollected[currentLevel];
    }
}
