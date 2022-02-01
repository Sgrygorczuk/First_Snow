using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    //============= Shop Data 
    //Keeps track of the title of the item, is modifiable 
    private string[] _titleLines = new[]
        { "[Particle Collector Lvl. 0]", "[Lucky Wind Lvl. 0]", "[Larger Surface Lvl. 0]", "[Exit Shop]" };

    //Keeps track of the descriptions of the data, not modifiable 
    private readonly string[] _descLines = new[]
    {
        "Water Particles Gravitate towards you.", "You can press [Space] and the wind will make you rise. By the way do you smell that?",
        "Your surface area increases making your descent slower. Hexagons are the Bestagons.", "Begin falling."
    };

    private int[] _upgradeRank = new[] { 0, 0, 0 };         //Keep track of what rank each upgrade is at 
    private int[] _upgradeCost = new[] { 20, 40, 60, };     //Keeps track of how much it cost to upgrade it each item 
    
    private int _shopPosition; //Current position of the user in the shop menu 
    
    //============ External Objects 
    
    //Keeps track of the background color, to show which item is highlighted 
    private List<SpriteRenderer> _colors = new List<SpriteRenderer>();

    private TextMeshPro _title; //Displays the title of the highlighted menu object
    private TextMeshPro _desc;  //Displays the description of the highlighted menu object 
    private TextMeshPro _cost;  //Displays the description of the highlighted menu object 
    private AudioSource _approve;   //Plays approving noise after player purchased something 
    private AudioSource _deny;      //Plays denying noise if the player can't afford or is maxed out on item 
    
    
    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //==================================================================================================================
    // Base Function  
    //==================================================================================================================
    
    // Start is called before the first frame update
    private void Start()
    {
        //Finds the title text display and sets the data to first position in array 
        _title = transform.Find($"Menu").Find($"Title").GetComponent<TextMeshPro>();

        //Finds the desc text display and sets the data to first position in array 
        _desc = transform.Find($"Menu").Find($"Desc").GetComponent<TextMeshPro>();
        
        //Finds the desc text display and sets the data to first position in array 
        _cost = transform.Find($"Menu").Find($"Cost").GetComponent<TextMeshPro>();

        _approve = transform.Find($"Approve").GetComponent<AudioSource>();
        
        _deny = transform.Find($"Deny").GetComponent<AudioSource>();
        
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

    //==================================================================================================================
    // Shop Movement  
    //==================================================================================================================
    
    //Returns the current position of the player in the shop menu 
    public int GetShopPosition()
    {
        return _shopPosition;
        
    }

    //Updates the current position of the player in the shop menu 
    public void SetShopPosition(int newPosition)
    {
        _shopPosition = newPosition;
    }
    
    //==================================================================================================================
    // Upgrade Functions  
    //==================================================================================================================

    //Upgrades the currently looked at upgrade and updates it's data 
    public void Upgrade()
    {
        //Checks if the player has enough money to do the upgrade and that the rank is less than 3 (MAX) rank
        if (GameObject.Find($"Player").GetComponent<Player>().WaterParticleValue() >= _upgradeCost[_shopPosition] &&
            _upgradeRank[_shopPosition] < 3)
        {
            //Takes out the player cash
            GameObject.Find($"Player").GetComponent<Player>().PayWaterParticles(_upgradeCost[_shopPosition]);
            //Updates the title to have the rank shown, if it's 2 or 3 (MAX) 
            _titleLines[_shopPosition] = _upgradeRank[_shopPosition] != 2
                ? _titleLines[_shopPosition].Replace(_upgradeRank[_shopPosition].ToString(),
                    (_upgradeRank[_shopPosition] + 1).ToString())
                : _titleLines[_shopPosition].Replace(_upgradeRank[_shopPosition].ToString(), ("MAX"));
            //Updates the rank value 
            _upgradeRank[_shopPosition]++;
            //Doubles cost for next upgrade for that item 
            _upgradeCost[_shopPosition] *= 2;
            //Update the visuals 
            UpdateData();
            //Updates the Data inside of player with the purchase choice 
            UpgradePlayer();
            _approve.Play();
        }
        else
        {
            _deny.Play();
        }
    }

    //Goes through the choices of upgrading the player component based on which area of the shop we're in 
    private void UpgradePlayer()
    {
        switch (_shopPosition)
        {
            case 0:
            {
                GameObject.Find($"Player").GetComponent<Player>().UpgradeRadius();
                break;
            }
            case 1:
            {
                GameObject.Find($"Player").GetComponent<Player>().UpgradeJumps();
                break;
            }
            case 2:
            {
                GameObject.Find($"Player").GetComponent<Player>().UpgradeSurface();
                break;
            }
        }
    }
    
    //==================================================================================================================
    // Visual Update  
    //==================================================================================================================

    //Updates the state of the data to show what's currently being displayed 
    public void UpdateData()
    {
        //Updates title and desc of currently looked item
        _title.text = _titleLines[_shopPosition];
        _desc.text = _descLines[_shopPosition];
        //If the rank is 3 (MAX) or the position is 4 (EXIT) don't show a price 
        _cost.text = _shopPosition < 3 && _upgradeRank[_shopPosition] != 3 ? _upgradeCost[_shopPosition] + " WP" : " ";

        //Updates the highlight of each item
        for (var i = 0; i < _colors.Count; i++)
        {
            _colors[i].color = i == _shopPosition ? Color.white : Color.clear;
        }
    }
}