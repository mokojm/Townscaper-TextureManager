using MelonLoader;
using UnityEngine;
using ModUI;
using System;
using TMPro;
using UnityEngine.UI;

namespace TextureManager
{
	public static class TextureManagerUI
	{
		public static MelonMod myMod;
		public static ModSettings myModSettings;
		public static Slider referencedSlider;

		public static Slider refPlantSliderR;
		public static Slider refPlantSliderG;
		public static Slider refPlantSliderB;

		public static bool isInitialized;

		public static void Initialize(MelonMod thisMod)
		{

			myModSettings = UIManager.Register(thisMod, new Color32(201, 245, 184, 255));

			myModSettings.AddSlider("RED", "General", new Color32(119, 206, 224, 255), 0f, 1f, false, (float)TextureManagerMain.plantColor.r, new Action<float>(delegate (float value) { TextureManagerMain.plantColor.r = value; Update(); }));
			myModSettings.AddSlider("GREEN", "General", new Color32(119, 206, 224, 255), 0f, 1f, false, (float)TextureManagerMain.plantColor.g, new Action<float>(delegate (float value) { TextureManagerMain.plantColor.g = value; Update(); }));
			myModSettings.AddSlider("BLUE", "General", new Color32(119, 206, 224, 255), 0f, 1f, false, (float)TextureManagerMain.plantColor.b, new Action<float>(delegate (float value) { TextureManagerMain.plantColor.b = value; Update(); }));

			myModSettings.AddButton("Reset", "General", new Color32(255, 179, 174, 255), new Action(delegate { TextureManagerMain.Reset(); Update(); }));

			refPlantSliderR = myModSettings.controlSliders["RED"].GetComponent<Slider>();
			refPlantSliderG = myModSettings.controlSliders["GREEN"].GetComponent<Slider>();
			refPlantSliderB = myModSettings.controlSliders["BLUE"].GetComponent<Slider>();

            isInitialized = true;
		}

		public static void Update()
		{
			if (isInitialized)
            {
				TextureManagerMain.plantMat.color = TextureManagerMain.plantColor;
				refPlantSliderR.value = (float)TextureManagerMain.plantColor.r;
				refPlantSliderG.value = (float)TextureManagerMain.plantColor.g;
				refPlantSliderB.value = (float)TextureManagerMain.plantColor.b;
			}
		}
	}
}
