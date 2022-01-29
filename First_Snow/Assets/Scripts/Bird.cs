using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    //================ Movement 
    private List<Transform> _patrolPoints = new List<Transform>(); //Holds all the positions that the enemy will travel
    private const float Speed = 2f;                                //Speed of the enemy 
    private int _currentPointIndex;                                //Which point are they at right now
    
    //================== Pause 
    private float _waitTime = 0.5f;   //Timer 
    private const float StartWaitTime = 0.5f; //What timer resets to 

    public bool isStartingOnPointOne;
    
    // Start is called before the first frame update
    private void Start()
    {
        _patrolPoints.Add(transform.parent.transform.Find($"Point_1"));
        _patrolPoints.Add(transform.parent.transform.Find($"Point_2"));

        transform.position = isStartingOnPointOne ? _patrolPoints[1].position : _patrolPoints[0].position;
        transform.rotation = isStartingOnPointOne ? _patrolPoints[1].rotation : _patrolPoints[0].rotation;

    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _patrolPoints[_currentPointIndex].position,
            Speed * Time.deltaTime);
        transform.rotation = _patrolPoints[_currentPointIndex].rotation;
        if (transform.position != _patrolPoints[_currentPointIndex].position) return;
        if (_waitTime <= 0)
        {
            if (_currentPointIndex == _patrolPoints.Count - 1)
            {
                _currentPointIndex = 0;
            }
            else
            {
                _currentPointIndex++;
            }

            transform.rotation = _patrolPoints[_currentPointIndex].transform.rotation;
            _waitTime = StartWaitTime;
        }
        else
        {
            _waitTime -= Time.deltaTime;
        }
    }
}
