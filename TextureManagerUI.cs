using MelonLoader;
using UnityEngine;
using ModUI;
using System;
using TMPro;
using UnityEngine.UI;
using System.Resources;
using System.Reflection;

namespace TextureManager
{
	public static class TextureManagerUI
	{
		public static MelonMod myMod;
		public static ModSettings myModSettings;

		public static SelectionButton refSelectTheme;
		public static SelectionButton refSelectPalette;

		public static bool isInitialized;

		public static void Initialize(MelonMod thisMod)
		{
			myModSettings = UIManager.Register(thisMod, new Color32(201, 245, 184, 255));

			//Search for existing setting if none is found, create the default setting
			myModSettings.GetValueString("SelectTheme", "Theme", out string currentTheme);
			myModSettings.GetValueString("SelectPalette", "Palette", out string currentPalette);
			currentTheme = currentTheme == null || currentTheme == "" ? "DEFAULT" : currentTheme;
			currentPalette = currentPalette == null || currentPalette == "" ? "DEFAULT" : currentPalette;

			refSelectTheme = myModSettings.AddSelectionButton
				("SelectTheme", 
				"Theme", 
				new Color32(119, 206, 224, 255), 
				new Action(delegate { PrevTheme(); }), 
				new Action(delegate { NextTheme(); }),
				currentTheme);

			refSelectPalette = myModSettings.AddSelectionButton
				("SelectPalette", 
				"Palette", 
				new Color32(243, 227, 182, 255), 
				new Action(delegate { PrevPalette(); }), 
				new Action(delegate { NextPalette(); }),
				currentPalette);

			myModSettings.GetValueString("SelectTheme", "Theme", out currentTheme);
			myModSettings.GetValueString("SelectPalette", "Palette", out currentPalette);
			currentTheme = currentTheme == null || currentTheme == "" ? "DEFAULT" : currentTheme;
			currentPalette = currentPalette == null || currentPalette == "" ? "DEFAULT" : currentPalette;

			TextureManagerMain.currentTheme = currentTheme;
			TextureManagerMain.currentPalette = currentPalette;

			isInitialized = true;
		}

		public static void UpdateSettings()
        {
			myModSettings.SetValueString("SelectTheme", "Theme", TextureManagerMain.currentTheme);
			myModSettings.SetValueString("SelectPalette", "Palette", TextureManagerMain.currentPalette);
			myModSettings.SaveToFile();
		}

		public static void NextTheme()
        {
			if (TextureManagerMain.UpdateNextPrev(true, true))
            {
				UpdateSettings();
				refSelectTheme.selectValue = TextureManagerMain.currentTheme;
            }
        }

		public static void PrevTheme()
        {
			if (TextureManagerMain.UpdateNextPrev(false, true))
            {
				UpdateSettings();
				refSelectTheme.selectValue = TextureManagerMain.currentTheme;
			}
		}

		public static void NextPalette()
		{
			if (TextureManagerMain.UpdateNextPrev(true, false))
            {
				UpdateSettings();
				refSelectPalette.selectValue = TextureManagerMain.currentPalette;
			}
		}

		public static void PrevPalette()
		{
			if (TextureManagerMain.UpdateNextPrev(false, false))
			{
				UpdateSettings();
				refSelectPalette.selectValue = TextureManagerMain.currentPalette;
			}
		}
	}
}
