using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    //================ External Objects 
    private BoxCollider2D _boxCollider2D;           //To Enable and Disable the box collider 
    private CircleCollider2D _circleCollider2D;      //To Enable and Disable the circle collider and update the radius 
    private Rigidbody2D _rigidbody2D;               //To change the gravity setting 
    private SpriteRenderer _spriteRenderer;         //To change the sprite visibility 
    private TextMeshPro _waterParticleText;         //To update the currency in text for player to see 
    
    //============ Jumping Vars 
    
    private List<SpriteRenderer> _jumps = new List<SpriteRenderer>();   //Holds the sprites for the jump power up 
    private int _jumpsMax;                                              //Keeps track of how many times the player can jump (MAX times)
    private int _jumpsAvailable;                                        //How many times the player can use the jump action this round 
    
    //========== Speed Vars 
    private float _gravity = 0.1f;           //The gravity at which the player will fall
    private float _maxY = -12;               //Controls how fast you can fall at max speed 
    
    //=========== Misc 
    private int _waterParticles = 0;           //How much money the player has 
    private Vector3 _screenSpeed = Vector3.zero; //Keeps track of speed  
    
    //==================================================================================================================
    //Functions 
    //==================================================================================================================
    
    //==================================================================================================================
    // Base Functions 
    //==================================================================================================================

    // Start is called before the first frame update
    private void Start()
    {
        //Grab components directly from the game object 
        _boxCollider2D = transform.GetComponent<BoxCollider2D>();
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
        _circleCollider2D = transform.GetComponent<CircleCollider2D>();
        //Grab the component from child of this game object 
        _spriteRenderer = transform.Find($"Sprite").transform.GetComponent<SpriteRenderer>();
        
        //Grabs the child of the Virtual Camera and immediately updates it to show the value of current player currency 
        _waterParticleText = GameObject.Find($"VirtualCamera").transform.Find($"PlayerStats").transform.Find($"WP_Text").GetComponent<TextMeshPro>();
        _waterParticleText.text = _waterParticles + " WP";

        //Goes through the children of Virtual Camera and pulls out anything with the name Fly saving them to the array to be later modified 
        for (var i = 0; i < GameObject.Find($"VirtualCamera").transform.Find($"PlayerStats").transform.childCount; i++)
        {
            if (GameObject.Find($"VirtualCamera").transform.Find($"PlayerStats").transform.GetChild(i).name == "Fly")
            {
                _jumps.Add(GameObject.Find($"VirtualCamera").transform.Find($"PlayerStats").transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }

        //Goes through all the saved "Fly" objects and clears the sprite until the player unlocks them 
        foreach (var sprite in _jumps)
        {
            sprite.color = Color.clear;
        }
        
        //Hides the player 
        HidePlayer();
    }

    //The Fixed Update checks if player has reached max falling speed and caps them to it 
    private void FixedUpdate()
    {
        if (_rigidbody2D.velocity.y < _maxY)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _maxY);
        }
    }

    //==================================================================================================================
    // Upgrades 
    //==================================================================================================================
    
    //Upgrades the max number of jumps the player can perform during a fall, then updates the visual data 
    public void UpgradeJumps()
    {
        _jumpsMax++;
        ResetJumpCounter();
    }

    //Upgrades decent speed, making gravity half as strong 
    public void UpgradeSurface()
    {
        _gravity -= 0.015f;
        _maxY += 2f;
    }

    //Upgrades the radius of which can affect the water particles to be attracted to the player 
    public void UpgradeRadius()
    {
        _circleCollider2D.radius++;
    }
    
    //==================================================================================================================
    // Visual Updates 
    //==================================================================================================================

    //Resets the amount of available jumps back to the Max and updates visual data 
    private void ResetJumpCounter()
    {
        _jumpsAvailable = _jumpsMax;
        UpdateJumpVisual();
    }

    //Updates the visual for the amount of jumps the player can perform 
    private void UpdateJumpVisual()
    {
        for (var i = 0; i < _jumps.Count; i++)
        {
            _jumps[i].color = i < _jumpsAvailable ? Color.white : Color.clear;
        }
    }

    //Hides player by removing the gravity, turning off the box collider and making the sprite clear 
    private void HidePlayer()
    {
        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.velocity = Vector2.zero;
        transform.position = new Vector3(0, transform.position.y, 0);
        _boxCollider2D.enabled = false;
        _circleCollider2D.enabled = false;
        _spriteRenderer.color = Color.clear;
    }
    
    //Shows player by adding the gravity, turning on the box collider and making the sprite white 
    public void ShowPlayer()
    {
        _rigidbody2D.gravityScale = _gravity;
        _boxCollider2D.enabled = true;
        _circleCollider2D.enabled = true;
        _spriteRenderer.color = Color.white;
    }
    
    //==================================================================================================================
    // Cutscene Movement 
    //==================================================================================================================

    //Pulls the player back to the shop from wherever they are at a increasing speed 
    public void BackToShop()
    {
        if (_screenSpeed.y < 1f)
        {
            _screenSpeed += Vector3.up / 250;
        }
        
        transform.position += _screenSpeed;
    }


    //Resets the speed to zero 
    public void ScreenSpeedReset()
    {
        _screenSpeed = Vector3.zero;
    }
    
    //==================================================================================================================
    // Economy
    //==================================================================================================================

    //Gives back the current value of the water particle, used to see if the player has enough to pay
    public int WaterParticleValue()
    {
        return _waterParticles;
    }

    //Pays the amount required and updates the visual 
    public void PayWaterParticles(int cost)
    {
        _waterParticles -= cost;
        _waterParticleText.text = _waterParticles + " WP";
    }
    
    //==================================================================================================================
    // Player Movement 
    //==================================================================================================================

    //Moves the player left and right
    public void MovePlayer(bool side)
    {
        var xSpeed = side ?  3 : -3;
        _rigidbody2D.velocity = new Vector2(xSpeed, _rigidbody2D.velocity.y);
    }

    //Stops player from moving 
    public void StopMoving()
    {
        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
    }
    
    //==================================================================================================================
    // Jumping
    //==================================================================================================================

    //If possible sends the snowflake flying up 
    public void Jump()
    {
        if (_jumpsAvailable <= 0) return;
        var velocity = _rigidbody2D.velocity;
        var yVelocity = velocity.y;
        _jumpsAvailable--;
        velocity = new Vector2(velocity.x, 1);
        _rigidbody2D.velocity = velocity;
        UpdateJumpVisual();
        StartCoroutine(JumpFall(yVelocity));
    }
    
    //2 Seconds after the jump the player gets back half their initial falling speed 
    private IEnumerator JumpFall(float currentSpeed)
    {
        yield return new WaitForSeconds(2f);
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, currentSpeed/2);
    }
    
    //==================================================================================================================
    // Collision 
    //==================================================================================================================

    //If the player hits collides with anything it dies or if it gets to the goal zone it processed to end the game 
    private void OnTriggerEnter2D(Collider2D col)
    {
        
        //Updates player counter for WP 
        if (col.CompareTag($"WP") && _boxCollider2D.IsTouching(col))
        {
            PayWaterParticles(-10);
            col.transform.GetComponent<WaterParticles>().TurnOff();
        }
        
        //Updates if the player hits a colliding object 
        if (col.CompareTag($"Damage") && _boxCollider2D.IsTouching(col))
        {
            HidePlayer();
            ResetJumpCounter();
            col.transform.gameObject.GetComponent<AudioSource>().Play();
            GameObject.Find($"Camera").GetComponent<LevelGameFlow>().Death();
        }

        //Updates if the player hits the win zone 
        if (col.CompareTag($"Win"))
        {
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = Vector2.zero;
            GameObject.Find($"Camera").GetComponent<LevelGameFlow>().Win();
        }
        
    }
}
