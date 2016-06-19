﻿using UnityEngine;
using UnityEngine.UI;
using DanmakU;
using System.Collections;

public class LivesCounter : MonoBehaviour {

    [SerializeField]
    private GameObject heartPrefab;
    [SerializeField]
    private GameObject overFlow;

    private int maxLives;
    private float width;
    private float heartSize;

    [SerializeField]
    private float gap = 6;
    [SerializeField]
    private int maxDisplayCount = 5;

    private GameObject[] livesCounter;
    
	void Start()
    {
        maxLives = ((GameController)GameController.Instance).Player.Lives;
        heartSize = heartPrefab.GetComponent<RectTransform>().sizeDelta.x;
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(2 * gap + heartSize * transform.localScale.x / 2, -2 * gap - heartSize * transform.localScale.y / 2);
        rt.sizeDelta = new Vector2((heartSize + gap) * maxDisplayCount, 30);
        rt.pivot = new Vector2(0.5f / maxDisplayCount, 0.5f);

        livesCounter = new GameObject[maxLives];
        for (int i = 0; i < Mathf.Min(maxLives, maxDisplayCount); i++)
        {
            livesCounter[i] = (GameObject)Instantiate(heartPrefab);
            livesCounter[i].transform.SetParent(transform);
            livesCounter[i].transform.localScale = new Vector3(1, 1, 1);
            livesCounter[i].transform.localPosition = new Vector2(i * (heartSize + gap), 0);
        }
        if (maxLives > maxDisplayCount)
        {
            livesCounter[maxDisplayCount] = (GameObject)Instantiate(overFlow);
            livesCounter[maxDisplayCount].transform.SetParent(transform);
            livesCounter[maxDisplayCount].transform.localScale = this.transform.localScale;
            livesCounter[maxDisplayCount].transform.localPosition = new Vector2((heartSize + gap) / 2, 0);
        }

        UpdateCounter(maxLives);
	}
	
	public void UpdateCounter (int lives)
    {
        if (lives >= 0)
        {
            if (lives <= maxDisplayCount)
            {
                for (int i = 0; i < lives; i++)
                {
                    livesCounter[i].SetActive(true);
                }
                for (int i = lives; i < maxDisplayCount; i++)
                {
                    livesCounter[i].SetActive(false);
                }
                if (maxLives > maxDisplayCount)
                    livesCounter[maxDisplayCount].SetActive(false);
            }
            else
            {
                livesCounter[0].SetActive(true);
                for (int i = 1; i < maxDisplayCount; i++)
                {
                    livesCounter[i].SetActive(false);
                }
                livesCounter[maxDisplayCount].GetComponent<Text>().text = " × " + lives;
                livesCounter[maxDisplayCount].SetActive(true);
            }
            
        }
	}
}
