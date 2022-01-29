using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGameFlow : MonoBehaviour
{
    private Player _player;
    private Shop _shop;
    private float _shopYPosition;
    private GameObject _waterParticleHolder;

    private enum GameState             
    {
        Start,      //Loads in the scene using Exit Canvas, then makes the dad say the chat bubble
        Moving,     //Moves from current position to Shop 
        Shop,       //Allows the user to buy items in the cloud and exit to fall down 
        Falling,    //The user is in control of the snowflake 
        End,        //The end cut scene plays, and we exit to credits scene 
        Done,
    }
    
    private GameState _currentState = GameState.Start;      //Keeps track of what state we're currently in


    private void Start()
    {
        _player = GameObject.Find($"Player").transform.GetComponent<Player>();
        _shop = GameObject.Find($"Shop").transform.GetComponent<Shop>();
        _shopYPosition = GameObject.Find($"Shop").transform.position.y - 2;
        _waterParticleHolder = GameObject.Find($"WaterParticles");
    }

    // Update is called once per frame
    public void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
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
        }
    }

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

    private void Falling()
    {
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
        
        if (!Input.GetButtonDown($"Action")) return;
        _player.Jump();
    }

    public void Death()
    {
        _currentState = GameState.Moving;
        StartCoroutine(ResetAllWaterParticles());
    }

    private IEnumerator ResetAllWaterParticles()
    {
        yield return new WaitForSeconds(1f);
        for (var i = 0; i < _waterParticleHolder.transform.childCount; i++)
        {
            _waterParticleHolder.transform.GetChild(i).GetComponent<WaterParticles>().TurnOn();
        }
    }

    public void Win()
    {
        _currentState = GameState.End;
    }

    private void End()
    {
        GameObject.Find("WinArt").GetComponent<Animator>().Play($"Win");
        GameObject.Find($"Camera").GetComponent<AudioSource>().Stop();
        GameObject.Find($"Goal").GetComponent<AudioSource>().Play();
        StartCoroutine(EndWin());
        _currentState = GameState.Done;
    }
    
    private IEnumerator EndWin()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("WinArt").GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(2);
        GameObject.Find("ExitCanvas").transform.Find($"Panel").GetComponent<Animator>().Play($"FadeIn");
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene("Credits");
    }
}