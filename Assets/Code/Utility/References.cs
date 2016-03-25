using UnityEngine;
using System.Collections.Generic;

namespace Utility {
    public class References : MonoBehaviour {

        private static Dictionary<string, GameObject> _references;

        void Start() {
            _references = new Dictionary<string, GameObject>();
        }

        public static void addReferences(string goName) {
            GameObject go = GameObject.Find(goName);
            if (go != null)
                _references.Add(goName, go);
            else
                Debug.Log("Not found Go : " + goName);
        }

        public static GameObject getReferences(string goName, bool recursive = false) {
            if (_references != null) {
                if (_references.ContainsKey(goName))
                    return _references[goName];
                else if(recursive) {
                    addReferences(goName);
                    return getReferences(goName, true);
                }
            }
            return null;
        }
    }
}