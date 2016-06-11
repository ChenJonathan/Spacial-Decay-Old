using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Generate
{
    public static GameController.Map RandomMap(int width, int height, int difficulty)
    {
        GameController.Map map = new GameController.Map();
        map.difficulty = difficulty;
        map.size = new IntVector(width, height);

        map.rooms = new GameController.Room[width][];
        for (int i = 0; i < width; i++)
        {
            map.rooms[i] = new GameController.Room[height];
            for (int j = 0; j < height; j++)
            {
                map.rooms[i][j] = RandomRoom(difficulty);
                map.rooms[i][j].up = true;
                map.rooms[i][j].down = true;
                map.rooms[i][j].left = true;
                map.rooms[i][j].right = true;
            }
        }

        map.start = new IntVector(0, 0);
        map.end = new IntVector(0, 2);

        return map;
    }

    public static GameController.Room RandomRoom(int numWaves)
    {
        GameController.Room room = new GameController.Room();
        room.active = true;
        room.cleared = 0;
        room.waves = new List<Wave>();
        for (int i = 0; i < numWaves; i++)
        {
            room.waves.Add(RandomWave());
        }
        return room;
    }

    public static Wave RandomWave()
    {
        // TODO
        return new TestWave();
    }
}
