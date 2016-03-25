using AI;
using GamePlay;
using Mechanics;
using System.Threading;
using Utility;

namespace Entity.Ai {
    class Gobelin : Mob {

        void Start() {
            this._statistique = new EntityStatistique(5, 1, 2);
            baseLoad();
            this.entityName = "Gobelin";
        }

        void Update() {
            /*if(this.canPlay) {
                //References.getReferences("FightHandler").GetComponent<Fight>().endTurn();
                Thread.Sleep(2000);
            }*/
        }

    }
}
