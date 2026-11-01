using UnityEngine;

// Offline build: Steamworks.NET and steam_api64.dll were removed. The component stays so the
// scene keeps its original object; Steam is never initialized and nothing connects to Steam.
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	public static bool Initialized => false;
}
