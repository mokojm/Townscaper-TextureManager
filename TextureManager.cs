using MelonLoader;
using UnityEngine;

namespace TextureManager
{
    public class TextureManagerMain : MelonMod
    {

		//Plants
		public static Color defaultPlantColor = new Color (0.362f, 0.5569f, 0.4292f, 1f);
		public static Color plantColor = new Color(0.362f, 0.5569f, 0.4292f, 1f);
		public static Material plantMat;
		public static bool isInitialized = false;




		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			if (sceneName == "Placemaker")
			{
				MelonLogger.Msg("Main scene loaded");

				// Initializing
				//Initialize();
				TextureManagerUI.Initialize(this);

			}
		}

		public static void Initialize()
        {

        }

		public static void Reset()
		{

		}

		public override void OnApplicationStart()
		{
			
		}

		public override void OnUpdate()
		{

		}

	}
}

