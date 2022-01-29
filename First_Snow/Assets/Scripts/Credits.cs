using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(End());
    }

    void Update()
    {
        if (Input.GetButtonDown($"Action"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        
    }
    
    private static IEnumerator End()
    {
        yield return new WaitForSeconds(26f);
        SceneManager.LoadScene("MainMenu");
    }
}
