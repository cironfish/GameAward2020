using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public GameObject obj;

    public int m_MapX;
    public int m_MapY;

    private int[] m_Map;
    
    public int[] GetMap
    {
        get { return m_Map; }
    }

   public enum MapType
    {
        Non = 0,
        Hole,
        Floor,
        Wall,
        Goal,
        Max
    }

    int[,] num;

    // Vector2型からm_Map配列の添え字を算出
    public int GetMapIndex(Vector2 Position)
    {
        int index = m_MapX * (int)-Position.y + (int)Position.x;
        
        return index;
    }

    public int GetMapIndex(Vector3 Position)
    {
        int index = m_MapX * (int)-Position.y + (int)Position.x;

        return index;
    }

    // GameObject型からm_Map配列の添え字を算出
    public int GetMapIndex(GameObject o)
    {
        return GetMapIndex(o.transform.position);
    }



    void Start()
    {
        // 配列サイズ
        int mapNum = m_MapX * m_MapY;

        // マップ初期化
        m_Map = new int[mapNum];
        for (int i = 0; i < mapNum; i++)
            m_Map[i] = (int)MapType.Non;


        // タイルの取得
        Tilemap tileMap = GameObject.FindObjectOfType<Tilemap>();

        // タイル情報の一時保存先
        var spriteList = new List<Sprite>();
        List<Vector3> positionList = new List<Vector3>();

        var bound = tileMap.cellBounds;

        // マップチップからのデータを取得
        for (int y = bound.max.y - 1; y >= bound.min.y; --y) 
        {
            for (int x = bound.min.x; x < bound.max.x; ++x)
            {
                // タイル情報
                var tile = tileMap.GetTile<Tile>(new Vector3Int(x, y, 0));

                // 座標
                Vector3Int localPlace = (new Vector3Int(x, y, (int)tileMap.transform.position.y));
                Vector3 place = tileMap.CellToWorld(localPlace);

                if(tile != null && tileMap.HasTile(localPlace))
                {
                    spriteList.Add(tile.sprite);
                    place.y++;  // 調整
                    positionList.Add(place);
                }
            }
        }

        // データの書き込み
        for (int i = 0; i < spriteList.Count; i++)
        {
            // マップ範囲外だったら次のループへ
            if (GetMapIndex(positionList[i]) < 0 || mapNum - 1 < GetMapIndex(positionList[i]))
            {
                continue;
            }
            switch (spriteList[i].name.ToString())
            {
                case "block_147":
                    m_Map[GetMapIndex(positionList[i])] = (int)MapType.Wall;
                    break;

                case "block_460":
                    m_Map[GetMapIndex(positionList[i])] = (int)MapType.Hole;
                    break;

                case "block_43":
                    m_Map[GetMapIndex(positionList[i])] = (int)MapType.Floor;
                    break;

                case "block_459":
                    m_Map[GetMapIndex(positionList[i])] = (int)MapType.Goal;
                    break;
            }
        }




        // 穴の取得
        {
            GameObject[] holes = GameObject.FindGameObjectsWithTag("Hole");

            foreach(GameObject i in holes)
            {
                m_Map[GetMapIndex(i)] = (int)MapType.Hole;
            }
        }

        // 床の取得
        {
            GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");

            foreach(GameObject i in floors)
            {
                m_Map[GetMapIndex(i)] = (int)MapType.Floor;
            }
        }
    
        //// ゴールの取得
        //{
        //    GameObject goal = GameObject.FindGameObjectWithTag("Goal");

        //    m_Map[GetMapIndex((Vector2)goal.transform.position)] = (int)MapType.Goal;
        //}

        // 壁の取得
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

            foreach (GameObject i in walls)
            {
                // 仮。１は壁とする。
                m_Map[GetMapIndex((Vector2)i.transform.position)] = (int)MapType.Wall ;
            }
        }

        int breakpoint = 0;

    }

    void Update()
    {
        
    }
}
