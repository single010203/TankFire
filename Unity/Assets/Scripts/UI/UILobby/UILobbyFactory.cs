﻿using System;
using UnityEngine;

namespace Model
{
    [UIFactory((int)UIType.UILobby)]
    public class UILobbyFactory : IUIFactory
    {
        public UI Create(Scene scene, UIType type, GameObject gameObject)
        {
	        try
	        {
				ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
		        resourcesComponent.LoadBundle($"{type}.unity3d");
				GameObject bundleGameObject = resourcesComponent.GetAsset<GameObject>($"{type}.unity3d", $"{type}");
				GameObject lobby = UnityEngine.Object.Instantiate(bundleGameObject);
				lobby.layer = LayerMask.NameToLayer(LayerNames.UI);
				UI ui = EntityFactory.Create<UI, Scene, UI, GameObject>(scene, null, lobby);

				ui.AddComponent<UILobbyComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e.ToString());
		        return null;
	        }
		}

	    public void Remove(UIType type)
	    {
		    Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
		}
    }
}