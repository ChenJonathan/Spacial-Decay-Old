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
    
    [SerializeField]
    private GameObject waveMessage;
    [SerializeField]
    private GameObject roomMessage;
    [SerializeField]
    private GameObject upArrow;
    [SerializeField]
    private GameObject downArrow;
    [SerializeField]
    private GameObject leftArrow;
    [SerializeField]
    private GameObject rightArrow;

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
            if (playerLocation.x + 1 != currentMap.size.x && currentMap.rooms[playerLocation.x + 1][playerLocation.y].active)
            {
                rightArrow.SetActive(true);
            }
            if (playerLocation.x != 0 && currentMap.rooms[playerLocation.x - 1][playerLocation.y].active)
            {
                leftArrow.SetActive(true);
            }
            if (playerLocation.y + 1 != currentMap.size.y && currentMap.rooms[playerLocation.x][playerLocation.y + 1].active)
            {
                downArrow.SetActive(true);
            }
            if (playerLocation.y != 0 && currentMap.rooms[playerLocation.x][playerLocation.y - 1].active)
            {
                upArrow.SetActive(true);
            }
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
    
    public void ChangeRoom(string direction)
    {
        int dx = 0, dy = 0;
        Vector2 playerEndLoc = new Vector2(0, 0);

        switch (direction)
        {
            case "Up":
                dx = 0;
                dy = -1;
                playerEndLoc = new Vector2(0, -9);
                break;
            case "Down":
                dx = 0;
                dy = 1;
                playerEndLoc = new Vector2(0, 9);
                break;
            case "Left":
                dx = -1;
                dy = 0;
                playerEndLoc = new Vector2(16.5f, 0);
                break;
            case "Right":
                dx = 1;
                dy = 0;
                playerEndLoc = new Vector2(-16.5f, 0);
                break;
            default:
                Debug.Log("Invalid direction");
                break;
        }

        IntVector newLocation = new IntVector(playerLocation.x + dx, playerLocation.y + dy);
        SetRoom(newLocation);

        upArrow.SetActive(false);
        downArrow.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);

        player.SetMoveTarget(playerEndLoc);

        StartRoom();
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
            StartCoroutine(EndRoomMessage());
        }
        else
        {
            StartCoroutine(EndWaveMessage());
        }
    }

    IEnumerator EndWaveMessage()
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

    IEnumerator EndRoomMessage()
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
}
