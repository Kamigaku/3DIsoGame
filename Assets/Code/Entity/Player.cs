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

        void Start () {
            this._statistique = new EntityStatistique(10, 7, 3);
            baseLoad();
        }

		void Update () {
			if(this.canPlay) {

            }
			if (this._statistique.healthPoint <= 0) {
				Destroy(this.gameObject);
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