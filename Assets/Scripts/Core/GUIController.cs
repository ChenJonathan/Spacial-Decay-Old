using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {

    [SerializeField]
    private GameObject DamageGUI;
    
	void Start()
    {
	
	}
	
	void Update()
    {

	}
    
    public void GenerateDamageIndicator(int damage, Vector2 position)
    {
        GameObject damageGUI = (GameObject) Instantiate(DamageGUI, position, Quaternion.identity);
        damageGUI.transform.parent = ((GameController)GameController.Instance).Field.transform;
        damageGUI.GetComponent<TextMesh>().text = "" + damage;
    }
    
}
