using System.Collections;
using UnityEngine;

public class LevelGameFlow : MonoBehaviour
{
    private Player _player;
    private Shop _shop;
    private float _shopYPosition;
    
    private enum GameState             
    {
        Start,      //Loads in the scene using Exit Canvas, then makes the dad say the chat bubble
        Moving,     //Moves from current position to Shop 
        Shop,       //Allows the user to buy items in the cloud and exit to fall down 
        Falling,    //The user is in control of the snowflake 
        End,        //The end cut scene plays, and we exit to credits scene 
    }
    
    private GameState _currentState = GameState.Start;      //Keeps track of what state we're currently in


    private void Start()
    {
        _player = GameObject.Find($"Player").transform.GetComponent<Player>();
        _shop = GameObject.Find($"Shop").transform.GetComponent<Shop>();
        _shopYPosition = GameObject.Find($"Shop").transform.position.y - 3;
    }

    // Update is called once per frame
    public void Update()
    {
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
        if (_player.transform.position.y < _shopYPosition)
        {
            _player.BackToShop();
        }
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
                
                break;
        }
    }
}