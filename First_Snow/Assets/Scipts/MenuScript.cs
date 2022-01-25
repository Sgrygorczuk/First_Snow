using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public static string nextSceneName = "Menu"; //Allows us to place the name of the scene we should transition to after completing the action
    
    private Animator _animator; //The animator that will allow us to fade out of the scene 
    private bool _actionCalled; //Keeps track of if the user has activated the fade 
    
    // Start is called before the first frame update
    private void Start()
    {
        //Grabs the data from a Snowflake object in the scene
        _animator = GameObject.Find("ExitCanvas").transform.Find("Panel").GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Update()
    {
        //If player clicks the Action Button play the Fade animation and stop user from activating it again 
        if (!Input.GetButtonDown($"Action") || _actionCalled) return;
        _animator.Play($"Fade");
        _actionCalled = true;
        StartCoroutine(ExitScene());
    }
    
    private static IEnumerator ExitScene()
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(nextSceneName);
    }
}
