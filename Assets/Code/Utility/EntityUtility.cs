using GamePlay;
using Maps;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility {
    public class EntityUtility : ScriptableObject {

        #region 2D
        public static Vector2 screenToMap(GameObject anObject, bool isPlayer = false) {
            Vector2 map = new Vector2();
            map.x = (anObject.transform.position.x / (Constants.ESPACEMENT_CASE_X2D / 2) + anObject.transform.position.y / (Constants.ESPACEMENT_CASE_Y2D / 2)) / 2;
            if(isPlayer)
                map.y = ((anObject.transform.position.y - 0.75f) / (Constants.ESPACEMENT_CASE_Y2D / 2) - (anObject.transform.position.x / (Constants.ESPACEMENT_CASE_X2D / 2))) / 2;
            else
                map.y = (anObject.transform.position.y / (Constants.ESPACEMENT_CASE_Y2D / 2) - (anObject.transform.position.x / (Constants.ESPACEMENT_CASE_X2D / 2))) / 2;
            return map;
        }

        public static Vector2 mapToScreen(Vector2 mapPosition) {
            Vector2 screen = new Vector2();
            screen.x = (mapPosition.x - mapPosition.y) / 2;
            screen.y = (mapPosition.x + mapPosition.y) / 4;
            return screen;
        } 
        #endregion

        #region 3D
        /*public static Vector2 screenToMap3D(GameObject anObject) {
            Vector2 map = new Vector2();
            map.x = (anObject.transform.position.x / (Constants.ESPACEMENT_CASE_X3D) + anObject.transform.position.z / (Constants.ESPACEMENT_CASE_Z3D / 2)) / 2;
            map.y = (anObject.transform.position.z / (Constants.ESPACEMENT_CASE_Z3D / 2) - (anObject.transform.position.x / (Constants.ESPACEMENT_CASE_Z3D / 2))) / 2;
            return map;
        }

        public static Vector2 mapToScreen3D(Vector3 mapPosition) {
            Vector2 screen = new Vector2();
            screen.x = (mapPosition.x - mapPosition.z) / 2;
            screen.y = (mapPosition.x + mapPosition.z) / 4;
            return screen;
        }*/
        #endregion

    }
}