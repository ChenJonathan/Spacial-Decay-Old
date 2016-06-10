using UnityEngine;
using System.Collections;

public class DamageIndicator : MonoBehaviour {
    
    [SerializeField]
    private int scrollSpeed = 5;
    [SerializeField]
    private int duration = 40;
    private int remaining;

    [SerializeField]
    public static GameObject DamageGUI;

    private MeshRenderer damageRenderer;

    public bool Paused
    {
        get;
        set;
    }

    void Start()
    {
        transform.Translate(new Vector2(Random.insideUnitCircle.x * 0.7f, 0));
        damageRenderer = gameObject.GetComponent<MeshRenderer>();
        remaining = duration;
    }

	void Update()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y + scrollSpeed), Time.deltaTime * remaining / duration);
        Color c = damageRenderer.material.color;
        damageRenderer.material.color = new Color(c.r, c.g, c.b, Mathf.Lerp(c.a, 0, Time.deltaTime * scrollSpeed)); // Fade away effect
        
        if (remaining-- <= 0)
        {
            Destroy(gameObject);
        }
    }
}
