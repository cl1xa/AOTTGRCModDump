using System;
using System.Collections.Generic;
using Anticheat;
using ExitGames.Client.Photon;
using Photon;
using Settings;
using UI;
using UnityEngine;

public class InRoomChat : Photon.MonoBehaviour
{
	private bool AlignBottom = true;

	public static readonly string ChatRPC = "Chat";

	public static Rect GuiRect = new Rect(0f, 100f, 300f, 470f);

	public static Rect GuiRect2 = new Rect(30f, 575f, 300f, 25f);

	private string inputLine = string.Empty;

	public bool IsVisible = true;

	public static LinkedList<string> messages = new LinkedList<string>();

	private float deltaTime;

	private int _maxLines = 15;

	private void ShowFPS()
	{
		Rect position = new Rect((float)Screen.width / 4f - 75f, 10f, 150f, 30f);
		int num = (int)Math.Round(1f / deltaTime);
		GUI.Label(position, $"FPS: {num}");
	}

	private void ShowMessageWindow()
	{
		GUI.SetNextControlName(string.Empty);
		GUILayout.BeginArea(GuiRect);
		GUILayout.FlexibleSpace();
		string text = string.Empty;
		foreach (string message in messages)
		{
			text = text + message + "\n";
		}
		GUILayout.Label(text);
		GUILayout.EndArea();
	}

	public void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}

	public void addLINE(string newLine)
	{
		newLine = newLine.FilterSizeTag();
		messages.AddLast(newLine);
		while (messages.Count > _maxLines)
		{
			messages.RemoveFirst();
		}
	}

	public void OnGUI()
	{
		if (SettingsManager.GraphicsSettings.ShowFPS.Value)
		{
			ShowFPS();
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && SettingsManager.UISettings.GameFeed.Value)
		{
			ShowMessageWindow();
		}
		if (!IsVisible || PhotonNetwork.connectionStateDetailed != PeerStates.Joined)
		{
			return;
		}
		if (Event.current.type == EventType.KeyDown)
		{
			if ((Event.current.keyCode != KeyCode.Tab && Event.current.character != '\t') || GameMenu.Paused || SettingsManager.InputSettings.General.Chat.Contains(KeyCode.Tab))
			{
				goto IL_012a;
			}
			Event.current.Use();
		}
		else
		{
			if (Event.current.type != EventType.KeyUp || Event.current.keyCode == KeyCode.None || !SettingsManager.InputSettings.General.Chat.Contains(Event.current.keyCode) || !(GUI.GetNameOfFocusedControl() != "ChatInput") || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return)
			{
				goto IL_012a;
			}
			inputLine = string.Empty;
			GUI.FocusControl("ChatInput");
		}
		goto IL_1c1a;
		IL_012a:
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
		{
			if (!string.IsNullOrEmpty(inputLine))
			{
				if (inputLine == "\t")
				{
					inputLine = string.Empty;
					GUI.FocusControl(string.Empty);
					return;
				}
				if (FengGameManagerMKII.RCEvents.ContainsKey("OnChatInput"))
				{
					string key = (string)FengGameManagerMKII.RCVariableNames["OnChatInput"];
					if (FengGameManagerMKII.stringVariables.ContainsKey(key))
					{
						FengGameManagerMKII.stringVariables[key] = inputLine;
					}
					else
					{
						FengGameManagerMKII.stringVariables.Add(key, inputLine);
					}
					((RCEvent)FengGameManagerMKII.RCEvents["OnChatInput"]).checkEvent();
				}
				if (!inputLine.StartsWith("/"))
				{
					string text = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]).hexColor();
					if (text == string.Empty)
					{
						text = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]);
						if (PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam] != null)
						{
							if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 1)
							{
								text = "<color=#00FFFF>" + text + "</color>";
							}
							else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 2)
							{
								text = "<color=#FF00FF>" + text + "</color>";
							}
						}
					}
					object[] parameters = new object[2] { inputLine, text };
					FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
				}
				else if (inputLine.StartsWith("/aso"))
				{
					if (PhotonNetwork.isMasterClient)
					{
						LegacyGameSettings legacyGameSettings = SettingsManager.LegacyGameSettings;
						LegacyGameSettings legacyGameSettingsUI = SettingsManager.LegacyGameSettingsUI;
						string text2 = inputLine.Substring(5);
						if (!(text2 == "kdr"))
						{
							if (text2 == "racing")
							{
								if (!legacyGameSettings.RacingEndless.Value)
								{
									BoolSetting racingEndless = legacyGameSettings.RacingEndless;
									bool value = (legacyGameSettingsUI.RacingEndless.Value = true);
									racingEndless.Value = value;
									addLINE("<color=#FFCC00>Endless racing enabled.</color>");
								}
								else
								{
									BoolSetting racingEndless2 = legacyGameSettings.RacingEndless;
									bool value = (legacyGameSettingsUI.RacingEndless.Value = false);
									racingEndless2.Value = value;
									addLINE("<color=#FFCC00>Endless racing disabled.</color>");
								}
							}
						}
						else if (!legacyGameSettings.PreserveKDR.Value)
						{
							legacyGameSettings.PreserveKDR.Value = true;
							legacyGameSettingsUI.PreserveKDR.Value = true;
							addLINE("<color=#FFCC00>KDRs will be preserved from disconnects.</color>");
						}
						else
						{
							legacyGameSettings.PreserveKDR.Value = false;
							legacyGameSettingsUI.PreserveKDR.Value = false;
							addLINE("<color=#FFCC00>KDRs will not be preserved from disconnects.</color>");
						}
					}
				}
				else if (inputLine == "/pause")
				{
					if (PhotonNetwork.isMasterClient)
					{
						FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, true);
						object[] parameters2 = new object[2] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
						FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters2);
					}
					else
					{
						addLINE("<color=#FFCC00>error: not master client</color>");
					}
				}
				else if (inputLine == "/unpause")
				{
					if (PhotonNetwork.isMasterClient)
					{
						FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, false);
						object[] parameters2 = new object[2] { "<color=#FFCC00>MasterClient has unpaused the game.</color>", "" };
						FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters2);
					}
					else
					{
						addLINE("<color=#FFCC00>error: not master client</color>");
					}
				}
				else if (inputLine == "/checklevel")
				{
					PhotonPlayer[] playerList = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer in playerList)
					{
						addLINE(RCextensions.returnStringFromObject(photonPlayer.customProperties[PhotonPlayerProperty.currentLevel]));
					}
				}
				else if (inputLine == "/isrc")
				{
					if (FengGameManagerMKII.masterRC)
					{
						addLINE("is RC");
					}
					else
					{
						addLINE("not RC");
					}
				}
				else if (inputLine == "/ignorelist")
				{
					foreach (int ignore in FengGameManagerMKII.ignoreList)
					{
						addLINE(ignore.ToString());
					}
				}
				else if (inputLine.StartsWith("/room"))
				{
					if (PhotonNetwork.isMasterClient)
					{
						if (inputLine.Substring(6).StartsWith("max"))
						{
							int maxPlayers = Convert.ToInt32(inputLine.Substring(10));
							FengGameManagerMKII.instance.maxPlayers = maxPlayers;
							PhotonNetwork.room.maxPlayers = maxPlayers;
							object[] parameters2 = new object[2]
							{
								"<color=#FFCC00>Max players changed to " + inputLine.Substring(10) + "!</color>",
								""
							};
							FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters2);
						}
						else if (inputLine.Substring(6).StartsWith("time"))
						{
							FengGameManagerMKII.instance.addTime(Convert.ToSingle(inputLine.Substring(11)));
							object[] parameters2 = new object[2]
							{
								"<color=#FFCC00>" + inputLine.Substring(11) + " seconds added to the clock.</color>",
								""
							};
							FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters2);
						}
					}
					else
					{
						addLINE("<color=#FFCC00>error: not master client</color>");
					}
				}
				else if (inputLine.StartsWith("/resetkd"))
				{
					if (inputLine == "/resetkdall")
					{
						if (PhotonNetwork.isMasterClient)
						{
							PhotonPlayer[] playerList = PhotonNetwork.playerList;
							for (int i = 0; i < playerList.Length; i++)
							{
								playerList[i].SetCustomProperties(new Hashtable
								{
									{
										PhotonPlayerProperty.kills,
										0
									},
									{
										PhotonPlayerProperty.deaths,
										0
									},
									{
										PhotonPlayerProperty.max_dmg,
										0
									},
									{
										PhotonPlayerProperty.total_dmg,
										0
									}
								});
							}
							object[] parameters2 = new object[2] { "<color=#FFCC00>All stats have been reset.</color>", "" };
							FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters2);
						}
						else
						{
							addLINE("<color=#FFCC00>error: not master client</color>");
						}
					}
					else
					{
						Hashtable hashtable = new Hashtable();
						hashtable.Add(PhotonPlayerProperty.kills, 0);
						hashtable.Add(PhotonPlayerProperty.deaths, 0);
						hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
						hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
						PhotonNetwork.player.SetCustomProperties(hashtable);
						addLINE("<color=#FFCC00>Your stats have been reset. </color>");
					}
				}
				else if (inputLine.StartsWith("/pm"))
				{
					string[] array = inputLine.Split(' ');
					PhotonPlayer photonPlayer2 = PhotonPlayer.Find(Convert.ToInt32(array[1]));
					string text = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]).hexColor();
					if (text == string.Empty)
					{
						text = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]);
						if (PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam] != null)
						{
							if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 1)
							{
								text = "<color=#00FFFF>" + text + "</color>";
							}
							else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 2)
							{
								text = "<color=#FF00FF>" + text + "</color>";
							}
						}
					}
					string text3 = RCextensions.returnStringFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.name]).hexColor();
					if (text3 == string.Empty)
					{
						text3 = RCextensions.returnStringFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.name]);
						if (photonPlayer2.customProperties[PhotonPlayerProperty.RCteam] != null)
						{
							if (RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.RCteam]) == 1)
							{
								text3 = "<color=#00FFFF>" + text3 + "</color>";
							}
							else if (RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.RCteam]) == 2)
							{
								text3 = "<color=#FF00FF>" + text3 + "</color>";
							}
						}
					}
					string text4 = string.Empty;
					for (int j = 2; j < array.Length; j++)
					{
						text4 = text4 + array[j] + " ";
					}
					FengGameManagerMKII.instance.photonView.RPC("ChatPM", photonPlayer2, text, text4);
					addLINE("<color=#FFC000>TO [" + photonPlayer2.ID + "]</color> " + text3 + ":" + text4);
				}
				else if (inputLine.StartsWith("/team"))
				{
					if (SettingsManager.LegacyGameSettings.TeamMode.Value == 1)
					{
						if (inputLine.Substring(6) == "1" || inputLine.Substring(6) == "cyan")
						{
							FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 1);
							addLINE("<color=#00FFFF>You have joined team cyan.</color>");
							GameObject[] array2 = GameObject.FindGameObjectsWithTag("Player");
							foreach (GameObject gameObject in array2)
							{
								if (gameObject.GetPhotonView().isMine)
								{
									gameObject.GetComponent<HERO>().markDie();
									gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
								}
							}
						}
						else if (inputLine.Substring(6) == "2" || inputLine.Substring(6) == "magenta")
						{
							FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 2);
							addLINE("<color=#FF00FF>You have joined team magenta.</color>");
							GameObject[] array2 = GameObject.FindGameObjectsWithTag("Player");
							foreach (GameObject gameObject2 in array2)
							{
								if (gameObject2.GetPhotonView().isMine)
								{
									gameObject2.GetComponent<HERO>().markDie();
									gameObject2.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
								}
							}
						}
						else if (inputLine.Substring(6) == "0" || inputLine.Substring(6) == "individual")
						{
							FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 0);
							addLINE("<color=#00FF00>You have joined individuals.</color>");
							GameObject[] array2 = GameObject.FindGameObjectsWithTag("Player");
							foreach (GameObject gameObject3 in array2)
							{
								if (gameObject3.GetPhotonView().isMine)
								{
									gameObject3.GetComponent<HERO>().markDie();
									gameObject3.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
								}
							}
						}
						else
						{
							addLINE("<color=#FFCC00>error: invalid team code. Accepted values are 0,1, and 2.</color>");
						}
					}
					else
					{
						addLINE("<color=#FFCC00>error: teams are locked or disabled. </color>");
					}
				}
				else if (inputLine == "/restart")
				{
					if (PhotonNetwork.isMasterClient)
					{
						object[] parameters2 = new object[2] { "<color=#FFCC00>MasterClient has restarted the game!</color>", "" };
						FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters2);
						FengGameManagerMKII.instance.restartRC();
					}
					else
					{
						addLINE("<color=#FFCC00>error: not master client</color>");
					}
				}
				else if (inputLine.StartsWith("/specmode"))
				{
					SettingsManager.LegacyGeneralSettings.SpecMode.Value = !SettingsManager.LegacyGeneralSettings.SpecMode.Value;
					if (SettingsManager.LegacyGeneralSettings.SpecMode.Value)
					{
						FengGameManagerMKII.instance.EnterSpecMode(enter: true);
						addLINE("<color=#FFCC00>You have entered spectator mode.</color>");
					}
					else
					{
						FengGameManagerMKII.instance.EnterSpecMode(enter: false);
						addLINE("<color=#FFCC00>You have exited spectator mode.</color>");
					}
				}
				else if (inputLine.StartsWith("/fov"))
				{
					int num = Convert.ToInt32(inputLine.Substring(5));
					Camera.main.fieldOfView = num;
					addLINE("<color=#FFCC00>Field of vision set to " + num + ".</color>");
				}
				else if (inputLine == "/clear")
				{
					messages.Clear();
				}
				else if (inputLine.StartsWith("/spectate"))
				{
					int num2 = Convert.ToInt32(inputLine.Substring(10));
					GameObject[] array2 = GameObject.FindGameObjectsWithTag("Player");
					foreach (GameObject gameObject4 in array2)
					{
						if (gameObject4.GetPhotonView().owner.ID == num2)
						{
							Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObject4);
							Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: false);
						}
					}
				}
				else if (!inputLine.StartsWith("/kill"))
				{
					if (inputLine.StartsWith("/revive"))
					{
						if (PhotonNetwork.isMasterClient)
						{
							if (inputLine == "/reviveall")
							{
								object[] parameters3 = new object[2]
								{
									"<color=#FFCC00>All players have been revived.</color>",
									string.Empty
								};
								FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters3);
								PhotonPlayer[] playerList = PhotonNetwork.playerList;
								foreach (PhotonPlayer photonPlayer3 in playerList)
								{
									if (photonPlayer3.customProperties[PhotonPlayerProperty.dead] != null && RCextensions.returnBoolFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.isTitan]) != 2)
									{
										FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", photonPlayer3);
									}
								}
							}
							else
							{
								int num2 = Convert.ToInt32(inputLine.Substring(8));
								PhotonPlayer[] playerList = PhotonNetwork.playerList;
								foreach (PhotonPlayer photonPlayer4 in playerList)
								{
									if (photonPlayer4.ID == num2)
									{
										addLINE("<color=#FFCC00>Player " + num2 + " has been revived.</color>");
										if (photonPlayer4.customProperties[PhotonPlayerProperty.dead] != null && RCextensions.returnBoolFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.isTitan]) != 2)
										{
											object[] parameters3 = new object[2]
											{
												"<color=#FFCC00>You have been revived by the master client.</color>",
												string.Empty
											};
											FengGameManagerMKII.instance.photonView.RPC("Chat", photonPlayer4, parameters3);
											FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", photonPlayer4);
										}
									}
								}
							}
						}
						else
						{
							addLINE("<color=#FFCC00>error: not master client</color>");
						}
					}
					else if (inputLine.StartsWith("/unban"))
					{
						if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
						{
							FengGameManagerMKII.ServerRequestUnban(inputLine.Substring(7));
						}
						else if (PhotonNetwork.isMasterClient)
						{
							int num3 = Convert.ToInt32(inputLine.Substring(7));
							if (FengGameManagerMKII.banHash.ContainsKey(num3))
							{
								object[] parameters3 = new object[2]
								{
									"<color=#FFCC00>" + (string)FengGameManagerMKII.banHash[num3] + " has been unbanned from the server. </color>",
									string.Empty
								};
								FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters3);
								FengGameManagerMKII.banHash.Remove(num3);
							}
							else
							{
								addLINE("error: no such player");
							}
						}
						else
						{
							addLINE("<color=#FFCC00>error: not master client</color>");
						}
					}
					else if (inputLine.StartsWith("/rules"))
					{
						addLINE("<color=#FFCC00>Currently activated gamemodes:</color>");
						if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
						{
							addLINE("<color=#FFCC00>Bomb mode is on.</color>");
						}
						if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
						{
							if (SettingsManager.LegacyGameSettings.TeamMode.Value == 1)
							{
								addLINE("<color=#FFCC00>Team mode is on (no sort).</color>");
							}
							else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 2)
							{
								addLINE("<color=#FFCC00>Team mode is on (sort by size).</color>");
							}
							else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
							{
								addLINE("<color=#FFCC00>Team mode is on (sort by skill).</color>");
							}
						}
						if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
						{
							addLINE("<color=#FFCC00>Point mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.PointModeAmount.Value) + ").</color>");
						}
						if (!SettingsManager.LegacyGameSettings.RockThrowEnabled.Value)
						{
							addLINE("<color=#FFCC00>Punk Rock-Throwing is disabled.</color>");
						}
						if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
						{
							addLINE("<color=#FFCC00>Custom spawn rate is on (" + SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value.ToString("F2") + "% Normal, " + SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value.ToString("F2") + "% Abnormal, " + SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value.ToString("F2") + "% Jumper, " + SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value.ToString("F2") + "% Crawler, " + SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value.ToString("F2") + "% Punk </color>");
						}
						if (SettingsManager.LegacyGameSettings.TitanExplodeEnabled.Value)
						{
							addLINE("<color=#FFCC00>Titan explode mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanExplodeRadius.Value) + ").</color>");
						}
						if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
						{
							addLINE("<color=#FFCC00>Titan health mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanHealthMin.Value) + "-" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanHealthMax.Value) + ").</color>");
						}
						if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
						{
							addLINE("<color=#FFCC00>Infection mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.InfectionModeAmount.Value) + ").</color>");
						}
						if (SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value)
						{
							addLINE("<color=#FFCC00>Nape armor is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanArmor.Value) + ").</color>");
						}
						if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
						{
							addLINE("<color=#FFCC00>Custom titan # is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanNumber.Value) + ").</color>");
						}
						if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
						{
							addLINE("<color=#FFCC00>Custom titan size is on (" + SettingsManager.LegacyGameSettings.TitanSizeMin.Value.ToString("F2") + "," + SettingsManager.LegacyGameSettings.TitanSizeMax.Value.ToString("F2") + ").</color>");
						}
						if (SettingsManager.LegacyGameSettings.KickShifters.Value)
						{
							addLINE("<color=#FFCC00>Anti-shifter is on. Using shifters will get you kicked.</color>");
						}
						if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
						{
							addLINE("<color=#FFCC00>Custom wave mode is on (" + Convert.ToString(SettingsManager.LegacyGameSettings.TitanMaxWaves.Value) + ").</color>");
						}
						if (SettingsManager.LegacyGameSettings.FriendlyMode.Value)
						{
							addLINE("<color=#FFCC00>Friendly-Fire disabled. PVP is prohibited.</color>");
						}
						if (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)
						{
							if (SettingsManager.LegacyGameSettings.BladePVP.Value == 1)
							{
								addLINE("<color=#FFCC00>AHSS/Blade PVP is on (team-based).</color>");
							}
							else if (SettingsManager.LegacyGameSettings.BladePVP.Value == 2)
							{
								addLINE("<color=#FFCC00>AHSS/Blade PVP is on (FFA).</color>");
							}
						}
						if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
						{
							addLINE("<color=#FFCC00>Max Wave set to " + SettingsManager.LegacyGameSettings.TitanMaxWaves.Value + "</color>");
						}
						if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
						{
							addLINE("<color=#FFCC00>Horses are enabled.</color>");
						}
						if (!SettingsManager.LegacyGameSettings.AHSSAirReload.Value)
						{
							addLINE("<color=#FFCC00>AHSS Air-Reload disabled.</color>");
						}
						if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
						{
							addLINE("<color=#FFCC00>Punks will not spawn every five waves.</color>");
						}
						if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
						{
							addLINE("<color=#FFCC00>Endless Respawn is enabled (" + SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value + " seconds).</color>");
						}
						if (SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
						{
							addLINE("<color=#FFCC00>Minimaps are disabled.</color>");
						}
						if (SettingsManager.LegacyGameSettings.Motd.Value != string.Empty)
						{
							addLINE("<color=#FFCC00>MOTD:" + SettingsManager.LegacyGameSettings.Motd.Value + "</color>");
						}
						if (SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
						{
							addLINE("<color=#FFCC00>Cannons will kill humans.</color>");
						}
					}
					else if (inputLine.StartsWith("/kick"))
					{
						int num2 = Convert.ToInt32(inputLine.Substring(6));
						if (num2 == PhotonNetwork.player.ID)
						{
							addLINE("error:can't kick yourself.");
						}
						else if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType != 0 && !PhotonNetwork.isMasterClient)
						{
							object[] parameters4 = new object[2]
							{
								"/kick #" + Convert.ToString(num2),
								LoginFengKAI.player.name
							};
							FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters4);
						}
						else
						{
							bool flag3 = false;
							PhotonPlayer[] playerList = PhotonNetwork.playerList;
							foreach (PhotonPlayer photonPlayer5 in playerList)
							{
								if (num2 == photonPlayer5.ID)
								{
									flag3 = true;
									if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
									{
										FengGameManagerMKII.instance.kickPlayerRC(photonPlayer5, ban: false, "");
									}
									else if (PhotonNetwork.isMasterClient)
									{
										FengGameManagerMKII.instance.kickPlayerRC(photonPlayer5, ban: false, "");
										object[] parameters5 = new object[2]
										{
											"<color=#FFCC00>" + RCextensions.returnStringFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.name]) + " has been kicked from the server!</color>",
											string.Empty
										};
										FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters5);
									}
								}
							}
							if (!flag3)
							{
								addLINE("error:no such player.");
							}
						}
					}
					else if (inputLine.StartsWith("/ban"))
					{
						if (inputLine == "/banlist")
						{
							addLINE("<color=#FFCC00>List of banned players:</color>");
							foreach (int key2 in FengGameManagerMKII.banHash.Keys)
							{
								addLINE("<color=#FFCC00>" + Convert.ToString(key2) + ":" + (string)FengGameManagerMKII.banHash[key2] + "</color>");
							}
						}
						else
						{
							int num2 = Convert.ToInt32(inputLine.Substring(5));
							if (num2 == PhotonNetwork.player.ID)
							{
								addLINE("error:can't kick yourself.");
							}
							else if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType != 0 && !PhotonNetwork.isMasterClient)
							{
								object[] parameters4 = new object[2]
								{
									"/kick #" + Convert.ToString(num2),
									LoginFengKAI.player.name
								};
								FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters4);
							}
							else
							{
								bool flag3 = false;
								PhotonPlayer[] playerList = PhotonNetwork.playerList;
								foreach (PhotonPlayer photonPlayer6 in playerList)
								{
									if (num2 == photonPlayer6.ID)
									{
										flag3 = true;
										if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
										{
											FengGameManagerMKII.instance.kickPlayerRC(photonPlayer6, ban: true, "");
										}
										else if (PhotonNetwork.isMasterClient)
										{
											FengGameManagerMKII.instance.kickPlayerRC(photonPlayer6, ban: true, "");
											object[] parameters5 = new object[2]
											{
												"<color=#FFCC00>" + RCextensions.returnStringFromObject(photonPlayer6.customProperties[PhotonPlayerProperty.name]) + " has been banned from the server!</color>",
												string.Empty
											};
											FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters5);
										}
									}
								}
								if (!flag3)
								{
									addLINE("error:no such player.");
								}
							}
						}
					}
				}
				inputLine = string.Empty;
				GUI.FocusControl(string.Empty);
				return;
			}
			inputLine = "\t";
			GUI.FocusControl("ChatInput");
		}
		goto IL_1c1a;
		IL_1c1a:
		ShowMessageWindow();
		GUILayout.BeginArea(GuiRect2);
		GUILayout.BeginHorizontal();
		GUI.SetNextControlName("ChatInput");
		inputLine = GUILayout.TextField(inputLine);
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	public void setPosition()
	{
		if (AlignBottom)
		{
			GuiRect = new Rect(0f, Screen.height - 500, 300f, 470f);
			GuiRect2 = new Rect(30f, Screen.height - 300 + 275, 300f, 25f);
		}
	}

	public void Start()
	{
		setPosition();
	}
}
