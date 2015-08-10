using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour 
{
    public GameObject Player;
    private static int Deaths;
	// Use this for initialization
	void Start ()
    {
        if (Player == null)
            this.enabled = false;
        Deaths = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void ResetPlayer()
    {
        Player.transform.position = Vector2.zero;
        Deaths++;
    }
    public static int GetDeaths()
    {
        return Deaths;
    }
}
