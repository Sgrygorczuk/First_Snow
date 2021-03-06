using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    //==== External Objects 
    private Animator _animator; //The animator that will allow us to fade out of the scene 
    
    //===== Misc 
    private string _levelName = "Level"; //Allows us to place the name of the scene we should transition to after completing the action
    private bool _actionCalled;         //Keeps track of if the user has activated the fade 
    
    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //==================================================================================================================
    // Base Functions  
    //==================================================================================================================
    
    // Start is called before the first frame update
    private void Start()
    {
        //Grabs the data from a Snowflake object in the scene
        _animator = GameObject.Find("ExitCanvas").transform.Find("Panel").GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        //Checks if this is the desktop version and if it is allow the player to used ESC to quit 
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
        //If player clicks the Action Button play the Fade animation and stop user from activating it again 
        if (!Input.GetButtonDown($"Action") || _actionCalled) return;
        _animator.Play($"Fade");
        _actionCalled = true;
        StartCoroutine(ExitScene());
        
    }
    
    //Allows time for the animation to play out then moves on to the level screen 
    private IEnumerator ExitScene()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(_levelName);
    }
}
