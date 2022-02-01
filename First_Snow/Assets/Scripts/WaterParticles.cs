using System.Collections;
using UnityEngine;

public class WaterParticles : MonoBehaviour
{
    //====== External Objects  
    private Transform _playerTransform;     //Keeps track of where the player is 
    private CircleCollider2D _collider2D;   //Enables or disables the hit box of this Water Particle 
    private SpriteRenderer _spriteRenderer; //Makes the sprite visible and not 
    private Animator _animator;             //Turns on the animation when collected 
    private Vector3 _originalPosition;      //Keeps track of where it used to be for when the level resets 
    private AudioSource _collectSfx; 

    //===== Misc 
    private bool _isInRange;                //Tells the particle it's close enough to move towards the player 
    
    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //==================================================================================================================
    // Base Functions  
    //==================================================================================================================
    
    // Start is called before the first frame update
    private void Start()
    {
        _playerTransform = GameObject.Find($"Player").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<CircleCollider2D>();
        _originalPosition = transform.position;
        _collectSfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Update()
    {
        //If is within range of the player start moving towards them 
        if (_isInRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, 8 * Time.deltaTime);
        }
    }
    
    //==================================================================================================================
    // Water Particle Active/Inactive  
    //==================================================================================================================

    //Turns off the visibility and interactivity of the water particle till the end of this round 
    public void TurnOff()
    {
        _collider2D.enabled = false;
        _isInRange = false;
        _spriteRenderer.color = Color.clear;
        _animator.Play($"WaterParticleCollected");
        _collectSfx.Play();
        StartCoroutine(FinishTurnOff());
    }

    //Waits till the animation of +10 finishes playing to make the sprite fully clear 
    private IEnumerator FinishTurnOff()
    {
        yield return new WaitForSeconds(2.2f);
        _spriteRenderer.color = Color.clear;
    }
    
    //When level resets all of the points come back 
    public void TurnOn()
    {
        _collider2D.enabled = true;
        transform.position = _originalPosition;
        _spriteRenderer.color = Color.white;
    }
    
    //==================================================================================================================
    // Collisions  
    //==================================================================================================================

    //Checks if the WP is near the player, if is then allow it to follow 
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag($"Snowflake"))
        {
            _isInRange = true;
        }
    }
    
    //Checks if the WP has been left behind by the player and stop it from moving 
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag($"Snowflake"))
        {
            _isInRange = false;
        }
    }
}
