using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{


    public float difficulty;

    private GameManager gameManager;

    public GameObject titleScreen;
    public GameObject titleBackground;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        button.onClick.AddListener(SetDifficulty);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetDifficulty()
    {
        gameManager.StartGame(difficulty);


        titleScreen.SetActive(false);
        titleBackground.SetActive(false);

    }

}
