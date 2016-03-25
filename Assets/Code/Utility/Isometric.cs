using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Isometric : MonoBehaviour {

    void Update() {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.y);
    }

}
