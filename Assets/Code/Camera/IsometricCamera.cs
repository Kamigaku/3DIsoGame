using UnityEngine;
using System.Collections;
using Maps;
using Utility;

namespace CameraUtility {
    public class IsometricCamera : MonoBehaviour {

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
            this._camera = this.GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                if (this._camera.orthographicSize > maxZoom)
                    this._camera.orthographicSize -= zoomRatio;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                if (this._camera.orthographicSize < minZoom)
                    this._camera.orthographicSize += zoomRatio;
            }
            if (Input.GetKey(KeyCode.D)) {
                this.cameraTarget.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Q)) {
                this.cameraTarget.transform.Translate(Vector3.right * -speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Z)) {
                this.cameraTarget.transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S)) {
                this.cameraTarget.transform.Translate(-Vector3.up * speed * Time.deltaTime);
            }

            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null) {
                    Vector2 collisionPos = hit.collider.gameObject.transform.position;
                    Vector2 map = EntityUtility.screenToMap(hit.collider.gameObject);
                    Vector2 screen = EntityUtility.mapToScreen(map);
                    Debug.Log("Origine : " + collisionPos + " | Map : " + map + " Screen : " + screen);
                }
            }
            if(Input.GetMouseButtonDown(1)) {
                this._selector.SetActive(false);
                this._ui_InfoTile.SetActive(false);
            }
        }
    }
}