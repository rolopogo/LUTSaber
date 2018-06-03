﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using IllusionPlugin;
using UnityEngine.SceneManagement;

namespace LUTSaber
{
    public class LUTPlugin : IPlugin
    {
        public string Name => "LUTSaber";
        public string Version => "1.0.1";
        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            LUTLoader.OnLoad();
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {

        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }
    }
}