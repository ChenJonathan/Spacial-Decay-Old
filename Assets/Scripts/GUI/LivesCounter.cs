using UnityEngine;
using UnityEngine.UI;
using DanmakU;
using System.Collections;

public class LivesCounter : MonoBehaviour {

    [SerializeField]
    private int maxLives;
    [SerializeField]
    private int width;
    private int unit;

    private RectTransform rect;
    
	void Start()
    {
        maxLives = ((GameController)GameController.Instance).Player.Lives;
        unit = width / maxLives;
        rect = GetComponent<RectTransform>();

        UpdateCounter(maxLives);
	}
	
	public void UpdateCounter (int lives)
    {
        if (rect != null)
            rect.localPosition = new Vector2(lives * unit - width, 0);
	}
}
