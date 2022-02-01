using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //==================================================================================================================
    // Base Functions  
    //==================================================================================================================
    
    //Start the end function 
    private void Start()
    {
        StartCoroutine(End());
    }

    //Listens for the player to click the Action button to return them to the main menu 
    public void Update()
    {
        if (Input.GetButtonDown($"Action"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        
    }
    
    //Waits 26 second and if the player hasn't clicked a button goes back to the main menu 
    private static IEnumerator End()
    {
        yield return new WaitForSeconds(26f);
        SceneManager.LoadScene("MainMenu");
    }
}
