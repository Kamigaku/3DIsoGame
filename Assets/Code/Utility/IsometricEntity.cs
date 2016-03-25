using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode]
public class IsometricEntity : MonoBehaviour {

    private SpriteRenderer _sprite;

	void Start () {
        this._sprite = this.GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        this._sprite.sortingOrder = (int)-(Math.Ceiling(this.transform.position.y));
	}
}
