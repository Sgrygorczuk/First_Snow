using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGameFlow : MonoBehaviour
{
    //===== External Objects 
    private Player _player;                         //Script that controls the player 
    private Shop _shop;                             //Script that controls the player 
    private float _shopYPosition;                   //Where the shop is located used to go up whenever game starts/player dies 
    private GameObject _waterParticleHolder;        //Holds to the Game Object where all of the Water Particles are stored in

    //===== Game Flow 
    private enum GameState             
    {
        Start,      //Loads in the scene using Exit Canvas, then makes the dad say the chat bubble
        Moving,     //Moves from current position to Shop 
        Shop,       //Allows the user to buy items in the cloud and exit to fall down 
        Falling,    //The user is in control of the snowflake 
        End,        //The end cut scene plays, and we exit to credits scene 
        Done,       //After end we just go to this state where nothing updates 
    }
    
    private GameState _currentState = GameState.Start;      //Keeps track of what state we're currently in

    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //==================================================================================================================
    // Base Functions  
    //==================================================================================================================

    private void Start()
    {
        _player = GameObject.Find($"Player").transform.GetComponent<Player>();      //Gets the player script 
        _shop = GameObject.Find($"Shop").transform.GetComponent<Shop>();            //Gets the shop script 
        _shopYPosition = GameObject.Find($"Shop").transform.position.y - 2;         //Gets height - 2 so camera is positioned right 
        _waterParticleHolder = GameObject.Find($"WaterParticles");                  //Gets the water particle collector object 
    }

    // Update is called once per frame
    public void Update()
    {
        //Checks if the player is playing a desktop version if they are enables them to use ESC to quit out of the game 
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
        //Checks what state the game is currently in and updates it 
        switch (_currentState)
        {
            case GameState.Start:
            {
                StartCoroutine(StartSession());
                break;
            }
            case GameState.Moving:
            {
                MovingSession();
                break;
            }
            case GameState.Shop:
            {
                Shopping();
                break;
            }
            case GameState.Falling:
            {
                Falling();
                break;
            }
            case GameState.End:
            {
                End();
                break;
            }
            case GameState.Done:
            {
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //==================================================================================================================
    // Game States   
    //==================================================================================================================
    
    //Starts the game by playing the chat animation, giving time for the player to read it and then transitioning 
    //to showing the whole level to the player by moving across the map to the very top
    private IEnumerator StartSession()
    {
        yield return new WaitForSeconds(1.2f);
        GameObject.Find($"Goal").transform.Find($"ChatBubble").GetComponent<Animator>().Play($"TalkingText");
        yield return new WaitForSeconds(4f);
        _currentState = GameState.Moving;
    }

    //Moves the player from wherever they are back to the shop 
    private void MovingSession()
    {
        //If the player hasn't reached the shop move to the top
        if (_player.transform.position.y < _shopYPosition)
        {
            _player.BackToShop();
        }
        //If player has reached shop allow them to start shopping 
        else
        {
            _player.ScreenSpeedReset();
            _currentState = GameState.Shop;
        }
    }

    //The player has reached the shop and now is allowed to look at all of the the upgrades and purchase them if 
    //they have enough water particles 
    private void Shopping()
    {
        var shopPosition = _shop.GetShopPosition();
        
        //If Left or Right is clicked move in the menu and update the data
        if (Input.GetButtonDown($"Left"))
        {
            if (shopPosition == 0) return;
            _shop.SetShopPosition(shopPosition - 1);
            _shop.UpdateData();

        }
        else if (Input.GetButtonDown($"Right"))
        {
            if (shopPosition == 3) return;
            _shop.SetShopPosition(shopPosition + 1);
            _shop.UpdateData();
        }

        //If the Action button is pressed the player will attempt to buy an item or if they case is 3 will leave 
        //shop and start falling. 
        if (!Input.GetButtonDown($"Action")) return;
        switch (shopPosition)
        {
            case 0:
            case 1:
            case 2:
                _shop.Upgrade();
                break;
            case 3:
                _player.ShowPlayer();
                _currentState = GameState.Falling;
                _shop.SetShopPosition(0);
                _shop.UpdateData();
                break;
        }
    }

    //The player is now in the game controlling the snow false they can move left or right and if have the ability 
    //Jump 
    private void Falling()
    {
        
        //Moves left or right, or stops if no button is pressed 
        if (Input.GetButton($"Left"))
        {
            _player.MovePlayer(false);
        }
        else if (Input.GetButton($"Right"))
        {
            _player.MovePlayer(true);
        }
        else
        {
            _player.StopMoving();
        }
        
        //If player presses action button performs jump 
        if (!Input.GetButtonDown($"Action")) return;
        _player.Jump();
    }

    //The player reached the goal and now we set the animation to play out 
    private void End()
    {
        GameObject.Find("WinArt").GetComponent<Animator>().Play($"Win");
        GameObject.Find($"Camera").GetComponent<AudioSource>().Stop();
        GameObject.Find($"Goal").GetComponent<AudioSource>().Play();
        StartCoroutine(EndWin());
        _currentState = GameState.Done;
    }
    
    //Waiting for the animation to finish and load the Credits scene 
    private IEnumerator EndWin()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("WinArt").GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(2);
        GameObject.Find("ExitCanvas").transform.Find($"Panel").GetComponent<Animator>().Play($"FadeIn");
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene("Credits");
    }
    
    //==================================================================================================================
    // Support  
    //==================================================================================================================
    
    //The player hit a damage object and now we go back to scrolling to the top of the screen 
    public void Death()
    {
        _currentState = GameState.Moving;
        StartCoroutine(ResetAllWaterParticles());
    }

    //Goes through all of the water particles and resets them to their original position and intractability 
    private IEnumerator ResetAllWaterParticles()
    {
        yield return new WaitForSeconds(1f);
        for (var i = 0; i < _waterParticleHolder.transform.childCount; i++)
        {
            _waterParticleHolder.transform.GetChild(i).GetComponent<WaterParticles>().TurnOn();
        }
    }

    //A call from player that they won the game and we can move to the End Game State 
    public void Win()
    {
        _currentState = GameState.End;
    }
}