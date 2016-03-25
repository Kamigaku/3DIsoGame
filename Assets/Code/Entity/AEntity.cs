using UnityEngine;
using System.Collections.Generic;
using GamePlay;
using UnityEngine.UI;
using Utility;
using System;
using HUD;
using System.Threading;
using Maps;

namespace Entity {

	public abstract class AEntity : MonoBehaviour, IEntity {

        public int x;
        public int y;

        public String entityName;
        public int groupId;

		protected EntityStatistique _statistique;
        protected bool _isMainPlayer = false;
        protected Map3D _map;
        private bool _canPlay = false;

        private List<Vector2> _path;

        // La valeur est changé, cela signifie qu'il faut réactuliser les différentes valeurs
        public bool canPlay {
            get { return this._canPlay; }
            set {
                OnMouseExit();
                this._statistique.newTurn();
                this._path = IA.Dijkstra.allPathAtRange(this.x, this.y, this._statistique.actionPoint, this._map.nodes);
                this._canPlay = value;
            }
        }
        public bool isMainPlayer {
            set { this._isMainPlayer = value; }
        }
        public EntityStatistique statistique {
            get { return this._statistique; }
        }

        protected void baseLoad() {
            this._map = References.getReferences("Map", true).GetComponent<Map3D>();
            this._path = new List<Vector2>();
            this._path = IA.Dijkstra.allPathAtRange((int)this.transform.position.x, (int)this.transform.position.z, this.statistique.actionPoint, this._map.nodes);
        }

        public void affectEntity(BaseStats statsConcerned, int amount) {
            this._statistique.addBonus(statsConcerned, amount);
        }


        // Lorsque je clique sur une case, je bouge mon entité
        // Si la position que j'ai cliqué est trop eloigné par rapport au nombre de point d'actions qu'il me reste
        // je ne peux pas bouger. Sinon je me déplace et réduit mon nombre de points d'action.
        public void moveEntity(int x, int y) {
            if(canMoveTo(x, y)) {
                Debug.Log((Math.Abs(x - this.x) + Math.Abs(y - this.y)) + " | " + this.statistique.actionPoint);
                this._statistique.actionPoint -= (Math.Abs(x - this.x) + Math.Abs(y - this.y));
                this._map.updateNode(this.x, this.y, Constants.VALUE_EMPTY_CASE);
                this._map.updateNode(x, y, this.groupId);
                Vector3 newPosition = new Vector3(x, 2, y);
                this.x = x;
                this.y = y;
                this.transform.position = newPosition;
                this._path = IA.Dijkstra.allPathAtRange(this.x, this.y, this.statistique.actionPoint, this._map.nodes); // J'actualise le chemin en fonction des nouveaux points
            }
        }

        // Est-ce que le chemin le plus court pour aller à ma destination n'est pas trop eloigné par rapport
        // à mon nombre de point d'actions restant
        private bool canMoveTo(int x, int y) {
            List<Vector2> targetPath = IA.Dijkstra.shortestPath(this.x, this.y, x, y, this._map.nodes, this._map.map);
            if(this._path.Count > this._statistique.actionPoint)
                return false;
            return true;
        }

        public void OnMouseOver() {
            for(int i = 0; i < this._path.Count; i++) {
                GameObject.Find(_path[i].x + "|" + _path[i].y).GetComponent<Tile>().changeMaterialColor(Color.blue);
            }
        }

        public void OnMouseExit() {
            for(int i = 0; i < _path.Count; i++) {
                GameObject.Find(_path[i].x + "|" + _path[i].y).GetComponent<Tile>().changeMaterialColorToDefault();
            }
        }

        public void addSkill(string skillName) {
            throw new NotImplementedException();
        }

        public void playSkill(string skillName) {
            throw new NotImplementedException();
        }

    }

}