using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grid where stuff grows, or things happen.
/// </summary>
public class TileMap : MonoBehaviour
{
    public Vector2 _mapSize;
    public Vector3 _origin;
    public TileCell _tilePrefab;

    private List<List<TileCell>> m_tileCells;

    private LayerMask m_tileLayer;

    #region Singleton
    public static TileMap Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            m_tileLayer = LayerMask.NameToLayer("Tile");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        m_tileCells = new List<List<TileCell>>();

        // Init the grid layout. 
        for (int i = 0; i < _mapSize.x; i++)
        {
            List<TileCell> row = new List<TileCell>();
            for (int j = 0; j < _mapSize.y; j++)
            {
                TileCell newCell = Instantiate<TileCell>(_tilePrefab, _origin + new Vector3(i, 0, j), Quaternion.identity, transform);
                row.Add(newCell);
            }
            m_tileCells.Add(row);
        }
    }

    public TileCell GetTileUnderPoint(Vector3 position)
    {
        TileCell cellUnderPoint = null;
        // Do a raycast from the position and get the tile that hit. Expensive? Maybe. Optimize later.
        RaycastHit hitInfo;
        if (Physics.Raycast(position, Vector3.down, out hitInfo, 10, m_tileLayer))
        {
            cellUnderPoint = hitInfo.collider.GetComponent<TileCell>();
        }
        return cellUnderPoint;
    }
}


