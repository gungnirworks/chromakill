using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public bool CinematicOn { get; set; }

    private List<GameObject> players = new List<GameObject>();
    public List<GameObject> Players { get { return players; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FindAllPlayers();
    }

    public bool FindAllPlayers()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        if (playerObjects.Length < 0)
            return false;
        else
        {
            foreach (GameObject obj in playerObjects)
            {
                if (!players.Contains(obj))
                {
                    players.Add(obj);
                }
            }
        }
        return true;
    }
}