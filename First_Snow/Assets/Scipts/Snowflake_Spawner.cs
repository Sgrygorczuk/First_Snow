using UnityEngine;
using Random = UnityEngine.Random;

public class Snowflake_Spawner : MonoBehaviour
{
    //Used to generate a position for the new snowflake to spawn at
    private float _leftPointX;              //Left bound       
    private float _rightPointX;             //Right bound
    private float _yPosition;                       //Height
    
    //Snowflake management 
    private GameObject _snowflakes;         //Holds all the snowflakes 
    private GameObject _snowflake;          //A copy of a snowflake to spawn

    //Used for timing the spawn of new snowflakes 
    private float _spawnTime = 0;   //The timer
    private const float StartSpawn = 1.5f; //Max time the timer can be a

    private void Start()
    {
        //Grabs the data from the children of the game object 
        _leftPointX = transform.Find("Left").position.x;       
        _rightPointX = transform.Find("Right").position.x;
        _yPosition = transform.Find("Left").position.y;
        _snowflakes = transform.Find("Snowflakes").gameObject;
        
        //Grabs the data from a Snowflake object in the scene
        _snowflake = GameObject.Find("Snowflake");
    }

    // Update is called once per frame
    public void Update()
    {
        //Checks if both the timer and the child count are good for the new snowflake to be spawned
        if (_spawnTime <= 0 && _snowflakes.transform.childCount <= 6)
        {
            //Generates an new x value for the snowflakes position 
            var xPosition =  Random.Range(_leftPointX, _rightPointX);
            //Creates the snowflake 
            var instantiate = Instantiate(_snowflake, new Vector3(xPosition, _yPosition, 0), Quaternion.identity);
            //Sets the snowflake as a child of the Snowflakes object so we can count it
            instantiate.transform.SetParent(_snowflakes.transform);
            //Resets the timer 
            _spawnTime = StartSpawn;
        }
        else
        {
            _spawnTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        //Checks to see if any snowflakes touched the hixBox, if they did destory them 
        if (hitBox.CompareTag($"Snowflake"))
        {
            Destroy(hitBox.gameObject);
        }
    }
}
