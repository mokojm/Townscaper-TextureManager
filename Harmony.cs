using ModUI;
using System.Collections;

namespace TextureManager
{
	public class Harmony_Main
	{
		[HarmonyLib.HarmonyPatch(typeof(UIManager), "ToggleUI")]
		public class TextureManager
		{


			public static void Postfix()
			{
			}
		}
	}
}
				
