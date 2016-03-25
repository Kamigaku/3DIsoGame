using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Utility;
using Mechanics;

namespace HUD {
    public class ButtonTour : MonoBehaviour {

        private bool _canPassTurn = false;
        
        public void changeText(bool canPlayCard) {
            if (canPlayCard)
                this.GetComponentInChildren<Text>().text = "END TOUR";
            else
                this.GetComponentInChildren<Text>().text = "WAITING";
            _canPassTurn = canPlayCard;
        }

        public void OnClick() {
            //if(this._canPassTurn)
                References.getReferences("Fight", true).GetComponent<Fight>().endTurn();
        }
    }
}