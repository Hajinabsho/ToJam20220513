using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverCanvas;
   // BossHP boss;

    // Update is called once per frame

    private void Start()
    {
        //boss = GetComponent<BossHP>();
    }
    void Update()
    {

        if (FindObjectOfType<BossHP>().getbossDead() == true)
        {
            gameOverCanvas.SetActive(true);
           
        }

        Debug.Log(FindObjectOfType<BossHP>().getbossDead());
    }
}
