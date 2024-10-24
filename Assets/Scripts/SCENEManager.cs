using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SCENEManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log($"scene manage is ready.");
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
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
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
        if (id == -1 && name == null)
        {
            Debug.LogError($"scene arg is missing.");
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
