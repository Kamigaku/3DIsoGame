using UnityEngine;
using System.Collections.Generic;
using Utility;
using UnityEngine.UI;
using GamePlay;
using Entity;
using HUD;

namespace Mechanics {

    public class Fight : MonoBehaviour {

        public int tourTimer = 20;
        
        private float _currentTime; // Le temps courant, réinitialiser à chaque changement d'état
        private int _currentIteration = 0;
        private List<AEntity> _playOrder = new List<AEntity>();
        public AEntity currentPlayer;

        void Awake() {
            calculatePlayOrder();
        }

        void Update() {
            if (this._playOrder.Count > 0) {
                this._currentTime += Time.deltaTime;
                if (this._currentTime > tourTimer)
                    endTurn();
            }
            else {
                calculatePlayOrder();
            }
        }

        public void calculatePlayOrder() {
            List<GameObject> allEntities = new List<GameObject>();
            foreach(GameObject entity in GameObject.FindGameObjectsWithTag("Entity"))
                allEntities.Add(entity);
            this._playOrder.Clear();
            for (int i = 0; i < allEntities.Count; i++)
                if (allEntities[i].GetComponent<AEntity>() != null && allEntities[i].GetComponent<AEntity>().statistique != null) 
                    for (int j = 0; j < allEntities[i].GetComponent<AEntity>().statistique.initiative; j++) 
                        this._playOrder.Add(allEntities[i].GetComponent<AEntity>());
            if (this._playOrder.Count > 0) {
                this._playOrder.Shuffle();
                this._playOrder[0].canPlay = true;
                this.currentPlayer = this._playOrder[0];
            }
        }

        public void endTurn() {
            if(this._playOrder.Count > 0) {
                this._playOrder[this._currentIteration].canPlay = false;
                this._currentIteration += 1;
                if(this._currentIteration >= this._playOrder.Count) {
                    this._currentIteration = 0;
                }
                this._playOrder[this._currentIteration].canPlay = true;
                this.currentPlayer = this._playOrder[this._currentIteration];
                this._currentTime = 0f;
            }
        }

    }

}