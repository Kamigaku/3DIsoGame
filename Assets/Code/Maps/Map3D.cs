using UnityEngine;
using System.Collections;
using Utility;
using System.Collections.Generic;
using Entity;
using GamePlay;

public class Map3D : MonoBehaviour {

    private int[,] _map;

    public List<GameObject> forest;
    public List<GameObject> ground;
    public GameObject emptySprite;
    public GameObject playerPrefab;
    public GameObject mobPrefab;

    private Dictionary<int, List<GameObject>> _groupPlayers = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> _groupMobs = new Dictionary<int, List<GameObject>>();
    private Node[] _nodes;

    private GameObject _mainPlayer;

    #region Getter / setters
    public Node[] nodes {
        get { return this._nodes; }
    }
    public int[,] map {
        get { return this._map; }
    }
    public GameObject mainPlayer {
        get { return this._mainPlayer; }
    }
    #endregion

    void Start() {
        this._map = new int[Constants.MAP_SIZE_X, Constants.MAP_SIZE_Y];

        #region Map generation
        GameObject terrain = new GameObject("Terrain");
        terrain.transform.position = new Vector3(0, 0);

        for (int y = 0; y < Constants.MAP_SIZE_Y; y++) {
            GameObject xGo = new GameObject();
            xGo.transform.position = new Vector3(0, 0);
            xGo.name = "Ligne " + y;
            xGo.transform.parent = terrain.transform;
            for (int x = 0; x < Constants.MAP_SIZE_X; x++) {
                GameObject go;
                #region Seed shit
                int seed = (int)(Random.value * 2);
                if (seed == 0) {
                    int value = (int)(Random.value * this.forest.Count);
                    go = forest[value];
                }
                else {
                    int value = (int)(Random.value * this.ground.Count);
                    go = ground[value];
                }
                #endregion
                Vector3 position = new Vector3();
                position.x = x * Constants.ESPACEMENT_CASE_X3D;
                position.y = 0;
                position.z = y * Constants.ESPACEMENT_CASE_Z3D;
                GameObject instantiatedGo = Instantiate(go, position, Quaternion.identity) as GameObject;
                instantiatedGo.transform.SetParent(xGo.transform);
                instantiatedGo.name = "" + position.x + "|" + position.z;
            }
        }
        #endregion

        initializeDijkstra();

        spawnMonster(3, 4, 2, "Gobelin");
        spawnPlayer(3, 3, 1, "Kamigaku", true);
    }


    #region Dijsktra Nodes handling
    private void initializeDijkstra() {
        this._nodes = new Node[this._map.GetLength(0) * this._map.GetLength(1)];
        for(int x = 0; x < this._map.GetLength(0); x++) {
            for(int y = 0; y < this._map.GetLength(1); y++) {
                /* Je créer un noeud et je rajoute les voisins qui peuvent être parcourus, à savoir les noeud == EMPTY_CASE */
                int value = XYValue(x, y);
                this._nodes[value] = new Node(value);
                if(x - 1 >= 0 && this._map[x - 1, y] == Constants.VALUE_EMPTY_CASE)
                    this._nodes[value].addNeighbors(XYValue(x - 1, y));
                if(x + 1 < Constants.MAP_SIZE_X && this._map[x + 1, y] == Constants.VALUE_EMPTY_CASE)
                    this._nodes[value].addNeighbors(XYValue(x + 1, y));
                if(y - 1 >= 0 && this._map[x, y - 1] == Constants.VALUE_EMPTY_CASE)
                    this._nodes[value].addNeighbors(XYValue(x, y - 1));
                if(y + 1 < Constants.MAP_SIZE_Y && this._map[x, y + 1] == Constants.VALUE_EMPTY_CASE)
                    this._nodes[value].addNeighbors(XYValue(x, y + 1));
            }
        }
    }

    public static int XYValue(int x, int y) {
        return x + (y * Constants.MAP_SIZE_X);
    }

    public static int XValue(int value) {
        return (int)(value % Constants.MAP_SIZE_X);
    }

    public static int YValue(int value) {
        return (int)(value / Constants.MAP_SIZE_X);
    }

    public void updateNode(int x, int y, int value) {
        /* Si c'est un noeud déjà existant et que
         *    => sa value == EMPTY_CASE et que le nouveau != EMPTY_CASE, je supprime le noeud ces voisins
         *    => sa value != EMPTY_CASE et que le nouveau == EMPTY_CASE je le rajoute dans le noeud voisin
        */
        int valueXY = XYValue(x, y);
        if(this._map[x, y] == Constants.VALUE_EMPTY_CASE) { // avant c'était une case vide
            if(value != Constants.VALUE_EMPTY_CASE) { // maintenant c'est une case avec une entité dessus, je le supprime de ces voisins
                for(int i = 0; i < this._nodes[valueXY].neighbors.Count; i++) {
                    this._nodes[this._nodes[valueXY].neighbors[i]].neighbors.Remove(valueXY);
                }
            }
        }
        else { // avant c'était une case avec une entité
            if(value == Constants.VALUE_EMPTY_CASE) { // maintenant c'est une vide, je le rajoute à ces voisins
                for(int i = 0; i < this._nodes[valueXY].neighbors.Count; i++) {
                    this._nodes[this._nodes[valueXY].neighbors[i]].addNeighbors(valueXY);
                }
            }
        }
        this._map[x, y] = value;
    }

    #endregion

    public void spawnPlayer(int x, int y, int group, string playerName, bool isMainPlayer = false) {
        Vector3 entityPosition = new Vector3(x, 2, y);
        this.updateNode(x, y, group);
        GameObject player = Instantiate(this.playerPrefab, entityPosition, Quaternion.identity) as GameObject;
        if (!this._groupPlayers.ContainsKey(group)) {
            this._groupPlayers.Add(group, new List<GameObject>());
        }
        player.AddComponent<Player>();
        player.GetComponent<AEntity>().entityName = playerName;
        player.GetComponent<AEntity>().groupId = group;
        player.GetComponent<AEntity>().x = x;
        player.GetComponent<AEntity>().y = y;
        this._groupPlayers[group].Add(player);
        if (isMainPlayer) {
            player.name = "MainPlayer";
            References.addReferences("MainPlayer");
            player.GetComponent<Player>().isMainPlayer = true;
            this._mainPlayer = player;
        }
    }

    public void spawnMonster(int x, int y, int group, string mobType) {
        Vector3 entityPosition = new Vector3(x, 2, y);
        this.updateNode(x, y, group);
        GameObject mob = Instantiate(this.mobPrefab, entityPosition, Quaternion.identity) as GameObject;
        if (!this._groupMobs.ContainsKey(group)) {
            this._groupMobs.Add(group, new List<GameObject>());
        }
        this._groupMobs[group].Add(mob);
        MobFactory.addComponentToMob(mob.transform.gameObject, mobType);
        mob.GetComponent<AEntity>().x = x;
        mob.GetComponent<AEntity>().y = y;
    }
}
