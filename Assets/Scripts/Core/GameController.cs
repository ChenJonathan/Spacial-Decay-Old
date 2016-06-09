using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;
using System;

public partial class GameController : DanmakuGameController
{
    [SerializeField]
    private DanmakuField field;
    public DanmakuField Field
    {
        get
        {
            return field;
        }
    }

    [SerializeField]
    private Player playerPrefab;
    private Player player;
    public Player Player
    {
        get
        {
            return player;
        }
    }

    [SerializeField]
    private int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
    }

    private Map currentMap;

    private IntVector playerLocation;
    private Room currentRoom;

    private int waveCount;
    private Wave currentWave;

    public struct Map
    {
        public int difficulty;

        public IntVector size;
        public Room[][] rooms;

        public IntVector start;
        public IntVector end;
    }

    public struct Room
    {
        public bool active;
        public int cleared;
        public List<Wave> waves;

        // Doors
        public bool up;
        public bool down;
        public bool left;
        public bool right;
    }

    public override void Awake()
    {
        base.Awake();

        Vector2 spawnPos = Field.WorldPoint(new Vector2(0, 0));
        player = (Player)Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        if (player != null)
        {
            player.transform.parent = Field.transform;
            player.Field = Field;
        }

        currentMap = Generate.RandomMap(3, 3, 3);
        StartMap();
    }

	public override void Update ()
    {
        base.Update();

        if(currentWave != null)
        {
            currentWave.Update();
        }
    }

    public void StartMap()
    {
        SetRoom(currentMap.start);
        StartRoom();
    }

    public void EndMap()
    {
        // TODO
        Debug.Log("Map finished");
    }

    public void StartRoom()
    {
        // player.transform.position = new Vector2(0, 0);

        waveCount = 0;
        StartWave();
    }

    public void EndRoom()
    {
        // TODO
        Debug.Log("Room finished");
        if(playerLocation.Equals(currentMap.end))
        {
            EndMap();
        }
        else
        {
            SetRoom(new IntVector(playerLocation.x, playerLocation.y + 1));
            StartRoom();
        }
    }

    public void SetRoom(IntVector newLocation)
    {
        if(currentMap.rooms[newLocation.x][newLocation.y].active)
        {
            playerLocation = newLocation;
            currentRoom = currentMap.rooms[playerLocation.x][playerLocation.y];
        }
    }

    public void StartWave()
    {
        currentWave = currentRoom.waves[waveCount];
        currentWave.Start();
    }

    public void EndWave()
    {
        //TODO
        Debug.Log("Wave finished");
        currentWave = null;
        waveCount++;
        if (waveCount == currentRoom.waves.Count)
        {
            EndRoom();
        }
        else
        {
            StartWave();
        }
    }
}
