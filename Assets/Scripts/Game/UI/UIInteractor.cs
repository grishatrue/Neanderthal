//This script changes Text to the name of the GameObject with the script attached
//Attach this script to a GameObject
//Attach a Text GameObject in the Inspector (Create>UI>Text)
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractor : MonoBehaviour
{
    public Text thealth;
    public Text tarmor;
    public Text tmana;

    public PlayerValues PlayerValues; // for PREFAB

    private void Awake()
    {
        //plv = new PlayerValues();
    }

    private void FixedUpdate()
    {
        UpdatePlayersStats();
        UpdateMiniMap();
    }

    public void UpdatePlayersStats()
    {
        
        thealth.text = PlayerValues.health.ToString() + "/100";
        tarmor.text = PlayerValues.armor.ToString() + "/100";
        tmana.text = PlayerValues.mana.ToString() + "/100";
    }

    public void UpdateMiniMap()
    {

    }
}