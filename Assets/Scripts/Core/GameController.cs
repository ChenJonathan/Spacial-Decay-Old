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
        get { return field; }
    }

    [SerializeField]
    private Player playerPrefab;
    private Player player;
    public Player Player
    {
        get { return player; }
    }

    public int Difficulty
    {
        get { return currentMap.difficulty; }
    }

    private Map currentMap;
    private Room currentRoom;
    private Wave currentWave;

    private IntVector playerLocation;
    public IntVector Location
    {
        get { return playerLocation; }
    }
    private int waveCount;
    
    private HashSet<IntVector> cleared = new HashSet<IntVector>();
    public HashSet<IntVector> ClearedRooms
    {
        get { return cleared; }
    }
    private HashSet<IntVector> opened = new HashSet<IntVector>();
    public HashSet<IntVector> OpenedRooms
    {
        get { return opened; }
    }

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
        currentMap = Generate.RandomMap(9, 9, 1, 0.6f);
        StartMap();
    }

    public override void Update()
    {
        if (!Paused)
            base.Update();

        if (Input.GetKeyDown(KeyCode.Escape) && MapIndicator.Instance.CurrentState != MapIndicator.State.Clickable)
            Pause(!Paused);
    }

    public void StartMap()
    {
        MapIndicator.Instance.Generate(currentMap);
        MapIndicator.Instance.CurrentState = MapIndicator.State.Invisible;

        opened.Add(currentMap.start);
        SetRoom(currentMap.start);
    }

    public void EndMap()
    {
        // TODO
    }

    public void StartRoom()
    {
        waveCount = 0;
        StartWave();
    }

    public void EndRoom()
    {
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
            MapIndicator.Instance.CurrentState = MapIndicator.State.Clickable;
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

        MapIndicator.Instance.CurrentState = MapIndicator.State.Invisible;
        Pause(false);
        StartRoom();
    }

    public void StartWave()
    {
        currentWave = (Wave)Instantiate(currentRoom.waves[waveCount], Vector2.zero, Quaternion.identity);
        currentWave.transform.parent = transform;
    }

    public void EndWave()
    {
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
        
        if (Paused)
            MapIndicator.Instance.CurrentState = MapIndicator.State.ViewOnly;
        else
            MapIndicator.Instance.CurrentState = MapIndicator.State.Invisible;
    }
}
