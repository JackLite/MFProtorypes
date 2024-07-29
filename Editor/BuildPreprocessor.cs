﻿using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Modules.Extensions.Prototypes.Editor
{
    public class BuildPreprocessor : BuildPlayerProcessor
    {
        public override int callbackOrder => int.MinValue + 1;

        public override void PrepareForBuild(BuildPlayerContext buildPlayerContext)
        {
            Debug.Log("Modules prototypes preprocessing started");
            PostProcessorCompilation.ForceUpdateAssemblies();
            Debug.Log("Modules prototypes preprocessing finished");
            Debug.Log("Modules prototypes refresh assets");
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            Debug.Log("Modules prototypes refresh assets finished");
        }
    }
}