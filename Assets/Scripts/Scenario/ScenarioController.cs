﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class ScenarioController : MonoBehaviour
{
    private ScenarioMap scenarioMap;
    private List<List<ScenarioMazeElement>> scenarioGrid;
    private PlayerMovimentation playerMovimentation;

    [Header("Data")]
    [SerializeField] private float distance = 1f;
    [SerializeField] private int levelWidth, levelHeight;
    [SerializeField] private TextAsset mapText;

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    [Header("Prefabs")]
    [SerializeField] private GameObject tile;

    [Header("Sprites")]
    [SerializeField] private Sprite dotSprite;

    private void Awake()
    {
        scenarioMap = new ScenarioMap();

        string inlineMap = System.Text.RegularExpressions.Regex.Replace(mapText.text, @"\t|\n|\r", "");

        scenarioMap.SetScenarioString(inlineMap);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovimentation = playerController.GetPlayerMovimentation();
        SpawnScenario();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerMovimentBasedOnLocal();
    }


    //!!!
    private void SpawnScenario()
    {
        this.scenarioGrid = scenarioMap.GetScenarioGrid(levelWidth, levelHeight);

        for(int h=0; h< this.scenarioGrid.Count; h++)
        {
            for (int w = 0; w < this.scenarioGrid[h].Count; w++)
            {
                if (this.scenarioGrid[h][w].elementChar.Equals('0')) continue;

                Vector3 _tilePosition = Vector3.right * scenarioMap.GetCenteredTilePositionByIndex(levelWidth, w, distance);
                _tilePosition += Vector3.down * scenarioMap.GetCenteredTilePositionByIndex(levelHeight, h, distance);

                GameObject tileObject = Instantiate(tile, _tilePosition, Quaternion.identity, transform);

                this.scenarioGrid[h][w].elementSpriteRenderer = tileObject.GetComponent<SpriteRenderer>();

                switch(this.scenarioGrid[h][w].elementChar)
                {
                    case '2':
                        this.scenarioGrid[h][w].elementSpriteRenderer.sprite = dotSprite;
                        break;
                    case '3':
                        tileObject.GetComponent<SpriteRenderer>().color = Color.green;
                        break;
                }
            }
        }
    }

    private void CheckPlayerMovimentBasedOnLocal()
    {
        float levelPositionX = scenarioMap.GetCenteredTilePositionByIndex(levelWidth, 0, distance);
        float levelPositionY = scenarioMap.GetCenteredTilePositionByIndex(levelHeight, 0, distance);

       
        Vector2 playerPositionGrid =
            playerMovimentation.GetPositionInGrid(new Vector2(levelPositionX, -levelPositionY),
                                              new Vector2(-levelPositionX, levelPositionY),
                                              new Vector2(levelWidth, levelHeight));

        ChangeMovementPermissionBasedOnLocal(playerPositionGrid);
        CheckCollectedPacDot(playerPositionGrid);
    }

    private void ChangeMovementPermissionBasedOnLocal(Vector2 characterPosition)
    {
        int _x = (int)characterPosition.x;
        int _y = (int)characterPosition.y;

        MovementPermission _movementPermission = playerMovimentation.GetMovementPermission();

        _movementPermission.SetMovePermission(true, true, true, true);

        if (_y != 0)
            _movementPermission.
                SetOneMovePermission(Direction.up, !scenarioGrid[_y - 1][_x].elementChar.Equals('1'));

        if (_y != levelHeight-1)
            _movementPermission.
                SetOneMovePermission(Direction.down, !scenarioGrid[_y + 1][_x].elementChar.Equals('1'));

        if (_x != 0)
            _movementPermission.
                SetOneMovePermission(Direction.left, !scenarioGrid[_y][_x-1].elementChar.Equals('1'));

        if (_x != levelWidth - 1)
            _movementPermission.
                SetOneMovePermission(Direction.right, !scenarioGrid[_y][_x+1].elementChar.Equals('1'));

    }

    private void CheckCollectedPacDot(Vector2 characterPosition)
    {

        int _x = (int)characterPosition.x;
        int _y = (int)characterPosition.y;

        if(scenarioGrid[_y][_x].elementChar.Equals('2') ||
            scenarioGrid[_y][_x].elementChar.Equals('3'))
        {
            scenarioGrid[_y][_x].elementSpriteRenderer.enabled = false;
        }
    }




}
