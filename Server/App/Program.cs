﻿using System;
using System.Net;
using System.Threading;
using Model;
using NLog;

namespace App
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			// 异步方法全部会回掉到主线程
			OneThreadSynchronizationContext contex = new OneThreadSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(contex);

			MongoHelper.Init();
			
			try
			{
				ObjectEvents.Instance.Add("Model", typeof(Game).Assembly);
				ObjectEvents.Instance.Add("Hotfix", DllHelper.GetHotfixAssembly());

				Options options = Game.Scene.AddComponent<OptionComponent, string[]>(args).Options;
				StartConfig startConfig = Game.Scene.AddComponent<StartConfigComponent, string, int>(options.Config, options.AppId).StartConfig;
                Log.Info(startConfig.ToString()+":startconfig"+startConfig.ServerIP.ToString()+":IP"+startConfig.Id.ToString());
				if (!options.AppType.Is(startConfig.AppType))
				{
					Log.Error("命令行参数apptype与配置不一致");
					return;
				}

				IdGenerater.AppId = options.AppId;

				LogManager.Configuration.Variables["appType"] = startConfig.AppType.ToString();
				LogManager.Configuration.Variables["appId"] = startConfig.AppId.ToString();
				LogManager.Configuration.Variables["appTypeFormat"] = $"{startConfig.AppType,-8}";
				LogManager.Configuration.Variables["appIdFormat"] = $"{startConfig.AppId:D3}";

				Log.Info("server start........................");

				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatherComponent>();

				// 根据不同的AppType添加不同的组件
				OuterConfig outerConfig = startConfig.GetComponent<OuterConfig>();
				InnerConfig innerConfig = startConfig.GetComponent<InnerConfig>();
				ClientConfig clientConfig = startConfig.GetComponent<ClientConfig>();
               
                //Log.Info(clientConfig.ToString() + ":clientConfig" + clientConfig.Address.ToString() + ":IP" + clientConfig.Port.ToString() + ":Port");
                switch (startConfig.AppType)
				{
					case AppType.Manager:
						Game.Scene.AddComponent<NetInnerComponent, IPEndPoint>(innerConfig.IPEndPoint);
						Game.Scene.AddComponent<NetOuterComponent, IPEndPoint>(outerConfig.IPEndPoint);
						Game.Scene.AddComponent<AppManagerComponent>();
                        Log.Info(":outerConfig--1:" + outerConfig.Address.ToString());
                        Log.Info(":outerConfig--2:" + outerConfig.Address2.ToString());
                        break;
                    case AppType.DB:
                        Game.Scene.AddComponent<NetInnerComponent, IPEndPoint>(innerConfig.IPEndPoint);
                        Game.Scene.AddComponent<DBComponent>();
                        Game.Scene.AddComponent<DBCacheComponent>();                      
                        break;
					case AppType.Realm:
						Game.Scene.AddComponent<ActorMessageDispatherComponent>();
						Game.Scene.AddComponent<ActorManagerComponent>();
						Game.Scene.AddComponent<NetInnerComponent, IPEndPoint>(innerConfig.IPEndPoint);
						Game.Scene.AddComponent<NetOuterComponent, IPEndPoint>(outerConfig.IPEndPoint);
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<RealmGateAddressComponent>();
                        Game.Scene.AddComponent<PlayerLoginManagerComponent>();
                        Game.Scene.AddComponent<DBProxyComponent>();
                        Log.Info(":outerConfig--1:" + outerConfig.Address.ToString());
                        Log.Info(":outerConfig--2:" + outerConfig.Address2.ToString());
                        break;
					case AppType.Gate:
						Game.Scene.AddComponent<PlayerComponent>();
						Game.Scene.AddComponent<ActorMessageDispatherComponent>();
						Game.Scene.AddComponent<ActorManagerComponent>();
						Game.Scene.AddComponent<NetInnerComponent, IPEndPoint>(innerConfig.IPEndPoint);
						Game.Scene.AddComponent<NetOuterComponent, IPEndPoint>(outerConfig.IPEndPoint);
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<ActorProxyComponent>();
						Game.Scene.AddComponent<GateSessionKeyComponent>();
                        Game.Scene.AddComponent<DBProxyComponent>();
                        Log.Info(":outerConfig--1:" + outerConfig.Address.ToString());
                        Log.Info(":outerConfig--2:" + outerConfig.Address2.ToString());
                        break;
					case AppType.Location:
						Game.Scene.AddComponent<NetInnerComponent, IPEndPoint>(innerConfig.IPEndPoint);
						Game.Scene.AddComponent<LocationComponent>();                       
                        break;
					case AppType.Map:
						Game.Scene.AddComponent<NetInnerComponent, IPEndPoint>(innerConfig.IPEndPoint);
						Game.Scene.AddComponent<ActorManagerComponent>();
						Game.Scene.AddComponent<UnitComponent>();
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<ActorProxyComponent>();
						Game.Scene.AddComponent<ActorMessageDispatherComponent>();
						Game.Scene.AddComponent<ServerFrameComponent>();
                        Game.Scene.AddComponent<RoomComponent>();
                        Game.Scene.AddComponent<DBProxyComponent>();                     
                        break;
                    case AppType.Match:
                        Game.Scene.AddComponent<NetInnerComponent,IPEndPoint>(innerConfig.IPEndPoint);
                        Game.Scene.AddComponent<ActorManagerComponent>();
                        Game.Scene.AddComponent<LocationProxyComponent>();
                        Game.Scene.AddComponent<ActorProxyComponent>();
                        Game.Scene.AddComponent<ActorMessageDispatherComponent>();
                        Game.Scene.AddComponent<MatcherComponent>();
                        Game.Scene.AddComponent<MatchComponent>();
                        Game.Scene.AddComponent<RoomManagerComponent>();
                        Game.Scene.AddComponent<RealmMapAddressComponent>();                     
                        break;
					case AppType.AllServer:
						Game.Scene.AddComponent<ActorProxyComponent>();
						Game.Scene.AddComponent<PlayerComponent>();
						Game.Scene.AddComponent<UnitComponent>();                       
						Game.Scene.AddComponent<DBComponent>();
                        Game.Scene.AddComponent<DBCacheComponent>();
						Game.Scene.AddComponent<DBProxyComponent>();
						Game.Scene.AddComponent<LocationComponent>();
						Game.Scene.AddComponent<ActorMessageDispatherComponent>();
						Game.Scene.AddComponent<ActorManagerComponent>();
						Game.Scene.AddComponent<NetInnerComponent, IPEndPoint>(innerConfig.IPEndPoint);
						Game.Scene.AddComponent<NetOuterComponent, IPEndPoint>(outerConfig.IPEndPoint);
						Game.Scene.AddComponent<LocationProxyComponent>();
						Game.Scene.AddComponent<AppManagerComponent>();
						Game.Scene.AddComponent<RealmGateAddressComponent>();
						Game.Scene.AddComponent<GateSessionKeyComponent>();
						Game.Scene.AddComponent<ConfigComponent>();
						Game.Scene.AddComponent<ServerFrameComponent>();
                        Game.Scene.AddComponent<PlayerLoginManagerComponent>();
                        Game.Scene.AddComponent<MatcherComponent>();
                        Game.Scene.AddComponent<MatchComponent>();
                        Game.Scene.AddComponent<RoomManagerComponent>();
                        Game.Scene.AddComponent<RealmMapAddressComponent>();
                        Game.Scene.AddComponent<RoomComponent>();
						Game.Scene.AddComponent<SystemHeatkickExeComponent,long>(10*1000);
                        Log.Info(":outerConfig--1:" + outerConfig.Address.ToString());
                        Log.Info(":outerConfig--2:" + outerConfig.Address2.ToString());
                        break;
					case AppType.Benchmark:
						Game.Scene.AddComponent<NetOuterComponent>();
						Game.Scene.AddComponent<BenchmarkComponent, IPEndPoint>(clientConfig.IPEndPoint);
						break;
					default:
						throw new Exception($"命令行参数没有设置正确的AppType: {startConfig.AppType}");
				}

				while (true)
				{
					try
					{
						Thread.Sleep(1);
						contex.Update();
						ObjectEvents.Instance.Update();
					}
					catch (Exception e)
					{
						Log.Error(e.ToString());
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}
	}
}
