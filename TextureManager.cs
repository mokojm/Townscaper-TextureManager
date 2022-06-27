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

		public static string textureFolderPath = Path.GetDirectoryName(Path.GetDirectoryName(UnityEngine.Application.persistentDataPath)) + "/Oskar Stalberg/Townscaper/Textures";
		public static string modTextureFolderPath = textureFolderPath + "/ModTextures";

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

		//To be deleted
		/*public static Placemaker.Ui.OrbitalCamera orbit;
		public static Unity.Mathematics.float2 rota = new Unity.Mathematics.float2(0.01f, 0);
		public static Vector3 pan = new Vector3(0.1f, 0, 0);
		public static float zoom = -0.03f;*/




		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			if (sceneName == "Placemaker")
			{
				MelonLogger.Msg("Main scene loaded");

				// Initializing
				Initialize();
				TextureManagerUI.Initialize(this);
				//Initialize();
				

				if (master == null)
				{
					GameObject masterGameObj = GameObject.Find("WorldMaster");
					if (masterGameObj != null)
					{
						master = masterGameObj.GetComponent<WorldMaster>();
					}
				}

			}
		}

		public static void LoadRessource(Bitmap image, bool isTheme, string name, string type)
        {
			string filePath;
			if (isTheme)
            {
				string themeDirectory = modTextureFolderPath + "/" + name;
				filePath = themeDirectory + (type == "TownColor" ? "/TownColor.png" : "/TownMaterial.png");
				if (Directory.Exists(themeDirectory) == false)
                {
					Directory.CreateDirectory(themeDirectory);
				}

			}
			else
            {
				filePath = modTextureFolderPath + "/Palettes/" + name + ".png";
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

			if (Directory.Exists(modTextureFolderPath) == false)
			{
				Directory.CreateDirectory(modTextureFolderPath);
				Directory.CreateDirectory(modTextureFolderPath + "/Palettes");
			}


			//Load all default textures in "ModTexture"
			//"ModTexture" contains a subfolder named "Palette" and one subfolder per theme, the name of theme's subfolder is the name of the theme
			//Files in theme subfolder will be "TownColor.png" and "TownMaterial.png" whatever the folder
			LoadRessource(InitialTextures.TownColor_Green_Matrix, true, "Matrix", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Green_Matrix, true, "Matrix", "TownMaterial");
			
			LoadRessource(InitialTextures.TownColor_Winter, true, "Winter", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Winter, true, "Winter", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_Winter, false, "Winter", "TownPalette");

			LoadRessource(InitialTextures.TownColor_Mediterranean, true, "Mediterranean", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Mediterranean, true, "Mediterranean", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_Mediterranean, false, "Mediterranean", "TownPalette");

			LoadRessource(InitialTextures.TownColor_Rusty_Metal, true, "HD Rusty", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Rusty_Metal, true, "HD Rusty", "TownMaterial");

			LoadRessource(InitialTextures.TownColor_Industrial, true, "Industrial", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Industrial, true, "Industrial", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_Industrial, false, "Industrial", "TownPalette");

			LoadRessource(InitialTextures.TownColor_Snow, true, "Snow", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Snow, true, "Snow", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_Snow, false, "Snow", "TownPalette");

			LoadRessource(InitialTextures.TownColor_Salty, true, "Salty", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Salty, true, "Salty", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_Salty, false, "Salty", "TownPalette");

			LoadRessource(InitialTextures.TownColor_Pastel, true, "Pastel", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Pastel, true, "Pastel", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_Pastel, false, "Pastel", "TownPalette");

			LoadRessource(InitialTextures.TownColor_Wireframe_Dark, true, "Wireframe Dark", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Wireframe_Dark, true, "Wireframe Dark", "TownMaterial");

			LoadRessource(InitialTextures.TownColor_Roofs_like_Borders, true, "Roofs = Borders", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Roofs_like_Borders, true, "Roofs = Borders", "TownMaterial");

			LoadRessource(InitialTextures.TownColor_Wireframe_Dark_2, true, "Wireframe Dark 2", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Pastel, true, "Wireframe Dark 2", "TownMaterial");

			LoadRessource(InitialTextures.TownPalette_Port_Roc, false, "Port Roc", "TownPalette");

			LoadRessource(InitialTextures.TownPalette_Palette_1, false, "Palette 1", "TownPalette");

			LoadRessource(InitialTextures.TownPalette_Palette_2, false, "Palette 2", "TownPalette");

			LoadRessource(InitialTextures.TownPalette_Palette_3, false, "Palette 3", "TownPalette");

			LoadRessource(InitialTextures.TownPalette_Palette_4, false, "Palette 4", "TownPalette");

			LoadRessource(InitialTextures.TownColor_GhostTown, true, "HD Ghost Town", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Pastel, true, "HD Ghost Town", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_GhostTown, false, "HD Ghost Town", "TownPalette");

			LoadRessource(InitialTextures.TownColor_GothicOrange, true, "Gothic Orange", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Pastel, true, "Gothic Orange", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_GothicOrange, false, "Gothic Orange", "TownPalette");

			LoadRessource(InitialTextures.TownPalette_DeepBlue, false, "Deep Blue", "TownPalette");

			LoadRessource(InitialTextures.TownColor_DarkOctober, true, "Dark October", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Pastel, true, "Dark October", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_DarkOctober, false, "Dark October", "TownPalette");

			LoadRessource(InitialTextures.TownColor_Bricks_fun, true, "HD Pink Bricks", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Pastel, true, "HD Pink Bricks", "TownMaterial");

			LoadRessource(InitialTextures.TownColor_Modern_cobbled, true, "HD Cobbled", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Pastel, true, "HD Cobbled", "TownMaterial");

			LoadRessource(InitialTextures.TownColor_Halloween, true, "Halloween", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_Pastel, true, "Halloween", "TownMaterial");

			LoadRessource(InitialTextures.TownColor_GoldenAge, true, "Golden Age", "TownColor");
			LoadRessource(InitialTextures.TownMaterial_GoldenAge, true, "Golden Age", "TownMaterial");
			LoadRessource(InitialTextures.TownPalette_GoldenAge, false, "Golden Age", "TownPalette");
		}

		public static void Initialize()
        {
			LocatePaletteAnchors();
			if (Directory.Exists(modTextureFolderPath) == false)
            {
				FirstStart();
            }

			//themeList = Directory.EnumerateDirectories(modTextureFolderPath).ToList<string>();
			string[] themeListPaths = Directory.GetDirectories(modTextureFolderPath);
            string[] paletteListPaths = Directory.GetFiles(modTextureFolderPath + "/Palettes");
			foreach (string themePath in themeListPaths)
            {
				themeList.Add(Path.GetFileName(themePath));
            }
			themeList.Remove("Palettes");
			themeList.Sort();
			themeList.Insert(0, "DEFAULT");

			foreach (string palettePath in paletteListPaths)
			{
                paletteList.Add(Path.GetFileName(palettePath).Replace(".png",""));
			}
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
			/*MelonLogger.Msg("UpdateT started");
			MelonLogger.Msg("CurrentTheme : " + currentTheme);
			MelonLogger.Msg("CurrentPalette : " + currentPalette);*/
			//Specific case for default
			if (theme && currentTheme == "DEFAULT")
            {
				try
                {
					File.Delete(textureFolderPath + "/TownColor.png");
					File.Delete(textureFolderPath + "/TownMaterial.png");
					File.Delete(textureFolderPath + "/TownPalette.png");

					currentPalette = "DEFAULT";
					TextureManagerUI.refSelectPalette.selectValue = currentPalette;
					TextureManagerUI.refSelectPalette.textField.text = currentPalette;
				}
				catch (IOException)
				{
					return false;
				}

				//Update real-time
				master.texturePngMaster.OnApplicationFocus(true);

				//Texture update
				//currentTownColorTex = defaultTownColorTex;
				//currentTownMaterialTex = defaultTownMaterialTex;
				return true;
			}
			else if (!theme && currentPalette == "DEFAULT")
            {
				try
                {
					File.Delete(textureFolderPath + "/TownPalette.png");
                }
				catch (IOException)
				{
					return false;
				}

				//Update real-time
				master.texturePngMaster.OnApplicationFocus(true);

				//Texture update
				//currentPaletteTex = defaultPaletteTex;
				return true;
			}
			//Get current path
			if (theme)
			{
				string pathTownColor = modTextureFolderPath + "/" + currentTheme + "/TownColor.png";
				if (!UpdateInGameTex(pathTownColor, textureFolderPath + "/TownColor.png"))
				{
					return false;
				}

				string pathTownMaterial = modTextureFolderPath + "/" + currentTheme + "/TownMaterial.png";
				if (!UpdateInGameTex(pathTownMaterial, textureFolderPath + "/TownMaterial.png"))
				{
					return false;
				}
				else
                {
					//Check for corresponding Palette
					currentPalette = paletteList.Contains(currentTheme) ? currentTheme : "DEFAULT";
					TextureManagerUI.refSelectPalette.selectValue = currentPalette;
					TextureManagerUI.refSelectPalette.textField.text = currentPalette;
					UpdateTextures(false);

                }
			}
			else
			{
				string pathPalette = modTextureFolderPath + "/Palettes/" + currentPalette + ".png";
				if (!UpdateInGameTex(pathPalette, textureFolderPath + "/TownPalette.png"))
				{
					MelonLogger.Error("Issue");
					return false;
				}
			}

			//Update real-time
			master.texturePngMaster.OnApplicationFocus(true);

			//Update UI textures
			/*if (theme)
			{
				ImageConversion.LoadImage(currentTownColorTex, File.ReadAllBytes(textureFolderPath + "/TownColor.png"));
				ImageConversion.LoadImage(currentTownMaterialTex, File.ReadAllBytes(textureFolderPath + "/TownMaterial.png"));
			}
			else
			{
				ImageConversion.LoadImage(currentPaletteTex, File.ReadAllBytes(textureFolderPath + "/TownPalette.png"));
			}*/
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
					currentTheme = themeList[themeList.Count - 1];
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
					currentPalette = paletteList[paletteList.Count - 1];
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
				openFileDialog.InitialDirectory = "c:/";
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
				string themeDirectory = modTextureFolderPath + "/" + newName;

				//Case no name has been provided
				int c = 1;
				while(Directory.Exists(themeDirectory))
                {
					newName = initialName + " " + c.ToString();
					themeDirectory = modTextureFolderPath + "/" + newName;
					c++;
				}

				//After the name is chosen
				Directory.CreateDirectory(themeDirectory);
				if (newTownColor == "")
                {
					ImageConverter converter = new ImageConverter();
					byte[] bytes = (byte[])converter.ConvertTo(InitialTextures.TownColor_default, typeof(byte[]));
					File.WriteAllBytes(themeDirectory + "/TownColor.png", bytes);
				}
				else
                {
					File.Copy(newTownColor, themeDirectory + "/TownColor.png");
                }

				if (newTownMaterial == "")
				{
					ImageConverter converter = new ImageConverter();
					byte[] bytes = (byte[])converter.ConvertTo(InitialTextures.TownMaterial_default, typeof(byte[]));
					File.WriteAllBytes(themeDirectory + "/TownMaterial.png", bytes);
				}
				else
                {
					File.Copy(newTownMaterial, themeDirectory + "/TownMaterial.png");
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
				while (File.Exists(modTextureFolderPath + "/Palettes/" + newName + ".png"))
                {
					newName = initialName + " " + c.ToString();
					c++;
				}

				//Adding the new Palette
				if (newTownPalette != "")
                {
					File.Copy(newTownPalette, modTextureFolderPath + "/Palettes/" + newName + ".png");
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
					Directory.Delete(modTextureFolderPath + "/" + currentTheme, true);
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
					File.Delete(modTextureFolderPath + "/" + currentPalette + ".png");
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
			if (master == null)
			{
				GameObject masterGameObj = GameObject.Find("WorldMaster");
				if (masterGameObj != null)
				{
					master = masterGameObj.GetComponent<WorldMaster>();
				}
			}
			else
            {
				if (anchor0 != null)
				{
					MovePalette();
				}
				else
                {
					LocatePaletteAnchors();

				}
			}

			/*//Keyboard control
			if (Input.GetKeyDown(KeyCode.Keypad6))
            {
				TextureManagerUI.NextTheme();
				TextureManagerUI.refSelectTheme.UpdateSettingsValue();
            }
			else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
				TextureManagerUI.PrevTheme();
				TextureManagerUI.refSelectTheme.UpdateSettingsValue();
			}
			else if (Input.GetKeyDown(KeyCode.Keypad2))
			{
				TextureManagerUI.PrevPalette();
				TextureManagerUI.refSelectPalette.UpdateSettingsValue();
			}
			else if (Input.GetKeyDown(KeyCode.Keypad8))
			{
				TextureManagerUI.NextPalette();
				TextureManagerUI.refSelectPalette.UpdateSettingsValue();
			}

			//Camera move
			if (orbit == null)
            {
                orbit = GameObject.Find("OrbitalCamera")?.GetComponent<Placemaker.Ui.OrbitalCamera>();
            }
			else if (Input.GetKey(KeyCode.Keypad1))
            {
				orbit.RotateCamera(rota);
            }
			else if (Input.GetKey(KeyCode.Keypad3))
			{
				orbit.WASDCamera(pan);
			}
			else if (Input.GetKey(KeyCode.Keypad9))
			{
				orbit.GamepadZoom(zoom);
			}*/
		}

	}
}

