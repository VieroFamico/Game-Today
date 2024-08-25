using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Nav_Manager : MonoBehaviour
{
    public static Scene_Nav_Manager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Restart()
    {
        int temp = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(temp);
    }
}
