using Entity.Ai;
using UnityEngine;

namespace GamePlay {
    public abstract class MobFactory {

        public static void addComponentToMob(GameObject mob, string mobType) {
            switch (mobType) {
                case "Gobelin":
                    mob.AddComponent<Gobelin>();
                    break;
                default:
                    Debug.Log("Erreur dans la factory, le type de mob : " + mobType + " n'existe pas !");
                    break;
            }
        }

    }
}