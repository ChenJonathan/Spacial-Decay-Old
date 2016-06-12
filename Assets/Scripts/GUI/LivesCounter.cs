using UnityEngine;
using UnityEngine.UI;
using DanmakU;
using System.Collections;

public class LivesCounter : MonoBehaviour {

    [SerializeField]
    private GameObject heartPrefab;

    private int maxLives;
    private float width;
    private float unit;

    private GameObject[] livesCounter;
    
	void Start()
    {
        maxLives = ((GameController)GameController.Instance).Player.Lives;
        width = GetComponent<RectTransform>().rect.width;
        unit = width / maxLives;

        livesCounter = new GameObject[maxLives];
        for (int i = 0; i < maxLives; i++)
        {
            livesCounter[i] = (GameObject)Instantiate(heartPrefab);
            livesCounter[i].transform.SetParent(this.transform);
            livesCounter[i].transform.localScale = this.transform.localScale;
            livesCounter[i].transform.localPosition = new Vector2(i * unit - width/2 + unit/2, 0);
        }

        UpdateCounter(maxLives);
	}
	
	public void UpdateCounter (int lives)
    {
        if (lives >= 0)
        {
            for (int i = 0; i < lives; i++)
            {
                livesCounter[i].SetActive(true);
            }
            for (int i = lives; i < maxLives; i++)
            {
                livesCounter[i].SetActive(false);
            }
        }
	}
}
