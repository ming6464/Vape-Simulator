// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR
 
using UnityEngine;
using UnityEditor;

namespace Manul.Toolbar
{
	public enum Message
	{
		None,
		NoSettingsAsset,
		NoButtonListAsset,
		NoButtonListSetAsset,
		NoIndexInButtonListSetAsset,
		NoButtonListAssetInButtonListSetAsset,
		EmptyPrefFieldForSet,
		CantFindGameObject,
		CantFindFolder,
		TwoOrMoreSetingsFiles
	}

	public static class ManulToolbarMessages
	{
		static string messagePrefix = "[MANUL Inspector message] ";

		public static void ShowMessage(Message type, MessageType messageType, string[] textItems)
		{
			string messageText = messagePrefix;

			switch (type)
			{
				case Message.NoSettingsAsset:
					messageText += "Manul Toolbar Settings asset cannot be found in the Resources folder.";
					Debug.LogWarning(messageText); 
					return;
			} 

			if (!ManulToolbar.settings.settings.showConsoleMessages) return;

			switch (type)
			{
				case Message.None:
					messageText += "None";
					break;

				case Message.NoButtonListAsset:
					messageText += "Manul Toolbar Button List asset for " + textItems[0] + " toolbar side is missing! Please add this asset to the 'Override " + textItems[1] + " Side' field in the Manul Toolbar Settings.";
					break;

				case Message.NoButtonListSetAsset:
					messageText += "Manul Toolbar Button List Set asset for " + textItems[0] + " toolbar side is missing! Please add this asset to the 'Override " + textItems[1] + " Side' field in the Manul Toolbar Settings.";
					break;

				case Message.NoIndexInButtonListSetAsset:
					messageText += "Manul Toolbar Button List Set asset in the 'Override " + textItems[0] + " Side' field does not have an entry with index: " + textItems[1];
					break;

				case Message.NoButtonListAssetInButtonListSetAsset:
					messageText += "Manul Toolbar Button List Set asset in the 'Override " + textItems[0] + " Side' field has an entry with index " + textItems[1] + ", but this entry does not have the Manul Toolbar Button List asset.";
					break;

				case Message.EmptyPrefFieldForSet:
					messageText += "Manul Toolbar Button List Set asset in the 'Override " + textItems[0] + " Side' field has an empty 'Int Pref Name' field.";
					break;

				case Message.CantFindGameObject:
					messageText += "Could not find GameObject with name '" + textItems[0] + "' in the current opened scene.";
					break;

				case Message.CantFindFolder:
					messageText += "Cannot find a folder with the path: '" + textItems[0] + "'.";
					break;

				case Message.TwoOrMoreSetingsFiles:
					messageText +=
						"You have two or more Manul Toolbar Settings assets named 'Manul Toolbar', but they are located in different 'Resources' folders. " +
						"There must be exactly one Manul Toolbar Settings asset named 'Manul Toolbar' in exactly one 'Resource' folder, " +
						"otherwise you will encounter problems while using Manul Toolbar tool.";
					break;
			}

			switch (messageType)
			{
				case MessageType.Info: Debug.Log(messageText); break;
				case MessageType.Warning: Debug.LogWarning(messageText); break;
				case MessageType.Error: Debug.LogError(messageText); break;
			}
		}
	}
}

#endif


 