using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace OM
{
    public class IconGenerator : MonoBehaviour
    {
        public List<GameObject> sceneObjects;
        public List<ItemSO> dataObjects;
        [SerializeField] private string pathFolder;

        private new Camera camera;

        private void Start()
        {
            camera = GetComponent<Camera>();
        }

        [ContextMenu("Screenshot")]
        private void ProcessScreenshot()
        {
            StartCoroutine(Screenshot());
        }

        private IEnumerator Screenshot()
        {
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                GameObject obj = sceneObjects[i];
                ItemSO data = dataObjects[i];

                obj.gameObject.SetActive(true);

                yield return null;

                TakeScreenShot($"{Application.dataPath}/{pathFolder}/{data.itemName}_Icon.png");

                yield return null;
                obj.gameObject.SetActive(false);

#if UNITY_EDITOR
                Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/{pathFolder}/{data.itemName}_Icon.png");
                if (s != null)
                {
                    data.image = s;
                    EditorUtility.SetDirty(data);
                }
#endif
                yield return null;
            }
        }

        private void TakeScreenShot(string fullPath)
        {
            if (camera == null)
            {
                camera = GetComponent<Camera>();
            }
            RenderTexture rt = new RenderTexture(256, 256, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;

            if (Application.isEditor)
            {
                DestroyImmediate(rt);
            }
            else
            {
                Destroy(rt);
            }

            byte[] bytes = screenShot.EncodeToPNG();
            System.IO.File.WriteAllBytes(fullPath, bytes);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}