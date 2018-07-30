using UnityEngine;
using System;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using DigitalRuby.SimpleLUT;

namespace LUTSaber
{
    public class LUTLoader : MonoBehaviour
    {
        public static LUTLoader Instance;

        Texture2D lutTex;
        string[] pngPaths;
        int pngIndex = 0;
        int hue = 0;
        SimpleLUT[] luts = new SimpleLUT[0];
        Shader shader;
        string customPath = "CustomLUT";
        bool controlsEnabled = true;

        public static void OnLoad()
        {
            if (Instance != null) return;
            new GameObject("LUTSaber").AddComponent<LUTLoader>();
        }

        public void SetLUTTexture(Texture2D newTex)
        {
            if (newTex == null)
            {
                Log("SetLUTTexture recieved null texture");
                return;
            }
            if(newTex.width != newTex.height * newTex.height)
            {
                Log("SetLUTTexture recieved texture with wrong dimensions. Should be w = h^2");
                return;
            }
            if(newTex.mipmapCount > 1 || newTex.format != TextureFormat.RGB24)
            {
                Texture2D tex2 = new Texture2D(newTex.width, newTex.height, TextureFormat.RGB24, false);
                tex2.SetPixels(newTex.GetPixels());
                newTex = tex2;
            }

            lutTex = newTex;

        }
        
        public void SetHue(int newHue)
        {
            hue = newHue % 360;
        }

        public void SetControlsEnabled(bool enabled)
        {
            controlsEnabled = enabled;
        }
        
        public void UpdateLUTComponents()
        {
            for (int i = 0; i < luts.Length; i++)
            {
                if (shader == null) Log("Missing Shader");
                luts[i].Shader = shader;
                if (shader == null) Log("Missing Texture");
                luts[i].LookupTexture = lutTex;
                luts[i].Hue = hue;
            }
        }

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            LoadFromFile();
            StartCoroutine(LoadPNG(pngPaths[pngIndex]));
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            StartCoroutine(AddComponentsToCamerasDelay());
        }

        private IEnumerator AddComponentsToCamerasDelay()
        {
            yield return new WaitForSeconds(.1f);

            AddComponentsToCameras();
            UpdateLUTComponents();
        }

        private void LoadFromFile()
        {
            Log("Loading assetbundle");
            // load shader
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.Combine(Environment.CurrentDirectory, "PluginsContent"), "LUTSaber.bundle"));

            if (myLoadedAssetBundle == null)
            {
                Log("Failed to load AssetBundle! Make sure PluginsContent/LUTSaber.bundle exists!");
                return;
            }

            shader = myLoadedAssetBundle.LoadAsset<Shader>("SimpleLUT");

            pngPaths = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, customPath), "*.png");
            Array.Sort(pngPaths);
        }
        
        private void AddComponentsToCameras()
        {
            Camera[] cams = FindObjectsOfType<Camera>();
            luts = new SimpleLUT[cams.Length];

            for (int i = 0; i < cams.Length; i++)
            {
                if (cams[i].GetComponent<SimpleLUT>())
                {
                    luts[i] = cams[i].GetComponent<SimpleLUT>();
                    
                } else
                {
                    Log("Adding SimpleLUT component to camera: " + cams[i].name);
                    luts[i] = cams[i].gameObject.AddComponent<SimpleLUT>();
                }
            }
        }
        
        private void Update()
        {
            if (!controlsEnabled) return;
            if (Input.GetKeyDown(KeyCode.Z))
            {
                pngIndex = (pngIndex + 1) % pngPaths.Length;
                StartCoroutine(LoadPNG(pngPaths[pngIndex]));
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                hue = (hue - 18) % 360;
                foreach (SimpleLUT lut in luts)
                {
                    lut.Hue = hue;
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                hue = (hue + 18) % 360;
                foreach (SimpleLUT lut in luts)
                {   
                    lut.Hue = hue;
                }
            }
        }

        private IEnumerator LoadPNG(string filePath)
        {
            Log("Loading Lookup Texture: " + pngPaths[pngIndex]);
            
            if (File.Exists(filePath))
            {
                Texture2D tex;
                using (WWW www = new WWW(filePath))
                {
                    yield return www;
                    int w = www.texture.width;
                    int h = www.texture.height;
                    tex = new Texture2D(w, h, TextureFormat.RGB24, false);
                    www.LoadImageIntoTexture(tex);
                }
                lutTex = tex;
                UpdateLUTComponents();
            }
            else
            {
                Log("File not found");
            }
        }
        
        private void Log(string s)
        {
            Console.WriteLine("[LUTSaber] " + s);
        }
    }
}
