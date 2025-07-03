using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZPackage
{
    public static class Z
    {
        public static GameManager GM => GameManager.Instance;
        public static CameraController CamC => CameraController.Instance;
        public static CanvasManager CanM => CanvasManager.Instance;
        public static LevelSpawner LS => LevelSpawner.Instance;
        private static Player _player;
        public static Player Player
        {
            get
            {
                if (_player == null)
                {
                    _player = Object.FindFirstObjectByType<Player>(FindObjectsInactive.Include);
                }
                return _player;
            }
        }
        public static void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }
        public static int TileDistance = 200;

    }
}

