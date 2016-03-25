using Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace HUD {
    public class ButtonMove : MonoBehaviour {


        private List<Vector2> _path = new List<Vector2>();
        private Map3D _map;
        private Fight _fight;
        private bool _isActivated = false;

        public void Update() {
            if(Input.GetMouseButtonDown(1)) {
                this._isActivated = false;
                PointerExit();
            }
            if(this._isActivated)
                displayHighlighted();
        }

        public void PointerDown() {
            this._isActivated = true;
        }

        public void PointerEnter() {
            if(!_isActivated) {
                if(this._map == null)
                    this._map = References.getReferences("Map", true).GetComponent<Map3D>();
                if(this._fight == null)
                    this._fight = References.getReferences("Fight", true).GetComponent<Fight>();
                _path = IA.Dijkstra.allPathAtRange((int)this._fight.currentPlayer.transform.position.x, (int)this._fight.currentPlayer.transform.position.z, 
                                                    this._fight.currentPlayer.statistique.actionPoint, this._map.nodes);
                displayHighlighted();
            }
        }

        private void displayHighlighted() {
            for(int i = 0; i < _path.Count; i++) {
                GameObject.Find(_path[i].x + "|" + _path[i].y).GetComponent<Tile>().changeMaterialColor(Color.blue);
            }
        }

        public void PointerExit() {
            if(!this._isActivated) {
                for(int i = _path.Count - 1; i >= 0; i--) {
                    GameObject.Find(_path[i].x + "|" + _path[i].y).GetComponent<Tile>().changeMaterialColorToDefault();
                    this._path.RemoveAt(i);
                }
            }
        }

    }
}
