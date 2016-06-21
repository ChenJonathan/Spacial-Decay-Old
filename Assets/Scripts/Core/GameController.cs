﻿using UnityEngine;
using System.Collections;
using DanmakU;
using System.Collections.Generic;

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

    public int Difficulty
    {
        get
        {
            return currentMap.difficulty;
        }
    }

    private Map currentMap;
    private Room currentRoom;
    private Wave currentWave;

    private IntVector playerLocation;
    private int waveCount;

    private HashSet<IntVector> cleared = new HashSet<IntVector>();
    private HashSet<IntVector> opened = new HashSet<IntVector>();

    private GameObject mapIndicator;

    private bool roomSelecting;

    [SerializeField]
    private GameObject waveMessage;
    [SerializeField]
    private GameObject roomMessage;

    public bool Paused
    {
        get;
        set;
    }

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
        public List<string> waves;

        // Doors
        public bool up;
        public bool down;
        public bool left;
        public bool right;
    }

    public override void Awake()
    {
        base.Awake();

        Vector2 spawnPos = Field.WorldPoint(Vector2.zero);
        player = (Player)Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        if (player != null)
        {
            player.transform.parent = Field.transform;
            player.Field = Field;
        }
    }

    public void Start()
    {
        mapIndicator = GameObject.FindGameObjectWithTag("Map");

        currentMap = Generate.RandomMap(3, 3, 1, 0.6f);
        StartMap();
    }

    public override void Update()
    {
        if (!Paused)
            base.Update();

        if (Input.GetKeyDown(KeyCode.Escape) && !roomSelecting)
            Pause(!Paused);
    }

    public void StartMap()
    {
        mapIndicator.GetComponent<MapIndicator>().Generate(currentMap);

        opened.Add(currentMap.start);
        SetRoom(currentMap.start);
    }

    public void EndMap()
    {
        // TODO
        Debug.Log("Map finished");
    }

    public void StartRoom()
    {
        waveCount = 0;
        StartWave();
    }

    public void EndRoom()
    {
        // TODO
        Debug.Log("Room finished");
        
        if (playerLocation.Equals(currentMap.end))
        {
            EndMap();
        }
        else
        {
            cleared.Add(new IntVector(playerLocation.x, playerLocation.y));

            if (currentRoom.right)
            {
                opened.Add(new IntVector(playerLocation.x + 1, playerLocation.y));
            }
            if (currentRoom.left)
            {
                opened.Add(new IntVector(playerLocation.x - 1, playerLocation.y));
            }
            if (currentRoom.down)
            {
                opened.Add(new IntVector(playerLocation.x, playerLocation.y + 1));
            }
            if (currentRoom.up)
            {
                opened.Add(new IntVector(playerLocation.x, playerLocation.y - 1));
            }
            roomSelecting = true;
            Pause(true);
        }
    }

    public void SetRoom(int index)
    {
        int x = index / currentMap.size.y;
        int y = index - x * currentMap.size.y;
        SetRoom(new IntVector(x, y));
    }

    public void SetRoom(IntVector newLocation)
    {
        if (currentMap.rooms[newLocation.x][newLocation.y].active)
        {
            playerLocation = newLocation;
            currentRoom = currentMap.rooms[playerLocation.x][playerLocation.y];
        }
        player.SetMoveTarget(Vector2.zero);

        roomSelecting = false;
        Pause(false);
        StartRoom();
    }

    public void StartWave()
    {
        currentWave = (Wave)gameObject.AddComponent(WaveManager.Instance.Get(currentRoom.waves[waveCount]));
    }

    public void EndWave()
    {
        Debug.Log("Wave finished");

        Destroy(currentWave);
        waveCount++;
        if (waveCount == currentRoom.waves.Count)
        {
            StartCoroutine(EndRoomMessage());
        }
        else
        {
            StartCoroutine(EndWaveMessage());
        }
    }

    private IEnumerator EndWaveMessage()
    {
        CanvasRenderer messageRender = waveMessage.GetComponent<CanvasRenderer>();
        waveMessage.SetActive(true);

        for (float a = 0f; a <= 1f; a += 0.01f)
        {
            messageRender.SetAlpha(a);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        for (float a = 1f; a >= 0f; a -= 0.01f)
        {
            messageRender.SetAlpha(a);
            yield return null;
        }
        
        waveMessage.SetActive(false);
        StartWave();
    }

    private IEnumerator EndRoomMessage()
    {
        CanvasRenderer messageRender = roomMessage.GetComponent<CanvasRenderer>();
        roomMessage.SetActive(true);

        for (float a = 0f; a <= 1f; a += 0.01f)
        {
            messageRender.SetAlpha(a);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        for (float a = 1f; a >= 0f; a -= 0.01f)
        {
            messageRender.SetAlpha(a);
            yield return null;
        }

        roomMessage.SetActive(false);
        
        EndRoom();
    }
    
    public void Pause(bool value)
    {
        Paused = value;
        Player.Paused = value;
        if (currentWave != null)
        {
            currentWave.Paused = value;
            foreach (Enemy enemy in currentWave.Enemies)
                enemy.Paused = value;
        }

        mapIndicator.SetActive(value);
        if (value)
            mapIndicator.GetComponent<MapIndicator>().ViewMap(opened, cleared, playerLocation, !roomSelecting);
    }
}
