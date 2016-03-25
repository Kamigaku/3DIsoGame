using UnityEngine;
using System.Collections;
using Utility;
using Mechanics;

public class Tile : MonoBehaviour {

    private GameObject _highlighter;
    private Renderer _renderer;
    private Color _defaultColor;

    void Start() {
        this._renderer = this.GetComponent<Renderer>();
        this._defaultColor = this._renderer.material.color;
    }

    void OnMouseOver() {
        this.changeMaterialColor(Color.red);
        if(Input.GetMouseButtonDown(0)) {
            References.getReferences("Fight", true).GetComponent<Fight>().currentPlayer.moveEntity((int)this.transform.position.x, (int)this.transform.position.z);
        }
    }

    void OnMouseExit() {
        this.changeMaterialColorToDefault();
    }

    public void changeMaterialColor(Color color) {
        this._renderer.material.color = color;
    }

    public void changeMaterialColorToDefault() {
        this._renderer.material.color = this._defaultColor;
    }
}
