using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _screenSpeed = Vector3.zero;

    private BoxCollider2D _boxCollider2D;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private int _waterParticles = 1000;
    private TextMeshPro _waterParticleText;
    private List<SpriteRenderer> _jumps = new List<SpriteRenderer>();
    private int _jumpsMAX;
    private int _jumpsAvailable;
    private float _gravity = 0.1f;

    // Start is called before the first frame update
    private void Start()
    {
        _boxCollider2D = transform.GetComponent<BoxCollider2D>();
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.Find($"Sprite").transform.GetComponent<SpriteRenderer>();
        
        _waterParticleText = GameObject.Find($"Camera").transform.Find($"WP_Text").GetComponent<TextMeshPro>();
        _waterParticleText.text = _waterParticles.ToString();
        
        for (var i = 0; i < GameObject.Find($"Camera").transform.childCount; i++)
        {
            if (GameObject.Find($"Camera").transform.GetChild(i).name == "Fly")
            {
                _jumps.Add(GameObject.Find($"Camera").transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }

        foreach (var sprite in _jumps)
        {
            sprite.color = Color.clear;
        }
        
        HidePlayer();
    }

    public void UpgradeJumps()
    {
        _jumpsMAX++;
        ResetJumpCounter();
    }

    public void ResetJumpCounter()
    {
        _jumpsAvailable = _jumpsMAX;
        for (var i = 0; i < _jumps.Count; i++)
        {
            _jumps[i].color = i < _jumpsAvailable ? Color.white : Color.clear;
        }
    }

    //Hides player by removing the gravity, turning off the box collider and making the sprite clear 
    public void HidePlayer()
    {
        _rigidbody2D.gravityScale = 0;
        _boxCollider2D.enabled = false;
        _spriteRenderer.color = Color.clear;
    }
    
    //Shows player by adding the gravity, turning on the box collider and making the sprite white 
    public void ShowPlayer()
    {
        _rigidbody2D.gravityScale = 0.1f;
        _boxCollider2D.enabled = true;
        _spriteRenderer.color = Color.white;
    }

    //Pulls the player back to the shop from wherever they are at a increasing speed 
    public void BackToShop()
    {
        if (_screenSpeed.y < 0.5f)
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

    public int WaterParticleValue()
    {
        return _waterParticles;
    }

    public void PayWaterParticles(int cost)
    {
        _waterParticles -= cost;
        _waterParticleText.text = _waterParticles.ToString();
    }
}
