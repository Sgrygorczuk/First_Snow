using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGameFlow : MonoBehaviour
{
    private Player _player;
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
            //Set HitBox Off
            //Set Sprite Off
            _player.BackToShop();
        }
        else
        {
            //Set HitBox Off
            //Set Sprite Off
            _player.ScreenSpeedReset();
            _currentState = GameState.Shop;
        }
    }
}
