using UnityEngine;
using GamePlay;
using Utility;
using HUD;
using Mechanics;
using Entity;
using System.Collections.Generic;
using Maps;

namespace Utility {
	public class Player : AEntity {

        private float previousTime = 0f;

        private short _displayPath = 0;

        void Start () {
            this._statistique = new EntityStatistique(10, 1, 4);
            baseLoad();
        }

		void Update () {
			if(this.canPlay) {

            }
			if (this._statistique.healthPoint <= 0) {
				Destroy(this.gameObject);
			}
            if(this._isMainPlayer) {
                if(Input.GetKeyDown(KeyCode.X))
                    this._displayPath = 1;
                if(Input.GetKeyUp(KeyCode.X))
                    this._displayPath = 2;
                if(this._displayPath == 1)
                    OnMouseOver();
                else if(this._displayPath == 2) {
                    this._displayPath = 0;
                    OnMouseExit();
                }
                    
            }
		}

        /*private void doAction(GameObject affectedGo, Vector3 position) {
            for (int i = 0; i < this._hand.Count; i++) {
                if (this._hand[i].name == this._cardNamePlaying) {
                    this._hand[i].doAction(affectedGo, position);
                    break;
                }
            }
        }*/

        /*private void cardHandling() {
            addCardToDeck("Fire Ball");
            addCardToDeck("Heal");
            addCardToDeck("Block");
            addCardToDeck("Slash");
            addCardToDeck("Footstep");

            this._deck.Shuffle();
            pickACard(1);

            HUDRefresher.refreshCardHUD();
        }*/

	}
}