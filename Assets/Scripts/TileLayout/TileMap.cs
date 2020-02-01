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

}


