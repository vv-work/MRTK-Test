using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public bool _autoLoad = true;


    public void Awake()
    {
        if(_autoLoad)
            LoadSceneTwo(); 

    }
    public static void LoadSceneOne()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public static void LoadSceneTwo()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
