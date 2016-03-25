using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamePlay {
    public class EntityStatistique : Statistique {

        private int _initiative = 1;

        private int _actionPoint = 1;
        private int _maxActionPoint;

        public int initiative {
            get {
                return this._initiative;
            }
        }
        public int actionPoint {
            get {
                return this._actionPoint;
            }
            set {
                this._actionPoint = value;
            }
        }

        public EntityStatistique(int healthPoint, int initiative, int actionPoint) : base(healthPoint) {
            this._initiative = initiative;
            this._actionPoint = actionPoint;
            this._maxActionPoint = actionPoint;
        }

        public void newTurn() {
            this._actionPoint = this._maxActionPoint;
        }

    }
}
