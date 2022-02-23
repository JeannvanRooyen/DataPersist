using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartButton : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMain()
    {
        if (string.IsNullOrEmpty(MainManager.userName))
        {
            MainManager.userName = "unknown";
        }

        SceneManager.LoadScene(1);
    }
}
