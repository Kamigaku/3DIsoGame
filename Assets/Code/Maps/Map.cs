using Entity;
using GamePlay;
using Mechanics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace Maps {
    public class Map : MonoBehaviour {

        private int[,] _map;

        public List<Sprite> forest;
        public List<Sprite> ground;
        public GameObject emptySprite;
        public GameObject playerPrefab;
        public GameObject mobPrefab;

        private Dictionary<int, List<GameObject>> _groupPlayers = new Dictionary<int, List<GameObject>>();
        private Dictionary<int, List<GameObject>> _groupMobs = new Dictionary<int, List<GameObject>>();

        private Node[] _nodes;

        public Node[] nodes
        {
            get { return this._nodes; }
        }

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
                    Sprite sprite = null;
                    GameObject go;
                    #region Seed shit
                    int seed = (int)(Random.value * 2);
                    if (seed == 0) {
                        int value = (int)(Random.value * this.forest.Count);
                        sprite = forest[value];
                    }
                    else {
                        int value = (int)(Random.value * this.ground.Count);
                        sprite = ground[value];
                    }
                    #endregion
                    Vector2 position = new Vector2();
                    position.x = (x * (Constants.ESPACEMENT_CASE_X2D / 2)) - (y * Constants.ESPACEMENT_CASE_X2D / 2);
                    position.y = (x * (Constants.ESPACEMENT_CASE_Y2D / 2)) + (y * Constants.ESPACEMENT_CASE_Y2D / 2);
                    go = Instantiate(emptySprite, position, Quaternion.identity) as GameObject;
                    go.GetComponent<SpriteRenderer>().sprite = sprite;
                    go.transform.SetParent(xGo.transform);
                }
            }
            #endregion

            initializeDijkstra();

            spawnPlayer(3, 3, 1, "Kamigaku", true);
            spawnMonster(6, 6, 2, "Gobelin");
            spawnMonster(6, 5, 2, "Gobelin");
            spawnMonster(6, 7, 2, "Gobelin");

            //ScriptableObject.CreateInstance<Fight>();
        }

        private void initializeDijkstra() {
            this._nodes = new Node[this._map.GetLength(0) * this._map.GetLength(1)];
            for (int x = 0; x < this._map.GetLength(0); x++) {
                for (int y = 0; y < this._map.GetLength(1); y++) {
                    if (this._map[x, y] == Constants.VALUE_EMPTY_CASE) {

                        // Ajout du neoud
                        int value = XYValue(x, y);
                        this._nodes[value] = new Node(value);

                        // Ajout des voisins avec vérification
                        if (x - 1 >= 0 && this._map[x - 1, y] == Constants.VALUE_EMPTY_CASE)
                            this._nodes[value].addNeighbors(XYValue(x - 1, y));
                        if (x + 1 < Constants.MAP_SIZE_X && this._map[x + 1, y] == Constants.VALUE_EMPTY_CASE)
                            this._nodes[value].addNeighbors(XYValue(x + 1, y));
                        if (y - 1 >= 0 && this._map[x, y - 1] == Constants.VALUE_EMPTY_CASE)
                            this._nodes[value].addNeighbors(XYValue(x, y - 1));
                        if (y + 1 < Constants.MAP_SIZE_Y && this._map[x, y + 1] == Constants.VALUE_EMPTY_CASE)
                            this._nodes[value].addNeighbors(XYValue(x, y + 1));
                    }
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

        public void updateMap(int oldX, int oldY, int newX, int newY, int value) {
            this._map[newX, newY] = value;
            this._map[oldX, oldY] = Constants.VALUE_EMPTY_CASE;
            int valueXY_old = XYValue(oldX, oldY);
            int valueXY_new = XYValue(newX, newY);
            List<int> neigbors = this.nodes[valueXY_old].neighbors;
            for(int i = 0; i < neigbors.Count; i++) {
                this.nodes[neigbors[i]].neighbors.Remove(valueXY_old);
            }
        }

        public void addNode(int x, int y, int value) { // Je rajoute un noeud
            this._map[x, y] = value;
            int valueXY = XYValue(x, y);
            this._nodes[valueXY] = new Node(valueXY);
            // Je parcours les noeuds à proximités pour tester si ils sont valide, en cas de validité
            // je les rajoutent dans le noeud courant, il faut aussi les ajouter dans le voisin si il n'existait pas.
            if(x - 1 >= 0 && this._map[x - 1, y] == Constants.VALUE_EMPTY_CASE) {
                this._nodes[value].addNeighbors(XYValue(x - 1, y));
                this._nodes[XYValue(x - 1, y)].addNeighbors(value);
            }
            if(x + 1 < Constants.MAP_SIZE_X && this._map[x + 1, y] == Constants.VALUE_EMPTY_CASE) {
                this._nodes[value].addNeighbors(XYValue(x + 1, y));
                this._nodes[XYValue(x + 1, y)].addNeighbors(value);
            }
            if(y - 1 >= 0 && this._map[x, y - 1] == Constants.VALUE_EMPTY_CASE) {
                this._nodes[value].addNeighbors(XYValue(x, y - 1));
                this._nodes[XYValue(x, y - 1)].addNeighbors(value);
            }
            if(y + 1 < Constants.MAP_SIZE_Y && this._map[x, y + 1] == Constants.VALUE_EMPTY_CASE) {
                this._nodes[value].addNeighbors(XYValue(x, y + 1));
                this._nodes[XYValue(x, y + 1)].addNeighbors(value);
            }
        }

        public void removeNode(int x, int y, int value) { // Un noeud est supprimé, cela signfie que sur la map c'est un bloc que l'on ne peux dépasser (un mur, un rocher, etc...)
            Debug.Log("Remove : " + x + " | " + y);
            this._map[x, y] = value;
            int valueXY = XYValue(x, y);
            List<int> neigbors = this.nodes[valueXY].neighbors;
            for(int i = 0; i < neigbors.Count; i++) {
                this.nodes[neigbors[i]].neighbors.Remove(valueXY);
            }
            this.nodes[valueXY] = null;
        }

        public void spawnPlayer(int x, int y, int group, string playerName, bool isMainPlayer = false) {
            Vector2 entityPosition = EntityUtility.mapToScreen(new Vector2(x, y));
            entityPosition.y += 0.75f;
            GameObject player = Instantiate(this.playerPrefab, entityPosition, Quaternion.identity) as GameObject;
            if (!this._groupPlayers.ContainsKey(group)) {
                this._groupPlayers.Add(group, new List<GameObject>());
            }
            player.AddComponent<Player>();
            player.GetComponentInChildren<AEntity>().entityName = playerName;
            player.GetComponentInChildren<AEntity>().groupId = group;
            player.GetComponentInChildren<AEntity>().x = x;
            player.GetComponentInChildren<AEntity>().y = y;
            this._groupPlayers[group].Add(player);
            if (isMainPlayer) {
                player.name = "MainPlayer";
                References.addReferences("MainPlayer");
                player.GetComponentInChildren<Player>().isMainPlayer = true;
            }
            this.addNode(x, y, group);
        }

        public void spawnMonster(int x, int y, int group, string mobType) {
            Vector2 entityPosition = EntityUtility.mapToScreen(new Vector2(x, y));
            entityPosition.y += 0.75f;
            GameObject mob = Instantiate(this.mobPrefab, entityPosition, Quaternion.identity) as GameObject;
            if (!this._groupMobs.ContainsKey(group)) {
                this._groupMobs.Add(group, new List<GameObject>());
            }
            this._groupMobs[group].Add(mob);
            MobFactory.addComponentToMob(mob.transform.gameObject, mobType);
            mob.GetComponentInChildren<AEntity>().groupId = group;
            mob.GetComponentInChildren<AEntity>().x = x;
            mob.GetComponentInChildren<AEntity>().y = y;
            this.addNode(x, y, group);
        }
    }
}
