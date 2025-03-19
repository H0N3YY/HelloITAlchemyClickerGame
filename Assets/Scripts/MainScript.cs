using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MainScript : MonoBehaviour
{
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void StartSetup()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Wyjdz()
    {
        Application.Quit();
    }

   
}
