using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using ModUI;
using System.Linq;
using Placemaker;
using System.Drawing;

namespace TextureManager
{
	[Serializable]
	public class TextureSettingFile
    {
		public List<TextureEntry> listTextureEntry;
    }

	[Serializable]
	public class TextureEntry
	{
		public string name;
		public string type;
		public string path;
	}

	public class TextureManagerMain : MelonMod
    {
		public static WorldMaster master;

		public static string textureFolderPath = UnityEngine.Application.persistentDataPath + "\\Textures";
		public static string modTextureFolderPath = textureFolderPath + "\\ModTextures";

		public static List<string> themeList = new List<string>();
		public static List<string> paletteList = new List<string>();

		public static string currentTheme;
		public static string currentPalette;

		public static string newTownColor = string.Empty;
		public static string newTownMaterial = string.Empty;
		public static string newTownPalette = string.Empty;

		//Default name for new texture
		public static string defaultName = "Custom";

		//Name of new texture
		public static string newName = defaultName;

		public static Texture2D defaultTownColorTex = new Texture2D(128, 128);
		public static Texture2D defaultTownMaterialTex = new Texture2D(128, 128);
		public static Texture2D defaultPaletteTex = new Texture2D(128, 128);

		public static Texture2D currentTownColorTex = new Texture2D(128, 128);
		public static Texture2D currentTownMaterialTex = new Texture2D(128, 128);
		public static Texture2D currentPaletteTex = new Texture2D(128, 128);

		public static Texture2D newTownColorTex = new Texture2D(128, 128);
		public static Texture2D newTownMaterialTex = new Texture2D(128, 128);
		public static Texture2D newPaletteTex = new Texture2D(128, 128);

		//Palette anchors
		public static GameObject anchor0;
		public static GameObject anchor1;

		public static bool isInitialized = false;




		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			if (sceneName == "Placemaker")
			{
				MelonLogger.Msg("Main scene loaded");

				// Initializing
				//Initialize();
				TextureManagerUI.Initialize(this);

				master = GameObject.Find("WorldMaster").GetComponent<WorldMaster>();

			}
		}

		public static void LoadRessource(Bitmap image, bool isTheme, string name, string type)
        {
			string filePath;
			if (isTheme)
            {
				string themeDirectory = modTextureFolderPath + "\\" + name;
				filePath = themeDirectory + type == "TownColor" ? "\\TownColor.png" : "\\TownMaterial.png";
				if (!Directory.Exists(themeDirectory))
                {
					Directory.CreateDirectory(themeDirectory);
				}

			}
			else
            {
				filePath = modTextureFolderPath + "\\Palettes\\" + name + ".png";
			}

			//Actual loading
			ImageConverter converter = new ImageConverter();
			byte[] bytes = (byte[])converter.ConvertTo(image, typeof(byte[]));
			File.WriteAllBytes(filePath, bytes);
			//Texture2D tex = new Texture2D(image.Width, image.Height);
			//ImageConversion.LoadImage(tex, bytes);
			//File.WriteAllBytes(filePath, ImageConversion.EncodeToPNG(tex));
		}

		public static void FirstStart()
		{

			if (!Directory.Exists(modTextureFolderPath))
			{
				Directory.CreateDirectory(modTextureFolderPath);
				Directory.CreateDirectory(modTextureFolderPath + "\\Palettes");
			}


			//Load all default textures in "ModTexture"
			//"ModTexture" contains a subfolder named "Palette" and one subfolder per theme, the name of theme's subfolder is the name of the theme
			//Files in theme subfolder will be "TownColor.png" and "TownMaterial.png" whatever the folder
			LoadRessource(InitialTextures.TownColor_Green_Matrix, true, "Green Matrix", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Green_Matrix, true, "Green Matrix", "TownMaterial");
		}

		public static void Initialize()
        {
			LocatePaletteAnchors();

			if (!Directory.Exists(modTextureFolderPath))
            {
				FirstStart();
            }

			themeList = Directory.EnumerateDirectories(modTextureFolderPath).ToList<string>();
			themeList.Remove("Palettes");
			themeList.Sort();
			themeList.Insert(0, "DEFAULT");

			paletteList = Directory.EnumerateDirectories(modTextureFolderPath + "\\Palettes").ToList<string>();
			paletteList.Sort();
			paletteList.Insert(0, "DEFAULT");

			//Initialize default textures
			ImageConverter converter = new ImageConverter();
			byte[] bytes = (byte[])converter.ConvertTo(InitialTextures.TownColor_default, typeof(byte[]));
			ImageConversion.LoadImage(defaultTownColorTex, bytes);

			bytes = (byte[])converter.ConvertTo(InitialTextures.TownMaterial_default, typeof(byte[]));
			ImageConversion.LoadImage(defaultTownMaterialTex, bytes);

			bytes = (byte[])converter.ConvertTo(InitialTextures.TownPalette_default, typeof(byte[]));
			ImageConversion.LoadImage(defaultPaletteTex, bytes);

		}

		public static bool UpdateInGameTex(string origin, string dest)
        {
            try
            {
				File.Copy(origin, dest, true);
				File.SetLastWriteTime(dest, DateTime.Now);
				return true;
            }
            catch (IOException)
            {
				return false;
            }
		}

		public static bool UpdateTextures(bool theme)
        {
			//Specific case for default
			if (theme && currentTheme == "DEFAULT")
            {
				try
                {
					File.Delete(textureFolderPath + "\\TownColor.png");
					File.Delete(textureFolderPath + "\\TownMaterial.png");
				}
				catch (IOException)
				{
					return false;
				}

				//Update real-time
				master.texturePngMaster.OnApplicationFocus(true);

				//Texture update
				currentTownColorTex = defaultTownColorTex;
				currentTownMaterialTex = defaultTownMaterialTex;
				return true;
			}
			else if (!theme && currentPalette == "DEFAULT")
            {
				try
                {
					File.Delete(textureFolderPath + "\\TownPalette.png");
                }
				catch (IOException)
				{
					return false;
				}

				//Update real-time
				master.texturePngMaster.OnApplicationFocus(true);

				//Texture update
				currentPaletteTex = defaultPaletteTex;
				return true;
			}
			//Get current path
			if (theme)
			{
				string pathTownColor = modTextureFolderPath + "\\" + currentTheme + "\\TownColor.png";
				if (!UpdateInGameTex(pathTownColor, textureFolderPath + "\\TownColor.png"))
				{
					return false;
				}

				string pathTownMaterial = modTextureFolderPath + "\\" + currentTheme + "\\TownMaterial.png";
				if (!UpdateInGameTex(pathTownMaterial, textureFolderPath + "\\TownMaterial.png"))
				{
					return false;
				}
			}
			else
			{
				string pathPalette = modTextureFolderPath + "\\" + currentPalette + ".png";
				if (!UpdateInGameTex(pathPalette, textureFolderPath + "\\TownPalette.png"))
				{
					return false;
				}
			}

			//Update real-time
			master.texturePngMaster.OnApplicationFocus(true);

			//Update UI textures
			if (theme)
			{
				ImageConversion.LoadImage(currentTownColorTex, File.ReadAllBytes(textureFolderPath + "\\TownColor.png"));
				ImageConversion.LoadImage(currentTownMaterialTex, File.ReadAllBytes(textureFolderPath + "\\TownMaterial.png"));
			}
			else
			{
				ImageConversion.LoadImage(currentPaletteTex, File.ReadAllBytes(textureFolderPath + "\\TownPalette.png"));
			}
			return true;
		}

		public static bool UpdateNextPrev(bool next, bool theme)
        {
			//Change Current Label
			if (theme)
            {
				int index = themeList.IndexOf(currentTheme);
				if (next && index == themeList.Count - 1)
                {
					currentTheme = themeList[0];
                }
				else if (next)
                {
					currentTheme = themeList[index + 1];
				}
				else if (!next && index == 0)
                {
					currentTheme = themeList[-1];
				}
				else
                {
					currentTheme = themeList[index - 1];
				}
            }
			else
			{
				int index = paletteList.IndexOf(currentPalette);
				if (next && index == paletteList.Count - 1)
				{
					currentPalette = paletteList[0];
				}
				else if (next)
				{
					currentPalette = paletteList[index + 1];
				}
				else if (!next && index == 0)
				{
					currentPalette = paletteList[-1];
				}
				else
				{
					currentPalette = paletteList[index - 1];
				}
			}

			return UpdateTextures(theme);
		}

		public static void ResetAddScreen()
        {
			//Update newTex
			newTownColorTex = defaultTownColorTex;
			newTownMaterialTex = defaultTownMaterialTex;
			newPaletteTex = defaultPaletteTex;

			//Reseting the default name
			newName = defaultName;
        }

		public static string SelectFile()
        {
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = "c:\\";
				openFileDialog.Filter = "Image files (*.png)|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					//Get the path of specified file
					return openFileDialog.FileName;
				}
				else
                {
					return "";
                }
			}
		}

		public static void SelectTownColor()
        {
			newTownColor = SelectFile();
			if (newTownColor != "")
            {
				ImageConversion.LoadImage(newTownColorTex, File.ReadAllBytes(newTownColor));
            }
        }

		public static void SelectTownMaterial()
		{
			newTownMaterial = SelectFile();
			if (newTownMaterial != "")
			{
				ImageConversion.LoadImage(newTownMaterialTex, File.ReadAllBytes(newTownMaterial));
			}
		}

		public static void SelectPalette()
        {
			newTownPalette = SelectFile();
			if (newTownPalette != "")
			{
				ImageConversion.LoadImage(newPaletteTex, File.ReadAllBytes(newTownPalette));
			}
		}

		public static void AddTexture(bool isTheme)
        {
			string initialName = newName;
			if (isTheme)
			{
				string themeDirectory = modTextureFolderPath + "\\" + newName;

				//Case no name has been provided
				int c = 1;
				while(Directory.Exists(themeDirectory))
                {
					newName = initialName + " " + c.ToString();
					themeDirectory = modTextureFolderPath + "\\" + newName;
					c++;
				}

				//After the name is chosen
				Directory.CreateDirectory(themeDirectory);
				if (newTownColor == "")
                {
					ImageConverter converter = new ImageConverter();
					byte[] bytes = (byte[])converter.ConvertTo(InitialTextures.TownColor_default, typeof(byte[]));
					File.WriteAllBytes(themeDirectory + "\\TownColor.png", bytes);
				}
				else
                {
					File.Copy(newTownColor, themeDirectory + "\\TownColor.png");
                }

				if (newTownMaterial == "")
				{
					ImageConverter converter = new ImageConverter();
					byte[] bytes = (byte[])converter.ConvertTo(InitialTextures.TownMaterial_default, typeof(byte[]));
					File.WriteAllBytes(themeDirectory + "\\TownMaterial.png", bytes);
				}
				else
                {
					File.Copy(newTownMaterial, themeDirectory + "\\TownMaterial.png");
				}

				//newName added to the list
				themeList.Add(newName);
				themeList.Sort();
				themeList.Remove("DEFAULT");
				themeList.Insert(0, "DEFAULT");
			}
			else
			{
				//Checking for file already exists
				int c = 1;
				while (File.Exists(modTextureFolderPath + "\\Palettes\\" + newName + ".png"))
                {
					newName = initialName + " " + c.ToString();
					c++;
				}

				//Adding the new Palette
				if (newTownPalette != "")
                {
					File.Copy(newTownPalette, modTextureFolderPath + "\\Palettes\\" + newName + ".png");
                }

				//newName added to the list
				paletteList.Add(newName);
				paletteList.Sort();
				paletteList.Remove("DEFAULT");
				paletteList.Insert(0, "DEFAULT");
			}

			//Everything is reset at the end
			ResetAddScreen();
		}

		public static bool DeleteTexture(bool isTheme)
        {
			//Case default is selected
			if (isTheme && currentTheme == "DEFAULT" || !isTheme && currentPalette == "DEFAULT")
            {
				return false;
            }
			else if (isTheme)
            {
				//Delete the directory
				try
                {
					Directory.Delete(modTextureFolderPath + "\\" + currentTheme, true);
				}
				catch (IOException)
				{
					return false;
				}
			}
			else if (!isTheme)
            {
				try
                {
					File.Delete(modTextureFolderPath + "\\" + currentPalette + ".png");
                }
				catch (IOException)
				{
					return false;
				}
			}

			return UpdateNextPrev(false, isTheme);
        }

		public static void MovePalette()
        {
			bool isOpen = UIManager.isOpen;
			int width = UnityEngine.Screen.currentResolution.width;
			float delta = (float)width * 20 / 1080;
			
			if (isOpen)
            {
				anchor0.transform.position = new Vector3(180f + delta, anchor0.transform.position.y, anchor0.transform.position.z);
				anchor1.transform.position = new Vector3(180f + delta, anchor1.transform.position.y, anchor1.transform.position.z);
			}
			else
            {
				anchor0.transform.position = new Vector3(30, anchor0.transform.position.y, anchor0.transform.position.z);
				anchor1.transform.position = new Vector3(30, anchor1.transform.position.y, anchor1.transform.position.z);
			}
        }

		public static void LocatePaletteAnchors()
        {
			anchor0 = GameObject.Find("Anchor0");
			anchor1 = GameObject.Find("Anchor1");
        }

		public static void Reset()
		{

		}

		public override void OnApplicationStart()
		{

		}

		public override void OnUpdate()
		{
			MovePalette();
		}

	}
}

