using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSpawn : MonoBehaviour
{
    [Header("Properties")]
    public GameObject cell;
    public Vector2 gridSize;
    public GameObject [,] cellGrid;
    public Transform parent;
    public float cellChanceOfLive;
    public bool changeStates;
    public bool starmanMode;
    public bool useRandomSeed;
    public Vector2 randSeed;
    public int roundsToDo;
    public float secondsBtwRounds;
    public Camera wtfCamara;
    public Color[] colors;

    [Header("UI")]
    public Text generationText;
    public Text aliveCellsText;
    public Text deadCellsText;
    public Text totalCellsText;
    public Text startText;
    public Text probOfAliveText;
    public Text simulationSpeedText;
    public Text roundsToDoText;
    public Slider probOfAlive;
    public Slider simulationSpeed;
    public Slider roundsToDoSlider;
    public InputField seedField;

    int cellID;
    int currentColorID;
    int currentRounds;
    int seed;
    bool played;
    float currentSecondsBtwRounds;
    float currentTimeBtwCells;

    public void Awake()
    {
        Screen.SetResolution(1920, 1080, true);

        GenerateSeed();
        GenerateGrid();
        UI();

        simulationSpeed.minValue = 0;
        simulationSpeed.maxValue = secondsBtwRounds;
    }

    void Update()
    {   
        if (changeStates)
        {
            UpdateCells();
            
            if (starmanMode)
            {
                if (!played)
                {
                    GetComponent<AudioSource>().Play();
                    played = true;
                    wtfCamara.backgroundColor = GetCustomColor(0f, 0f, 0f);
                }
            }
        }

        else
        {
            if (starmanMode)
            {
                GetComponent<AudioSource>().Stop();
                played = false;
                wtfCamara.backgroundColor = GetCustomColor(137f, 137f, 137f);
            }

            else
                wtfCamara.backgroundColor = GetCustomColor(137f, 137f, 137f);
        }

        UI();
    }

    public void ChangeSimulationSpeed(float value)
    {
        simulationSpeed.value = value;
        secondsBtwRounds = value;
    }

    public void ChangeCellProbability(float value)
    {
        probOfAlive.value = value;
        cellChanceOfLive = value;
    }

    public void RoundsToDoChange(float value)
    {
        roundsToDoSlider.value = value;
        roundsToDo = (int)value;
    }

    void UI()
    {
        totalCellsText.text = "Total: " + (gridSize.x * gridSize.y).ToString();
        generationText.text = "Generation: " + currentRounds.ToString();
        simulationSpeedText.text = secondsBtwRounds.ToString();
        probOfAliveText.text = (Mathf.Abs(cellChanceOfLive - 1f)).ToString();
        roundsToDoText.text = roundsToDo.ToString();
    }

    public void TriggerStartStop()
    {
        if (currentRounds >= roundsToDo)
            RegenerateGrid();

        changeStates = !changeStates;

        if (changeStates)
            startText.text = "Stop";
        else
            startText.text = "Play";
    }

    public void SetAllAlive()
    {
        foreach (GameObject cell in cellGrid)
            cell.GetComponent<Cell>().ReviveCell();
    }

    public void SetAllDead()
    {
        foreach (GameObject cell in cellGrid)
            cell.GetComponent<Cell>().KillCell();
    }

    public void InvertAll()
    {
        foreach (GameObject cell in cellGrid)
            if (cell.GetComponent<Cell>().isAlive)
                cell.GetComponent<Cell>().KillCell();
            else
                cell.GetComponent<Cell>().ReviveCell();
    }

    public void RegenerateGrid()
    {
        foreach(GameObject cell in cellGrid)
            Destroy(cell.gameObject);
        
        cellID = 0;
        changeStates = false;
        currentRounds = 0;

        GenerateGrid();
    }

    public void GenerateNewGrid()
    {
        RegenerateSeed();
        RegenerateGrid();
    }

    void UpdateCells()
    {   
        if (currentRounds < roundsToDo)
        {
            if (currentSecondsBtwRounds <= 0)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {
                        int aliveNeighbors = 0;
                        
                        for (int i = 0; i < cellGrid[x, y].GetComponent<Cell>().aliveNeighbors.Length; i++)
                            if (cellGrid[x, y].GetComponent<Cell>().aliveNeighbors[i])
                                aliveNeighbors++;

                        //Debug.Log(cellGrid[x, y].name + " has " + aliveNeighbors + " alive neighbors and is alive?: " + cellGrid[x, y].GetComponent<Cell>().isAlive);

                        //Current Cell is Alive
                        if (cellGrid[x, y].GetComponent<Cell>().isAlive)
                        {
                            if (aliveNeighbors < 2)
                                cellGrid[x, y].GetComponent<Cell>().KillCell();
                            
                            else if (aliveNeighbors > 3)
                                cellGrid[x, y].GetComponent<Cell>().KillCell();
                        }
                        //Current Cell is NOT Alive
                        else
                        {
                            if (aliveNeighbors == 3)
                                cellGrid[x, y].GetComponent<Cell>().ReviveCell();
                        }

                        if (cellGrid[x, y].GetComponent<Cell>().isAlive && starmanMode)
                            ChangeNextColor(cellGrid[x, y].GetComponent<Cell>().renderers[1], currentColorID);
                    }
                }

                currentRounds++;
                currentSecondsBtwRounds = secondsBtwRounds;
            }

            else
                currentSecondsBtwRounds -= Time.deltaTime;
        }

        else
            changeStates = false;
    }

    void ChangeNextColor(SpriteRenderer color, int index)
    {
        if (currentColorID > colors.Length - 1)
            currentColorID = 0;
        
        color.color = colors[currentColorID];
        currentColorID++;
    }

    void ChangeColor(SpriteRenderer sprite, Color color)
    {
        sprite.color = color;
    }

    public void GenerateSeed()
    {
        if (seedField.text == "" || seedField.text == null)
        {
            seed = (int)Random.Range(randSeed.x, randSeed.y);
            seedField.text = seed.ToString();
        }

        else
            seed = int.Parse(seedField.text);
        
        seedField.text = seed.ToString();
    }

    public void RegenerateSeed()
    {
        seed = (int)Random.Range(randSeed.x, randSeed.y);
        seedField.text = seed.ToString();
    }

    public void SecretMode()
    {
        starmanMode = !starmanMode;
    }

    public void ChangeSeedText(string _seed)
    {   
        if (_seed != "" || _seed != null)
        {
            seedField.text = _seed;
            seed = int.Parse(_seed);
        }
    }

    void GenerateGrid()
    {
        Random.InitState(seed);

        cellGrid = new GameObject[(int)gridSize.x, (int)gridSize.y];

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                cellGrid[i, j] = (GameObject)Instantiate(cell, new Vector3(i, j, 0), Quaternion.identity);
                cellGrid[i, j].GetComponent<Cell>().isAlive = cellGrid[i, j].GetComponent<Cell>().SetState(true, cellChanceOfLive);
                cellGrid[i, j].name = cellGrid[i, j].name + cellID.ToString();
                cellGrid[i, j].transform.parent = parent;
                cellID++;
            }
        }

        Random.State gridState = Random.state;
    }

    Color GetCustomColor(float r, float g, float b, float a = 1f)
    {
        return new Color(r / 255f, g / 255f, b / 255f, a);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(Input.mousePosition, Vector2.up * 100f);
    }
}
