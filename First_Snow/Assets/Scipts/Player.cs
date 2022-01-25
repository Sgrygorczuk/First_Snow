using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _screenSpeed = Vector3.zero; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToShop()
    {
        if (_screenSpeed.y < 0.5f)
        {
            _screenSpeed += Vector3.up / 250;
        }
        
        transform.position += _screenSpeed;
    }

    public void ScreenSpeedReset()
    {
        _screenSpeed = Vector3.zero;
    }
}
