using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private readonly string[] _titleLines = new[]
        { "[Particle Collector]", "[Lucky Wind]", "[Larger Surface]", "[Exit Shop]" };

    private readonly string[] _descLines = new[]
    {
        "Water Particles Gravitate towards you.", "You can press [Space] and the wind will make you rise.",
        "Your surface area increases making your decent slower", "Begin falling"
    };

    //Keeps track of the background color, to show which item is highlighted 
    private List<SpriteRenderer> _colors = new List<SpriteRenderer>(); 

    private int _shopPosition = 0; //Current position of the user in the shop menu 

    private TextMeshPro _title; //Displays the title of the highlighted menu object
    private TextMeshPro _desc; //Displays the description of the highlighted menu object 
    
    // Start is called before the first frame update
    private void Start()
    {
        //Finds the title text display and sets the data to first position in array 
        _title = transform.Find($"Menu").Find($"Title").GetComponent<TextMeshPro>();

        //Finds the desc text display and sets the data to first position in array 
        _desc = transform.Find($"Menu").Find($"Desc").GetComponent<TextMeshPro>();

        //Goes through all the children in the Menu, looks for those with the name Item and then extra the child 
        //called White, this is used to highlight what is currently looked at 
        for (var i = 0; i < transform.Find($"Menu").childCount; i++)
        {
            if (transform.Find($"Menu").GetChild(i).name == "Item")
            {
                _colors.Add(transform.Find($"Menu").GetChild(i).transform.Find($"White").GetComponent<SpriteRenderer>());
            }
        }
        
        //Updates all the data to resemble that of the current position 
        UpdateData();
    }

    // Update is called once per frame
    private void Update()
    {
        //If Left or Right is clicked move in the menu and update the data
        if (Input.GetButtonDown($"Left"))
        {
            if (_shopPosition == 0) return;
            _shopPosition--;
            UpdateData();

        }
        else if (Input.GetButtonDown($"Right"))
        {
            if (_shopPosition == 3) return;
            _shopPosition++;
            UpdateData();
        }
    }

    //Updates the state of the data to show what's currently being displayed 
    private void UpdateData()
    {
        //Updates title and desc of currently looked item
        _title.text = _titleLines[_shopPosition];
        _desc.text = _descLines[_shopPosition];

        //Updates the highlight of each item
        for (var i = 0; i < _colors.Count; i++)
        {
            _colors[i].color = i == _shopPosition ? Color.white : Color.clear;
        }
    }
}
