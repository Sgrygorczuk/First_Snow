using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    //================ Movement 
    private List<Transform> _patrolPoints = new List<Transform>(); //Holds all the positions that the enemy will travel
    private const float Speed = 2f;                                //Speed of the enemy 
    private int _currentPointIndex;                                //Which point are they at right now
    
    //================== Pause 
    private float _waitTime = 0.5f;                                //Timer 
    private const float StartWaitTime = 0.5f;                       //What timer resets to 

    //================== Misc 
    public bool isStartingOnPointOne;                               //Tells the enemy if they start from point 1 or 2 
    
    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    // Start is called before the first frame update
    private void Start()
    {
        //Gets the transforms of the two points 
        _patrolPoints.Add(transform.parent.transform.Find($"Point_1"));
        _patrolPoints.Add(transform.parent.transform.Find($"Point_2"));

        //Sets the position and rotation to point 1 or 2 based on isStartingOnPointOne state 
        transform.position = isStartingOnPointOne ? _patrolPoints[1].position : _patrolPoints[0].position;
        transform.rotation = isStartingOnPointOne ? _patrolPoints[1].rotation : _patrolPoints[0].rotation;

    }

    //Updates the enemy to move between different points provided in the array 
    private void Update()
    {
        //Moves towards the current objective 
        transform.position = Vector2.MoveTowards(transform.position, _patrolPoints[_currentPointIndex].position,
            Speed * Time.deltaTime);
        
        //Checks if the enemy has reached the point it's moving towards 
        if (transform.position != _patrolPoints[_currentPointIndex].position) return;
        
        //Checks if it has stayed on the point for long enough 
        if (_waitTime <= 0)
        {
            //If its last point in array go to first point 
            if (_currentPointIndex == _patrolPoints.Count - 1)
            {
                _currentPointIndex = 0;
            }
            //Go to next point in array
            else
            {
                _currentPointIndex++;
            }

            //Update the rotation of the sprite 
            transform.rotation = _patrolPoints[_currentPointIndex].transform.rotation;
            //Rest the timer 
            _waitTime = StartWaitTime;
        }
        //Else count down till it leave the point 
        else
        {
            _waitTime -= Time.deltaTime;
        }
    }
}
