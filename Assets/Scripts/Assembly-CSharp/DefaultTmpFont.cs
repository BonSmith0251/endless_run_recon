using TMPro;
using UnityEngine;
using UnityEngine.Localization;

[ExecuteInEditMode]
public class DefaultTmpFont : MonoBehaviour
{
	public LocalizedTmpFont localizedFont = new LocalizedTmpFont();

	public bool updateExistingText = true;

	private void OnEnable()
	{
		localizedFont.AssetChanged += LocalizedFont_AssetChanged;
	}

	private void OnDisable()
	{
		localizedFont.AssetChanged -= LocalizedFont_AssetChanged;
	}

	private void LocalizedFont_AssetChanged(TMP_FontAsset value)
	{
		// TMP 3.0.7 (Unity 2022.3) has no public setter for the default font.
		typeof(TMP_Settings).GetField("m_defaultFontAsset", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
			.SetValue(TMP_Settings.instance, value);
		if (updateExistingText)
		{
			TextMeshProUGUI[] array = Object.FindObjectsOfType<TextMeshProUGUI>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].font = null;
			}
		}
	}
}
