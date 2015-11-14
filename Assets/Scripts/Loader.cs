using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;

    public void Awake()
    {
        if (GameManager.instance == null)
            Instantiate(gameManager);
    }
}
