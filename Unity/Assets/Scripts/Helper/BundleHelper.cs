﻿using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Model
{
	public static class BundleHelper
	{
		public static async Task DownloadBundle()
		{
			Game.Scene.GetComponent<EventComponent>().Run(EventIdType.LoadingBegin);
			await StartDownLoadResources();
			Game.Scene.GetComponent<EventComponent>().Run(EventIdType.LoadingFinish);
		}
		
		public static async Task StartDownLoadResources()
		{
			if (Define.IsAsync)
			{
				try
				{
					//using (BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.AddComponent<BundleDownloaderComponent>())
					//{
					//	await bundleDownloaderComponent.StartAsync();
					//}
					//Game.Scene.GetComponent<ResourcesComponent>().LoadOneBundle("StreamingAssets");
					//ResourcesComponent.AssetBundleManifestObject = Game.Scene.GetComponent<ResourcesComponent>().GetAsset<AssetBundleManifest>("StreamingAssets", "AssetBundleManifest");
				}
				catch (Exception e)
				{
					Log.Error(e.ToString());
				}

			}
		}
	}
}
