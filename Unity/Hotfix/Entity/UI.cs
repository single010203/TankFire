﻿using System.Collections.Generic;
using UnityEngine;
using Model;

namespace Hotfix
{
	[Model.ObjectEvent]
	public class UIEvent : ObjectEvent<UI>, IAwake<Scene, UI, GameObject>
	{
		public void Awake(Scene scene, UI parent, GameObject gameObject)
		{
			this.Get().Awake(scene, parent, gameObject);
		}
	}
	
	
	public sealed class UI: Entity
	{
		public Scene Scene { get; set; }

		public UIType UIType { get; }

		public string Name
		{
			get
			{
				return this.GameObject.name;
			}
		}

		public GameObject GameObject { get; private set; }

		public Dictionary<string, UI> children = new Dictionary<string, UI>();

        public UI() { }
		
		public void Awake(Scene scene, UI parent, GameObject gameObject)
		{
			this.children.Clear();
			
			this.Scene = scene;

			if (parent != null)
			{
				gameObject.transform.SetParent(parent.GameObject.transform, false);
			}
			this.GameObject = gameObject;
		}

		public override void Dispose()
		{
			if (this.Id == 0)
			{
				return;
			}
			
			base.Dispose();

			foreach (UI ui in this.children.Values)
			{
				ui.Dispose();
			}
			
			UnityEngine.Object.Destroy(GameObject);
			children.Clear();
		}

		public void SetAsFirstSibling()
		{
			this.GameObject.transform.SetAsFirstSibling();
		}

		public UI(Scene scene, UIType uiType, UI parent, GameObject gameObject)
		{
			this.Scene = scene;
			this.UIType = uiType;

			if (parent != null)
			{
				gameObject.transform.SetParent(parent.GameObject.transform, false);
			}
			this.GameObject = gameObject;
		}

		public void Add(UI ui)
		{
			this.children.Add(ui.Name, ui);
            ui.Parent = this;
		}

		public void Remove(string name)
		{
			UI ui;
			if (!this.children.TryGetValue(name, out ui))
			{
				return;
			}
			this.children.Remove(name);
			ui.Dispose();
		}

		public UI Get(string name)
		{
			UI child;
			if (this.children.TryGetValue(name, out child))
			{
				return child;
			}
			GameObject childGameObject = this.GameObject.transform.Find(name)?.gameObject;
			if (childGameObject == null)
			{
				return null;
			}
			child = EntityFactory.Create<UI, Scene, UI, GameObject>(this.Scene, this, childGameObject);
			this.Add(child);
			return child;
		}
	}
}