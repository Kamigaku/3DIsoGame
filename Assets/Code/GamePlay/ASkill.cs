using Maps;
using System;
using UnityEngine;
using Utility;

namespace GamePlay {
    public abstract class ASkill : MonoBehaviour, ISkill {

        private Map _map;
        private int range;
        public RangeSkill typeRange = RangeSkill.UNDEFINED;

        public bool isInRange(Vector2 destination) {
            switch(typeRange) {
                case RangeSkill.CIRCLE:
                    break;
                case RangeSkill.LINE:
                    Vector2 curPos = EntityUtility.screenToMap(this.gameObject);
                    for(int y = -1; y < 2; y += 2) {
                        for(int x = 0; x < 2; x++) {
                            Vector2 testPos;
                            if (curPos.y % 2 == 0)
                                testPos = new Vector2(x * y, y) + curPos;
                            else
                                testPos = new Vector2(x, y) + curPos;
                        }
                    }
                    break;
            }
            return false;
        }
    }

    public enum RangeSkill {
        UNDEFINED, LINE, CIRCLE
    };
}
