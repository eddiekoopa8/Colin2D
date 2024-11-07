using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Assets.Scripts;
using System;

public class SCENEManager : MonoBehaviour
{
    bool restarting = false;
    void Start()
    {
        Debug.Log($"scene manage is ready.");
        restarting = false;
    }

    public void ChangeScene(int id)
    {
        changeScene__(null, id);
    }

    public void ChangeScene(string name)
    {
        changeScene__(name, -1);
    }

    public void Restart()
    {
        if (!restarting)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            restarting = true;
        }
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    bool verifyScene__(string name)
    {
        if (name == null) return true;

        for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            //Debug.Log(SceneUtility.GetScenePathByBuildIndex(i));
            if (SceneUtility.GetScenePathByBuildIndex(i) == "Assets/" + name + ".unity")
            {
                return true;
            }
        }
        return false;
    }

    private void changeScene__(string name, int id)
    {
        GAMEManager.PopStars();
        if (id == -1 && name == null)
        {
            Debug.LogError($"scene arg is missing. please supply them");
            Debug.Break();
        }

        if (!verifyScene__(name))
        {
            Debug.LogError($"scene is not found.");
            Debug.Break();
        }
        else
        {
            if (id != -1)
            {
                SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);
            }
            else if (name != null)
            {
                SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogError($"arg parse err: choose EITHER id or name");
                Debug.Break();
            }
        }
    }
}
