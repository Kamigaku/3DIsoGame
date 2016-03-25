using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utility;

namespace CameraUtility {
    class IsometricCamera3D : MonoBehaviour {

        public int speed = 50;
        public float zoomRatio = 5;
        public int maxZoom = 30;
        public int minZoom = 100;
        public GameObject cameraTarget;
        private GameObject _ui_InfoTile;
        private GameObject _selector;
        private Camera _camera;

        // Use this for initialization
        void Start() {
            this._camera = this.GetComponentInChildren<Camera>();
        }

        // Update is called once per frame
        void Update() {
            if(Input.GetAxis("Mouse ScrollWheel") > 0) {
                if(this._camera.orthographicSize > maxZoom)
                    this._camera.orthographicSize -= zoomRatio;
            }
            if(Input.GetAxis("Mouse ScrollWheel") < 0) {
                if(this._camera.orthographicSize < minZoom)
                    this._camera.orthographicSize += zoomRatio;
            }
            if(Input.GetKey(KeyCode.D)) {
                this.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.Q)) {
                this.transform.Translate(Vector3.right * -speed * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.Z)) {
                this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.S)) {
                this.transform.Translate(Vector3.forward * -speed * Time.deltaTime);
            }

            /*if(Input.GetMouseButtonDown(0)) {
                Ray r = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down);
                if(Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out h LayerMask.NameToLayer("Ground"), out hit)) {
                    Vector3 collisionPos = hit.collider.gameObject.transform.position;
                    Vector3 map = EntityUtility.screenToMap3D(hit.collider.gameObject);
                    Vector3 screen = EntityUtility.mapToScreen3D(map);
                    Debug.Log("Origine : " + collisionPos + " | Map : " + map + " Screen : " + screen);
                }
            }
            if(Input.GetMouseButtonDown(1)) {
                this._selector.SetActive(false);
                this._ui_InfoTile.SetActive(false);
            }*/
        }
    }
}