using UnityEngine;
using System.Collections;
using DanmakU;

public class GameController : DanmakuGameController
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

    [SerializeField]
    private Vector2 playerSpawnLocation;

    public override void Awake()
    {
        base.Awake();
        Vector2 spawnPos = Field.WorldPoint((Vector2)playerSpawnLocation);
        Player player = (Player)Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        if (player != null)
        {
            player.transform.parent = Field.transform;
            player.Field = Field;
        }
    }

	public override void Update ()
    {
        base.Update();
	}
}
