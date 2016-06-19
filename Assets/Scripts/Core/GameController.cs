using UnityEngine;
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

    private Map currentMap;
    private Room currentRoom;
    private Wave currentWave;

    private IntVector playerLocation;
    private int waveCount;

    private HashSet<IntVector> cleared = new HashSet<IntVector>();
    private HashSet<IntVector> available = new HashSet<IntVector>();

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
        currentMap = Generate.RandomMap(3, 3, 3, 0.6f);
        StartMap();
    }

    public override void Update()
    {
        if (!Paused)
            base.Update();

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause(!Paused);
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

        if (playerLocation.Equals(currentMap.end))
        {
            EndMap();
        }
        else
        {
            cleared.Add(new IntVector(playerLocation.x, playerLocation.y));

            if (currentRoom.right)
            {
                available.Add(new IntVector(playerLocation.x + 1, playerLocation.y));
                rightArrow.SetActive(true);
            }
            if (currentRoom.left)
            {
                available.Add(new IntVector(playerLocation.x - 1, playerLocation.y));
                leftArrow.SetActive(true);
            }
            if (currentRoom.down)
            {
                available.Add(new IntVector(playerLocation.x, playerLocation.y + 1));
                downArrow.SetActive(true);
            }
            if (currentRoom.up)
            {
                available.Add(new IntVector(playerLocation.x, playerLocation.y - 1));
                upArrow.SetActive(true);
            }
        }
    }

    public void SetRoom(IntVector newLocation)
    {
        if (currentMap.rooms[newLocation.x][newLocation.y].active)
        {
            playerLocation = newLocation;
            currentRoom = currentMap.rooms[playerLocation.x][playerLocation.y];
        }
    }

    public void ChangeRoom(string direction)
    {
        int dx = 0, dy = 0;
        Vector2 playerEndLoc = Vector2.zero;

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
    }
}
