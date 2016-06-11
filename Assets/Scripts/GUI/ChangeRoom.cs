using UnityEngine;
using System.Collections;

public class ChangeRoom : MonoBehaviour {
    private enum Dir { Up, Down, Left, Right };

    [SerializeField]
    private Dir direction;

    private GameController gc;

    void Start()
    {
    }
}
