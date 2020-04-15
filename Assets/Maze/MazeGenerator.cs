
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{

    #region Variables:

    public int mazeRows;

    public int mazeColumns;

    [SerializeField]
    private GameObject cellPrefab;   
    public bool disableCellSprite;

    // ------------------------------------------------------
    // System defined variables - You don't need to touch these:
    // ------------------------------------------------------

    // Variable to store size of centre room. Hard coded to be 2.
    private int centreSize = 2;

    private Dictionary<Vector2, Cell> allCells = new Dictionary<Vector2, Cell>();
    public List<Cell> unvisited = new List<Cell>();
    public List<Cell> stack = new List<Cell>();

    private Cell[] centreCells = new Cell[4];

    private Cell currentCell;
    private Cell checkCell;

    private Vector2[] neighbourPositions = new Vector2[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };

    private float cellSize;

    private GameObject mazeParent;
    #endregion
    Cell target;
    Cell first;
    public Cell[,] a = new Cell[100, 100];
    public Sprite bug;
    public Sprite redDot;
    public Text level;
    public GameObject nextLevel;
    List<Cell> pathCell = new List<Cell>();
    List<int> path = new List<int>();
    bool check = false;
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
    private void Awake()
    {
        float aspect = (float)Screen.height / (float)Screen.width; // Portrait
        if (aspect >= 2)
        {
            Camera.main.orthographicSize = 18;
        }
        else if (aspect >= 1.74)  // 16:9
        {
            Camera.main.orthographicSize = 15;
        }
        else
        { // 4:3
            Camera.main.orthographicSize = 15;
        }
    }
    private void Start()
    {
        level.text = "Lv. " + (DIContainer.GetModule<IMapManager>().CUR_MAP.ID + 1);
        cellList = new List<Cell>();
        string key = level.text;
        int m = 0;
        GenerateMaze(mazeRows, mazeColumns);
        foreach (KeyValuePair<Vector2, Cell> ele1 in allCells)
        {
            ele1.Value.ID = m;
            m++;
            cellList.Add(ele1.Value);

        }
        for (int i = 0; i < mazeColumns; i++)
        {
            for (int j = 0; j < mazeRows; j++)
            {
                a[i, j] = cellList[i * mazeRows + j];
            }
        }
        if (PlayerPrefs.GetString(key, "") == "")
        {
            int range = Random.Range(50, 129);
            target = cellList[range];
            target.cScript.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = redDot;
            target.cScript.transform.GetChild(0).gameObject.SetActive(true);
            List<Data> cellScript = new List<Data>();
            for(int i = 0; i < cellList.Count; i++)
            {
                Data data = new Data(cellList[i].cScript.LeftWall, cellList[i].cScript.RightWall, cellList[i].cScript.UpWall, cellList[i].cScript.DWall);
                cellScript.Add(data);
            }
            DataSave dataSave = new DataSave(cellScript, range);
            PlayerPrefs.SetString(key, JsonUtility.ToJson(dataSave));
        }
        else
        {
            DataSave dataSave = JsonUtility.FromJson<DataSave>(PlayerPrefs.GetString(key, ""));
            for (int i = 0; i < cellList.Count; i++)
            {
                cellList[i].cScript.DWall = dataSave.cells[i].down;
                cellList[i].cScript.LeftWall = dataSave.cells[i].left;
                cellList[i].cScript.UpWall = dataSave.cells[i].up;
                cellList[i].cScript.RightWall = dataSave.cells[i].right;
            }
            target = cellList[dataSave.target];
            target.cScript.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = redDot;
            target.cScript.transform.GetChild(0).gameObject.SetActive(true);
        }
        first = cellList[0];
        first.cScript.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bug;
        first.cScript.transform.GetChild(0).gameObject.SetActive(true);
        mazeParent.transform.position = new Vector2(0, -1);
        mazeParent.transform.Rotate(new Vector3(0, 0, 270));
        AStar(first, target);
        // List<Cell> path = AStar(first, target);
    }
    public void Replay()
    {
        SceneManager.LoadScene(0);
    }
    public void AStar()
    {
        DrawPath(target, path);
    }
    public void AutoMove()
    {
        if (!check)
        {
            check = true;
            MapDataStat cur = DIContainer.GetModule<IMapManager>().CUR_MAP;
            cur.STAR = 3;
            DIContainer.GetModule<IMapManager>().SetupNextLevel(cur);
            StartCoroutine(_AutoMove());
        }
    }
    IEnumerator _AutoMove()
    {
        for (int i = pathCell.Count - 1; i >= 0; i--)
        {
            Tween move = first.cScript.transform.GetChild(0).transform.DOMove(pathCell[i].cellObject.transform.position, 0.1f);
            yield return move.WaitForCompletion();
        }
        nextLevel.SetActive(true);
        yield return new WaitForSeconds(1f); //move.WaitForCompletion();
        SceneManager.LoadScene(1);
    }
    public List<Cell> cellList;
    public List<Cell> FindPoints(int ID)
    {
        List<Cell> listPoint = new List<Cell>();
        for (int i = 0; i < mazeColumns; i++)
        {
            for (int j = 0; j < mazeRows; j++)
            {
                if (ID == a[i, j].ID)
                {
                    if (!a[i, j].cScript.LeftWall)
                    {
                        if (i - 1 >= 0)
                            listPoint.Add(a[i - 1, j]);
                    }
                    if (!a[i, j].cScript.RightWall)
                    {
                        if (i + 1 < mazeColumns)
                            listPoint.Add(a[i + 1, j]);
                    }
                    if (!a[i, j].cScript.UpWall)
                    {
                        if (j + 1 < mazeRows)
                            listPoint.Add(a[i, j + 1]);
                    }
                    if (!a[i, j].cScript.DWall)
                    {
                        if (j - 1 >= 0)
                            listPoint.Add(a[i, j - 1]);
                    }
                    return listPoint;
                }
            }
        }
        return null;
    }
    List<int> AStar(Cell start, Cell target)
    {
        //Debug.Log(target);
        if (target == null)
            return null;
        List<Cell> Open = new List<Cell>();
        List<Cell> Close = new List<Cell>();
        List<Cell> paths = new List<Cell>();
        List<int> path = new List<int>();
        Open.Clear();
        Close.Clear();
        start.G = 0;
        start.H = Mathf.Abs(start.gridPos.x - target.gridPos.x) + Mathf.Abs(start.gridPos.y - target.gridPos.y);
        start.F = start.G + start.H;
        Open.Add(start);
        while (Open.Count != 0)
        {
            Cell currentCell = Open[0];
            for (int i = 0; i < Open.Count; i++)
            {
                if (Open[i].F < currentCell.F)
                {
                    currentCell = Open[i];
                }
            }
            Open.Remove(currentCell);
            Close.Add(currentCell);
            if (currentCell.ID == target.ID)
            {
                Open.Clear();
                Close.Clear();
                return repath(target, path); //viet sau
            }
            else
            {

                List<Cell> nearCur = FindPoints(currentCell.ID);
                for (int i = 0; i < nearCur.Count; i++)
                {
                    Cell cellIndex = nearCur[i];
                    if (Close.Contains(cellIndex))
                    {
                        continue;
                    }
                    // Debug.Log(currentCell);
                    //Debug.Log(cellIndex);
                    float tmp_current_g = currentCell.G + Mathf.Abs(currentCell.gridPos.x - cellIndex.gridPos.x) + Mathf.Abs(currentCell.gridPos.y - cellIndex.gridPos.y);
                    if (!Open.Contains(cellIndex) || tmp_current_g < cellIndex.G)
                    {
                        cellIndex.PASS = currentCell;
                        cellIndex.G = tmp_current_g;
                        cellIndex.H = Mathf.Abs(cellIndex.gridPos.x - target.gridPos.x) + Mathf.Abs(cellIndex.gridPos.y - target.gridPos.y);
                        cellIndex.F = cellIndex.G + cellIndex.H;
                        if (!Open.Contains(cellIndex))
                        {
                            Open.Add(cellIndex);
                            // cellIndex.cScript.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                    }
                }
            }
        }
        return path;
    }
    public List<int> repath(Cell start, List<int> path)
    {
        Cell _start = start;
        pathCell.Clear();
        path.Clear();
        while (_start != null)
        {
            if (_start.PASS == null) return path;
            //if (_start.ID != target.ID)
            //{
            //    _start.cScript.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            //    _start.cScript.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            //}
            path.Add(_start.ID);
            pathCell.Add(_start);
            _start = _start.PASS;
        }
        return path;
    }
    public void DrawPath(Cell start, List<int> path)
    {
        Cell _start = start;
        Cell prev = null;
        while (_start != null)
        {
            if (_start.ID != target.ID && prev != null)
            {
                if (_start.gridPos.x - prev.gridPos.x == 0)
                {
                    if (_start.gridPos.y - prev.gridPos.y < 0)
                    {
                        // 0 left, 1 right, 2 up 3 down
                        prev.cScript.DIRECT = 3;
                    }
                    else
                    {
                        prev.cScript.DIRECT = 2;
                    }
                }
                else if (_start.gridPos.y - prev.gridPos.y == 0)
                {
                    if (_start.gridPos.x - prev.gridPos.x < 0)
                    {
                        prev.cScript.DIRECT = 0;
                    }
                    else
                    {
                        prev.cScript.DIRECT = 1;
                    }
                }
            }
            prev = _start;
            path.Add(_start.ID);
            _start = _start.PASS;
        }
    }
    private void GenerateMaze(int rows, int columns)
    {
        if (mazeParent != null) DeleteMaze();

        mazeRows = rows;
        mazeColumns = columns;
        CreateLayout();
    }

    public void CreateLayout()
    {
        InitValues();

        Vector2 startPos = new Vector2(-(cellSize * (mazeColumns / 2)) + (cellSize / 2), -(cellSize * (mazeRows / 2)) + (cellSize / 2));
        Vector2 spawnPos = startPos;

        for (int x = 1; x <= mazeColumns; x++)
        {
            for (int y = 1; y <= mazeRows; y++)
            {
                GenerateCell(spawnPos, new Vector2(x, y));
                spawnPos.y += cellSize;
            }

            spawnPos.y = startPos.y;
            spawnPos.x += cellSize;
        }

        CreateCentre();
        RunAlgorithm();
        MakeExit();

    }

    public void RunAlgorithm()
    {
        unvisited.Remove(currentCell);

        while (unvisited.Count > 0)
        {
            List<Cell> unvisitedNeighbours = GetUnvisitedNeighbours(currentCell);
            if (unvisitedNeighbours.Count > 0)
            {
                checkCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                stack.Add(currentCell);

                CompareWalls(currentCell, checkCell);
                currentCell = checkCell;
                unvisited.Remove(currentCell);
            }
            else if (stack.Count > 0)
            {
                currentCell = stack[stack.Count - 1];
                stack.Remove(currentCell);
            }
        }
    }

    public void MakeExit()
    {
        List<Cell> edgeCells = new List<Cell>();

        foreach (KeyValuePair<Vector2, Cell> cell in allCells)
        {
            if (cell.Key.x == 0 || cell.Key.x == mazeColumns || cell.Key.y == 0 || cell.Key.y == mazeRows)
            {
                edgeCells.Add(cell.Value);
            }
        }

        Cell newCell = edgeCells[Random.Range(0, edgeCells.Count)];

        if (newCell.gridPos.x == 0) RemoveWall(newCell.cScript, 1);
        else if (newCell.gridPos.x == mazeColumns) RemoveWall(newCell.cScript, 2);
        else if (newCell.gridPos.y == mazeRows) RemoveWall(newCell.cScript, 3);
        else RemoveWall(newCell.cScript, 4);

        Debug.Log("Maze generation finished.");
    }

    public List<Cell> GetUnvisitedNeighbours(Cell curCell)
    {

        List<Cell> neighbours = new List<Cell>();

        Cell nCell = curCell;
 
        Vector2 cPos = curCell.gridPos;

        foreach (Vector2 p in neighbourPositions)
        {

            Vector2 nPos = cPos + p;

            if (allCells.ContainsKey(nPos)) nCell = allCells[nPos];
    
            if (unvisited.Contains(nCell)) neighbours.Add(nCell);
        }

        return neighbours;
    }


    public void CompareWalls(Cell cCell, Cell nCell)
    {

        if (nCell.gridPos.x < cCell.gridPos.x)
        {
            RemoveWall(nCell.cScript, 2);
            RemoveWall(cCell.cScript, 1);
        }

        else if (nCell.gridPos.x > cCell.gridPos.x)
        {
            RemoveWall(nCell.cScript, 1);
            RemoveWall(cCell.cScript, 2);
        }

        else if (nCell.gridPos.y > cCell.gridPos.y)
        {
            RemoveWall(nCell.cScript, 4);
            RemoveWall(cCell.cScript, 3);
        }

        else if (nCell.gridPos.y < cCell.gridPos.y)
        {
            RemoveWall(nCell.cScript, 3);
            RemoveWall(cCell.cScript, 4);
        }
    }

    public void RemoveWall(CellScript cScript, int wallID)
    {
        if (wallID == 1) cScript.wallL.SetActive(false);
        else if (wallID == 2) cScript.wallR.SetActive(false);
        else if (wallID == 3) cScript.wallU.SetActive(false);
        else if (wallID == 4) cScript.wallD.SetActive(false);
    }

    public void CreateCentre()
    {
        centreCells[0] = allCells[new Vector2((mazeColumns / 2), (mazeRows / 2) + 1)];
        RemoveWall(centreCells[0].cScript, 4);
        RemoveWall(centreCells[0].cScript, 2);
        centreCells[1] = allCells[new Vector2((mazeColumns / 2) + 1, (mazeRows / 2) + 1)];
        RemoveWall(centreCells[1].cScript, 4);
        RemoveWall(centreCells[1].cScript, 1);
        centreCells[2] = allCells[new Vector2((mazeColumns / 2), (mazeRows / 2))];
        RemoveWall(centreCells[2].cScript, 3);
        RemoveWall(centreCells[2].cScript, 2);
        centreCells[3] = allCells[new Vector2((mazeColumns / 2) + 1, (mazeRows / 2))];
        RemoveWall(centreCells[3].cScript, 3);
        RemoveWall(centreCells[3].cScript, 1);

        List<int> rndList = new List<int> { 0, 1, 2, 3 };
        int startCell = rndList[Random.Range(0, rndList.Count)];
        rndList.Remove(startCell);
        currentCell = centreCells[startCell];
        foreach (int c in rndList)
        {
            unvisited.Remove(centreCells[c]);
        }
    }

    public void GenerateCell(Vector2 pos, Vector2 keyPos)
    {
        Cell newCell = new Cell();

        newCell.gridPos = keyPos;
        newCell.cellObject = Instantiate(cellPrefab, pos, cellPrefab.transform.rotation);
        if (mazeParent != null) newCell.cellObject.transform.parent = mazeParent.transform;

        newCell.cellObject.name = "Cell - X:" + keyPos.x + " Y:" + keyPos.y;
        Debug.Log("Pos - X:" + pos.x + " Y:" + pos.y);

        newCell.cScript = newCell.cellObject.GetComponent<CellScript>();

        if (disableCellSprite) newCell.cellObject.GetComponent<SpriteRenderer>().enabled = false;


        allCells[keyPos] = newCell;
        unvisited.Add(newCell);
    }

    public void DeleteMaze()
    {
        if (mazeParent != null) Destroy(mazeParent);
    }

    public void InitValues()
    {


        if (mazeRows <= 3) mazeRows = 4;
        if (mazeColumns <= 3) mazeColumns = 4;

        cellSize = 1.5f;


        mazeParent = new GameObject();
        mazeParent.transform.position = Vector2.zero;
        mazeParent.name = "Maze";
    }

    public bool IsOdd(int value)
    {
        return value % 2 != 0;
    }
    [System.Serializable]
    public class Cell
    {
        public Vector2 gridPos;
        public GameObject cellObject;
        public CellScript cScript;
        int id;
        float g;
        float f;
        public int ID
        {
            set
            {
                id = value;
            }
            get
            {
                return id;
            }
        }

        public float G
        {
            set
            {
                g = value;
            }
            get
            {
                return g;
            }
        }
        public float F
        {
            set
            {
                f = value;
            }
            get
            {
                return f;
            }
        }
        float h;
        public float H
        {
            set
            {
                h = value;
            }
            get
            {
                return h;
            }
        }
        Cell pass;
        public Cell PASS
        {
            set
            {
                pass = value;
            }
            get
            {
                return pass;
            }
        }
        //public int DAD
        //{
        //    set
        //    {
        //        DAD = value;
        //    }
        //    get
        //    {
        //        return DAD;
        //    }
        //}
    }
    [System.Serializable]
    public class DataSave
    {
        public List<Data> cells;
        public int target;
        public DataSave(List<Data> cells, int target)
        {
            this.cells = cells;
            this.target = target;
        }
    }
    [System.Serializable]
    public class Data
    {
        public bool left;
        public bool right;
        public bool up;
        public bool down;

        public Data(bool left, bool right, bool up, bool down)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
        }
    }
}


