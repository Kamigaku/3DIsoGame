using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GamePlay {

    public abstract class Statistique {

        private int _healthPoint;
        private int _maxHealthPoint;
        private int _defense;
        private int _mouvement;

        public int healthPoint {
            get { return this._healthPoint; }
        }

        public int defense {
            get { return this._defense; }
        }

        public int mouvement {
            get { return this._mouvement; }
        }

        public int maxHealthPoint {
            get { return this._maxHealthPoint; }
            set { this._maxHealthPoint = value; }
        }

        private List<Status> _status;
        private List<Status> _cureStatus;

		public Statistique(int healthPoint) {
			this._status = new List<Status>();
            this._cureStatus = new List<Status>();
            this._healthPoint = healthPoint;
            this._maxHealthPoint = healthPoint;
		}

		// A changer le fonctionnement des status, créer une classe Status contenant un enum, les dégats et la source (nom de la carte)
        public void addStatus(Status status) {
			this._status.Add (status);
        }

        public void addCureStatus(Status status) {
            this._cureStatus.Add(status);
        }

        public void addBonus(BaseStats statsConcerned, int value) {
            switch(statsConcerned) {
                case BaseStats.DEFENSE:
                    this._defense += value;
                    break;
                case BaseStats.HP:
                    if(value <= 0) {
                        if (defense > 0) {
                            this._healthPoint = this._healthPoint + value + this._defense;
                            this._defense = value + defense;
                            this._defense = this.defense < 0 ? 0 : this._defense;
                        }
                        else {
                            this._healthPoint = this._healthPoint + value;
                        }
                    }
                    else
                        this._healthPoint += value;
                    if (this._healthPoint > this._maxHealthPoint)
                        this._healthPoint = this._maxHealthPoint;
                    break;
                case BaseStats.MOUVEMENT:
                    this._mouvement += mouvement;
                    break;
                default:
                    break;
            }
        }

    }

    public enum Status {
        BURNING
    }

    public enum BaseStats {
        HP, DEFENSE, MOUVEMENT
    }
}
