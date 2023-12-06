using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ApplicationManagers;
using CustomSkins;
using ExitGames.Client.Photon;
using GameManagers;
using Photon;
using Settings;
using UI;
using UnityEngine;

internal class FengGameManagerMKII : Photon.MonoBehaviour
{
	private enum LoginStates
	{
		notlogged,
		loggingin,
		loginfailed,
		loggedin
	}

	public static bool JustLeftRoom = false;

	public Dictionary<int, CannonValues> allowedToCannon;

	public Dictionary<string, Texture2D> assetCacheTextures;

	public static ExitGames.Client.Photon.Hashtable banHash;

	public static ExitGames.Client.Photon.Hashtable boolVariables;

	public static Dictionary<string, GameObject> CachedPrefabs;

	private ArrayList chatContent;

	public InRoomChat chatRoom;

	public GameObject checkpoint;

	private ArrayList cT;

	public static string currentLevel;

	private float currentSpeed;

	public static bool customLevelLoaded;

	public int cyanKills;

	public int difficulty;

	public float distanceSlider;

	private bool endRacing;

	private ArrayList eT;

	public static ExitGames.Client.Photon.Hashtable floatVariables;

	private ArrayList fT;

	private float gameEndCD;

	private float gameEndTotalCDtime = 9f;

	public bool gameStart;

	private bool gameTimesUp;

	public static ExitGames.Client.Photon.Hashtable globalVariables;

	public List<GameObject> groundList;

	public static bool hasLogged;

	private ArrayList heroes;

	public static ExitGames.Client.Photon.Hashtable heroHash;

	private int highestwave = 1;

	private ArrayList hooks;

	private int humanScore;

	public static List<int> ignoreList;

	public static ExitGames.Client.Photon.Hashtable imatitan;

	public static FengGameManagerMKII instance;

	public static ExitGames.Client.Photon.Hashtable intVariables;

	public static bool isAssetLoaded;

	public bool isFirstLoad;

	private bool isLosing;

	private bool isPlayer1Winning;

	private bool isPlayer2Winning;

	public bool isRecompiling;

	public bool isRestarting;

	public bool isSpawning;

	public bool isUnloading;

	private bool isWinning;

	public bool justSuicide;

	private ArrayList kicklist;

	private ArrayList killInfoGO = new ArrayList();

	public static bool LAN;

	public static string level = string.Empty;

	public List<string[]> levelCache;

	public static ExitGames.Client.Photon.Hashtable[] linkHash;

	private string localRacingResult;

	public static bool logicLoaded;

	public static int loginstate;

	public int magentaKills;

	private IN_GAME_MAIN_CAMERA mainCamera;

	public static bool masterRC;

	public int maxPlayers;

	private float maxSpeed;

	public float mouseSlider;

	private string myLastHero;

	private string myLastRespawnTag = "playerRespawn";

	public float myRespawnTime;

	public new string name;

	public static string nameField;

	public bool needChooseSide;

	public static bool noRestart;

	public static string oldScript;

	public static string oldScriptLogic;

	public static string passwordField;

	public float pauseWaitTime;

	public string playerList;

	public List<Vector3> playerSpawnsC;

	public List<Vector3> playerSpawnsM;

	public List<PhotonPlayer> playersRPC;

	public static ExitGames.Client.Photon.Hashtable playerVariables;

	public Dictionary<string, int[]> PreservedPlayerKDR;

	public static string PrivateServerAuthPass;

	public static string privateServerField;

	public static string privateLobbyField;

	public int PVPhumanScore;

	private int PVPhumanScoreMax = 200;

	public int PVPtitanScore;

	private int PVPtitanScoreMax = 200;

	public float qualitySlider;

	public List<GameObject> racingDoors;

	private ArrayList racingResult;

	public Vector3 racingSpawnPoint;

	public bool racingSpawnPointSet;

	public static AssetBundle RCassets;

	public static ExitGames.Client.Photon.Hashtable RCEvents;

	public static ExitGames.Client.Photon.Hashtable RCRegions;

	public static ExitGames.Client.Photon.Hashtable RCRegionTriggers;

	public static ExitGames.Client.Photon.Hashtable RCVariableNames;

	public List<float> restartCount;

	public bool restartingBomb;

	public bool restartingEren;

	public bool restartingHorse;

	public bool restartingMC;

	public bool restartingTitan;

	public float retryTime;

	public float roundTime;

	public Vector2 scroll;

	public Vector2 scroll2;

	public GameObject selectedObj;

	public static object[] settingsOld;

	private int single_kills;

	private int single_maxDamage;

	private int single_totalDamage;

	public List<GameObject> spectateSprites;

	private bool startRacing;

	public static ExitGames.Client.Photon.Hashtable stringVariables;

	private int[] teamScores;

	private int teamWinner;

	public Texture2D textureBackgroundBlack;

	public Texture2D textureBackgroundBlue;

	public int time = 600;

	private float timeElapse;

	private float timeTotalServer;

	private ArrayList titans;

	private int titanScore;

	public List<TitanSpawner> titanSpawners;

	public List<Vector3> titanSpawns;

	public static ExitGames.Client.Photon.Hashtable titanVariables;

	public float transparencySlider;

	public GameObject ui;

	public float updateTime;

	public static string usernameField;

	public int wave = 1;

	public Dictionary<string, Material> customMapMaterials;

	public float LastRoomPropertyCheckTime;

	private SkyboxCustomSkinLoader _skyboxCustomSkinLoader;

	private ForestCustomSkinLoader _forestCustomSkinLoader;

	private CityCustomSkinLoader _cityCustomSkinLoader;

	private CustomLevelCustomSkinLoader _customLevelCustomSkinLoader;

	public void OnJoinedLobby()
	{
		if (JustLeftRoom)
		{
			PhotonNetwork.Disconnect();
			JustLeftRoom = false;
		}
		else if (UIManager.CurrentMenu != null && UIManager.CurrentMenu.GetComponent<MainMenu>() != null)
		{
			UIManager.CurrentMenu.GetComponent<MainMenu>().ShowMultiplayerRoomListPopup();
		}
	}

	private void Awake()
	{
		_skyboxCustomSkinLoader = base.gameObject.AddComponent<SkyboxCustomSkinLoader>();
		_forestCustomSkinLoader = base.gameObject.AddComponent<ForestCustomSkinLoader>();
		_cityCustomSkinLoader = base.gameObject.AddComponent<CityCustomSkinLoader>();
		_customLevelCustomSkinLoader = base.gameObject.AddComponent<CustomLevelCustomSkinLoader>();
		base.gameObject.AddComponent<CustomRPCManager>();
	}

	private string getMaterialHash(string material, string x, string y)
	{
		return material + "," + x + "," + y;
	}

	public void addCamera(IN_GAME_MAIN_CAMERA c)
	{
		mainCamera = c;
	}

	public void addCT(COLOSSAL_TITAN titan)
	{
		cT.Add(titan);
	}

	public void addET(TITAN_EREN hero)
	{
		eT.Add(hero);
	}

	public void addFT(FEMALE_TITAN titan)
	{
		fT.Add(titan);
	}

	public void addHero(HERO hero)
	{
		heroes.Add(hero);
	}

	public void addHook(Bullet h)
	{
		hooks.Add(h);
	}

	public void addTime(float time)
	{
		timeTotalServer -= time;
	}

	public void addTitan(TITAN titan)
	{
		titans.Add(titan);
	}

	private void cache()
	{
		ClothFactory.ClearClothCache();
		chatRoom = GameObject.Find("Chatroom").GetComponent<InRoomChat>();
		playersRPC.Clear();
		titanSpawners.Clear();
		groundList.Clear();
		PreservedPlayerKDR = new Dictionary<string, int[]>();
		noRestart = false;
		isSpawning = false;
		retryTime = 0f;
		logicLoaded = false;
		customLevelLoaded = true;
		isUnloading = false;
		isRecompiling = false;
		Time.timeScale = 1f;
		Camera.main.farClipPlane = 1500f;
		pauseWaitTime = 0f;
		spectateSprites = new List<GameObject>();
		isRestarting = false;
		if (PhotonNetwork.isMasterClient)
		{
			StartCoroutine(WaitAndResetRestarts());
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			roundTime = 0f;
			if (level.StartsWith("Custom"))
			{
				customLevelLoaded = false;
			}
			if (PhotonNetwork.isMasterClient)
			{
				if (isFirstLoad)
				{
					setGameSettings(checkGameGUI());
				}
				if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
				{
					StartCoroutine(respawnE(SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value));
				}
			}
		}
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			chatRoom.addLINE("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round Start.");
		}
		isFirstLoad = false;
		RecompilePlayerList(0.5f);
	}

	[RPC]
	private void Chat(string content, string sender, PhotonMessageInfo info)
	{
		if (sender != string.Empty)
		{
			content = sender + ":" + content;
		}
		content = "<color=#FFC000>[" + Convert.ToString(info.sender.ID) + "]</color> " + content;
		chatRoom.addLINE(content);
	}

	[RPC]
	private void ChatPM(string sender, string content, PhotonMessageInfo info)
	{
		content = sender + ":" + content;
		content = "<color=#FFC000>FROM [" + Convert.ToString(info.sender.ID) + "]</color> " + content;
		chatRoom.addLINE(content);
	}

	private ExitGames.Client.Photon.Hashtable checkGameGUI()
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		LegacyGameSettings legacyGameSettingsUI = SettingsManager.LegacyGameSettingsUI;
		if (legacyGameSettingsUI.InfectionModeEnabled.Value)
		{
			legacyGameSettingsUI.BombModeEnabled.Value = false;
			legacyGameSettingsUI.TeamMode.Value = 0;
			legacyGameSettingsUI.PointModeEnabled.Value = false;
			legacyGameSettingsUI.BladePVP.Value = 0;
			if (legacyGameSettingsUI.InfectionModeAmount.Value > PhotonNetwork.countOfPlayers)
			{
				legacyGameSettingsUI.InfectionModeAmount.Value = 1;
			}
			hashtable.Add("infection", legacyGameSettingsUI.InfectionModeAmount.Value);
			if (!SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value || SettingsManager.LegacyGameSettings.InfectionModeAmount.Value != legacyGameSettingsUI.InfectionModeAmount.Value)
			{
				imatitan.Clear();
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
					ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
					hashtable2.Add(PhotonPlayerProperty.isTitan, 1);
					photonPlayer.SetCustomProperties(hashtable2);
				}
				int num = PhotonNetwork.playerList.Length;
				int num2 = legacyGameSettingsUI.InfectionModeAmount.Value;
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					PhotonPlayer photonPlayer2 = PhotonNetwork.playerList[i];
					if (num > 0 && UnityEngine.Random.Range(0f, 1f) <= (float)num2 / (float)num)
					{
						ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
						hashtable3.Add(PhotonPlayerProperty.isTitan, 2);
						photonPlayer2.SetCustomProperties(hashtable3);
						imatitan.Add(photonPlayer2.ID, 2);
						num2--;
					}
					num--;
				}
			}
		}
		if (legacyGameSettingsUI.BombModeEnabled.Value)
		{
			hashtable.Add("bomb", 1);
		}
		if (legacyGameSettingsUI.BombModeCeiling.Value)
		{
			hashtable.Add("bombCeiling", 1);
		}
		else
		{
			hashtable.Add("bombCeiling", 0);
		}
		if (legacyGameSettingsUI.BombModeInfiniteGas.Value)
		{
			hashtable.Add("bombInfiniteGas", 1);
		}
		else
		{
			hashtable.Add("bombInfiniteGas", 0);
		}
		if (legacyGameSettingsUI.GlobalHideNames.Value)
		{
			hashtable.Add("globalHideNames", 1);
		}
		if (legacyGameSettingsUI.GlobalMinimapDisable.Value)
		{
			hashtable.Add("globalDisableMinimap", 1);
		}
		if (legacyGameSettingsUI.TeamMode.Value > 0)
		{
			hashtable.Add("team", legacyGameSettingsUI.TeamMode.Value);
			if (SettingsManager.LegacyGameSettings.TeamMode.Value != legacyGameSettingsUI.TeamMode.Value)
			{
				int num2 = 1;
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
					switch (num2)
					{
					case 1:
						base.photonView.RPC("setTeamRPC", photonPlayer, 1);
						num2 = 2;
						break;
					case 2:
						base.photonView.RPC("setTeamRPC", photonPlayer, 2);
						num2 = 1;
						break;
					}
				}
			}
		}
		if (legacyGameSettingsUI.PointModeEnabled.Value)
		{
			hashtable.Add("point", legacyGameSettingsUI.PointModeAmount.Value);
		}
		if (!legacyGameSettingsUI.RockThrowEnabled.Value)
		{
			hashtable.Add("rock", 1);
		}
		if (legacyGameSettingsUI.TitanExplodeEnabled.Value)
		{
			hashtable.Add("explode", legacyGameSettingsUI.TitanExplodeRadius.Value);
		}
		if (legacyGameSettingsUI.TitanHealthMode.Value > 0)
		{
			hashtable.Add("healthMode", legacyGameSettingsUI.TitanHealthMode.Value);
			hashtable.Add("healthLower", legacyGameSettingsUI.TitanHealthMin.Value);
			hashtable.Add("healthUpper", legacyGameSettingsUI.TitanHealthMax.Value);
		}
		if (legacyGameSettingsUI.KickShifters.Value)
		{
			hashtable.Add("eren", 1);
		}
		if (legacyGameSettingsUI.TitanNumberEnabled.Value)
		{
			hashtable.Add("titanc", legacyGameSettingsUI.TitanNumber.Value);
		}
		if (legacyGameSettingsUI.TitanArmorEnabled.Value)
		{
			hashtable.Add("damage", legacyGameSettingsUI.TitanArmor.Value);
		}
		if (legacyGameSettingsUI.TitanSizeEnabled.Value)
		{
			hashtable.Add("sizeMode", 1);
			hashtable.Add("sizeLower", legacyGameSettingsUI.TitanSizeMin.Value);
			hashtable.Add("sizeUpper", legacyGameSettingsUI.TitanSizeMax.Value);
		}
		if (legacyGameSettingsUI.TitanSpawnEnabled.Value)
		{
			if (legacyGameSettingsUI.TitanSpawnNormal.Value + legacyGameSettingsUI.TitanSpawnAberrant.Value + legacyGameSettingsUI.TitanSpawnCrawler.Value + legacyGameSettingsUI.TitanSpawnJumper.Value + legacyGameSettingsUI.TitanSpawnPunk.Value > 100f)
			{
				legacyGameSettingsUI.TitanSpawnNormal.Value = 20f;
				legacyGameSettingsUI.TitanSpawnAberrant.Value = 20f;
				legacyGameSettingsUI.TitanSpawnCrawler.Value = 20f;
				legacyGameSettingsUI.TitanSpawnJumper.Value = 20f;
				legacyGameSettingsUI.TitanSpawnPunk.Value = 20f;
			}
			hashtable.Add("spawnMode", 1);
			hashtable.Add("nRate", legacyGameSettingsUI.TitanSpawnNormal.Value);
			hashtable.Add("aRate", legacyGameSettingsUI.TitanSpawnAberrant.Value);
			hashtable.Add("jRate", legacyGameSettingsUI.TitanSpawnJumper.Value);
			hashtable.Add("cRate", legacyGameSettingsUI.TitanSpawnCrawler.Value);
			hashtable.Add("pRate", legacyGameSettingsUI.TitanSpawnPunk.Value);
		}
		if (legacyGameSettingsUI.AllowHorses.Value)
		{
			hashtable.Add("horse", 1);
		}
		if (legacyGameSettingsUI.TitanPerWavesEnabled.Value)
		{
			hashtable.Add("waveModeOn", 1);
			hashtable.Add("waveModeNum", legacyGameSettingsUI.TitanPerWaves.Value);
		}
		if (legacyGameSettingsUI.FriendlyMode.Value)
		{
			hashtable.Add("friendly", 1);
		}
		if (legacyGameSettingsUI.BladePVP.Value > 0)
		{
			hashtable.Add("pvp", legacyGameSettingsUI.BladePVP.Value);
		}
		if (legacyGameSettingsUI.TitanMaxWavesEnabled.Value)
		{
			hashtable.Add("maxwave", legacyGameSettingsUI.TitanMaxWaves.Value);
		}
		if (legacyGameSettingsUI.EndlessRespawnEnabled.Value)
		{
			hashtable.Add("endless", legacyGameSettingsUI.EndlessRespawnTime.Value);
		}
		if (legacyGameSettingsUI.Motd.Value != string.Empty)
		{
			hashtable.Add("motd", legacyGameSettingsUI.Motd.Value);
		}
		if (!legacyGameSettingsUI.AHSSAirReload.Value)
		{
			hashtable.Add("ahssReload", 1);
		}
		if (!legacyGameSettingsUI.PunksEveryFive.Value)
		{
			hashtable.Add("punkWaves", 1);
		}
		if (legacyGameSettingsUI.CannonsFriendlyFire.Value)
		{
			hashtable.Add("deadlycannons", 1);
		}
		if (legacyGameSettingsUI.RacingEndless.Value)
		{
			hashtable.Add("asoracing", 1);
		}
		hashtable.Add("racingStartTime", legacyGameSettingsUI.RacingStartTime.Value);
		LegacyGameSettings legacyGameSettings = SettingsManager.LegacyGameSettings;
		legacyGameSettings.PreserveKDR.Value = legacyGameSettingsUI.PreserveKDR.Value;
		legacyGameSettings.TitanSpawnCap.Value = legacyGameSettingsUI.TitanSpawnCap.Value;
		legacyGameSettings.GameType.Value = legacyGameSettingsUI.GameType.Value;
		legacyGameSettings.LevelScript.Value = legacyGameSettingsUI.LevelScript.Value;
		legacyGameSettings.LogicScript.Value = legacyGameSettingsUI.LogicScript.Value;
		return hashtable;
	}

	private bool checkIsTitanAllDie()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<TITAN>() != null && !gameObject.GetComponent<TITAN>().hasDie)
			{
				return false;
			}
			if (gameObject.GetComponent<FEMALE_TITAN>() != null)
			{
				return false;
			}
		}
		return true;
	}

	public void checkPVPpts()
	{
		if (PVPtitanScore >= PVPtitanScoreMax)
		{
			PVPtitanScore = PVPtitanScoreMax;
			gameLose2();
		}
		else if (PVPhumanScore >= PVPhumanScoreMax)
		{
			PVPhumanScore = PVPhumanScoreMax;
			gameWin2();
		}
	}

	[RPC]
	private void clearlevel(string[] link, int gametype, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			switch (gametype)
			{
			case 0:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.KILL_TITAN;
				break;
			case 1:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.SURVIVE_MODE;
				break;
			case 2:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.PVP_AHSS;
				break;
			case 3:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.RACING;
				break;
			case 4:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.None;
				break;
			}
			if (info.sender.isMasterClient && link.Length > 6)
			{
				StartCoroutine(clearlevelE(link));
			}
		}
	}

	private IEnumerator clearlevelE(string[] skybox)
	{
		if (IsValidSkybox(skybox))
		{
			yield return StartCoroutine(_skyboxCustomSkinLoader.LoadSkinsFromRPC(skybox));
			yield return StartCoroutine(_customLevelCustomSkinLoader.LoadSkinsFromRPC(skybox));
		}
		else
		{
			SkyboxCustomSkinLoader.SkyboxMaterial = null;
		}
		StartCoroutine(reloadSky());
	}

	public void compileScript(string str)
	{
		string[] array = str.Replace(" ", string.Empty).Split(new string[2] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		int num = 0;
		int num2 = 0;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == "{")
			{
				num++;
				continue;
			}
			if (array[i] == "}")
			{
				num2++;
				continue;
			}
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			string text = array[i];
			for (int j = 0; j < text.Length; j++)
			{
				switch (text[j])
				{
				case '(':
					num3++;
					break;
				case ')':
					num4++;
					break;
				case '"':
					num5++;
					break;
				}
			}
			if (num3 != num4)
			{
				chatRoom.addLINE("Script Error: Parentheses not equal! (line " + (i + 1) + ")");
				flag = true;
			}
			if (num5 % 2 != 0)
			{
				chatRoom.addLINE("Script Error: Quotations not equal! (line " + (i + 1) + ")");
				flag = true;
			}
		}
		if (num != num2)
		{
			chatRoom.addLINE("Script Error: Bracket count not equivalent!");
			flag = true;
		}
		if (flag)
		{
			return;
		}
		try
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].StartsWith("On") || !(array[i + 1] == "{"))
				{
					continue;
				}
				int num6 = i;
				int num7 = i + 2;
				int num8 = 0;
				for (int k = i + 2; k < array.Length; k++)
				{
					if (array[k] == "{")
					{
						num8++;
					}
					if (array[k] == "}")
					{
						if (num8 > 0)
						{
							num8--;
							continue;
						}
						num7 = k - 1;
						k = array.Length;
					}
				}
				hashtable.Add(num6, num7);
				i = num7;
			}
			foreach (int key in hashtable.Keys)
			{
				string text2 = array[key];
				int num7 = (int)hashtable[key];
				string[] array2 = new string[num7 - key + 1];
				int num10 = 0;
				for (int i = key; i <= num7; i++)
				{
					array2[num10] = array[i];
					num10++;
				}
				RCEvent rCEvent = parseBlock(array2, 0, 0, null);
				if (text2.StartsWith("OnPlayerEnterRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
						regionTrigger.playerEventEnter = rCEvent;
						regionTrigger.myName = text3;
						RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							playerEventEnter = rCEvent,
							myName = text3
						};
						RCRegionTriggers.Add(text3, regionTrigger);
					}
					RCVariableNames.Add("OnPlayerEnterRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnPlayerLeaveRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
						regionTrigger.playerEventExit = rCEvent;
						regionTrigger.myName = text3;
						RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							playerEventExit = rCEvent,
							myName = text3
						};
						RCRegionTriggers.Add(text3, regionTrigger);
					}
					RCVariableNames.Add("OnPlayerExitRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnTitanEnterRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
						regionTrigger.titanEventEnter = rCEvent;
						regionTrigger.myName = text3;
						RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							titanEventEnter = rCEvent,
							myName = text3
						};
						RCRegionTriggers.Add(text3, regionTrigger);
					}
					RCVariableNames.Add("OnTitanEnterRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnTitanLeaveRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)RCRegionTriggers[text3];
						regionTrigger.titanEventExit = rCEvent;
						regionTrigger.myName = text3;
						RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							titanEventExit = rCEvent,
							myName = text3
						};
						RCRegionTriggers.Add(text3, regionTrigger);
					}
					RCVariableNames.Add("OnTitanExitRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnFirstLoad()"))
				{
					RCEvents.Add("OnFirstLoad", rCEvent);
				}
				else if (text2.StartsWith("OnRoundStart()"))
				{
					RCEvents.Add("OnRoundStart", rCEvent);
				}
				else if (text2.StartsWith("OnUpdate()"))
				{
					RCEvents.Add("OnUpdate", rCEvent);
				}
				else if (text2.StartsWith("OnTitanDie"))
				{
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string[] array3 = text2.Substring(num11 + 1, num12 - num11 - 1).Split(',');
					array3[0] = array3[0].Substring(1, array3[0].Length - 2);
					array3[1] = array3[1].Substring(1, array3[1].Length - 2);
					RCVariableNames.Add("OnTitanDie", array3);
					RCEvents.Add("OnTitanDie", rCEvent);
				}
				else if (text2.StartsWith("OnPlayerDieByTitan"))
				{
					RCEvents.Add("OnPlayerDieByTitan", rCEvent);
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string[] array3 = text2.Substring(num11 + 1, num12 - num11 - 1).Split(',');
					array3[0] = array3[0].Substring(1, array3[0].Length - 2);
					array3[1] = array3[1].Substring(1, array3[1].Length - 2);
					RCVariableNames.Add("OnPlayerDieByTitan", array3);
				}
				else if (text2.StartsWith("OnPlayerDieByPlayer"))
				{
					RCEvents.Add("OnPlayerDieByPlayer", rCEvent);
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string[] array3 = text2.Substring(num11 + 1, num12 - num11 - 1).Split(',');
					array3[0] = array3[0].Substring(1, array3[0].Length - 2);
					array3[1] = array3[1].Substring(1, array3[1].Length - 2);
					RCVariableNames.Add("OnPlayerDieByPlayer", array3);
				}
				else if (text2.StartsWith("OnChatInput"))
				{
					RCEvents.Add("OnChatInput", rCEvent);
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string value = text2.Substring(num11 + 1, num12 - num11 - 1);
					RCVariableNames.Add("OnChatInput", value.Substring(1, value.Length - 2));
				}
			}
		}
		catch (UnityException ex)
		{
			chatRoom.addLINE(ex.Message);
		}
	}

	public int conditionType(string str)
	{
		if (!str.StartsWith("Int"))
		{
			if (str.StartsWith("Bool"))
			{
				return 1;
			}
			if (str.StartsWith("String"))
			{
				return 2;
			}
			if (str.StartsWith("Float"))
			{
				return 3;
			}
			if (str.StartsWith("Titan"))
			{
				return 5;
			}
			if (str.StartsWith("Player"))
			{
				return 4;
			}
		}
		return 0;
	}

	private void core2()
	{
		if ((int)settingsOld[64] >= 100)
		{
			coreeditor();
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && needChooseSide)
		{
			if (SettingsManager.InputSettings.Human.Flare1.GetKeyDown())
			{
				if (NGUITools.GetActive(ui.GetComponent<UIReferArray>().panels[3]))
				{
					NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[0], state: true);
					NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
					NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
					NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: false);
					Camera.main.GetComponent<SpectatorMovement>().disable = false;
					Camera.main.GetComponent<MouseLook>().disable = false;
				}
				else
				{
					NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[0], state: false);
					NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
					NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
					NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: true);
					Camera.main.GetComponent<SpectatorMovement>().disable = true;
					Camera.main.GetComponent<MouseLook>().disable = true;
				}
			}
			if (SettingsManager.InputSettings.General.Pause.GetKeyDown() && !GameMenu.Paused)
			{
				Camera.main.GetComponent<SpectatorMovement>().disable = true;
				Camera.main.GetComponent<MouseLook>().disable = true;
				GameMenu.TogglePause(pause: true);
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			coreadd();
			ShowHUDInfoTopLeft(playerList);
			if (Camera.main != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING && Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !needChooseSide && !SettingsManager.LegacyGeneralSettings.SpecMode.Value)
			{
				ShowHUDInfoCenter("Press [F7D358]" + SettingsManager.InputSettings.General.SpectateNextPlayer.ToString() + "[-] to spectate the next player. \nPress [F7D358]" + SettingsManager.InputSettings.General.SpectatePreviousPlayer.ToString() + "[-] to spectate the previous player.\nPress [F7D358]" + SettingsManager.InputSettings.Human.AttackSpecial.ToString() + "[-] to enter the spectator mode.\n\n\n\n");
				if (LevelInfo.getInfo(level).respawnMode == RespawnMode.DEATHMATCH || SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value || ((SettingsManager.LegacyGameSettings.BombModeEnabled.Value || SettingsManager.LegacyGameSettings.BladePVP.Value > 0) && SettingsManager.LegacyGameSettings.PointModeEnabled.Value))
				{
					myRespawnTime += Time.deltaTime;
					int num = 5;
					if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
					{
						num = 10;
					}
					if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
					{
						num = SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value;
					}
					ShowHUDInfoCenterADD("Respawn in " + (num - (int)myRespawnTime) + "s.");
					if (myRespawnTime > (float)num)
					{
						myRespawnTime = 0f;
						Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
						if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
						{
							SpawnNonAITitan2(myLastHero);
						}
						else
						{
							StartCoroutine(WaitAndRespawn1(0.1f, myLastRespawnTag));
						}
						Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
						ShowHUDInfoCenter(string.Empty);
					}
				}
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
			{
				if (!isLosing)
				{
					currentSpeed = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity.magnitude;
					maxSpeed = Mathf.Max(maxSpeed, currentSpeed);
					ShowHUDInfoTopLeft("Current Speed : " + (int)currentSpeed + "\nMax Speed:" + maxSpeed);
				}
			}
			else
			{
				ShowHUDInfoTopLeft("Kills:" + single_kills + "\nMax Damage:" + single_maxDamage + "\nTotal Damage:" + single_totalDamage);
			}
		}
		if (isLosing && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					ShowHUDInfoCenter("Survive " + wave + " Waves!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
				else
				{
					ShowHUDInfoCenter("Humanity Fail!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
			}
			else
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					ShowHUDInfoCenter("Survive " + wave + " Waves!\nGame Restart in " + (int)gameEndCD + "s\n\n");
				}
				else
				{
					ShowHUDInfoCenter("Humanity Fail!\nAgain!\nGame Restart in " + (int)gameEndCD + "s\n\n");
				}
				if (gameEndCD <= 0f)
				{
					gameEndCD = 0f;
					if (PhotonNetwork.isMasterClient)
					{
						restartRC();
					}
					ShowHUDInfoCenter(string.Empty);
				}
				else
				{
					gameEndCD -= Time.deltaTime;
				}
			}
		}
		if (isWinning)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
				{
					ShowHUDInfoCenter((float)(int)(timeTotalServer * 10f) * 0.1f - 5f + "s !\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					ShowHUDInfoCenter("Survive All Waves!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
				else
				{
					ShowHUDInfoCenter("Humanity Win!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
			}
			else
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
				{
					ShowHUDInfoCenter(localRacingResult + "\n\nGame Restart in " + (int)gameEndCD + "s");
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					ShowHUDInfoCenter("Survive All Waves!\nGame Restart in " + (int)gameEndCD + "s\n\n");
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
				{
					if (SettingsManager.LegacyGameSettings.BladePVP.Value == 0 && !SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
					{
						ShowHUDInfoCenter("Team " + teamWinner + " Win!\nGame Restart in " + (int)gameEndCD + "s\n\n");
					}
					else
					{
						ShowHUDInfoCenter(string.Concat(new object[3]
						{
							"Round Ended!\nGame Restart in ",
							(int)gameEndCD,
							"s\n\n"
						}));
					}
				}
				else
				{
					ShowHUDInfoCenter("Humanity Win!\nGame Restart in " + (int)gameEndCD + "s\n\n");
				}
				if (gameEndCD <= 0f)
				{
					gameEndCD = 0f;
					if (PhotonNetwork.isMasterClient)
					{
						restartRC();
					}
					ShowHUDInfoCenter(string.Empty);
				}
				else
				{
					gameEndCD -= Time.deltaTime;
				}
			}
		}
		timeElapse += Time.deltaTime;
		roundTime += Time.deltaTime;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
			{
				if (!isWinning)
				{
					timeTotalServer += Time.deltaTime;
				}
			}
			else if (!isLosing && !isWinning)
			{
				timeTotalServer += Time.deltaTime;
			}
		}
		else
		{
			timeTotalServer += Time.deltaTime;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (!isWinning)
				{
					ShowHUDInfoTopCenter("Time : " + (timeTotalServer - 5f).ToString("0.00"));
				}
				if (timeTotalServer < 5f)
				{
					ShowHUDInfoCenter("RACE START IN " + (5f - timeTotalServer).ToString("0.00"));
				}
				else if (!startRacing)
				{
					ShowHUDInfoCenter(string.Empty);
					startRacing = true;
					endRacing = false;
					GameObject.Find("door").SetActive(value: false);
				}
			}
			else
			{
				float value = SettingsManager.LegacyGameSettings.RacingStartTime.Value;
				ShowHUDInfoTopCenter("Time : " + ((roundTime >= value) ? (roundTime - value).ToString("0.00") : "WAITING"));
				if (roundTime < value)
				{
					ShowHUDInfoCenter("RACE START IN " + (value - roundTime).ToString("0.00") + ((!(localRacingResult == string.Empty)) ? ("\nLast Round\n" + localRacingResult) : "\n\n"));
				}
				else if (!startRacing)
				{
					ShowHUDInfoCenter(string.Empty);
					startRacing = true;
					endRacing = false;
					GameObject gameObject = GameObject.Find("door");
					if (gameObject != null)
					{
						gameObject.SetActive(value: false);
					}
					if (racingDoors != null && customLevelLoaded)
					{
						foreach (GameObject racingDoor in racingDoors)
						{
							racingDoor.SetActive(value: false);
						}
						racingDoors = null;
					}
				}
				else if (racingDoors != null && customLevelLoaded)
				{
					foreach (GameObject racingDoor2 in racingDoors)
					{
						racingDoor2.SetActive(value: false);
					}
					racingDoors = null;
				}
				if (needChooseSide)
				{
					string text = SettingsManager.InputSettings.Human.Flare1.ToString();
					ShowHUDInfoTopCenterADD("\n\nPRESS " + text + " TO ENTER GAME");
				}
			}
			if (Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !needChooseSide && customLevelLoaded && !SettingsManager.LegacyGeneralSettings.SpecMode.Value)
			{
				myRespawnTime += Time.deltaTime;
				if (myRespawnTime > 1.5f)
				{
					myRespawnTime = 0f;
					Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
					if (checkpoint != null)
					{
						StartCoroutine(WaitAndRespawn2(0.1f, checkpoint));
					}
					else
					{
						StartCoroutine(WaitAndRespawn1(0.1f, myLastRespawnTag));
					}
					Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
					ShowHUDInfoCenter(string.Empty);
				}
			}
		}
		if (timeElapse > 1f)
		{
			timeElapse -= 1f;
			string text2 = string.Empty;
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
			{
				text2 = text2 + "Time : " + (time - (int)timeTotalServer);
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.None)
			{
				text2 = "Titan Left: ";
				text2 = text2 + GameObject.FindGameObjectsWithTag("titan").Length + "  Time : ";
				text2 = ((IN_GAME_MAIN_CAMERA.gametype != 0) ? (text2 + (time - (int)timeTotalServer)) : (text2 + (int)timeTotalServer));
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
			{
				text2 = "Titan Left: ";
				text2 = text2 + GameObject.FindGameObjectsWithTag("titan").Length.ToString() + " Wave : " + wave;
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
			{
				text2 = "Time : ";
				text2 = text2 + (time - (int)timeTotalServer) + "\nDefeat the Colossal Titan.\nPrevent abnormal titan from running to the north gate";
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
			{
				string text3 = "| ";
				for (int i = 0; i < PVPcheckPoint.chkPts.Count; i++)
				{
					text3 = text3 + (PVPcheckPoint.chkPts[i] as PVPcheckPoint).getStateString() + " ";
				}
				text3 += "|";
				int num2 = time - (int)timeTotalServer;
				text2 = string.Concat(PVPtitanScoreMax - PVPtitanScore + "  " + text3 + "  " + (PVPhumanScoreMax - PVPhumanScore) + "\n", "Time : ", num2.ToString());
			}
			if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
			{
				text2 = text2 + "\n[00FFFF]Cyan:" + Convert.ToString(cyanKills) + "       [FF00FF]Magenta:" + Convert.ToString(magentaKills) + "[ffffff]";
			}
			if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
			{
				ShowHUDInfoTopCenter(text2);
			}
			text2 = string.Empty;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					text2 = "Time : ";
					text2 += (int)timeTotalServer;
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
			{
				text2 = "Humanity " + humanScore + " : Titan " + titanScore + " ";
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
			{
				text2 = "Humanity " + humanScore + " : Titan " + titanScore + " ";
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.CAGE_FIGHT)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					text2 = "Time : ";
					text2 += time - (int)timeTotalServer;
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
				{
					for (int j = 0; j < teamScores.Length; j++)
					{
						string text4 = text2;
						text2 = text4 + ((j == 0) ? string.Empty : " : ") + "Team" + (j + 1) + " " + teamScores[j] + string.Empty;
					}
					text2 = text2 + "\nTime : " + (time - (int)timeTotalServer);
				}
			}
			ShowHUDInfoTopRight(text2);
			string text5 = ((IN_GAME_MAIN_CAMERA.difficulty < 0) ? "Trainning" : ((IN_GAME_MAIN_CAMERA.difficulty == 0) ? "Normal" : ((IN_GAME_MAIN_CAMERA.difficulty != 1) ? "Abnormal" : "Hard")));
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.CAGE_FIGHT)
			{
				ShowHUDInfoTopRightMAPNAME((int)roundTime + "s\n" + level + " : " + text5);
			}
			else
			{
				ShowHUDInfoTopRightMAPNAME("\n" + level + " : " + text5);
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				char[] separator = new char[1] { "`"[0] };
				string text6 = PhotonNetwork.room.name.Split(separator)[0];
				if (text6.Length > 20)
				{
					text6 = text6.Remove(19) + "...";
				}
				ShowHUDInfoTopRightMAPNAME("\n" + text6 + " [FFC000](" + Convert.ToString(PhotonNetwork.room.playerCount) + "/" + Convert.ToString(PhotonNetwork.room.maxPlayers) + ")");
				if (needChooseSide && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
				{
					string text7 = SettingsManager.InputSettings.Human.Flare1.ToString();
					ShowHUDInfoTopCenterADD("\n\nPRESS " + text7 + " TO ENTER GAME");
				}
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && killInfoGO.Count > 0 && killInfoGO[0] == null)
		{
			killInfoGO.RemoveAt(0);
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || !PhotonNetwork.isMasterClient || !(timeTotalServer > (float)time))
		{
			return;
		}
		IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
		gameStart = false;
		string text8 = string.Empty;
		string text9 = string.Empty;
		string text10 = string.Empty;
		string text11 = string.Empty;
		string text12 = string.Empty;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer != null)
			{
				text8 = text8 + photonPlayer.customProperties[PhotonPlayerProperty.name]?.ToString() + "\n";
				text9 = text9 + photonPlayer.customProperties[PhotonPlayerProperty.kills]?.ToString() + "\n";
				text10 = text10 + photonPlayer.customProperties[PhotonPlayerProperty.deaths]?.ToString() + "\n";
				text11 = text11 + photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]?.ToString() + "\n";
				text12 = text12 + photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]?.ToString() + "\n";
			}
		}
		string text13;
		if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_AHSS)
		{
			text13 = ((IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.SURVIVE_MODE) ? ("Humanity " + humanScore + " : Titan " + titanScore) : ("Highest Wave : " + highestwave));
		}
		else
		{
			text13 = string.Empty;
			for (int l = 0; l < teamScores.Length; l++)
			{
				text13 += ((l == 0) ? ("Team" + (l + 1) + " " + teamScores[l] + " ") : " : ");
			}
		}
		object[] parameters = new object[6] { text8, text9, text10, text11, text12, text13 };
		base.photonView.RPC("showResult", PhotonTargets.AllBuffered, parameters);
	}

	private void coreadd()
	{
		if (PhotonNetwork.isMasterClient)
		{
			OnUpdate();
			if (customLevelLoaded)
			{
				for (int i = 0; i < titanSpawners.Count; i++)
				{
					TitanSpawner titanSpawner = titanSpawners[i];
					titanSpawner.time -= Time.deltaTime;
					if (!(titanSpawner.time <= 0f) || titans.Count + fT.Count >= Math.Min(SettingsManager.LegacyGameSettings.TitanSpawnCap.Value, 80))
					{
						continue;
					}
					string text = titanSpawner.name;
					if (text == "spawnAnnie")
					{
						PhotonNetwork.Instantiate("FEMALE_TITAN", titanSpawner.location, new Quaternion(0f, 0f, 0f, 1f), 0);
					}
					else
					{
						GameObject gameObject = PhotonNetwork.Instantiate("TITAN_VER3.1", titanSpawner.location, new Quaternion(0f, 0f, 0f, 1f), 0);
						switch (text)
						{
						case "spawnAbnormal":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
							break;
						case "spawnJumper":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
							break;
						case "spawnCrawler":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
							break;
						case "spawnPunk":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
							break;
						}
					}
					if (titanSpawner.endless)
					{
						titanSpawner.time = titanSpawner.delay;
					}
					else
					{
						titanSpawners.Remove(titanSpawner);
					}
				}
			}
		}
		if (!(Time.timeScale <= 0.1f))
		{
			return;
		}
		if (pauseWaitTime <= 3f)
		{
			pauseWaitTime -= Time.deltaTime * 1000000f;
			if (pauseWaitTime <= 1f)
			{
				Camera.main.farClipPlane = 1500f;
			}
			if (pauseWaitTime <= 0f)
			{
				pauseWaitTime = 0f;
				Time.timeScale = 1f;
			}
		}
		justRecompileThePlayerList();
	}

	private void coreeditor()
	{
		if (Input.GetKey(KeyCode.Tab))
		{
			GUI.FocusControl(null);
		}
		if (selectedObj != null)
		{
			float num = 0.2f;
			if (SettingsManager.InputSettings.RCEditor.Slow.GetKey())
			{
				num = 0.04f;
			}
			else if (SettingsManager.InputSettings.RCEditor.Fast.GetKey())
			{
				num = 0.6f;
			}
			if (SettingsManager.InputSettings.General.Forward.GetKey())
			{
				selectedObj.transform.position += num * new Vector3(Camera.mainCamera.transform.forward.x, 0f, Camera.mainCamera.transform.forward.z);
			}
			else if (SettingsManager.InputSettings.General.Back.GetKey())
			{
				selectedObj.transform.position -= num * new Vector3(Camera.mainCamera.transform.forward.x, 0f, Camera.mainCamera.transform.forward.z);
			}
			if (SettingsManager.InputSettings.General.Left.GetKey())
			{
				selectedObj.transform.position -= num * new Vector3(Camera.mainCamera.transform.right.x, 0f, Camera.mainCamera.transform.right.z);
			}
			else if (SettingsManager.InputSettings.General.Right.GetKey())
			{
				selectedObj.transform.position += num * new Vector3(Camera.mainCamera.transform.right.x, 0f, Camera.mainCamera.transform.right.z);
			}
			if (SettingsManager.InputSettings.RCEditor.Down.GetKey())
			{
				selectedObj.transform.position -= Vector3.up * num;
			}
			else if (SettingsManager.InputSettings.RCEditor.Up.GetKey())
			{
				selectedObj.transform.position += Vector3.up * num;
			}
			if (!selectedObj.name.StartsWith("misc,region"))
			{
				if (SettingsManager.InputSettings.RCEditor.RotateRight.GetKey())
				{
					selectedObj.transform.Rotate(Vector3.up * num);
				}
				else if (SettingsManager.InputSettings.RCEditor.RotateLeft.GetKey())
				{
					selectedObj.transform.Rotate(Vector3.down * num);
				}
				if (SettingsManager.InputSettings.RCEditor.RotateCCW.GetKey())
				{
					selectedObj.transform.Rotate(Vector3.forward * num);
				}
				else if (SettingsManager.InputSettings.RCEditor.RotateCW.GetKey())
				{
					selectedObj.transform.Rotate(Vector3.back * num);
				}
				if (SettingsManager.InputSettings.RCEditor.RotateBack.GetKey())
				{
					selectedObj.transform.Rotate(Vector3.left * num);
				}
				else if (SettingsManager.InputSettings.RCEditor.RotateForward.GetKey())
				{
					selectedObj.transform.Rotate(Vector3.right * num);
				}
			}
			if (SettingsManager.InputSettings.RCEditor.Place.GetKeyDown())
			{
				linkHash[3].Add(selectedObj.GetInstanceID(), selectedObj.name + "," + Convert.ToString(selectedObj.transform.position.x) + "," + Convert.ToString(selectedObj.transform.position.y) + "," + Convert.ToString(selectedObj.transform.position.z) + "," + Convert.ToString(selectedObj.transform.rotation.x) + "," + Convert.ToString(selectedObj.transform.rotation.y) + "," + Convert.ToString(selectedObj.transform.rotation.z) + "," + Convert.ToString(selectedObj.transform.rotation.w));
				selectedObj = null;
				Camera.main.GetComponent<MouseLook>().enabled = true;
			}
			if (SettingsManager.InputSettings.RCEditor.Delete.GetKeyDown())
			{
				UnityEngine.Object.Destroy(selectedObj);
				selectedObj = null;
				Camera.main.GetComponent<MouseLook>().enabled = true;
				linkHash[3].Remove(selectedObj.GetInstanceID());
			}
			return;
		}
		if (Camera.main.GetComponent<MouseLook>().enabled)
		{
			float num2 = 100f;
			if (SettingsManager.InputSettings.RCEditor.Slow.GetKey())
			{
				num2 = 20f;
			}
			else if (SettingsManager.InputSettings.RCEditor.Fast.GetKey())
			{
				num2 = 400f;
			}
			Transform transform = Camera.main.transform;
			if (SettingsManager.InputSettings.General.Forward.GetKey())
			{
				transform.position += transform.forward * num2 * Time.deltaTime;
			}
			else if (SettingsManager.InputSettings.General.Back.GetKey())
			{
				transform.position -= transform.forward * num2 * Time.deltaTime;
			}
			if (SettingsManager.InputSettings.General.Left.GetKey())
			{
				transform.position -= transform.right * num2 * Time.deltaTime;
			}
			else if (SettingsManager.InputSettings.General.Right.GetKey())
			{
				transform.position += transform.right * num2 * Time.deltaTime;
			}
			if (SettingsManager.InputSettings.RCEditor.Up.GetKey())
			{
				transform.position += transform.up * num2 * Time.deltaTime;
			}
			else if (SettingsManager.InputSettings.RCEditor.Down.GetKey())
			{
				transform.position -= transform.up * num2 * Time.deltaTime;
			}
		}
		if (SettingsManager.InputSettings.RCEditor.Cursor.GetKeyDown())
		{
			if (Camera.main.GetComponent<MouseLook>().enabled)
			{
				Camera.main.GetComponent<MouseLook>().enabled = false;
			}
			else
			{
				Camera.main.GetComponent<MouseLook>().enabled = true;
			}
		}
		if (!Input.GetKeyDown(KeyCode.Mouse0) || Screen.lockCursor || GUIUtility.hotControl != 0 || ((!(Input.mousePosition.x > 300f) || !(Input.mousePosition.x < (float)Screen.width - 300f)) && !((float)Screen.height - Input.mousePosition.y > 600f)))
		{
			return;
		}
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
		{
			Transform transform2 = hitInfo.transform;
			if (transform2.gameObject.name.StartsWith("custom") || transform2.gameObject.name.StartsWith("base") || transform2.gameObject.name.StartsWith("racing") || transform2.gameObject.name.StartsWith("photon") || transform2.gameObject.name.StartsWith("spawnpoint") || transform2.gameObject.name.StartsWith("misc"))
			{
				selectedObj = transform2.gameObject;
				Camera.main.GetComponent<MouseLook>().enabled = false;
				Screen.lockCursor = true;
				linkHash[3].Remove(selectedObj.GetInstanceID());
			}
			else if (transform2.parent.gameObject.name.StartsWith("custom") || transform2.parent.gameObject.name.StartsWith("base") || transform2.parent.gameObject.name.StartsWith("racing") || transform2.parent.gameObject.name.StartsWith("photon"))
			{
				selectedObj = transform2.parent.gameObject;
				Camera.main.GetComponent<MouseLook>().enabled = false;
				Screen.lockCursor = true;
				linkHash[3].Remove(selectedObj.GetInstanceID());
			}
		}
	}

	private IEnumerator customlevelcache()
	{
		for (int i = 0; i < levelCache.Count; i++)
		{
			customlevelclientE(levelCache[i], renewHash: false);
			yield return new WaitForEndOfFrame();
		}
	}

	private void customlevelclientE(string[] content, bool renewHash)
	{
		bool flag = false;
		bool flag2 = false;
		if (content[content.Length - 1].StartsWith("a"))
		{
			flag = true;
			customMapMaterials.Clear();
		}
		else if (content[content.Length - 1].StartsWith("z"))
		{
			flag2 = true;
			customLevelLoaded = true;
			spawnPlayerCustomMap();
			Minimap.TryRecaptureInstance();
			unloadAssets();
			Camera.main.GetComponent<TiltShift>().enabled = false;
		}
		if (renewHash)
		{
			if (flag)
			{
				currentLevel = string.Empty;
				levelCache.Clear();
				titanSpawns.Clear();
				playerSpawnsC.Clear();
				playerSpawnsM.Clear();
				for (int i = 0; i < content.Length; i++)
				{
					string[] array = content[i].Split(',');
					if (array[0] == "titan")
					{
						titanSpawns.Add(new Vector3(Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3])));
					}
					else if (array[0] == "playerC")
					{
						playerSpawnsC.Add(new Vector3(Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3])));
					}
					else if (array[0] == "playerM")
					{
						playerSpawnsM.Add(new Vector3(Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3])));
					}
				}
				spawnPlayerCustomMap();
			}
			currentLevel += content[content.Length - 1];
			levelCache.Add(content);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.currentLevel, currentLevel);
			PhotonNetwork.player.SetCustomProperties(hashtable);
		}
		if (flag || flag2)
		{
			return;
		}
		for (int i = 0; i < content.Length; i++)
		{
			string[] array = content[i].Split(',');
			float result;
			if (array[0].StartsWith("custom"))
			{
				float a = 1f;
				GameObject gameObject = null;
				gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])));
				if (array[2] != "default")
				{
					if (array[2].StartsWith("transparent"))
					{
						if (float.TryParse(array[2].Substring(11), out result))
						{
							a = result;
						}
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer in componentsInChildren)
						{
							renderer.material = (Material)RCassets.Load("transparent");
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								string materialHash = getMaterialHash(array[2], array[10], array[11]);
								if (customMapMaterials.ContainsKey(materialHash))
								{
									renderer.material = customMapMaterials[materialHash];
									continue;
								}
								renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array[11]));
								customMapMaterials.Add(materialHash, renderer.material);
							}
						}
					}
					else
					{
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer2 in componentsInChildren)
						{
							renderer2.material = (Material)RCassets.Load(array[2]);
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								string materialHash2 = getMaterialHash(array[2], array[10], array[11]);
								if (customMapMaterials.ContainsKey(materialHash2))
								{
									renderer2.material = customMapMaterials[materialHash2];
									continue;
								}
								renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer2.material.mainTextureScale.y * Convert.ToSingle(array[11]));
								customMapMaterials.Add(materialHash2, renderer2.material);
							}
						}
					}
				}
				float num = gameObject.transform.localScale.x * Convert.ToSingle(array[3]);
				num -= 0.001f;
				float y = gameObject.transform.localScale.y * Convert.ToSingle(array[4]);
				float z = gameObject.transform.localScale.z * Convert.ToSingle(array[5]);
				gameObject.transform.localScale = new Vector3(num, y, z);
				if (!(array[6] != "0"))
				{
					continue;
				}
				Color color = new Color(Convert.ToSingle(array[7]), Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), a);
				MeshFilter[] componentsInChildren2 = gameObject.GetComponentsInChildren<MeshFilter>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					Mesh mesh = componentsInChildren2[j].mesh;
					Color[] array2 = new Color[mesh.vertexCount];
					for (int k = 0; k < mesh.vertexCount; k++)
					{
						array2[k] = color;
					}
					mesh.colors = array2;
				}
			}
			else if (array[0].StartsWith("base"))
			{
				if (array.Length < 15)
				{
					UnityEngine.Object.Instantiate(Resources.Load(array[1]), new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])));
					continue;
				}
				float a = 1f;
				GameObject gameObject = null;
				gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(array[1]), new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])));
				if (array[2] != "default")
				{
					if (array[2].StartsWith("transparent"))
					{
						if (float.TryParse(array[2].Substring(11), out result))
						{
							a = result;
						}
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer3 in componentsInChildren)
						{
							renderer3.material = (Material)RCassets.Load("transparent");
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								string materialHash3 = getMaterialHash(array[2], array[10], array[11]);
								if (customMapMaterials.ContainsKey(materialHash3))
								{
									renderer3.material = customMapMaterials[materialHash3];
									continue;
								}
								renderer3.material.mainTextureScale = new Vector2(renderer3.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer3.material.mainTextureScale.y * Convert.ToSingle(array[11]));
								customMapMaterials.Add(materialHash3, renderer3.material);
							}
						}
					}
					else
					{
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer4 in componentsInChildren)
						{
							if (renderer4.name.Contains("Particle System") && gameObject.name.Contains("aot_supply"))
							{
								continue;
							}
							renderer4.material = (Material)RCassets.Load(array[2]);
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								string materialHash4 = getMaterialHash(array[2], array[10], array[11]);
								if (customMapMaterials.ContainsKey(materialHash4))
								{
									renderer4.material = customMapMaterials[materialHash4];
									continue;
								}
								renderer4.material.mainTextureScale = new Vector2(renderer4.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer4.material.mainTextureScale.y * Convert.ToSingle(array[11]));
								customMapMaterials.Add(materialHash4, renderer4.material);
							}
						}
					}
				}
				float num = gameObject.transform.localScale.x * Convert.ToSingle(array[3]);
				num -= 0.001f;
				float y = gameObject.transform.localScale.y * Convert.ToSingle(array[4]);
				float z = gameObject.transform.localScale.z * Convert.ToSingle(array[5]);
				gameObject.transform.localScale = new Vector3(num, y, z);
				if (!(array[6] != "0"))
				{
					continue;
				}
				Color color = new Color(Convert.ToSingle(array[7]), Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), a);
				MeshFilter[] componentsInChildren2 = gameObject.GetComponentsInChildren<MeshFilter>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					Mesh mesh = componentsInChildren2[j].mesh;
					Color[] array2 = new Color[mesh.vertexCount];
					for (int k = 0; k < mesh.vertexCount; k++)
					{
						array2[k] = color;
					}
					mesh.colors = array2;
				}
			}
			else if (array[0].StartsWith("misc"))
			{
				if (array[1].StartsWith("barrier"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
				}
				else if (array[1].StartsWith("racingStart"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					if (racingDoors != null)
					{
						racingDoors.Add(gameObject);
					}
				}
				else if (array[1].StartsWith("racingEnd"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					gameObject.AddComponent<LevelTriggerRacingEnd>();
				}
				else if (array[1].StartsWith("region") && PhotonNetwork.isMasterClient)
				{
					Vector3 vector = new Vector3(Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8]));
					RCRegion rCRegion = new RCRegion(vector, Convert.ToSingle(array[3]), Convert.ToSingle(array[4]), Convert.ToSingle(array[5]));
					string key = array[2];
					if (RCRegionTriggers.ContainsKey(key))
					{
						GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load("region"));
						gameObject2.transform.position = vector;
						gameObject2.AddComponent<RegionTrigger>();
						gameObject2.GetComponent<RegionTrigger>().CopyTrigger((RegionTrigger)RCRegionTriggers[key]);
						float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[3]);
						num -= 0.001f;
						float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[4]);
						float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[5]);
						gameObject2.transform.localScale = new Vector3(num, y, z);
						rCRegion.myBox = gameObject2;
					}
					RCRegions.Add(key, rCRegion);
				}
			}
			else if (array[0].StartsWith("racing"))
			{
				if (array[1].StartsWith("start"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					if (racingDoors != null)
					{
						racingDoors.Add(gameObject);
					}
				}
				else if (array[1].StartsWith("end"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					gameObject.GetComponentInChildren<Collider>().gameObject.AddComponent<LevelTriggerRacingEnd>();
				}
				else if (array[1].StartsWith("kill"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					gameObject.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingKillTrigger>();
				}
				else if (array[1].StartsWith("checkpoint"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					gameObject.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingCheckpointTrigger>();
				}
			}
			else if (array[0].StartsWith("map"))
			{
				if (array[1].StartsWith("disablebounds"))
				{
					UnityEngine.Object.Destroy(GameObject.Find("gameobjectOutSide"));
					UnityEngine.Object.Instantiate(RCassets.Load("outside"));
				}
			}
			else
			{
				if (!PhotonNetwork.isMasterClient || !array[0].StartsWith("photon"))
				{
					continue;
				}
				if (array[1].StartsWith("Cannon"))
				{
					if (array.Length > 15)
					{
						GameObject obj = PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])), 0);
						obj.GetComponent<CannonPropRegion>().settings = content[i];
						obj.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, content[i]);
					}
					else
					{
						PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])), 0).GetComponent<CannonPropRegion>().settings = content[i];
					}
					continue;
				}
				TitanSpawner titanSpawner = new TitanSpawner();
				float num = 30f;
				if (float.TryParse(array[2], out result))
				{
					num = Mathf.Max(Convert.ToSingle(array[2]), 1f);
				}
				titanSpawner.time = num;
				titanSpawner.delay = num;
				titanSpawner.name = array[1];
				if (array[3] == "1")
				{
					titanSpawner.endless = true;
				}
				else
				{
					titanSpawner.endless = false;
				}
				titanSpawner.location = new Vector3(Convert.ToSingle(array[4]), Convert.ToSingle(array[5]), Convert.ToSingle(array[6]));
				titanSpawners.Add(titanSpawner);
			}
		}
	}

	private IEnumerator customlevelE(List<PhotonPlayer> players)
	{
		string[] array;
		if (!(currentLevel == string.Empty))
		{
			for (int i = 0; i < levelCache.Count; i++)
			{
				foreach (PhotonPlayer player in players)
				{
					if (player.customProperties[PhotonPlayerProperty.currentLevel] != null && currentLevel != string.Empty && RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.currentLevel]) == currentLevel)
					{
						if (i == 0)
						{
							array = new string[1] { "loadcached" };
							base.photonView.RPC("customlevelRPC", player, new object[1] { array });
						}
					}
					else
					{
						base.photonView.RPC("customlevelRPC", player, new object[1] { levelCache[i] });
					}
				}
				if (i > 0)
				{
					yield return new WaitForSeconds(0.75f);
				}
				else
				{
					yield return new WaitForSeconds(0.25f);
				}
			}
			yield break;
		}
		array = new string[1] { "loadempty" };
		foreach (PhotonPlayer player2 in players)
		{
			base.photonView.RPC("customlevelRPC", player2, new object[1] { array });
		}
		customLevelLoaded = true;
	}

	[RPC]
	private void customlevelRPC(string[] content, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			if (content.Length == 1 && content[0] == "loadcached")
			{
				StartCoroutine(customlevelcache());
			}
			else if (content.Length == 1 && content[0] == "loadempty")
			{
				currentLevel = string.Empty;
				levelCache.Clear();
				titanSpawns.Clear();
				playerSpawnsC.Clear();
				playerSpawnsM.Clear();
				customMapMaterials.Clear();
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.currentLevel, currentLevel);
				PhotonNetwork.player.SetCustomProperties(hashtable);
				customLevelLoaded = true;
				spawnPlayerCustomMap();
			}
			else
			{
				customlevelclientE(content, renewHash: true);
			}
		}
	}

	public void debugChat(string str)
	{
		chatRoom.addLINE(str);
	}

	public void DestroyAllExistingCloths()
	{
		Cloth[] array = UnityEngine.Object.FindObjectsOfType<Cloth>();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				ClothFactory.DisposeObject(array[i].gameObject);
			}
		}
	}

	private void endGameInfectionRC()
	{
		imatitan.Clear();
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonNetwork.playerList[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
			{
				PhotonPlayerProperty.isTitan,
				1
			} });
		}
		int num = PhotonNetwork.playerList.Length;
		int num2 = SettingsManager.LegacyGameSettings.InfectionModeAmount.Value;
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			if (num > 0 && UnityEngine.Random.Range(0f, 1f) <= (float)num2 / (float)num)
			{
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.isTitan, 2);
				photonPlayer.SetCustomProperties(hashtable);
				imatitan.Add(photonPlayer.ID, 2);
				num2--;
			}
			num--;
		}
		gameEndCD = 0f;
		restartGame2();
	}

	private void endGameRC()
	{
		if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
		{
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				PhotonNetwork.playerList[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable
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
		}
		gameEndCD = 0f;
		restartGame2();
	}

	public void EnterSpecMode(bool enter)
	{
		if (enter)
		{
			spectateSprites = new List<GameObject>();
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = (GameObject)array[i];
				if (!(gameObject.GetComponent<UISprite>() != null) || !gameObject.activeInHierarchy)
				{
					continue;
				}
				string text = gameObject.name;
				if (text.Contains("blade") || text.Contains("bullet") || text.Contains("gas") || text.Contains("flare") || text.Contains("skill_cd"))
				{
					if (!spectateSprites.Contains(gameObject))
					{
						spectateSprites.Add(gameObject);
					}
					gameObject.SetActive(value: false);
				}
			}
			string[] array2 = new string[2] { "Flare", "LabelInfoBottomRight" };
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject2 = GameObject.Find(array2[i]);
				if (gameObject2 != null)
				{
					if (!spectateSprites.Contains(gameObject2))
					{
						spectateSprites.Add(gameObject2);
					}
					gameObject2.SetActive(value: false);
				}
			}
			foreach (HERO player in instance.getPlayers())
			{
				if (player.photonView.isMine)
				{
					PhotonNetwork.Destroy(player.photonView);
				}
			}
			if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2 && !RCextensions.returnBoolFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.dead]))
			{
				foreach (TITAN titan in instance.getTitans())
				{
					if (titan.photonView.isMine)
					{
						PhotonNetwork.Destroy(titan.photonView);
					}
				}
			}
			NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
			NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
			NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], state: false);
			instance.needChooseSide = false;
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
			GameObject gameObject3 = GameObject.FindGameObjectWithTag("Player");
			if (gameObject3 != null && gameObject3.GetComponent<HERO>() != null)
			{
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObject3);
			}
			else
			{
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
			}
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: false);
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
			StartCoroutine(reloadSky(specmode: true));
			return;
		}
		if (GameObject.Find("cross1") != null)
		{
			GameObject.Find("cross1").transform.localPosition = Vector3.up * 5000f;
		}
		if (spectateSprites != null)
		{
			foreach (GameObject spectateSprite in spectateSprites)
			{
				if (spectateSprite != null)
				{
					spectateSprite.SetActive(value: true);
				}
			}
		}
		spectateSprites = new List<GameObject>();
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], state: false);
		instance.needChooseSide = true;
		Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void gameLose2()
	{
		if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || PhotonNetwork.isMasterClient) && !isWinning && !isLosing)
		{
			isLosing = true;
			titanScore++;
			gameEndCD = gameEndTotalCDtime;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters = new object[1] { titanScore };
				base.photonView.RPC("netGameLose", PhotonTargets.Others, parameters);
			}
			if (SettingsManager.UISettings.GameFeed.Value)
			{
				chatRoom.addLINE("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game lose).");
			}
		}
	}

	public void gameWin2()
	{
		if (isLosing || isWinning)
		{
			return;
		}
		isWinning = true;
		humanScore++;
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
		{
			if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
			{
				gameEndCD = 1000f;
			}
			else
			{
				gameEndCD = 20f;
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters = new object[1] { 0 };
				base.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			gameEndCD = gameEndTotalCDtime;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters2 = new object[1] { teamWinner };
				base.photonView.RPC("netGameWin", PhotonTargets.Others, parameters2);
			}
			teamScores[teamWinner - 1]++;
		}
		else
		{
			gameEndCD = gameEndTotalCDtime;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters3 = new object[1] { humanScore };
				base.photonView.RPC("netGameWin", PhotonTargets.Others, parameters3);
			}
		}
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			chatRoom.addLINE("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).");
		}
	}

	public ArrayList getPlayers()
	{
		return heroes;
	}

	public ArrayList getErens()
	{
		return eT;
	}

	[RPC]
	private void getRacingResult(string player, float time, PhotonMessageInfo info)
	{
		if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
		{
			if (info != null)
			{
				kickPlayerRCIfMC(info.sender, ban: true, "racing exploit");
			}
		}
		else
		{
			RacingResult value = new RacingResult
			{
				name = player,
				time = time
			};
			racingResult.Add(value);
			refreshRacingResult2();
		}
	}

	public ArrayList getTitans()
	{
		return titans;
	}

	private string hairtype(int lol)
	{
		if (lol < 0)
		{
			return "Random";
		}
		return "Male " + lol;
	}

	[RPC]
	private void ignorePlayer(int ID, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
			if (photonPlayer != null && !ignoreList.Contains(ID))
			{
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					if (PhotonNetwork.playerList[i] == photonPlayer)
					{
						ignoreList.Add(ID);
						RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
						raiseEventOptions.TargetActors = new int[1] { ID };
						RaiseEventOptions options = raiseEventOptions;
						PhotonNetwork.RaiseEvent(254, null, sendReliable: true, options);
					}
				}
			}
		}
		RecompilePlayerList(0.1f);
	}

	[RPC]
	private void ignorePlayerArray(int[] IDS, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			foreach (int num in IDS)
			{
				PhotonPlayer photonPlayer = PhotonPlayer.Find(num);
				if (photonPlayer == null || ignoreList.Contains(num))
				{
					continue;
				}
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					if (PhotonNetwork.playerList[j] == photonPlayer)
					{
						ignoreList.Add(num);
						RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
						raiseEventOptions.TargetActors = new int[1] { num };
						RaiseEventOptions options = raiseEventOptions;
						PhotonNetwork.RaiseEvent(254, null, sendReliable: true, options);
					}
				}
			}
		}
		RecompilePlayerList(0.1f);
	}

	public static GameObject InstantiateCustomAsset(string key)
	{
		key = key.Substring(8);
		return (GameObject)RCassets.Load(key);
	}

	public bool isPlayerAllDead()
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if ((int)photonPlayer.customProperties[PhotonPlayerProperty.isTitan] == 1)
			{
				num++;
				if ((bool)photonPlayer.customProperties[PhotonPlayerProperty.dead])
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public bool isPlayerAllDead2()
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) == 1)
			{
				num++;
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]))
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public bool isTeamAllDead(int team)
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if ((int)photonPlayer.customProperties[PhotonPlayerProperty.isTitan] == 1 && (int)photonPlayer.customProperties[PhotonPlayerProperty.team] == team)
			{
				num++;
				if ((bool)photonPlayer.customProperties[PhotonPlayerProperty.dead])
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public bool isTeamAllDead2(int team)
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer.customProperties[PhotonPlayerProperty.isTitan] != null && photonPlayer.customProperties[PhotonPlayerProperty.team] != null && RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) == 1 && RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.team]) == team)
			{
				num++;
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]))
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public void justRecompileThePlayerList()
	{
		string text = string.Empty;
		if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			Dictionary<int, PhotonPlayer> dictionary = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
			PhotonPlayer[] array = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer in array)
			{
				if (photonPlayer.customProperties[PhotonPlayerProperty.dead] != null && !ignoreList.Contains(photonPlayer.ID))
				{
					switch (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.RCteam]))
					{
					case 0:
						dictionary3.Add(photonPlayer.ID, photonPlayer);
						break;
					case 1:
						dictionary.Add(photonPlayer.ID, photonPlayer);
						num += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.kills]);
						num3 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.deaths]);
						num5 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]);
						num7 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]);
						break;
					case 2:
						dictionary2.Add(photonPlayer.ID, photonPlayer);
						num2 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.kills]);
						num4 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.deaths]);
						num6 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]);
						num8 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]);
						break;
					}
				}
			}
			cyanKills = num;
			magentaKills = num2;
			if (PhotonNetwork.isMasterClient)
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value == 2)
				{
					array = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer2 in array)
					{
						int num9 = 0;
						if (dictionary.Count > dictionary2.Count + 1)
						{
							num9 = 2;
							if (dictionary.ContainsKey(photonPlayer2.ID))
							{
								dictionary.Remove(photonPlayer2.ID);
							}
							if (!dictionary2.ContainsKey(photonPlayer2.ID))
							{
								dictionary2.Add(photonPlayer2.ID, photonPlayer2);
							}
						}
						else if (dictionary2.Count > dictionary.Count + 1)
						{
							num9 = 1;
							if (!dictionary.ContainsKey(photonPlayer2.ID))
							{
								dictionary.Add(photonPlayer2.ID, photonPlayer2);
							}
							if (dictionary2.ContainsKey(photonPlayer2.ID))
							{
								dictionary2.Remove(photonPlayer2.ID);
							}
						}
						if (num9 > 0)
						{
							base.photonView.RPC("setTeamRPC", photonPlayer2, num9);
						}
					}
				}
				else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
				{
					array = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer3 in array)
					{
						int num10 = 0;
						int num11 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.RCteam]);
						if (num11 <= 0)
						{
							continue;
						}
						switch (num11)
						{
						case 1:
						{
							int num13 = 0;
							num13 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.kills]);
							if (num2 + num13 + 7 < num - num13)
							{
								num10 = 2;
								num2 += num13;
								num -= num13;
							}
							break;
						}
						case 2:
						{
							int num12 = 0;
							num12 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.kills]);
							if (num + num12 + 7 < num2 - num12)
							{
								num10 = 1;
								num += num12;
								num2 -= num12;
							}
							break;
						}
						}
						if (num10 > 0)
						{
							base.photonView.RPC("setTeamRPC", photonPlayer3, num10);
						}
					}
				}
			}
			text = text + "[00FFFF]TEAM CYAN" + "[ffffff]:" + cyanKills + "/" + num3 + "/" + num5 + "/" + num7 + "\n";
			foreach (PhotonPlayer value in dictionary.Values)
			{
				int num11 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.RCteam]);
				if (value.customProperties[PhotonPlayerProperty.dead] == null || num11 != 1)
				{
					continue;
				}
				if (ignoreList.Contains(value.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value.ID) + "] ";
				if (value.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num14 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.team]);
					if (num14 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num14 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				string empty = string.Empty;
				empty = RCextensions.returnStringFromObject(value.customProperties[PhotonPlayerProperty.name]);
				int num15 = 0;
				num15 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.kills]);
				int num16 = 0;
				num16 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.deaths]);
				int num17 = 0;
				num17 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.max_dmg]);
				int num18 = 0;
				num18 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text2 + string.Empty + empty + "[ffffff]:" + num15 + "/" + num16 + "/" + num17 + "/" + num18;
				if (RCextensions.returnBoolFromObject(value.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = text + " \n" + "[FF00FF]TEAM MAGENTA" + "[ffffff]:" + magentaKills + "/" + num4 + "/" + num6 + "/" + num8 + "\n";
			foreach (PhotonPlayer value2 in dictionary2.Values)
			{
				int num11 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.RCteam]);
				if (value2.customProperties[PhotonPlayerProperty.dead] == null || num11 != 2)
				{
					continue;
				}
				if (ignoreList.Contains(value2.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value2.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value2.ID) + "] ";
				if (value2.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value2.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num14 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.team]);
					if (num14 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num14 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				string empty = string.Empty;
				empty = RCextensions.returnStringFromObject(value2.customProperties[PhotonPlayerProperty.name]);
				int num15 = 0;
				num15 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.kills]);
				int num16 = 0;
				num16 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.deaths]);
				int num17 = 0;
				num17 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.max_dmg]);
				int num18 = 0;
				num18 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text2 + string.Empty + empty + "[ffffff]:" + num15 + "/" + num16 + "/" + num17 + "/" + num18;
				if (RCextensions.returnBoolFromObject(value2.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = string.Concat(new object[3] { text, " \n", "[00FF00]INDIVIDUAL\n" });
			foreach (PhotonPlayer value3 in dictionary3.Values)
			{
				int num11 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.RCteam]);
				if (value3.customProperties[PhotonPlayerProperty.dead] == null || num11 != 0)
				{
					continue;
				}
				if (ignoreList.Contains(value3.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value3.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value3.ID) + "] ";
				if (value3.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value3.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num14 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.team]);
					if (num14 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num14 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				string empty = string.Empty;
				empty = RCextensions.returnStringFromObject(value3.customProperties[PhotonPlayerProperty.name]);
				int num15 = 0;
				num15 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.kills]);
				int num16 = 0;
				num16 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.deaths]);
				int num17 = 0;
				num17 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.max_dmg]);
				int num18 = 0;
				num18 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text2 + string.Empty + empty + "[ffffff]:" + num15 + "/" + num16 + "/" + num17 + "/" + num18;
				if (RCextensions.returnBoolFromObject(value3.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
		}
		else
		{
			PhotonPlayer[] array = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer4 in array)
			{
				if (photonPlayer4.customProperties[PhotonPlayerProperty.dead] == null)
				{
					continue;
				}
				if (ignoreList.Contains(photonPlayer4.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!photonPlayer4.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(photonPlayer4.ID) + "] ";
				if (photonPlayer4.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num14 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.team]);
					if (num14 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num14 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text3 = text;
				string empty = string.Empty;
				empty = RCextensions.returnStringFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.name]);
				int num15 = 0;
				num15 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.kills]);
				int num16 = 0;
				num16 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.deaths]);
				int num17 = 0;
				num17 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.max_dmg]);
				int num18 = 0;
				num18 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text3 + string.Empty + empty + "[ffffff]:" + num15 + "/" + num16 + "/" + num17 + "/" + num18;
				if (RCextensions.returnBoolFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
		}
		playerList = text;
		if (!PhotonNetwork.isMasterClient || isWinning || isLosing || !(roundTime >= 5f))
		{
			return;
		}
		if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
		{
			int num19 = 0;
			for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
			{
				PhotonPlayer photonPlayer5 = PhotonNetwork.playerList[j];
				if (ignoreList.Contains(photonPlayer5.ID) || photonPlayer5.customProperties[PhotonPlayerProperty.dead] == null || photonPlayer5.customProperties[PhotonPlayerProperty.isTitan] == null)
				{
					continue;
				}
				if (RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.isTitan]) == 1)
				{
					if (RCextensions.returnBoolFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.deaths]) > 0)
					{
						if (!imatitan.ContainsKey(photonPlayer5.ID))
						{
							imatitan.Add(photonPlayer5.ID, 2);
						}
						ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
						hashtable.Add(PhotonPlayerProperty.isTitan, 2);
						photonPlayer5.SetCustomProperties(hashtable);
						base.photonView.RPC("spawnTitanRPC", photonPlayer5);
					}
					else
					{
						if (!imatitan.ContainsKey(photonPlayer5.ID))
						{
							continue;
						}
						for (int k = 0; k < heroes.Count; k++)
						{
							HERO hERO = (HERO)heroes[k];
							if (hERO.photonView.owner == photonPlayer5)
							{
								hERO.markDie();
								hERO.photonView.RPC("netDie2", PhotonTargets.All, -1, "no switching in infection");
							}
						}
					}
				}
				else if (RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.isTitan]) == 2 && !RCextensions.returnBoolFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.dead]))
				{
					num19++;
				}
			}
			if (num19 <= 0 && IN_GAME_MAIN_CAMERA.gamemode != 0)
			{
				gameWin2();
			}
		}
		else if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
		{
			if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
			{
				if (cyanKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
				{
					object[] parameters = new object[2]
					{
						"<color=#00FFFF>Team Cyan wins! </color>",
						string.Empty
					};
					base.photonView.RPC("Chat", PhotonTargets.All, parameters);
					gameWin2();
				}
				else if (magentaKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
				{
					object[] parameters2 = new object[2]
					{
						"<color=#FF00FF>Team Magenta wins! </color>",
						string.Empty
					};
					base.photonView.RPC("Chat", PhotonTargets.All, parameters2);
					gameWin2();
				}
			}
			else
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0)
				{
					return;
				}
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					PhotonPlayer photonPlayer6 = PhotonNetwork.playerList[j];
					if (RCextensions.returnIntFromObject(photonPlayer6.customProperties[PhotonPlayerProperty.kills]) >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
					{
						object[] parameters3 = new object[2]
						{
							"<color=#FFCC00>" + RCextensions.returnStringFromObject(photonPlayer6.customProperties[PhotonPlayerProperty.name]).hexColor() + " wins!</color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters3);
						gameWin2();
					}
				}
			}
		}
		else
		{
			if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value || (!SettingsManager.LegacyGameSettings.BombModeEnabled.Value && SettingsManager.LegacyGameSettings.BladePVP.Value <= 0))
			{
				return;
			}
			if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.playerList.Length > 1)
			{
				int num20 = 0;
				int num21 = 0;
				int num22 = 0;
				int num23 = 0;
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					PhotonPlayer photonPlayer7 = PhotonNetwork.playerList[j];
					if (ignoreList.Contains(photonPlayer7.ID) || photonPlayer7.customProperties[PhotonPlayerProperty.RCteam] == null || photonPlayer7.customProperties[PhotonPlayerProperty.dead] == null)
					{
						continue;
					}
					if (RCextensions.returnIntFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.RCteam]) == 1)
					{
						num22++;
						if (!RCextensions.returnBoolFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.dead]))
						{
							num20++;
						}
					}
					else if (RCextensions.returnIntFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.RCteam]) == 2)
					{
						num23++;
						if (!RCextensions.returnBoolFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.dead]))
						{
							num21++;
						}
					}
				}
				if (num22 > 0 && num23 > 0)
				{
					if (num20 == 0)
					{
						object[] parameters4 = new object[2]
						{
							"<color=#FF00FF>Team Magenta wins! </color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters4);
						gameWin2();
					}
					else if (num21 == 0)
					{
						object[] parameters5 = new object[2]
						{
							"<color=#00FFFF>Team Cyan wins! </color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters5);
						gameWin2();
					}
				}
			}
			else
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0 || PhotonNetwork.playerList.Length <= 1)
				{
					return;
				}
				int num24 = 0;
				string text4 = "Nobody";
				PhotonPlayer player = PhotonNetwork.playerList[0];
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					PhotonPlayer photonPlayer8 = PhotonNetwork.playerList[j];
					if (photonPlayer8.customProperties[PhotonPlayerProperty.dead] != null && !RCextensions.returnBoolFromObject(photonPlayer8.customProperties[PhotonPlayerProperty.dead]))
					{
						text4 = RCextensions.returnStringFromObject(photonPlayer8.customProperties[PhotonPlayerProperty.name]).hexColor();
						player = photonPlayer8;
						num24++;
					}
				}
				if (num24 > 1)
				{
					return;
				}
				string text5 = " 5 points added.";
				if (text4 == "Nobody")
				{
					text5 = string.Empty;
				}
				else
				{
					for (int j = 0; j < 5; j++)
					{
						playerKillInfoUpdate(player, 0);
					}
				}
				object[] parameters6 = new object[2]
				{
					"<color=#FFCC00>" + text4.hexColor() + " wins." + text5 + "</color>",
					string.Empty
				};
				base.photonView.RPC("Chat", PhotonTargets.All, parameters6);
				gameWin2();
			}
		}
	}

	private void kickPhotonPlayer(string name)
	{
		UnityEngine.MonoBehaviour.print("KICK " + name + "!!!");
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer.ID.ToString() == name && !photonPlayer.isMasterClient)
			{
				PhotonNetwork.CloseConnection(photonPlayer);
				break;
			}
		}
	}

	private void kickPlayer(string kickPlayer, string kicker)
	{
		bool flag = false;
		for (int i = 0; i < kicklist.Count; i++)
		{
			if (((KickState)kicklist[i]).name == kickPlayer)
			{
				KickState kickState = (KickState)kicklist[i];
				kickState.addKicker(kicker);
				tryKick(kickState);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			KickState kickState = new KickState();
			kickState.init(kickPlayer);
			kickState.addKicker(kicker);
			kicklist.Add(kickState);
			tryKick(kickState);
		}
	}

	public void kickPlayerRCIfMC(PhotonPlayer player, bool ban, string reason)
	{
		if (PhotonNetwork.isMasterClient)
		{
			kickPlayerRC(player, ban, reason);
		}
	}

	public void kickPlayerRC(PhotonPlayer player, bool ban, string reason)
	{
		if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
		{
			string empty = string.Empty;
			empty = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
			ServerCloseConnection(player, ban, empty);
			return;
		}
		if (PhotonNetwork.isMasterClient && player == PhotonNetwork.player && reason != string.Empty)
		{
			chatRoom.addLINE("Attempting to ban myself for:" + reason + ", please report this to the devs.");
			return;
		}
		PhotonNetwork.DestroyPlayerObjects(player);
		PhotonNetwork.CloseConnection(player);
		base.photonView.RPC("ignorePlayer", PhotonTargets.Others, player.ID);
		if (!ignoreList.Contains(player.ID))
		{
			ignoreList.Add(player.ID);
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.TargetActors = new int[1] { player.ID };
			RaiseEventOptions options = raiseEventOptions;
			PhotonNetwork.RaiseEvent(254, null, sendReliable: true, options);
		}
		if (ban && !banHash.ContainsKey(player.ID))
		{
			string empty = string.Empty;
			empty = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
			banHash.Add(player.ID, empty);
		}
		if (reason != string.Empty)
		{
			chatRoom.addLINE("Player " + player.ID + " was autobanned. Reason:" + reason);
		}
		RecompilePlayerList(0.1f);
	}

	[RPC]
	private void labelRPC(int setting, PhotonMessageInfo info)
	{
		if (!(PhotonView.Find(setting) != null))
		{
			return;
		}
		PhotonPlayer owner = PhotonView.Find(setting).owner;
		if (owner != info.sender)
		{
			return;
		}
		string text = RCextensions.returnStringFromObject(owner.customProperties[PhotonPlayerProperty.guildName]);
		string text2 = RCextensions.returnStringFromObject(owner.customProperties[PhotonPlayerProperty.name]);
		GameObject gameObject = PhotonView.Find(setting).gameObject;
		if (!(gameObject != null))
		{
			return;
		}
		HERO component = gameObject.GetComponent<HERO>();
		if (component != null)
		{
			if (text != string.Empty)
			{
				component.myNetWorkName.GetComponent<UILabel>().text = "[FFFF00]" + text + "\n[FFFFFF]" + text2;
			}
			else
			{
				component.myNetWorkName.GetComponent<UILabel>().text = text2;
			}
		}
	}

	private void LateUpdate()
	{
		if (!gameStart)
		{
			return;
		}
		foreach (HERO hero in heroes)
		{
			hero.lateUpdate2();
		}
		foreach (TITAN_EREN item in eT)
		{
			item.lateUpdate();
		}
		foreach (TITAN titan in titans)
		{
			titan.lateUpdate2();
		}
		foreach (FEMALE_TITAN item2 in fT)
		{
			item2.lateUpdate2();
		}
		core2();
	}

	private void loadconfig()
	{
		object[] array = new object[500];
		array[31] = 0;
		array[64] = 0;
		array[68] = 100;
		array[69] = "default";
		array[70] = "1";
		array[71] = "1";
		array[72] = "1";
		array[73] = 1f;
		array[74] = 1f;
		array[75] = 1f;
		array[76] = 0;
		array[77] = string.Empty;
		array[78] = 0;
		array[79] = "1.0";
		array[80] = "1.0";
		array[81] = 0;
		array[83] = "30";
		array[84] = 0;
		array[91] = 0;
		array[100] = 0;
		array[185] = 0;
		array[186] = 0;
		array[187] = 0;
		array[188] = 0;
		array[190] = 0;
		array[191] = string.Empty;
		array[230] = 0;
		array[263] = 0;
		linkHash = new ExitGames.Client.Photon.Hashtable[5]
		{
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable()
		};
		settingsOld = array;
		scroll = Vector2.zero;
		scroll2 = Vector2.zero;
		transparencySlider = 1f;
		SettingsManager.LegacyGeneralSettings.SetDefault();
		MaterialCache.Clear();
	}

	private void loadskin()
	{
		if ((int)settingsOld[64] >= 100)
		{
			string[] array = new string[5] { "Flare", "LabelInfoBottomRight", "LabelNetworkStatus", "skill_cd_bottom", "GasUI" };
			GameObject[] array2 = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			foreach (GameObject gameObject in array2)
			{
				if (gameObject.name.Contains("TREE") || gameObject.name.Contains("aot_supply") || gameObject.name.Contains("gameobjectOutSide"))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			GameObject.Find("Cube_001").renderer.material.mainTexture = ((Material)RCassets.Load("grass")).mainTexture;
			UnityEngine.Object.Instantiate(RCassets.Load("spawnPlayer"), new Vector3(-10f, 1f, -10f), new Quaternion(0f, 0f, 0f, 1f));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject2 = GameObject.Find(array[i]);
				if (gameObject2 != null)
				{
					UnityEngine.Object.Destroy(gameObject2);
				}
			}
			Camera.main.GetComponent<SpectatorMovement>().disable = true;
			return;
		}
		InstantiateTracker.instance.Dispose();
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			updateTime = 1f;
			if (oldScriptLogic != SettingsManager.LegacyGameSettings.LogicScript.Value)
			{
				intVariables.Clear();
				boolVariables.Clear();
				stringVariables.Clear();
				floatVariables.Clear();
				globalVariables.Clear();
				RCEvents.Clear();
				RCVariableNames.Clear();
				playerVariables.Clear();
				titanVariables.Clear();
				RCRegionTriggers.Clear();
				oldScriptLogic = SettingsManager.LegacyGameSettings.LogicScript.Value;
				compileScript(SettingsManager.LegacyGameSettings.LogicScript.Value);
				if (RCEvents.ContainsKey("OnFirstLoad"))
				{
					((RCEvent)RCEvents["OnFirstLoad"]).checkEvent();
				}
			}
			if (RCEvents.ContainsKey("OnRoundStart"))
			{
				((RCEvent)RCEvents["OnRoundStart"]).checkEvent();
			}
			base.photonView.RPC("setMasterRC", PhotonTargets.All);
		}
		logicLoaded = true;
		racingSpawnPoint = new Vector3(0f, 0f, 0f);
		racingSpawnPointSet = false;
		racingDoors = new List<GameObject>();
		allowedToCannon = new Dictionary<int, CannonValues>();
		bool flag = false;
		string[] array3 = new string[6]
		{
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty
		};
		if (SettingsManager.CustomSkinSettings.Skybox.SkinsEnabled.Value)
		{
			SkyboxCustomSkinSet skyboxCustomSkinSet = (SkyboxCustomSkinSet)SettingsManager.CustomSkinSettings.Skybox.GetSelectedSet();
			array3 = new string[6]
			{
				skyboxCustomSkinSet.Front.Value,
				skyboxCustomSkinSet.Back.Value,
				skyboxCustomSkinSet.Left.Value,
				skyboxCustomSkinSet.Right.Value,
				skyboxCustomSkinSet.Up.Value,
				skyboxCustomSkinSet.Down.Value
			};
			flag = true;
		}
		if (!level.StartsWith("Custom") && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.isMasterClient))
		{
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			if (LevelInfo.getInfo(level).mapName.Contains("City") && SettingsManager.CustomSkinSettings.City.SkinsEnabled.Value)
			{
				CityCustomSkinSet cityCustomSkinSet = (CityCustomSkinSet)SettingsManager.CustomSkinSettings.City.GetSelectedSet();
				List<string> list = new List<string>();
				foreach (StringSetting item in cityCustomSkinSet.Houses.GetItems())
				{
					list.Add(item.Value);
				}
				text2 = string.Join(",", list.ToArray());
				for (int j = 0; j < 250; j++)
				{
					text += Convert.ToString((int)UnityEngine.Random.Range(0f, 8f));
				}
				text3 = string.Join(",", new string[3]
				{
					cityCustomSkinSet.Ground.Value,
					cityCustomSkinSet.Wall.Value,
					cityCustomSkinSet.Gate.Value
				});
				flag = true;
			}
			else if (LevelInfo.getInfo(level).mapName.Contains("Forest") && SettingsManager.CustomSkinSettings.Forest.SkinsEnabled.Value)
			{
				ForestCustomSkinSet forestCustomSkinSet = (ForestCustomSkinSet)SettingsManager.CustomSkinSettings.Forest.GetSelectedSet();
				List<string> list2 = new List<string>();
				foreach (StringSetting item2 in forestCustomSkinSet.TreeTrunks.GetItems())
				{
					list2.Add(item2.Value);
				}
				text2 = string.Join(",", list2.ToArray());
				List<string> list3 = new List<string>();
				foreach (StringSetting item3 in forestCustomSkinSet.TreeLeafs.GetItems())
				{
					list3.Add(item3.Value);
				}
				list3.Add(forestCustomSkinSet.Ground.Value);
				text3 = string.Join(",", list3.ToArray());
				for (int k = 0; k < 150; k++)
				{
					string text4 = Convert.ToString((int)UnityEngine.Random.Range(0f, 8f));
					text += text4;
					text = (forestCustomSkinSet.RandomizedPairs.Value ? (text + Convert.ToString((int)UnityEngine.Random.Range(0f, 8f))) : (text + text4));
				}
				flag = true;
			}
			if (flag)
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					StartCoroutine(loadskinE(text, text2, text3, array3));
				}
				else if (PhotonNetwork.isMasterClient)
				{
					base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, text, text2, text3, array3);
				}
			}
		}
		else
		{
			if (!level.StartsWith("Custom") || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				return;
			}
			GameObject[] array4 = GameObject.FindGameObjectsWithTag("playerRespawn");
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].transform.position = new Vector3(UnityEngine.Random.Range(-5f, 5f), 0f, UnityEngine.Random.Range(-5f, 5f));
			}
			GameObject[] array2 = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			foreach (GameObject gameObject in array2)
			{
				if (gameObject.name.Contains("TREE") || gameObject.name.Contains("aot_supply"))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				else if (gameObject.name == "Cube_001" && gameObject.transform.parent.gameObject.tag != "player" && gameObject.renderer != null)
				{
					groundList.Add(gameObject);
					gameObject.renderer.material.mainTexture = ((Material)RCassets.Load("grass")).mainTexture;
				}
			}
			if (!PhotonNetwork.isMasterClient)
			{
				return;
			}
			string[] array5 = new string[7];
			for (int l = 0; l < 6; l++)
			{
				array5[l] = array3[l];
			}
			array5[6] = ((CustomLevelCustomSkinSet)SettingsManager.CustomSkinSettings.CustomLevel.GetSelectedSet()).Ground.Value;
			SettingsManager.LegacyGameSettings.TitanSpawnCap.Value = Math.Min(100, SettingsManager.LegacyGameSettings.TitanSpawnCap.Value);
			base.photonView.RPC("clearlevel", PhotonTargets.AllBuffered, array5, SettingsManager.LegacyGameSettings.GameType.Value);
			RCRegions.Clear();
			if (oldScript != SettingsManager.LegacyGameSettings.LevelScript.Value)
			{
				levelCache.Clear();
				titanSpawns.Clear();
				playerSpawnsC.Clear();
				playerSpawnsM.Clear();
				titanSpawners.Clear();
				currentLevel = string.Empty;
				if (SettingsManager.LegacyGameSettings.LevelScript.Value == string.Empty)
				{
					ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable.Add(PhotonPlayerProperty.currentLevel, currentLevel);
					PhotonNetwork.player.SetCustomProperties(hashtable);
					oldScript = SettingsManager.LegacyGameSettings.LevelScript.Value;
				}
				else
				{
					string[] array6 = Regex.Replace(SettingsManager.LegacyGameSettings.LevelScript.Value, "\\s+", "").Replace("\r\n", "").Replace("\n", "")
						.Replace("\r", "")
						.Split(';');
					for (int i = 0; i < Mathf.FloorToInt((array6.Length - 1) / 100) + 1; i++)
					{
						string[] array7;
						int num;
						if (i < Mathf.FloorToInt(array6.Length / 100))
						{
							array7 = new string[101];
							num = 0;
							for (int m = 100 * i; m < 100 * i + 100; m++)
							{
								if (array6[m].StartsWith("spawnpoint"))
								{
									string[] array8 = array6[m].Split(',');
									if (array8[1] == "titan")
									{
										titanSpawns.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
									}
									else if (array8[1] == "playerC")
									{
										playerSpawnsC.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
									}
									else if (array8[1] == "playerM")
									{
										playerSpawnsM.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
									}
								}
								array7[num] = array6[m];
								num++;
							}
							currentLevel += (array7[100] = UnityEngine.Random.Range(10000, 99999).ToString());
							levelCache.Add(array7);
							continue;
						}
						array7 = new string[array6.Length % 100 + 1];
						num = 0;
						for (int m = 100 * i; m < 100 * i + array6.Length % 100; m++)
						{
							if (array6[m].StartsWith("spawnpoint"))
							{
								string[] array8 = array6[m].Split(',');
								if (array8[1] == "titan")
								{
									titanSpawns.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
								}
								else if (array8[1] == "playerC")
								{
									playerSpawnsC.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
								}
								else if (array8[1] == "playerM")
								{
									playerSpawnsM.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
								}
							}
							array7[num] = array6[m];
							num++;
						}
						string text5 = UnityEngine.Random.Range(10000, 99999).ToString();
						array7[array6.Length % 100] = text5;
						currentLevel += text5;
						levelCache.Add(array7);
					}
					List<string> list4 = new List<string>();
					foreach (Vector3 titanSpawn in titanSpawns)
					{
						string[] obj = new string[6] { "titan,", null, null, null, null, null };
						float x = titanSpawn.x;
						obj[1] = x.ToString();
						obj[2] = ",";
						x = titanSpawn.y;
						obj[3] = x.ToString();
						obj[4] = ",";
						x = titanSpawn.z;
						obj[5] = x.ToString();
						list4.Add(string.Concat(obj));
					}
					foreach (Vector3 item4 in playerSpawnsC)
					{
						string[] obj2 = new string[6] { "playerC,", null, null, null, null, null };
						float x = item4.x;
						obj2[1] = x.ToString();
						obj2[2] = ",";
						x = item4.y;
						obj2[3] = x.ToString();
						obj2[4] = ",";
						x = item4.z;
						obj2[5] = x.ToString();
						list4.Add(string.Concat(obj2));
					}
					foreach (Vector3 item5 in playerSpawnsM)
					{
						string[] obj3 = new string[6] { "playerM,", null, null, null, null, null };
						float x = item5.x;
						obj3[1] = x.ToString();
						obj3[2] = ",";
						x = item5.y;
						obj3[3] = x.ToString();
						obj3[4] = ",";
						x = item5.z;
						obj3[5] = x.ToString();
						list4.Add(string.Concat(obj3));
					}
					string text6 = "a" + UnityEngine.Random.Range(10000, 99999);
					list4.Add(text6);
					currentLevel = text6 + currentLevel;
					levelCache.Insert(0, list4.ToArray());
					string text7 = "z" + UnityEngine.Random.Range(10000, 99999);
					levelCache.Add(new string[1] { text7 });
					currentLevel += text7;
					ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable.Add(PhotonPlayerProperty.currentLevel, currentLevel);
					PhotonNetwork.player.SetCustomProperties(hashtable);
					oldScript = SettingsManager.LegacyGameSettings.LevelScript.Value;
				}
			}
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
				if (!photonPlayer.isMasterClient)
				{
					playersRPC.Add(photonPlayer);
				}
			}
			StartCoroutine(customlevelE(playersRPC));
			StartCoroutine(customlevelcache());
		}
	}

	private IEnumerator loadskinE(string n, string url, string url2, string[] skybox)
	{
		if (IsValidSkybox(skybox))
		{
			yield return StartCoroutine(_skyboxCustomSkinLoader.LoadSkinsFromRPC(skybox));
		}
		else
		{
			SkyboxCustomSkinLoader.SkyboxMaterial = null;
		}
		if (n != string.Empty)
		{
			if (LevelInfo.getInfo(level).mapName.Contains("Forest"))
			{
				yield return StartCoroutine(_forestCustomSkinLoader.LoadSkinsFromRPC(new object[3] { n, url, url2 }));
			}
			else if (LevelInfo.getInfo(level).mapName.Contains("City"))
			{
				yield return StartCoroutine(_cityCustomSkinLoader.LoadSkinsFromRPC(new object[3] { n, url, url2 }));
			}
		}
		Minimap.TryRecaptureInstance();
		StartCoroutine(reloadSky());
		yield return null;
	}

	private bool IsValidSkybox(string[] skybox)
	{
		for (int i = 0; i < skybox.Length; i++)
		{
			if (TextureDownloader.ValidTextureURL(skybox[i]))
			{
				return true;
			}
		}
		return false;
	}

	[RPC]
	private void loadskinRPC(string n, string url1, string url2, string[] skybox, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient)
		{
			return;
		}
		if (LevelInfo.getInfo(level).mapName.Contains("Forest"))
		{
			BaseCustomSkinSettings<ForestCustomSkinSet> forest = SettingsManager.CustomSkinSettings.Forest;
			if (forest.SkinsEnabled.Value && (!forest.SkinsLocal.Value || PhotonNetwork.isMasterClient))
			{
				StartCoroutine(loadskinE(n, url1, url2, skybox));
			}
		}
		else if (LevelInfo.getInfo(level).mapName.Contains("City"))
		{
			BaseCustomSkinSettings<CityCustomSkinSet> city = SettingsManager.CustomSkinSettings.City;
			if (city.SkinsEnabled.Value && (!city.SkinsLocal.Value || PhotonNetwork.isMasterClient))
			{
				StartCoroutine(loadskinE(n, url1, url2, skybox));
			}
		}
	}

	private IEnumerator loginFeng()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("userid", usernameField);
		wWWForm.AddField("password", passwordField);
		WWW iteratorVariable1 = ((!Application.isWebPlayer) ? new WWW("http://fenglee.com/game/aog/require_user_info.php", wWWForm) : new WWW("http://aotskins.com/version/getinfo.php", wWWForm));
		yield return iteratorVariable1;
		if (iteratorVariable1.error == null && !iteratorVariable1.text.Contains("Error,please sign in again."))
		{
			char[] separator = new char[1] { '|' };
			string[] array = iteratorVariable1.text.Split(separator);
			LoginFengKAI.player.name = usernameField;
			LoginFengKAI.player.guildname = array[0];
			loginstate = 3;
		}
		else
		{
			loginstate = 2;
		}
	}

	private string mastertexturetype(int lol)
	{
		return lol switch
		{
			0 => "High", 
			1 => "Med", 
			_ => "Low", 
		};
	}

	public void multiplayerRacingFinsih()
	{
		float num = roundTime - SettingsManager.LegacyGameSettings.RacingStartTime.Value;
		if (PhotonNetwork.isMasterClient)
		{
			getRacingResult(LoginFengKAI.player.name, num, null);
		}
		else
		{
			object[] parameters = new object[2]
			{
				LoginFengKAI.player.name,
				num
			};
			base.photonView.RPC("getRacingResult", PhotonTargets.MasterClient, parameters);
		}
		gameWin2();
	}

	[RPC]
	private void netGameLose(int score, PhotonMessageInfo info)
	{
		isLosing = true;
		titanScore = score;
		gameEndCD = gameEndTotalCDtime;
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			chatRoom.addLINE("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game lose).");
		}
		if (info.sender != PhotonNetwork.masterClient && !info.sender.isLocal && PhotonNetwork.isMasterClient)
		{
			chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID + "</color>");
		}
	}

	[RPC]
	private void netGameWin(int score, PhotonMessageInfo info)
	{
		humanScore = score;
		isWinning = true;
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			teamWinner = score;
			teamScores[teamWinner - 1]++;
			gameEndCD = gameEndTotalCDtime;
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
		{
			if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
			{
				gameEndCD = 1000f;
			}
			else
			{
				gameEndCD = 20f;
			}
		}
		else
		{
			gameEndCD = gameEndTotalCDtime;
		}
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			chatRoom.addLINE("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).");
		}
		if (info.sender != PhotonNetwork.masterClient && !info.sender.isLocal)
		{
			chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID + "</color>");
		}
	}

	[RPC]
	private void netRefreshRacingResult(string tmp)
	{
		localRacingResult = tmp;
	}

	[RPC]
	public void netShowDamage(int speed)
	{
		GameObject.Find("Stylish").GetComponent<StylishComponent>().Style(speed);
		GameObject gameObject = GameObject.Find("LabelScore");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = speed.ToString();
			gameObject.transform.localScale = Vector3.zero;
			speed = (int)((float)speed * 0.1f);
			speed = Mathf.Max(40, speed);
			speed = Mathf.Min(150, speed);
			iTween.Stop(gameObject);
			object[] args = new object[10]
			{
				"x",
				speed,
				"y",
				speed,
				"z",
				speed,
				"easetype",
				iTween.EaseType.easeOutElastic,
				"time",
				1f
			};
			iTween.ScaleTo(gameObject, iTween.Hash(args));
			object[] args2 = new object[12]
			{
				"x",
				0,
				"y",
				0,
				"z",
				0,
				"easetype",
				iTween.EaseType.easeInBounce,
				"time",
				0.5f,
				"delay",
				2f
			};
			iTween.ScaleTo(gameObject, iTween.Hash(args2));
		}
	}

	public void NOTSpawnNonAITitan(string id)
	{
		myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", true } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			2
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void NOTSpawnNonAITitanRC(string id)
	{
		myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", true } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			2
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		ShowHUDInfoCenter("Syncing spawn locations...");
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void NOTSpawnPlayer(string id)
	{
		myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", true } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			1
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void NOTSpawnPlayerRC(string id)
	{
		myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", true } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			1
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void OnConnectedToMaster()
	{
		UnityEngine.MonoBehaviour.print("OnConnectedToMaster");
	}

	public void OnConnectedToPhoton()
	{
		UnityEngine.MonoBehaviour.print("OnConnectedToPhoton");
	}

	public void OnConnectionFail(DisconnectCause cause)
	{
		UnityEngine.MonoBehaviour.print("OnConnectionFail : " + cause);
		IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
		gameStart = false;
		NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[0], state: false);
		NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
		NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
		NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: false);
		NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[4], state: true);
		GameObject.Find("LabelDisconnectInfo").GetComponent<UILabel>().text = "OnConnectionFail : " + cause;
	}

	public void OnCreatedRoom()
	{
		kicklist = new ArrayList();
		racingResult = new ArrayList();
		teamScores = new int[2];
		UnityEngine.MonoBehaviour.print("OnCreatedRoom");
	}

	public void OnCustomAuthenticationFailed()
	{
		UnityEngine.MonoBehaviour.print("OnCustomAuthenticationFailed");
	}

	public void OnDisconnectedFromPhoton()
	{
		UnityEngine.MonoBehaviour.print("OnDisconnectedFromPhoton");
	}

	[RPC]
	public void oneTitanDown(string name1, bool onPlayerLeave)
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !PhotonNetwork.isMasterClient)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			if (name1 != string.Empty)
			{
				switch (name1)
				{
				case "Titan":
					PVPhumanScore++;
					break;
				case "Aberrant":
					PVPhumanScore += 2;
					break;
				case "Jumper":
					PVPhumanScore += 3;
					break;
				case "Crawler":
					PVPhumanScore += 4;
					break;
				case "Female Titan":
					PVPhumanScore += 10;
					break;
				default:
					PVPhumanScore += 3;
					break;
				}
			}
			checkPVPpts();
			object[] parameters = new object[2] { PVPhumanScore, PVPtitanScore };
			base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
		}
		else
		{
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.CAGE_FIGHT)
			{
				return;
			}
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN)
			{
				if (checkIsTitanAllDie())
				{
					gameWin2();
					Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
			{
				if (!checkIsTitanAllDie())
				{
					return;
				}
				wave++;
				if ((LevelInfo.getInfo(level).respawnMode == RespawnMode.NEWROUND || (level.StartsWith("Custom") && SettingsManager.LegacyGameSettings.GameType.Value == 1)) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
				{
					PhotonPlayer[] array = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer in array)
					{
						if (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) != 2)
						{
							base.photonView.RPC("respawnHeroInNewRound", photonPlayer);
						}
					}
				}
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
				{
					sendChatContentInfo("<color=#A8FF24>Wave : " + wave + "</color>");
				}
				if (wave > highestwave)
				{
					highestwave = wave;
				}
				if (PhotonNetwork.isMasterClient)
				{
					RequireStatus();
				}
				if ((!SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value && wave > 20) || (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value && wave > SettingsManager.LegacyGameSettings.TitanMaxWaves.Value))
				{
					gameWin2();
					return;
				}
				int abnormal = 90;
				if (difficulty == 1)
				{
					abnormal = 70;
				}
				if (!LevelInfo.getInfo(level).punk)
				{
					spawnTitanCustom("titanRespawn", abnormal, wave + 2, punk: false);
				}
				else if (wave == 5)
				{
					spawnTitanCustom("titanRespawn", abnormal, 1, punk: true);
				}
				else if (wave == 10)
				{
					spawnTitanCustom("titanRespawn", abnormal, 2, punk: true);
				}
				else if (wave == 15)
				{
					spawnTitanCustom("titanRespawn", abnormal, 3, punk: true);
				}
				else if (wave == 20)
				{
					spawnTitanCustom("titanRespawn", abnormal, 4, punk: true);
				}
				else
				{
					spawnTitanCustom("titanRespawn", abnormal, wave + 2, punk: false);
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
			{
				if (!onPlayerLeave)
				{
					humanScore++;
					int abnormal2 = 90;
					if (difficulty == 1)
					{
						abnormal2 = 70;
					}
					spawnTitanCustom("titanRespawn", abnormal2, 1, punk: false);
				}
			}
			else
			{
				_ = LevelInfo.getInfo(level).enemyNumber;
				_ = -1;
			}
		}
	}

	public void OnFailedToConnectToPhoton()
	{
		UnityEngine.MonoBehaviour.print("OnFailedToConnectToPhoton");
	}

	private void DrawBackgroundIfLoading()
	{
		if (AssetBundleManager.Status == AssetBundleStatus.Loading || AutoUpdateManager.Status == AutoUpdateStatus.Updating)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), textureBackgroundBlue);
		}
	}

	public void OnGUI()
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP && Application.loadedLevelName != "characterCreation")
		{
			LegacyPopupTemplate legacyPopupTemplate = new LegacyPopupTemplate(new Color(0f, 0f, 0f, 1f), textureBackgroundBlack, new Color(1f, 1f, 1f, 1f), (float)Screen.width / 2f, (float)Screen.height / 2f, 230f, 140f, 2f);
			DrawBackgroundIfLoading();
			if (AutoUpdateManager.Status == AutoUpdateStatus.Updating)
			{
				legacyPopupTemplate.DrawPopup("Auto-updating mod...", 130f, 22f);
			}
			else if (AutoUpdateManager.Status == AutoUpdateStatus.NeedRestart && !AutoUpdateManager.CloseFailureBox)
			{
				bool[] array = legacyPopupTemplate.DrawPopupWithTwoButtons("Mod has been updated and requires a restart.", 190f, 44f, "Restart Now", 90f, "Ignore", 90f, 25f);
				if (array[0])
				{
					if (Application.platform == RuntimePlatform.WindowsPlayer)
					{
						Process.Start(Application.dataPath.Replace("_Data", ".exe"));
					}
					else if (Application.platform == RuntimePlatform.OSXPlayer)
					{
						Process.Start(Application.dataPath + "/MacOS/MacTest");
					}
					Application.Quit();
				}
				else if (array[1])
				{
					AutoUpdateManager.CloseFailureBox = true;
				}
			}
			else if (AutoUpdateManager.Status == AutoUpdateStatus.LauncherOutdated && !AutoUpdateManager.CloseFailureBox)
			{
				if (legacyPopupTemplate.DrawPopupWithButton("Game launcher is outdated, visit aotrc.weebly.com for a new game version.", 190f, 66f, "Continue", 80f, 25f))
				{
					AutoUpdateManager.CloseFailureBox = true;
				}
			}
			else if (AutoUpdateManager.Status == AutoUpdateStatus.FailedUpdate && !AutoUpdateManager.CloseFailureBox)
			{
				if (legacyPopupTemplate.DrawPopupWithButton("Auto-update failed, check internet connection or aotrc.weebly.com for a new game version.", 190f, 66f, "Continue", 80f, 25f))
				{
					AutoUpdateManager.CloseFailureBox = true;
				}
			}
			else if (AutoUpdateManager.Status == AutoUpdateStatus.MacTranslocated && !AutoUpdateManager.CloseFailureBox)
			{
				if (legacyPopupTemplate.DrawPopupWithButton("Your game is not in the Applications folder, cannot auto-update and some bugs may occur.", 190f, 66f, "Continue", 80f, 25f))
				{
					AutoUpdateManager.CloseFailureBox = true;
				}
			}
			else if (AssetBundleManager.Status == AssetBundleStatus.Loading)
			{
				legacyPopupTemplate.DrawPopup("Downloading asset bundle...", 170f, 22f);
			}
			else if (AssetBundleManager.Status == AssetBundleStatus.Failed && !AssetBundleManager.CloseFailureBox && legacyPopupTemplate.DrawPopupWithButton("Failed to load asset bundle, check your internet connection.", 190f, 44f, "Continue", 80f, 25f))
			{
				AssetBundleManager.CloseFailureBox = true;
			}
		}
		else
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP)
			{
				return;
			}
			if ((int)settingsOld[64] >= 100)
			{
				float num = (float)Screen.width - 300f;
				GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
				GUI.DrawTexture(new Rect(7f, 7f, 291f, 586f), textureBackgroundBlue);
				GUI.DrawTexture(new Rect(num + 2f, 7f, 291f, 586f), textureBackgroundBlue);
				bool flag = false;
				bool flag2 = false;
				GUI.Box(new Rect(5f, 5f, 295f, 590f), string.Empty);
				GUI.Box(new Rect(num, 5f, 295f, 590f), string.Empty);
				if (GUI.Button(new Rect(10f, 10f, 60f, 25f), "Script", "box"))
				{
					settingsOld[68] = 100;
				}
				if (GUI.Button(new Rect(75f, 10f, 80f, 25f), "Full Screen", "box"))
				{
					FullscreenHandler.ToggleFullscreen();
				}
				if ((int)settingsOld[68] == 100 || (int)settingsOld[68] == 102)
				{
					GUI.Label(new Rect(115f, 40f, 100f, 20f), "Level Script:", "Label");
					GUI.Label(new Rect(115f, 115f, 100f, 20f), "Import Data", "Label");
					GUI.Label(new Rect(12f, 535f, 280f, 60f), "Warning: your current level will be lost if you quit or import data. Make sure to save the level to a text document.", "Label");
					settingsOld[77] = GUI.TextField(new Rect(10f, 140f, 285f, 350f), (string)settingsOld[77]);
					if (GUI.Button(new Rect(35f, 500f, 60f, 30f), "Apply"))
					{
						UnityEngine.Object[] array2 = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
						for (int i = 0; i < array2.Length; i++)
						{
							GameObject gameObject = (GameObject)array2[i];
							if (gameObject.name.StartsWith("custom") || gameObject.name.StartsWith("base") || gameObject.name.StartsWith("photon") || gameObject.name.StartsWith("spawnpoint") || gameObject.name.StartsWith("misc") || gameObject.name.StartsWith("racing"))
							{
								UnityEngine.Object.Destroy(gameObject);
							}
						}
						linkHash[3].Clear();
						settingsOld[186] = 0;
						string[] array3 = Regex.Replace((string)settingsOld[77], "\\s+", "").Replace("\r\n", "").Replace("\n", "")
							.Replace("\r", "")
							.Split(';');
						for (int j = 0; j < array3.Length; j++)
						{
							string[] array4 = array3[j].Split(',');
							if (array4[0].StartsWith("custom") || array4[0].StartsWith("base") || array4[0].StartsWith("photon") || array4[0].StartsWith("spawnpoint") || array4[0].StartsWith("misc") || array4[0].StartsWith("racing"))
							{
								GameObject gameObject2 = null;
								if (array4[0].StartsWith("custom"))
								{
									gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array4[1]), new Vector3(Convert.ToSingle(array4[12]), Convert.ToSingle(array4[13]), Convert.ToSingle(array4[14])), new Quaternion(Convert.ToSingle(array4[15]), Convert.ToSingle(array4[16]), Convert.ToSingle(array4[17]), Convert.ToSingle(array4[18])));
								}
								else if (array4[0].StartsWith("photon"))
								{
									gameObject2 = ((!array4[1].StartsWith("Cannon")) ? ((GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array4[1]), new Vector3(Convert.ToSingle(array4[4]), Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6])), new Quaternion(Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10])))) : ((array4.Length >= 15) ? ((GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array4[1] + "Prop"), new Vector3(Convert.ToSingle(array4[12]), Convert.ToSingle(array4[13]), Convert.ToSingle(array4[14])), new Quaternion(Convert.ToSingle(array4[15]), Convert.ToSingle(array4[16]), Convert.ToSingle(array4[17]), Convert.ToSingle(array4[18])))) : ((GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array4[1] + "Prop"), new Vector3(Convert.ToSingle(array4[2]), Convert.ToSingle(array4[3]), Convert.ToSingle(array4[4])), new Quaternion(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8]))))));
								}
								else if (array4[0].StartsWith("spawnpoint"))
								{
									gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array4[1]), new Vector3(Convert.ToSingle(array4[2]), Convert.ToSingle(array4[3]), Convert.ToSingle(array4[4])), new Quaternion(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8])));
								}
								else if (array4[0].StartsWith("base"))
								{
									gameObject2 = ((array4.Length >= 15) ? ((GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(array4[1]), new Vector3(Convert.ToSingle(array4[12]), Convert.ToSingle(array4[13]), Convert.ToSingle(array4[14])), new Quaternion(Convert.ToSingle(array4[15]), Convert.ToSingle(array4[16]), Convert.ToSingle(array4[17]), Convert.ToSingle(array4[18])))) : ((GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(array4[1]), new Vector3(Convert.ToSingle(array4[2]), Convert.ToSingle(array4[3]), Convert.ToSingle(array4[4])), new Quaternion(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8])))));
								}
								else if (array4[0].StartsWith("misc"))
								{
									if (array4[1].StartsWith("barrier"))
									{
										gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load("barrierEditor"), new Vector3(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7])), new Quaternion(Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10]), Convert.ToSingle(array4[11])));
									}
									else if (array4[1].StartsWith("region"))
									{
										gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load("regionEditor"));
										gameObject2.transform.position = new Vector3(Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8]));
										GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
										gameObject3.name = "RegionLabel";
										gameObject3.transform.parent = gameObject2.transform;
										float y = 1f;
										if (Convert.ToSingle(array4[4]) > 100f)
										{
											y = 0.8f;
										}
										else if (Convert.ToSingle(array4[4]) > 1000f)
										{
											y = 0.5f;
										}
										gameObject3.transform.localPosition = new Vector3(0f, y, 0f);
										gameObject3.transform.localScale = new Vector3(5f / Convert.ToSingle(array4[3]), 5f / Convert.ToSingle(array4[4]), 5f / Convert.ToSingle(array4[5]));
										gameObject3.GetComponent<UILabel>().text = array4[2];
										gameObject2.AddComponent<RCRegionLabel>();
										gameObject2.GetComponent<RCRegionLabel>().myLabel = gameObject3;
									}
									else if (array4[1].StartsWith("racingStart"))
									{
										gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load("racingStart"), new Vector3(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7])), new Quaternion(Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10]), Convert.ToSingle(array4[11])));
									}
									else if (array4[1].StartsWith("racingEnd"))
									{
										gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load("racingEnd"), new Vector3(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7])), new Quaternion(Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10]), Convert.ToSingle(array4[11])));
									}
								}
								else if (array4[0].StartsWith("racing"))
								{
									gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.Load(array4[1]), new Vector3(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7])), new Quaternion(Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10]), Convert.ToSingle(array4[11])));
								}
								if (array4[2] != "default" && (array4[0].StartsWith("custom") || (array4[0].StartsWith("base") && array4.Length > 15) || (array4[0].StartsWith("photon") && array4.Length > 15)))
								{
									Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
									foreach (Renderer renderer in componentsInChildren)
									{
										if (!renderer.name.Contains("Particle System") || !gameObject2.name.Contains("aot_supply"))
										{
											renderer.material = (Material)RCassets.Load(array4[2]);
											renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array4[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array4[11]));
										}
									}
								}
								if (array4[0].StartsWith("custom") || (array4[0].StartsWith("base") && array4.Length > 15) || (array4[0].StartsWith("photon") && array4.Length > 15))
								{
									float num2 = gameObject2.transform.localScale.x * Convert.ToSingle(array4[3]);
									num2 -= 0.001f;
									float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array4[4]);
									float z = gameObject2.transform.localScale.z * Convert.ToSingle(array4[5]);
									gameObject2.transform.localScale = new Vector3(num2, y2, z);
									if (array4[6] != "0")
									{
										Color color = new Color(Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), 1f);
										MeshFilter[] componentsInChildren2 = gameObject2.GetComponentsInChildren<MeshFilter>();
										for (int i = 0; i < componentsInChildren2.Length; i++)
										{
											Mesh mesh = componentsInChildren2[i].mesh;
											Color[] array5 = new Color[mesh.vertexCount];
											for (int k = 0; k < mesh.vertexCount; k++)
											{
												array5[k] = color;
											}
											mesh.colors = array5;
										}
									}
									gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3] + "," + array4[4] + "," + array4[5] + "," + array4[6] + "," + array4[7] + "," + array4[8] + "," + array4[9] + "," + array4[10] + "," + array4[11];
								}
								else if (array4[0].StartsWith("misc"))
								{
									if (array4[1].StartsWith("barrier") || array4[1].StartsWith("racing"))
									{
										float num2 = gameObject2.transform.localScale.x * Convert.ToSingle(array4[2]);
										num2 -= 0.001f;
										float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array4[3]);
										float z = gameObject2.transform.localScale.z * Convert.ToSingle(array4[4]);
										gameObject2.transform.localScale = new Vector3(num2, y2, z);
										gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3] + "," + array4[4];
									}
									else if (array4[1].StartsWith("region"))
									{
										float num2 = gameObject2.transform.localScale.x * Convert.ToSingle(array4[3]);
										num2 -= 0.001f;
										float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array4[4]);
										float z = gameObject2.transform.localScale.z * Convert.ToSingle(array4[5]);
										gameObject2.transform.localScale = new Vector3(num2, y2, z);
										gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3] + "," + array4[4] + "," + array4[5];
									}
								}
								else if (array4[0].StartsWith("racing"))
								{
									float num2 = gameObject2.transform.localScale.x * Convert.ToSingle(array4[2]);
									num2 -= 0.001f;
									float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array4[3]);
									float z = gameObject2.transform.localScale.z * Convert.ToSingle(array4[4]);
									gameObject2.transform.localScale = new Vector3(num2, y2, z);
									gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3] + "," + array4[4];
								}
								else if (array4[0].StartsWith("photon") && !array4[1].StartsWith("Cannon"))
								{
									gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3];
								}
								else
								{
									gameObject2.name = array4[0] + "," + array4[1];
								}
								linkHash[3].Add(gameObject2.GetInstanceID(), array3[j]);
							}
							else if (array4[0].StartsWith("map") && array4[1].StartsWith("disablebounds"))
							{
								settingsOld[186] = 1;
								if (!linkHash[3].ContainsKey("mapbounds"))
								{
									linkHash[3].Add("mapbounds", "map,disablebounds");
								}
							}
						}
						unloadAssets();
						settingsOld[77] = string.Empty;
					}
					else if (GUI.Button(new Rect(205f, 500f, 60f, 30f), "Exit"))
					{
						IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
						UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
						Application.LoadLevel("menu");
					}
					else if (GUI.Button(new Rect(15f, 70f, 115f, 30f), "Copy to Clipboard"))
					{
						string text = string.Empty;
						int num3 = 0;
						foreach (string value in linkHash[3].Values)
						{
							num3++;
							text = text + value + ";\n";
						}
						TextEditor textEditor = new TextEditor();
						textEditor.content = new GUIContent(text);
						textEditor.SelectAll();
						textEditor.Copy();
					}
					else if (GUI.Button(new Rect(175f, 70f, 115f, 30f), "View Script"))
					{
						settingsOld[68] = 102;
					}
					if ((int)settingsOld[68] == 102)
					{
						string text = string.Empty;
						int num3 = 0;
						foreach (string value2 in linkHash[3].Values)
						{
							num3++;
							text = text + value2 + ";\n";
						}
						float num4 = (float)(Screen.width / 2) - 110.5f;
						float num5 = (float)(Screen.height / 2) - 250f;
						GUI.DrawTexture(new Rect(num4 + 2f, num5 + 2f, 217f, 496f), textureBackgroundBlue);
						GUI.Box(new Rect(num4, num5, 221f, 500f), string.Empty);
						if (GUI.Button(new Rect(num4 + 10f, num5 + 460f, 60f, 30f), "Copy"))
						{
							TextEditor textEditor2 = new TextEditor();
							textEditor2.content = new GUIContent(text);
							textEditor2.SelectAll();
							textEditor2.Copy();
						}
						else if (GUI.Button(new Rect(num4 + 151f, num5 + 460f, 60f, 30f), "Done"))
						{
							settingsOld[68] = 100;
						}
						GUI.TextArea(new Rect(num4 + 5f, num5 + 5f, 211f, 415f), text);
						GUI.Label(new Rect(num4 + 10f, num5 + 430f, 150f, 20f), "Object Count: " + Convert.ToString(num3), "Label");
					}
				}
				if ((int)settingsOld[64] != 105 && (int)settingsOld[64] != 106)
				{
					GUI.Label(new Rect(num + 13f, 445f, 125f, 20f), "Scale Multipliers:", "Label");
					GUI.Label(new Rect(num + 13f, 470f, 50f, 22f), "Length:", "Label");
					settingsOld[72] = GUI.TextField(new Rect(num + 58f, 470f, 40f, 20f), (string)settingsOld[72]);
					GUI.Label(new Rect(num + 13f, 495f, 50f, 20f), "Width:", "Label");
					settingsOld[70] = GUI.TextField(new Rect(num + 58f, 495f, 40f, 20f), (string)settingsOld[70]);
					GUI.Label(new Rect(num + 13f, 520f, 50f, 22f), "Height:", "Label");
					settingsOld[71] = GUI.TextField(new Rect(num + 58f, 520f, 40f, 20f), (string)settingsOld[71]);
					if ((int)settingsOld[64] <= 106)
					{
						GUI.Label(new Rect(num + 155f, 554f, 50f, 22f), "Tiling:", "Label");
						settingsOld[79] = GUI.TextField(new Rect(num + 200f, 554f, 40f, 20f), (string)settingsOld[79]);
						settingsOld[80] = GUI.TextField(new Rect(num + 245f, 554f, 40f, 20f), (string)settingsOld[80]);
						GUI.Label(new Rect(num + 219f, 570f, 10f, 22f), "x:", "Label");
						GUI.Label(new Rect(num + 264f, 570f, 10f, 22f), "y:", "Label");
						GUI.Label(new Rect(num + 155f, 445f, 50f, 20f), "Color:", "Label");
						GUI.Label(new Rect(num + 155f, 470f, 10f, 20f), "R:", "Label");
						GUI.Label(new Rect(num + 155f, 495f, 10f, 20f), "G:", "Label");
						GUI.Label(new Rect(num + 155f, 520f, 10f, 20f), "B:", "Label");
						settingsOld[73] = GUI.HorizontalSlider(new Rect(num + 170f, 475f, 100f, 20f), (float)settingsOld[73], 0f, 1f);
						settingsOld[74] = GUI.HorizontalSlider(new Rect(num + 170f, 500f, 100f, 20f), (float)settingsOld[74], 0f, 1f);
						settingsOld[75] = GUI.HorizontalSlider(new Rect(num + 170f, 525f, 100f, 20f), (float)settingsOld[75], 0f, 1f);
						GUI.Label(new Rect(num + 13f, 554f, 57f, 22f), "Material:", "Label");
						if (GUI.Button(new Rect(num + 66f, 554f, 60f, 20f), (string)settingsOld[69]))
						{
							settingsOld[78] = 1;
						}
						if ((int)settingsOld[78] == 1)
						{
							string[] item = new string[4] { "bark", "bark2", "bark3", "bark4" };
							string[] item2 = new string[4] { "wood1", "wood2", "wood3", "wood4" };
							string[] item3 = new string[4] { "grass", "grass2", "grass3", "grass4" };
							string[] item4 = new string[4] { "brick1", "brick2", "brick3", "brick4" };
							string[] item5 = new string[4] { "metal1", "metal2", "metal3", "metal4" };
							string[] item6 = new string[3] { "rock1", "rock2", "rock3" };
							string[] item7 = new string[10] { "stone1", "stone2", "stone3", "stone4", "stone5", "stone6", "stone7", "stone8", "stone9", "stone10" };
							string[] item8 = new string[7] { "earth1", "earth2", "ice1", "lava1", "crystal1", "crystal2", "empty" };
							_ = new string[0];
							List<string[]> list = new List<string[]> { item, item2, item3, item4, item5, item6, item7, item8 };
							string[] array6 = new string[9] { "bark", "wood", "grass", "brick", "metal", "rock", "stone", "misc", "transparent" };
							int num6 = 78;
							int num7 = 69;
							float num4 = (float)(Screen.width / 2) - 110.5f;
							float num5 = (float)(Screen.height / 2) - 220f;
							int num8 = (int)settingsOld[185];
							float val = 10f + 104f * (float)(list[num8].Length / 3 + 1);
							val = Math.Max(val, 280f);
							GUI.DrawTexture(new Rect(num4 + 2f, num5 + 2f, 208f, 446f), textureBackgroundBlue);
							GUI.Box(new Rect(num4, num5, 212f, 450f), string.Empty);
							for (int j = 0; j < list.Count; j++)
							{
								int num9 = j / 3;
								int num10 = j % 3;
								if (GUI.Button(new Rect(num4 + 5f + 69f * (float)num10, num5 + 5f + (float)(30 * num9), 64f, 25f), array6[j], "box"))
								{
									settingsOld[185] = j;
								}
							}
							scroll2 = GUI.BeginScrollView(new Rect(num4, num5 + 110f, 225f, 290f), scroll2, new Rect(num4, num5 + 110f, 212f, val), alwaysShowHorizontal: true, alwaysShowVertical: true);
							if (num8 != 8)
							{
								for (int j = 0; j < list[num8].Length; j++)
								{
									int num9 = j / 3;
									int num10 = j % 3;
									GUI.DrawTexture(new Rect(num4 + 5f + 69f * (float)num10, num5 + 115f + 104f * (float)num9, 64f, 64f), RCLoadTexture("p" + list[num8][j]));
									if (GUI.Button(new Rect(num4 + 5f + 69f * (float)num10, num5 + 184f + 104f * (float)num9, 64f, 30f), list[num8][j]))
									{
										settingsOld[num7] = list[num8][j];
										settingsOld[num6] = 0;
									}
								}
							}
							GUI.EndScrollView();
							if (GUI.Button(new Rect(num4 + 24f, num5 + 410f, 70f, 30f), "Default"))
							{
								settingsOld[num7] = "default";
								settingsOld[num6] = 0;
							}
							else if (GUI.Button(new Rect(num4 + 118f, num5 + 410f, 70f, 30f), "Done"))
							{
								settingsOld[num6] = 0;
							}
						}
						bool flag3 = false;
						if ((int)settingsOld[76] == 1)
						{
							flag3 = true;
							Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
							texture2D.SetPixel(0, 0, new Color((float)settingsOld[73], (float)settingsOld[74], (float)settingsOld[75], 1f));
							texture2D.Apply();
							GUI.DrawTexture(new Rect(num + 235f, 445f, 30f, 20f), texture2D, ScaleMode.StretchToFill);
							UnityEngine.Object.Destroy(texture2D);
						}
						bool flag4 = GUI.Toggle(new Rect(num + 193f, 445f, 40f, 20f), flag3, "On");
						if (flag3 != flag4)
						{
							if (flag4)
							{
								settingsOld[76] = 1;
							}
							else
							{
								settingsOld[76] = 0;
							}
						}
					}
				}
				if (GUI.Button(new Rect(num + 5f, 10f, 60f, 25f), "General", "box"))
				{
					settingsOld[64] = 101;
				}
				else if (GUI.Button(new Rect(num + 70f, 10f, 70f, 25f), "Geometry", "box"))
				{
					settingsOld[64] = 102;
				}
				else if (GUI.Button(new Rect(num + 145f, 10f, 65f, 25f), "Buildings", "box"))
				{
					settingsOld[64] = 103;
				}
				else if (GUI.Button(new Rect(num + 215f, 10f, 50f, 25f), "Nature", "box"))
				{
					settingsOld[64] = 104;
				}
				else if (GUI.Button(new Rect(num + 5f, 45f, 70f, 25f), "Spawners", "box"))
				{
					settingsOld[64] = 105;
				}
				else if (GUI.Button(new Rect(num + 80f, 45f, 70f, 25f), "Racing", "box"))
				{
					settingsOld[64] = 108;
				}
				else if (GUI.Button(new Rect(num + 155f, 45f, 40f, 25f), "Misc", "box"))
				{
					settingsOld[64] = 107;
				}
				else if (GUI.Button(new Rect(num + 200f, 45f, 70f, 25f), "Credits", "box"))
				{
					settingsOld[64] = 106;
				}
				float result;
				if ((int)settingsOld[64] == 101)
				{
					scroll = GUI.BeginScrollView(new Rect(num, 80f, 305f, 350f), scroll, new Rect(num, 80f, 300f, 470f), alwaysShowHorizontal: true, alwaysShowVertical: true);
					GUI.Label(new Rect(num + 100f, 80f, 120f, 20f), "General Objects:", "Label");
					GUI.Label(new Rect(num + 108f, 245f, 120f, 20f), "Spawn Points:", "Label");
					GUI.Label(new Rect(num + 7f, 415f, 290f, 60f), "* The above titan spawn points apply only to randomly spawned titans specified by the Random Titan #.", "Label");
					GUI.Label(new Rect(num + 7f, 470f, 290f, 60f), "* If team mode is disabled both cyan and magenta spawn points will be randomly chosen for players.", "Label");
					GUI.DrawTexture(new Rect(num + 27f, 110f, 64f, 64f), RCLoadTexture("psupply"));
					GUI.DrawTexture(new Rect(num + 118f, 110f, 64f, 64f), RCLoadTexture("pcannonwall"));
					GUI.DrawTexture(new Rect(num + 209f, 110f, 64f, 64f), RCLoadTexture("pcannonground"));
					GUI.DrawTexture(new Rect(num + 27f, 275f, 64f, 64f), RCLoadTexture("pspawnt"));
					GUI.DrawTexture(new Rect(num + 118f, 275f, 64f, 64f), RCLoadTexture("pspawnplayerC"));
					GUI.DrawTexture(new Rect(num + 209f, 275f, 64f, 64f), RCLoadTexture("pspawnplayerM"));
					if (GUI.Button(new Rect(num + 27f, 179f, 64f, 60f), "Supply"))
					{
						flag = true;
						GameObject original = (GameObject)Resources.Load("aot_supply");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						selectedObj.name = "base,aot_supply";
					}
					else if (GUI.Button(new Rect(num + 118f, 179f, 64f, 60f), "Cannon \nWall"))
					{
						flag = true;
						GameObject original = (GameObject)RCassets.Load("CannonWallProp");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						selectedObj.name = "photon,CannonWall";
					}
					else if (GUI.Button(new Rect(num + 209f, 179f, 64f, 60f), "Cannon\n Ground"))
					{
						flag = true;
						GameObject original = (GameObject)RCassets.Load("CannonGroundProp");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						selectedObj.name = "photon,CannonGround";
					}
					else if (GUI.Button(new Rect(num + 27f, 344f, 64f, 60f), "Titan"))
					{
						flag = true;
						flag2 = true;
						GameObject original = (GameObject)RCassets.Load("titan");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						selectedObj.name = "spawnpoint,titan";
					}
					else if (GUI.Button(new Rect(num + 118f, 344f, 64f, 60f), "Player \nCyan"))
					{
						flag = true;
						flag2 = true;
						GameObject original = (GameObject)RCassets.Load("playerC");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						selectedObj.name = "spawnpoint,playerC";
					}
					else if (GUI.Button(new Rect(num + 209f, 344f, 64f, 60f), "Player \nMagenta"))
					{
						flag = true;
						flag2 = true;
						GameObject original = (GameObject)RCassets.Load("playerM");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						selectedObj.name = "spawnpoint,playerM";
					}
					GUI.EndScrollView();
				}
				else if ((int)settingsOld[64] == 107)
				{
					GUI.DrawTexture(new Rect(num + 30f, 90f, 64f, 64f), RCLoadTexture("pbarrier"));
					GUI.DrawTexture(new Rect(num + 30f, 199f, 64f, 64f), RCLoadTexture("pregion"));
					GUI.Label(new Rect(num + 110f, 243f, 200f, 22f), "Region Name:", "Label");
					GUI.Label(new Rect(num + 110f, 179f, 200f, 22f), "Disable Map Bounds:", "Label");
					bool flag5 = false;
					if ((int)settingsOld[186] == 1)
					{
						flag5 = true;
						if (!linkHash[3].ContainsKey("mapbounds"))
						{
							linkHash[3].Add("mapbounds", "map,disablebounds");
						}
					}
					else if (linkHash[3].ContainsKey("mapbounds"))
					{
						linkHash[3].Remove("mapbounds");
					}
					if (GUI.Button(new Rect(num + 30f, 159f, 64f, 30f), "Barrier"))
					{
						flag = true;
						flag2 = true;
						GameObject original2 = (GameObject)RCassets.Load("barrierEditor");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original2);
						selectedObj.name = "misc,barrier";
					}
					else if (GUI.Button(new Rect(num + 30f, 268f, 64f, 30f), "Region"))
					{
						if ((string)settingsOld[191] == string.Empty)
						{
							settingsOld[191] = "Region" + UnityEngine.Random.Range(10000, 99999);
						}
						flag = true;
						flag2 = true;
						GameObject original2 = (GameObject)RCassets.Load("regionEditor");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original2);
						GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
						gameObject3.name = "RegionLabel";
						if (!float.TryParse((string)settingsOld[71], out result))
						{
							settingsOld[71] = "1";
						}
						if (!float.TryParse((string)settingsOld[70], out result))
						{
							settingsOld[70] = "1";
						}
						if (!float.TryParse((string)settingsOld[72], out result))
						{
							settingsOld[72] = "1";
						}
						gameObject3.transform.parent = selectedObj.transform;
						float y = 1f;
						if (Convert.ToSingle((string)settingsOld[71]) > 100f)
						{
							y = 0.8f;
						}
						else if (Convert.ToSingle((string)settingsOld[71]) > 1000f)
						{
							y = 0.5f;
						}
						gameObject3.transform.localPosition = new Vector3(0f, y, 0f);
						gameObject3.transform.localScale = new Vector3(5f / Convert.ToSingle((string)settingsOld[70]), 5f / Convert.ToSingle((string)settingsOld[71]), 5f / Convert.ToSingle((string)settingsOld[72]));
						gameObject3.GetComponent<UILabel>().text = (string)settingsOld[191];
						selectedObj.AddComponent<RCRegionLabel>();
						selectedObj.GetComponent<RCRegionLabel>().myLabel = gameObject3;
						selectedObj.name = "misc,region," + (string)settingsOld[191];
					}
					settingsOld[191] = GUI.TextField(new Rect(num + 200f, 243f, 75f, 20f), (string)settingsOld[191]);
					bool flag6 = GUI.Toggle(new Rect(num + 240f, 179f, 40f, 20f), flag5, "On");
					if (flag6 != flag5)
					{
						if (flag6)
						{
							settingsOld[186] = 1;
						}
						else
						{
							settingsOld[186] = 0;
						}
					}
				}
				else if ((int)settingsOld[64] == 105)
				{
					GUI.Label(new Rect(num + 95f, 85f, 130f, 20f), "Custom Spawners:", "Label");
					GUI.DrawTexture(new Rect(num + 7.8f, 110f, 64f, 64f), RCLoadTexture("ptitan"));
					GUI.DrawTexture(new Rect(num + 79.6f, 110f, 64f, 64f), RCLoadTexture("pabnormal"));
					GUI.DrawTexture(new Rect(num + 151.4f, 110f, 64f, 64f), RCLoadTexture("pjumper"));
					GUI.DrawTexture(new Rect(num + 223.2f, 110f, 64f, 64f), RCLoadTexture("pcrawler"));
					GUI.DrawTexture(new Rect(num + 7.8f, 224f, 64f, 64f), RCLoadTexture("ppunk"));
					GUI.DrawTexture(new Rect(num + 79.6f, 224f, 64f, 64f), RCLoadTexture("pannie"));
					float result2;
					if (GUI.Button(new Rect(num + 7.8f, 179f, 64f, 30f), "Titan"))
					{
						if (!float.TryParse((string)settingsOld[83], out result2))
						{
							settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)RCassets.Load("spawnTitan");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)settingsOld[84];
						selectedObj.name = "photon,spawnTitan," + (string)settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 79.6f, 179f, 64f, 30f), "Aberrant"))
					{
						if (!float.TryParse((string)settingsOld[83], out result2))
						{
							settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)RCassets.Load("spawnAbnormal");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)settingsOld[84];
						selectedObj.name = "photon,spawnAbnormal," + (string)settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 151.4f, 179f, 64f, 30f), "Jumper"))
					{
						if (!float.TryParse((string)settingsOld[83], out result2))
						{
							settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)RCassets.Load("spawnJumper");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)settingsOld[84];
						selectedObj.name = "photon,spawnJumper," + (string)settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 223.2f, 179f, 64f, 30f), "Crawler"))
					{
						if (!float.TryParse((string)settingsOld[83], out result2))
						{
							settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)RCassets.Load("spawnCrawler");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)settingsOld[84];
						selectedObj.name = "photon,spawnCrawler," + (string)settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 7.8f, 293f, 64f, 30f), "Punk"))
					{
						if (!float.TryParse((string)settingsOld[83], out result2))
						{
							settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)RCassets.Load("spawnPunk");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)settingsOld[84];
						selectedObj.name = "photon,spawnPunk," + (string)settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 79.6f, 293f, 64f, 30f), "Annie"))
					{
						if (!float.TryParse((string)settingsOld[83], out result2))
						{
							settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)RCassets.Load("spawnAnnie");
						selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)settingsOld[84];
						selectedObj.name = "photon,spawnAnnie," + (string)settingsOld[83] + "," + num11;
					}
					GUI.Label(new Rect(num + 7f, 379f, 140f, 22f), "Spawn Timer:", "Label");
					settingsOld[83] = GUI.TextField(new Rect(num + 100f, 379f, 50f, 20f), (string)settingsOld[83]);
					GUI.Label(new Rect(num + 7f, 356f, 140f, 22f), "Endless spawn:", "Label");
					GUI.Label(new Rect(num + 7f, 405f, 290f, 80f), "* The above settingsOld apply only to the next placed spawner. You can have unique spawn times and settingsOld for each individual titan spawner.", "Label");
					bool flag7 = false;
					if ((int)settingsOld[84] == 1)
					{
						flag7 = true;
					}
					bool flag8 = GUI.Toggle(new Rect(num + 100f, 356f, 40f, 20f), flag7, "On");
					if (flag7 != flag8)
					{
						if (flag8)
						{
							settingsOld[84] = 1;
						}
						else
						{
							settingsOld[84] = 0;
						}
					}
				}
				else if ((int)settingsOld[64] == 102)
				{
					string[] array7 = new string[12]
					{
						"cuboid", "plane", "sphere", "cylinder", "capsule", "pyramid", "cone", "prism", "arc90", "arc180",
						"torus", "tube"
					};
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, 90f + 114f * (float)num9, 64f, 64f), RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, 159f + 114f * (float)num9, 64f, 30f), array7[j]))
						{
							flag = true;
							GameObject original2 = (GameObject)RCassets.Load(array7[j]);
							selectedObj = (GameObject)UnityEngine.Object.Instantiate(original2);
							selectedObj.name = "custom," + array7[j];
						}
					}
				}
				else if ((int)settingsOld[64] == 103)
				{
					List<string> list2 = new List<string> { "arch1", "house1" };
					string[] array7 = new string[44]
					{
						"tower1", "tower2", "tower3", "tower4", "tower5", "house1", "house2", "house3", "house4", "house5",
						"house6", "house7", "house8", "house9", "house10", "house11", "house12", "house13", "house14", "pillar1",
						"pillar2", "village1", "village2", "windmill1", "arch1", "canal1", "castle1", "church1", "cannon1", "statue1",
						"statue2", "wagon1", "elevator1", "bridge1", "dummy1", "spike1", "wall1", "wall2", "wall3", "wall4",
						"arena1", "arena2", "arena3", "arena4"
					};
					float val = 110f + 114f * (float)((array7.Length - 1) / 4);
					scroll = GUI.BeginScrollView(new Rect(num, 90f, 303f, 350f), scroll, new Rect(num, 90f, 300f, val), alwaysShowHorizontal: true, alwaysShowVertical: true);
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, 90f + 114f * (float)num9, 64f, 64f), RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, 159f + 114f * (float)num9, 64f, 30f), array7[j]))
						{
							flag = true;
							GameObject original4 = (GameObject)RCassets.Load(array7[j]);
							selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							if (list2.Contains(array7[j]))
							{
								selectedObj.name = "customb," + array7[j];
							}
							else
							{
								selectedObj.name = "custom," + array7[j];
							}
						}
					}
					GUI.EndScrollView();
				}
				else if ((int)settingsOld[64] == 104)
				{
					List<string> list2 = new List<string> { "tree0" };
					string[] array7 = new string[23]
					{
						"leaf0", "leaf1", "leaf2", "field1", "field2", "tree0", "tree1", "tree2", "tree3", "tree4",
						"tree5", "tree6", "tree7", "log1", "log2", "trunk1", "boulder1", "boulder2", "boulder3", "boulder4",
						"boulder5", "cave1", "cave2"
					};
					float val = 110f + 114f * (float)((array7.Length - 1) / 4);
					scroll = GUI.BeginScrollView(new Rect(num, 90f, 303f, 350f), scroll, new Rect(num, 90f, 300f, val), alwaysShowHorizontal: true, alwaysShowVertical: true);
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, 90f + 114f * (float)num9, 64f, 64f), RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, 159f + 114f * (float)num9, 64f, 30f), array7[j]))
						{
							flag = true;
							GameObject original4 = (GameObject)RCassets.Load(array7[j]);
							selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							if (list2.Contains(array7[j]))
							{
								selectedObj.name = "customb," + array7[j];
							}
							else
							{
								selectedObj.name = "custom," + array7[j];
							}
						}
					}
					GUI.EndScrollView();
				}
				else if ((int)settingsOld[64] == 108)
				{
					string[] array8 = new string[12]
					{
						"Cuboid", "Plane", "Sphere", "Cylinder", "Capsule", "Pyramid", "Cone", "Prism", "Arc90", "Arc180",
						"Torus", "Tube"
					};
					string[] array7 = new string[12];
					for (int j = 0; j < array7.Length; j++)
					{
						array7[j] = "start" + array8[j];
					}
					float val = 110f + 114f * (float)((array7.Length - 1) / 4);
					val *= 4f;
					val += 200f;
					scroll = GUI.BeginScrollView(new Rect(num, 90f, 303f, 350f), scroll, new Rect(num, 90f, 300f, val), alwaysShowHorizontal: true, alwaysShowVertical: true);
					GUI.Label(new Rect(num + 90f, 90f, 200f, 22f), "Racing Start Barrier");
					int num12 = 125;
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 114f * (float)num9, 64f, 64f), RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 69f + 114f * (float)num9, 64f, 30f), array8[j]))
						{
							flag = true;
							flag2 = true;
							GameObject original4 = (GameObject)RCassets.Load(array7[j]);
							selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							selectedObj.name = "racing," + array7[j];
						}
					}
					num12 += 114 * (array7.Length / 4) + 10;
					GUI.Label(new Rect(num + 93f, num12, 200f, 22f), "Racing End Trigger");
					num12 += 35;
					for (int j = 0; j < array7.Length; j++)
					{
						array7[j] = "end" + array8[j];
					}
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 114f * (float)num9, 64f, 64f), RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 69f + 114f * (float)num9, 64f, 30f), array8[j]))
						{
							flag = true;
							flag2 = true;
							GameObject original4 = (GameObject)RCassets.Load(array7[j]);
							selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							selectedObj.name = "racing," + array7[j];
						}
					}
					num12 += 114 * (array7.Length / 4) + 10;
					GUI.Label(new Rect(num + 113f, num12, 200f, 22f), "Kill Trigger");
					num12 += 35;
					for (int j = 0; j < array7.Length; j++)
					{
						array7[j] = "kill" + array8[j];
					}
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 114f * (float)num9, 64f, 64f), RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 69f + 114f * (float)num9, 64f, 30f), array8[j]))
						{
							flag = true;
							flag2 = true;
							GameObject original4 = (GameObject)RCassets.Load(array7[j]);
							selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							selectedObj.name = "racing," + array7[j];
						}
					}
					num12 += 114 * (array7.Length / 4) + 10;
					GUI.Label(new Rect(num + 95f, num12, 200f, 22f), "Checkpoint Trigger");
					num12 += 35;
					for (int j = 0; j < array7.Length; j++)
					{
						array7[j] = "checkpoint" + array8[j];
					}
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 114f * (float)num9, 64f, 64f), RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 69f + 114f * (float)num9, 64f, 30f), array8[j]))
						{
							flag = true;
							flag2 = true;
							GameObject original4 = (GameObject)RCassets.Load(array7[j]);
							selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							selectedObj.name = "racing," + array7[j];
						}
					}
					GUI.EndScrollView();
				}
				else if ((int)settingsOld[64] == 106)
				{
					GUI.Label(new Rect(num + 10f, 80f, 200f, 22f), "- Tree 2 designed by Ken P.", "Label");
					GUI.Label(new Rect(num + 10f, 105f, 250f, 22f), "- Tower 2, House 5 designed by Matthew Santos", "Label");
					GUI.Label(new Rect(num + 10f, 130f, 200f, 22f), "- Cannon retextured by Mika", "Label");
					GUI.Label(new Rect(num + 10f, 155f, 200f, 22f), "- Arena 1,2,3 & 4 created by Gun", "Label");
					GUI.Label(new Rect(num + 10f, 180f, 250f, 22f), "- Cannon Wall/Ground textured by Bellfox", "Label");
					GUI.Label(new Rect(num + 10f, 205f, 250f, 120f), "- House 7 - 14, Statue1, Statue2, Wagon1, Wall 1, Wall 2, Wall 3, Wall 4, CannonWall, CannonGround, Tower5, Bridge1, Dummy1, Spike1 created by meecube", "Label");
				}
				if (!flag || !(selectedObj != null))
				{
					return;
				}
				if (!float.TryParse((string)settingsOld[70], out result))
				{
					settingsOld[70] = "1";
				}
				if (!float.TryParse((string)settingsOld[71], out result))
				{
					settingsOld[71] = "1";
				}
				if (!float.TryParse((string)settingsOld[72], out result))
				{
					settingsOld[72] = "1";
				}
				if (!float.TryParse((string)settingsOld[79], out result))
				{
					settingsOld[79] = "1";
				}
				if (!float.TryParse((string)settingsOld[80], out result))
				{
					settingsOld[80] = "1";
				}
				if (!flag2)
				{
					float a = 1f;
					if ((string)settingsOld[69] != "default")
					{
						if (((string)settingsOld[69]).StartsWith("transparent"))
						{
							if (float.TryParse(((string)settingsOld[69]).Substring(11), out var result3))
							{
								a = result3;
							}
							Renderer[] componentsInChildren = selectedObj.GetComponentsInChildren<Renderer>();
							foreach (Renderer renderer2 in componentsInChildren)
							{
								renderer2.material = (Material)RCassets.Load("transparent");
								renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle((string)settingsOld[79]), renderer2.material.mainTextureScale.y * Convert.ToSingle((string)settingsOld[80]));
							}
						}
						else
						{
							Renderer[] componentsInChildren = selectedObj.GetComponentsInChildren<Renderer>();
							foreach (Renderer renderer3 in componentsInChildren)
							{
								if (!renderer3.name.Contains("Particle System") || !selectedObj.name.Contains("aot_supply"))
								{
									renderer3.material = (Material)RCassets.Load((string)settingsOld[69]);
									renderer3.material.mainTextureScale = new Vector2(renderer3.material.mainTextureScale.x * Convert.ToSingle((string)settingsOld[79]), renderer3.material.mainTextureScale.y * Convert.ToSingle((string)settingsOld[80]));
								}
							}
						}
					}
					float num13 = 1f;
					MeshFilter[] componentsInChildren2 = selectedObj.GetComponentsInChildren<MeshFilter>();
					foreach (MeshFilter meshFilter in componentsInChildren2)
					{
						if (selectedObj.name.StartsWith("customb"))
						{
							if (num13 < meshFilter.mesh.bounds.size.y)
							{
								num13 = meshFilter.mesh.bounds.size.y;
							}
						}
						else if (num13 < meshFilter.mesh.bounds.size.z)
						{
							num13 = meshFilter.mesh.bounds.size.z;
						}
					}
					float num14 = selectedObj.transform.localScale.x * Convert.ToSingle((string)settingsOld[70]);
					num14 -= 0.001f;
					float y3 = selectedObj.transform.localScale.y * Convert.ToSingle((string)settingsOld[71]);
					float z2 = selectedObj.transform.localScale.z * Convert.ToSingle((string)settingsOld[72]);
					selectedObj.transform.localScale = new Vector3(num14, y3, z2);
					if ((int)settingsOld[76] == 1)
					{
						Color color = new Color((float)settingsOld[73], (float)settingsOld[74], (float)settingsOld[75], a);
						componentsInChildren2 = selectedObj.GetComponentsInChildren<MeshFilter>();
						for (int i = 0; i < componentsInChildren2.Length; i++)
						{
							Mesh mesh = componentsInChildren2[i].mesh;
							Color[] array5 = new Color[mesh.vertexCount];
							for (int k = 0; k < mesh.vertexCount; k++)
							{
								array5[k] = color;
							}
							mesh.colors = array5;
						}
					}
					float num15 = selectedObj.transform.localScale.z;
					if (selectedObj.name.Contains("boulder2") || selectedObj.name.Contains("boulder3") || selectedObj.name.Contains("field2"))
					{
						num15 *= 0.01f;
					}
					float num16 = 10f + num15 * num13 * 1.2f / 2f;
					selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * num16, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * num16);
					selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
					string text4 = selectedObj.name;
					string[] array9 = new string[21]
					{
						text4,
						",",
						(string)settingsOld[69],
						",",
						(string)settingsOld[70],
						",",
						(string)settingsOld[71],
						",",
						(string)settingsOld[72],
						",",
						settingsOld[76].ToString(),
						",",
						((float)settingsOld[73]).ToString(),
						",",
						((float)settingsOld[74]).ToString(),
						",",
						((float)settingsOld[75]).ToString(),
						",",
						(string)settingsOld[79],
						",",
						(string)settingsOld[80]
					};
					selectedObj.name = string.Concat(array9);
					unloadAssetsEditor();
				}
				else if (selectedObj.name.StartsWith("misc"))
				{
					if (selectedObj.name.Contains("barrier") || selectedObj.name.Contains("region") || selectedObj.name.Contains("racing"))
					{
						float num13 = 1f;
						float num14 = selectedObj.transform.localScale.x * Convert.ToSingle((string)settingsOld[70]);
						num14 -= 0.001f;
						float y3 = selectedObj.transform.localScale.y * Convert.ToSingle((string)settingsOld[71]);
						float z2 = selectedObj.transform.localScale.z * Convert.ToSingle((string)settingsOld[72]);
						selectedObj.transform.localScale = new Vector3(num14, y3, z2);
						float num15 = selectedObj.transform.localScale.z;
						float num16 = 10f + num15 * num13 * 1.2f / 2f;
						selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * num16, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * num16);
						if (!selectedObj.name.Contains("region"))
						{
							selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
						}
						string text4 = selectedObj.name;
						selectedObj.name = text4 + "," + (string)settingsOld[70] + "," + (string)settingsOld[71] + "," + (string)settingsOld[72];
					}
				}
				else if (selectedObj.name.StartsWith("racing"))
				{
					float num13 = 1f;
					float num14 = selectedObj.transform.localScale.x * Convert.ToSingle((string)settingsOld[70]);
					num14 -= 0.001f;
					float y3 = selectedObj.transform.localScale.y * Convert.ToSingle((string)settingsOld[71]);
					float z2 = selectedObj.transform.localScale.z * Convert.ToSingle((string)settingsOld[72]);
					selectedObj.transform.localScale = new Vector3(num14, y3, z2);
					float num15 = selectedObj.transform.localScale.z;
					float num16 = 10f + num15 * num13 * 1.2f / 2f;
					selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * num16, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * num16);
					selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
					string text4 = selectedObj.name;
					selectedObj.name = text4 + "," + (string)settingsOld[70] + "," + (string)settingsOld[71] + "," + (string)settingsOld[72];
				}
				else
				{
					selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * 10f, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * 10f);
					selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
				}
				Screen.lockCursor = true;
				GUI.FocusControl(null);
			}
			else
			{
				if (GameMenu.Paused || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
				{
					return;
				}
				if (Time.timeScale <= 0.1f)
				{
					float num17 = (float)Screen.width / 2f;
					float num18 = (float)Screen.height / 2f;
					GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
					GUI.DrawTexture(new Rect(num17 - 98f, num18 - 48f, 196f, 96f), textureBackgroundBlue);
					GUI.Box(new Rect(num17 - 100f, num18 - 50f, 200f, 100f), string.Empty);
					if (pauseWaitTime <= 3f)
					{
						GUI.Label(new Rect(num17 - 43f, num18 - 15f, 200f, 22f), "Unpausing in:");
						GUI.Label(new Rect(num17 - 8f, num18 + 5f, 200f, 22f), pauseWaitTime.ToString("F1"));
					}
					else
					{
						GUI.Label(new Rect(num17 - 43f, num18 - 10f, 200f, 22f), "Game Paused.");
					}
				}
				else if (!logicLoaded || !customLevelLoaded)
				{
					float num17 = (float)Screen.width / 2f;
					float num18 = (float)Screen.height / 2f;
					GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
					GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), textureBackgroundBlack);
					GUI.DrawTexture(new Rect(num17 - 98f, num18 - 48f, 196f, 146f), textureBackgroundBlue);
					GUI.Box(new Rect(num17 - 100f, num18 - 50f, 200f, 150f), string.Empty);
					int length = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.currentLevel]).Length;
					int length2 = RCextensions.returnStringFromObject(PhotonNetwork.masterClient.customProperties[PhotonPlayerProperty.currentLevel]).Length;
					GUI.Label(new Rect(num17 - 60f, num18 - 30f, 200f, 22f), "Loading Level (" + length + "/" + length2 + ")");
					retryTime += Time.deltaTime;
					if (GUI.Button(new Rect(num17 - 20f, num18 + 50f, 40f, 30f), "Quit"))
					{
						PhotonNetwork.Disconnect();
						IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
						GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
						DestroyAllExistingCloths();
						UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
						Application.LoadLevel("menu");
					}
				}
			}
		}
	}

	public void OnJoinedRoom()
	{
		maxPlayers = PhotonNetwork.room.maxPlayers;
		playerList = string.Empty;
		char[] separator = new char[1] { "`"[0] };
		UnityEngine.MonoBehaviour.print("OnJoinedRoom " + PhotonNetwork.room.name + "    >>>>   " + LevelInfo.getInfo(PhotonNetwork.room.name.Split(separator)[1]).mapName);
		gameTimesUp = false;
		char[] separator2 = new char[1] { "`"[0] };
		string[] array = PhotonNetwork.room.name.Split(separator2);
		level = array[1];
		if (array[2] == "normal")
		{
			difficulty = 0;
		}
		else if (array[2] == "hard")
		{
			difficulty = 1;
		}
		else if (array[2] == "abnormal")
		{
			difficulty = 2;
		}
		IN_GAME_MAIN_CAMERA.difficulty = difficulty;
		time = int.Parse(array[3]);
		time *= 60;
		IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(level).type;
		PhotonNetwork.LoadLevel(LevelInfo.getInfo(level).mapName);
		name = SettingsManager.ProfileSettings.Name.Value;
		LoginFengKAI.player.name = name;
		LoginFengKAI.player.guildname = SettingsManager.ProfileSettings.Guild.Value;
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable
		{
			{
				PhotonPlayerProperty.name,
				LoginFengKAI.player.name
			},
			{
				PhotonPlayerProperty.guildName,
				LoginFengKAI.player.guildname
			},
			{
				PhotonPlayerProperty.kills,
				0
			},
			{
				PhotonPlayerProperty.max_dmg,
				0
			},
			{
				PhotonPlayerProperty.total_dmg,
				0
			},
			{
				PhotonPlayerProperty.deaths,
				0
			},
			{
				PhotonPlayerProperty.dead,
				true
			},
			{
				PhotonPlayerProperty.isTitan,
				0
			},
			{
				PhotonPlayerProperty.RCteam,
				0
			},
			{
				PhotonPlayerProperty.currentLevel,
				string.Empty
			}
		};
		PhotonNetwork.player.SetCustomProperties(customProperties);
		humanScore = 0;
		titanScore = 0;
		PVPtitanScore = 0;
		PVPhumanScore = 0;
		wave = 1;
		highestwave = 1;
		localRacingResult = string.Empty;
		needChooseSide = true;
		chatContent = new ArrayList();
		killInfoGO = new ArrayList();
		InRoomChat.messages.Clear();
		if (!PhotonNetwork.isMasterClient)
		{
			base.photonView.RPC("RequireStatus", PhotonTargets.MasterClient);
		}
		assetCacheTextures = new Dictionary<string, Texture2D>();
		customMapMaterials = new Dictionary<string, Material>();
		isFirstLoad = true;
		if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
		{
			ServerRequestAuthentication(PrivateServerAuthPass);
		}
	}

	public void OnLeftLobby()
	{
		UnityEngine.MonoBehaviour.print("OnLeftLobby");
	}

	public void OnLeftRoom()
	{
		PhotonPlayer.CleanProperties();
		InRoomChat.messages.Clear();
		if (Application.loadedLevel != 0)
		{
			Time.timeScale = 1f;
			if (PhotonNetwork.connected)
			{
				PhotonNetwork.Disconnect();
			}
			resetSettings(isLeave: true);
			loadconfig();
			IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
			gameStart = false;
			DestroyAllExistingCloths();
			JustLeftRoom = true;
			UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
			Application.LoadLevel("menu");
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		SkyboxCustomSkinLoader.SkyboxMaterial = null;
		if (level != 0 && Application.loadedLevelName != "characterCreation" && Application.loadedLevelName != "SnapShot")
		{
			UIManager.SetMenu(MenuType.Game);
			GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
			foreach (GameObject gameObject in array)
			{
				if (!(gameObject.GetPhotonView() != null) || !gameObject.GetPhotonView().owner.isMasterClient)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			isWinning = false;
			gameStart = true;
			ShowHUDInfoCenter(string.Empty);
			GameObject obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("MainCamera_mono"), GameObject.Find("cameraDefaultPosition").transform.position, GameObject.Find("cameraDefaultPosition").transform.rotation);
			UnityEngine.Object.Destroy(GameObject.Find("cameraDefaultPosition"));
			obj.name = "MainCamera";
			ui = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI_IN_GAME"));
			ui.name = "UI_IN_GAME";
			ui.SetActive(value: true);
			NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[0], state: true);
			NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[1], state: false);
			NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[2], state: false);
			NGUITools.SetActive(ui.GetComponent<UIReferArray>().panels[3], state: false);
			LevelInfo info = LevelInfo.getInfo(FengGameManagerMKII.level);
			cache();
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
			loadskin();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				single_kills = 0;
				single_maxDamage = 0;
				single_totalDamage = 0;
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
				Camera.main.GetComponent<SpectatorMovement>().disable = true;
				Camera.main.GetComponent<MouseLook>().disable = true;
				IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(FengGameManagerMKII.level).type;
				SpawnPlayer(IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper());
				int abnormal = 90;
				if (difficulty == 1)
				{
					abnormal = 70;
				}
				spawnTitanCustom("titanRespawn", abnormal, info.enemyNumber, punk: false);
			}
			else
			{
				PVPcheckPoint.chkPts = new ArrayList();
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = false;
				Camera.main.GetComponent<CameraShake>().enabled = false;
				IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.MULTIPLAYER;
				if (info.type == GAMEMODE.TROST)
				{
					GameObject.Find("playerRespawn").SetActive(value: false);
					UnityEngine.Object.Destroy(GameObject.Find("playerRespawn"));
					GameObject.Find("rock").animation["lift"].speed = 0f;
					GameObject.Find("door_fine").SetActive(value: false);
					GameObject.Find("door_broke").SetActive(value: true);
					UnityEngine.Object.Destroy(GameObject.Find("ppl"));
				}
				else if (info.type == GAMEMODE.BOSS_FIGHT_CT)
				{
					GameObject.Find("playerRespawnTrost").SetActive(value: false);
					UnityEngine.Object.Destroy(GameObject.Find("playerRespawnTrost"));
				}
				if (needChooseSide)
				{
					string text = SettingsManager.InputSettings.Human.Flare1.ToString();
					ShowHUDInfoTopCenterADD("\n\nPRESS " + text + " TO ENTER GAME");
				}
				else if (!SettingsManager.LegacyGeneralSettings.SpecMode.Value)
				{
					if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
					{
						Screen.lockCursor = true;
					}
					else
					{
						Screen.lockCursor = false;
					}
					if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
					{
						if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
						{
							checkpoint = GameObject.Find("PVPchkPtT");
						}
						else
						{
							checkpoint = GameObject.Find("PVPchkPtH");
						}
					}
					if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
					{
						SpawnNonAITitan2(myLastHero);
					}
					else
					{
						SpawnPlayer(myLastHero, myLastRespawnTag);
					}
				}
				if (info.type == GAMEMODE.BOSS_FIGHT_CT)
				{
					UnityEngine.Object.Destroy(GameObject.Find("rock"));
				}
				if (PhotonNetwork.isMasterClient)
				{
					if (info.type == GAMEMODE.TROST)
					{
						if (!isPlayerAllDead2())
						{
							PhotonNetwork.Instantiate("TITAN_EREN_trost", new Vector3(-200f, 0f, -194f), Quaternion.Euler(0f, 180f, 0f), 0).GetComponent<TITAN_EREN>().rockLift = true;
							int rate = 90;
							if (difficulty == 1)
							{
								rate = 70;
							}
							GameObject[] array2 = GameObject.FindGameObjectsWithTag("titanRespawn");
							GameObject gameObject2 = GameObject.Find("titanRespawnTrost");
							if (gameObject2 != null)
							{
								array = array2;
								foreach (GameObject gameObject3 in array)
								{
									if (gameObject3.transform.parent.gameObject == gameObject2)
									{
										spawnTitan(rate, gameObject3.transform.position, gameObject3.transform.rotation);
									}
								}
							}
						}
					}
					else if (info.type == GAMEMODE.BOSS_FIGHT_CT)
					{
						if (!isPlayerAllDead2())
						{
							PhotonNetwork.Instantiate("COLOSSAL_TITAN", -Vector3.up * 10000f, Quaternion.Euler(0f, 180f, 0f), 0);
						}
					}
					else if (info.type == GAMEMODE.KILL_TITAN || info.type == GAMEMODE.ENDLESS_TITAN || info.type == GAMEMODE.SURVIVE_MODE)
					{
						if (info.name == "Annie" || info.name == "Annie II")
						{
							PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
						}
						else
						{
							int abnormal2 = 90;
							if (difficulty == 1)
							{
								abnormal2 = 70;
							}
							spawnTitanCustom("titanRespawn", abnormal2, info.enemyNumber, punk: false);
						}
					}
					else if (info.type != GAMEMODE.TROST && info.type == GAMEMODE.PVP_CAPTURE && LevelInfo.getInfo(FengGameManagerMKII.level).mapName == "OutSide")
					{
						GameObject[] array3 = GameObject.FindGameObjectsWithTag("titanRespawn");
						if (array3.Length == 0)
						{
							return;
						}
						for (int j = 0; j < array3.Length; j++)
						{
							spawnTitanRaw(array3[j].transform.position, array3[j].transform.rotation).GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
						}
					}
				}
				if (!info.supply)
				{
					UnityEngine.Object.Destroy(GameObject.Find("aot_supply"));
				}
				if (!PhotonNetwork.isMasterClient)
				{
					base.photonView.RPC("RequireStatus", PhotonTargets.MasterClient);
				}
				if (LevelInfo.getInfo(FengGameManagerMKII.level).lavaMode)
				{
					UnityEngine.Object.Instantiate(Resources.Load("levelBottom"), new Vector3(0f, -29.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
					GameObject.Find("aot_supply").transform.position = GameObject.Find("aot_supply_lava_position").transform.position;
					GameObject.Find("aot_supply").transform.rotation = GameObject.Find("aot_supply_lava_position").transform.rotation;
				}
				if (SettingsManager.LegacyGeneralSettings.SpecMode.Value)
				{
					EnterSpecMode(enter: true);
				}
			}
		}
		unloadAssets(immediate: true);
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (!noRestart)
		{
			if (PhotonNetwork.isMasterClient)
			{
				restartingMC = true;
				if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
				{
					restartingTitan = true;
				}
				if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
				{
					restartingBomb = true;
				}
				if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
				{
					restartingHorse = true;
				}
				if (!SettingsManager.LegacyGameSettings.KickShifters.Value)
				{
					restartingEren = true;
				}
			}
			resetSettings(isLeave: false);
			if (!LevelInfo.getInfo(level).teamTitan)
			{
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.isTitan, 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
			if (!gameTimesUp && PhotonNetwork.isMasterClient)
			{
				restartGame2(masterclientSwitched: true);
				base.photonView.RPC("setMasterRC", PhotonTargets.All);
			}
		}
		noRestart = false;
	}

	public void OnPhotonCreateRoomFailed()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonCreateRoomFailed");
	}

	public void OnPhotonCustomRoomPropertiesChanged()
	{
	}

	public void OnPhotonInstantiate()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonInstantiate");
	}

	public void OnPhotonJoinRoomFailed()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonJoinRoomFailed");
	}

	public void OnPhotonMaxCccuReached()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonMaxCccuReached");
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonView photonView = base.photonView;
			if (banHash.ContainsValue(RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name])))
			{
				kickPlayerRC(player, ban: false, "banned.");
			}
			else
			{
				int num = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statACL]);
				int num2 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statBLA]);
				int num3 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statGAS]);
				int num4 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statSPD]);
				if (num > 150 || num2 > 125 || num3 > 150 || num4 > 140)
				{
					kickPlayerRC(player, ban: true, "excessive stats.");
					return;
				}
				if (SettingsManager.LegacyGameSettings.PreserveKDR.Value)
				{
					StartCoroutine(WaitAndReloadKDR(player));
				}
				if (level.StartsWith("Custom"))
				{
					StartCoroutine(customlevelE(new List<PhotonPlayer> { player }));
				}
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
				{
					hashtable.Add("bomb", 1);
				}
				if (SettingsManager.LegacyGameSettings.BombModeCeiling.Value)
				{
					hashtable.Add("bombCeiling", 1);
				}
				else
				{
					hashtable.Add("bombCeiling", 0);
				}
				if (SettingsManager.LegacyGameSettings.BombModeInfiniteGas.Value)
				{
					hashtable.Add("bombInfiniteGas", 1);
				}
				else
				{
					hashtable.Add("bombInfiniteGas", 0);
				}
				if (SettingsManager.LegacyGameSettings.GlobalHideNames.Value)
				{
					hashtable.Add("globalHideNames", 1);
				}
				if (SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
				{
					hashtable.Add("globalDisableMinimap", 1);
				}
				if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
				{
					hashtable.Add("team", SettingsManager.LegacyGameSettings.TeamMode.Value);
				}
				if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
				{
					hashtable.Add("point", SettingsManager.LegacyGameSettings.PointModeAmount.Value);
				}
				if (!SettingsManager.LegacyGameSettings.RockThrowEnabled.Value)
				{
					hashtable.Add("rock", 1);
				}
				if (SettingsManager.LegacyGameSettings.TitanExplodeEnabled.Value)
				{
					hashtable.Add("explode", SettingsManager.LegacyGameSettings.TitanExplodeRadius.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
				{
					hashtable.Add("healthMode", SettingsManager.LegacyGameSettings.TitanHealthMode.Value);
					hashtable.Add("healthLower", SettingsManager.LegacyGameSettings.TitanHealthMin.Value);
					hashtable.Add("healthUpper", SettingsManager.LegacyGameSettings.TitanHealthMax.Value);
				}
				if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
				{
					hashtable.Add("infection", SettingsManager.LegacyGameSettings.InfectionModeAmount.Value);
				}
				if (SettingsManager.LegacyGameSettings.KickShifters.Value)
				{
					hashtable.Add("eren", 1);
				}
				if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
				{
					hashtable.Add("titanc", SettingsManager.LegacyGameSettings.TitanNumber.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value)
				{
					hashtable.Add("damage", SettingsManager.LegacyGameSettings.TitanArmor.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
				{
					hashtable.Add("sizeMode", SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value);
					hashtable.Add("sizeLower", SettingsManager.LegacyGameSettings.TitanSizeMin.Value);
					hashtable.Add("sizeUpper", SettingsManager.LegacyGameSettings.TitanSizeMax.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
				{
					hashtable.Add("spawnMode", 1);
					hashtable.Add("nRate", SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value);
					hashtable.Add("aRate", SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value);
					hashtable.Add("jRate", SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value);
					hashtable.Add("cRate", SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value);
					hashtable.Add("pRate", SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
				{
					hashtable.Add("waveModeOn", 1);
					hashtable.Add("waveModeNum", SettingsManager.LegacyGameSettings.TitanPerWaves.Value);
				}
				if (SettingsManager.LegacyGameSettings.FriendlyMode.Value)
				{
					hashtable.Add("friendly", 1);
				}
				if (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)
				{
					hashtable.Add("pvp", SettingsManager.LegacyGameSettings.BladePVP.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
				{
					hashtable.Add("maxwave", SettingsManager.LegacyGameSettings.TitanMaxWaves.Value);
				}
				if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
				{
					hashtable.Add("endless", SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value);
				}
				if (SettingsManager.LegacyGameSettings.Motd.Value != string.Empty)
				{
					hashtable.Add("motd", SettingsManager.LegacyGameSettings.Motd.Value);
				}
				if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
				{
					hashtable.Add("horse", 1);
				}
				if (!SettingsManager.LegacyGameSettings.AHSSAirReload.Value)
				{
					hashtable.Add("ahssReload", 1);
				}
				if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
				{
					hashtable.Add("punkWaves", 1);
				}
				if (SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
				{
					hashtable.Add("deadlycannons", 1);
				}
				if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
				{
					hashtable.Add("asoracing", 1);
				}
				hashtable.Add("racingStartTime", SettingsManager.LegacyGameSettings.RacingStartTime.Value);
				if (ignoreList != null && ignoreList.Count > 0)
				{
					photonView.RPC("ignorePlayerArray", player, ignoreList.ToArray());
				}
				photonView.RPC("settingRPC", player, hashtable);
				photonView.RPC("setMasterRC", player);
				if (Time.timeScale <= 0.1f && pauseWaitTime > 3f)
				{
					photonView.RPC("pauseRPC", player, true);
					object[] parameters = new object[2] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
					photonView.RPC("Chat", player, parameters);
				}
			}
		}
		RecompilePlayerList(0.1f);
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		if (!gameTimesUp)
		{
			oneTitanDown(string.Empty, onPlayerLeave: true);
			someOneIsDead(0);
		}
		if (ignoreList.Contains(player.ID))
		{
			ignoreList.Remove(player.ID);
		}
		InstantiateTracker.instance.TryRemovePlayer(player.ID);
		if (PhotonNetwork.isMasterClient)
		{
			base.photonView.RPC("verifyPlayerHasLeft", PhotonTargets.All, player.ID);
			if (SettingsManager.LegacyGameSettings.PreserveKDR.Value)
			{
				string key = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
				if (PreservedPlayerKDR.ContainsKey(key))
				{
					PreservedPlayerKDR.Remove(key);
				}
				int[] value = new int[4]
				{
					RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.kills]),
					RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.deaths]),
					RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.max_dmg]),
					RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.total_dmg])
				};
				PreservedPlayerKDR.Add(key, value);
			}
		}
		RecompilePlayerList(0.1f);
	}

	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		RecompilePlayerList(0.1f);
		if (playerAndUpdatedProps == null || playerAndUpdatedProps.Length < 2 || (PhotonPlayer)playerAndUpdatedProps[0] != PhotonNetwork.player)
		{
			return;
		}
		ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)playerAndUpdatedProps[1];
		if (hashtable.ContainsKey("name") && RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]) != name)
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
			hashtable2.Add(PhotonPlayerProperty.name, name);
			PhotonNetwork.player.SetCustomProperties(hashtable2);
		}
		if (hashtable.ContainsKey("statACL") || hashtable.ContainsKey("statBLA") || hashtable.ContainsKey("statGAS") || hashtable.ContainsKey("statSPD"))
		{
			PhotonPlayer player = PhotonNetwork.player;
			int num = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statACL]);
			int num2 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statBLA]);
			int num3 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statGAS]);
			int num4 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statSPD]);
			if (num > 150)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { 
				{
					PhotonPlayerProperty.statACL,
					100
				} };
				PhotonNetwork.player.SetCustomProperties(hashtable2);
				num = 100;
			}
			if (num2 > 125)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { 
				{
					PhotonPlayerProperty.statBLA,
					100
				} };
				PhotonNetwork.player.SetCustomProperties(hashtable2);
				num2 = 100;
			}
			if (num3 > 150)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { 
				{
					PhotonPlayerProperty.statGAS,
					100
				} };
				PhotonNetwork.player.SetCustomProperties(hashtable2);
				num3 = 100;
			}
			if (num4 > 140)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
				hashtable2.Add(PhotonPlayerProperty.statSPD, 100);
				PhotonNetwork.player.SetCustomProperties(hashtable2);
			}
		}
	}

	public void OnPhotonRandomJoinFailed()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonRandomJoinFailed");
	}

	public void OnPhotonSerializeView()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonSerializeView");
	}

	public void OnReceivedRoomListUpdate()
	{
	}

	public void OnUpdate()
	{
		if (RCEvents.ContainsKey("OnUpdate"))
		{
			if (updateTime > 0f)
			{
				updateTime -= Time.deltaTime;
				return;
			}
			((RCEvent)RCEvents["OnUpdate"]).checkEvent();
			updateTime = 1f;
		}
	}

	public void OnUpdatedFriendList()
	{
		UnityEngine.MonoBehaviour.print("OnUpdatedFriendList");
	}

	public int operantType(string str, int condition)
	{
		switch (condition)
		{
		case 0:
		case 3:
			if (!str.StartsWith("Equals"))
			{
				if (str.StartsWith("NotEquals"))
				{
					return 5;
				}
				if (!str.StartsWith("LessThan"))
				{
					if (str.StartsWith("LessThanOrEquals"))
					{
						return 1;
					}
					if (str.StartsWith("GreaterThanOrEquals"))
					{
						return 3;
					}
					if (str.StartsWith("GreaterThan"))
					{
						return 4;
					}
				}
				return 0;
			}
			return 2;
		case 1:
		case 4:
		case 5:
			if (!str.StartsWith("Equals"))
			{
				if (str.StartsWith("NotEquals"))
				{
					return 5;
				}
				return 0;
			}
			return 2;
		case 2:
			if (!str.StartsWith("Equals"))
			{
				if (str.StartsWith("NotEquals"))
				{
					return 1;
				}
				if (str.StartsWith("Contains"))
				{
					return 2;
				}
				if (str.StartsWith("NotContains"))
				{
					return 3;
				}
				if (str.StartsWith("StartsWith"))
				{
					return 4;
				}
				if (str.StartsWith("NotStartsWith"))
				{
					return 5;
				}
				if (str.StartsWith("EndsWith"))
				{
					return 6;
				}
				if (str.StartsWith("NotEndsWith"))
				{
					return 7;
				}
				return 0;
			}
			return 0;
		default:
			return 0;
		}
	}

	public RCEvent parseBlock(string[] stringArray, int eventClass, int eventType, RCCondition condition)
	{
		List<RCAction> list = new List<RCAction>();
		RCEvent rCEvent = new RCEvent(null, null, 0, 0);
		for (int i = 0; i < stringArray.Length; i++)
		{
			if (stringArray[i].StartsWith("If") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
				int num7 = conditionType(text);
				int num8 = text.IndexOf('.');
				text = text.Substring(num8 + 1);
				int sentOperand = operantType(text, num7);
				num5 = text.IndexOf('(');
				num6 = text.LastIndexOf(")");
				string[] array2 = text.Substring(num5 + 1, num6 - num5 - 1).Split(',');
				RCCondition condition2 = new RCCondition(sentOperand, num7, returnHelper(array2[0]), returnHelper(array2[1]));
				RCEvent rCEvent2 = parseBlock(array, 1, 0, condition2);
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				rCEvent = rCEvent2;
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("While") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
				int num7 = conditionType(text);
				int num8 = text.IndexOf('.');
				text = text.Substring(num8 + 1);
				int sentOperand = operantType(text, num7);
				num5 = text.IndexOf('(');
				num6 = text.LastIndexOf(")");
				string[] array2 = text.Substring(num5 + 1, num6 - num5 - 1).Split(',');
				RCCondition condition2 = new RCCondition(sentOperand, num7, returnHelper(array2[0]), returnHelper(array2[1]));
				RCEvent rCEvent2 = parseBlock(array, 3, 0, condition2);
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("ForeachTitan") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 2, num6 - num5 - 3);
				int num7 = 0;
				RCEvent rCEvent2 = parseBlock(array, 2, num7, null);
				rCEvent2.foreachVariableName = text;
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("ForeachPlayer") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 2, num6 - num5 - 3);
				int num7 = 1;
				RCEvent rCEvent2 = parseBlock(array, 2, num7, null);
				rCEvent2.foreachVariableName = text;
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("Else") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				if (stringArray[i] == "Else")
				{
					RCEvent rCEvent2 = parseBlock(array, 0, 0, null);
					RCAction item = new RCAction(0, 0, rCEvent2, null);
					rCEvent.setElse(item);
					i = num2;
				}
				else if (stringArray[i].StartsWith("Else If"))
				{
					int num5 = stringArray[i].IndexOf("(");
					int num6 = stringArray[i].LastIndexOf(")");
					string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
					int num7 = conditionType(text);
					int num8 = text.IndexOf('.');
					text = text.Substring(num8 + 1);
					int sentOperand = operantType(text, num7);
					num5 = text.IndexOf('(');
					num6 = text.LastIndexOf(")");
					string[] array2 = text.Substring(num5 + 1, num6 - num5 - 1).Split(',');
					RCCondition condition2 = new RCCondition(sentOperand, num7, returnHelper(array2[0]), returnHelper(array2[1]));
					RCEvent rCEvent2 = parseBlock(array, 1, 0, condition2);
					RCAction item = new RCAction(0, 0, rCEvent2, null);
					rCEvent.setElse(item);
					i = num2;
				}
			}
			else if (stringArray[i].StartsWith("VariableInt"))
			{
				int category = 1;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("SetRandom"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Add"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Subtract"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Multiply"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Divide"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Modulo"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 5, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Power"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 6, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableBool"))
			{
				int category = 2;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("SetToOpposite"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 11, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("SetRandom"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableString"))
			{
				int category = 3;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Concat"))
				{
					RCActionHelper[] array4 = new RCActionHelper[array3.Length];
					for (int j = 0; j < array3.Length; j++)
					{
						array4[j] = returnHelper(array3[j]);
					}
					RCAction item = new RCAction(category, 7, null, array4);
					list.Add(item);
				}
				else if (text2.StartsWith("Append"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 8, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Replace"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCAction item = new RCAction(category, 10, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("Remove"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 9, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableFloat"))
			{
				int category = 4;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("SetRandom"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Add"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Subtract"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Multiply"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Divide"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Modulo"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 5, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Power"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 6, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariablePlayer"))
			{
				int category = 5;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableTitan"))
			{
				int category = 6;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("Player"))
			{
				int category = 7;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("KillPlayer"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnPlayerAt"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnPlayer"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("MovePlayer"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetKills"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetDeaths"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 5, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetMaxDmg"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 6, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetTotalDmg"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 7, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetName"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 8, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetGuildName"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 9, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetTeam"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 10, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomInt"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 11, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomBool"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomString"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 13, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomFloat"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 14, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("Titan"))
			{
				int category = 8;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("KillTitan"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnTitanAt"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCActionHelper rCActionHelper5 = returnHelper(array3[4]);
					RCActionHelper rCActionHelper6 = returnHelper(array3[5]);
					RCActionHelper rCActionHelper7 = returnHelper(array3[6]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[7] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4, rCActionHelper5, rCActionHelper6, rCActionHelper7 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnTitan"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetHealth"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("MoveTitan"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = returnHelper(array3[3]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("Game"))
			{
				int category = 9;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("PrintMessage"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("LoseGame"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("WinGame"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("Restart"))
				{
					RCActionHelper rCActionHelper = returnHelper(array3[0]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
			}
		}
		return new RCEvent(condition, list, eventClass, eventType);
	}

	[RPC]
	public void pauseRPC(bool pause, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			if (pause)
			{
				pauseWaitTime = 100000f;
				Time.timeScale = 1E-06f;
			}
			else
			{
				pauseWaitTime = 3f;
			}
		}
	}

	public void playerKillInfoSingleUpdate(int dmg)
	{
		single_kills++;
		single_maxDamage = Mathf.Max(dmg, single_maxDamage);
		single_totalDamage += dmg;
	}

	public void playerKillInfoUpdate(PhotonPlayer player, int dmg)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(PhotonPlayerProperty.kills, (int)player.customProperties[PhotonPlayerProperty.kills] + 1);
		player.SetCustomProperties(hashtable);
		hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(PhotonPlayerProperty.max_dmg, Mathf.Max(dmg, (int)player.customProperties[PhotonPlayerProperty.max_dmg]));
		player.SetCustomProperties(hashtable);
		hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(PhotonPlayerProperty.total_dmg, (int)player.customProperties[PhotonPlayerProperty.total_dmg] + dmg);
		player.SetCustomProperties(hashtable);
	}

	public GameObject randomSpawnOneTitan(string place, int rate)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(place);
		int num = UnityEngine.Random.Range(0, array.Length);
		GameObject gameObject = array[num];
		return spawnTitan(rate, gameObject.transform.position, gameObject.transform.rotation);
	}

	public void randomSpawnTitan(string place, int rate, int num, bool punk = false)
	{
		if (num == -1)
		{
			num = 1;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag(place);
		List<GameObject> list = new List<GameObject>(array);
		if (array.Length == 0)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			if (list.Count <= 0)
			{
				list = new List<GameObject>(array);
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			GameObject gameObject = list[index];
			list.RemoveAt(index);
			spawnTitan(rate, gameObject.transform.position, gameObject.transform.rotation, punk);
		}
	}

	public Texture2D RCLoadTexture(string tex)
	{
		if (assetCacheTextures == null)
		{
			assetCacheTextures = new Dictionary<string, Texture2D>();
		}
		if (assetCacheTextures.ContainsKey(tex))
		{
			return assetCacheTextures[tex];
		}
		Texture2D texture2D = (Texture2D)RCassets.Load(tex);
		assetCacheTextures.Add(tex, texture2D);
		return texture2D;
	}

	public void RecompilePlayerList(float time)
	{
		if (!isRecompiling)
		{
			isRecompiling = true;
			StartCoroutine(WaitAndRecompilePlayerList(time));
		}
	}

	[RPC]
	private void refreshPVPStatus(int score1, int score2)
	{
		PVPhumanScore = score1;
		PVPtitanScore = score2;
	}

	[RPC]
	private void refreshPVPStatus_AHSS(int[] score1)
	{
		teamScores = score1;
	}

	private void refreshRacingResult()
	{
		localRacingResult = "Result\n";
		IComparer comparer = new IComparerRacingResult();
		racingResult.Sort(comparer);
		int num = Mathf.Min(racingResult.Count, 6);
		for (int i = 0; i < num; i++)
		{
			string text = localRacingResult;
			object[] array = new object[4]
			{
				text,
				"Rank ",
				i + 1,
				" : "
			};
			localRacingResult = string.Concat(array);
			localRacingResult += (racingResult[i] as RacingResult).name;
			localRacingResult = localRacingResult + "   " + (float)(int)((racingResult[i] as RacingResult).time * 100f) * 0.01f + "s";
			localRacingResult += "\n";
		}
		object[] parameters = new object[1] { localRacingResult };
		base.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, parameters);
	}

	private void refreshRacingResult2()
	{
		localRacingResult = "Result\n";
		IComparer comparer = new IComparerRacingResult();
		racingResult.Sort(comparer);
		int num = Mathf.Min(racingResult.Count, 10);
		for (int i = 0; i < num; i++)
		{
			string text = localRacingResult;
			object[] array = new object[4]
			{
				text,
				"Rank ",
				i + 1,
				" : "
			};
			localRacingResult = string.Concat(array);
			localRacingResult += (racingResult[i] as RacingResult).name;
			localRacingResult = localRacingResult + "   " + (float)(int)((racingResult[i] as RacingResult).time * 100f) * 0.01f + "s";
			localRacingResult += "\n";
		}
		object[] parameters = new object[1] { localRacingResult };
		base.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, parameters);
	}

	[RPC]
	private void refreshStatus(int score1, int score2, int wav, int highestWav, float time1, float time2, bool startRacin, bool endRacin, PhotonMessageInfo info)
	{
		if (info.sender == PhotonNetwork.masterClient && !PhotonNetwork.isMasterClient)
		{
			humanScore = score1;
			titanScore = score2;
			wave = wav;
			highestwave = highestWav;
			roundTime = time1;
			timeTotalServer = time2;
			startRacing = startRacin;
			endRacing = endRacin;
			if (startRacing && GameObject.Find("door") != null)
			{
				GameObject.Find("door").SetActive(value: false);
			}
		}
	}

	public IEnumerator reloadSky(bool specmode = false)
	{
		yield return new WaitForSeconds(0.5f);
		Material skyboxMaterial = SkyboxCustomSkinLoader.SkyboxMaterial;
		if (skyboxMaterial != null && Camera.main.GetComponent<Skybox>().material != skyboxMaterial)
		{
			Camera.main.GetComponent<Skybox>().material = skyboxMaterial;
		}
	}

	public void removeCT(COLOSSAL_TITAN titan)
	{
		cT.Remove(titan);
	}

	public void removeET(TITAN_EREN hero)
	{
		eT.Remove(hero);
	}

	public void removeFT(FEMALE_TITAN titan)
	{
		fT.Remove(titan);
	}

	public void removeHero(HERO hero)
	{
		heroes.Remove(hero);
	}

	public void removeHook(Bullet h)
	{
		hooks.Remove(h);
	}

	public void removeTitan(TITAN titan)
	{
		titans.Remove(titan);
	}

	[RPC]
	private void RequireStatus()
	{
		object[] parameters = new object[8] { humanScore, titanScore, wave, highestwave, roundTime, timeTotalServer, startRacing, endRacing };
		base.photonView.RPC("refreshStatus", PhotonTargets.Others, parameters);
		object[] parameters2 = new object[2] { PVPhumanScore, PVPtitanScore };
		base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters2);
		object[] parameters3 = new object[1] { teamScores };
		base.photonView.RPC("refreshPVPStatus_AHSS", PhotonTargets.Others, parameters3);
	}

	private void resetGameSettings()
	{
		SettingsManager.LegacyGameSettings.SetDefault();
	}

	private void resetSettings(bool isLeave)
	{
		name = LoginFengKAI.player.name;
		masterRC = false;
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(PhotonPlayerProperty.RCteam, 0);
		if (isLeave)
		{
			currentLevel = string.Empty;
			hashtable.Add(PhotonPlayerProperty.currentLevel, string.Empty);
			levelCache = new List<string[]>();
			titanSpawns.Clear();
			playerSpawnsC.Clear();
			playerSpawnsM.Clear();
			titanSpawners.Clear();
			intVariables.Clear();
			boolVariables.Clear();
			stringVariables.Clear();
			floatVariables.Clear();
			globalVariables.Clear();
			RCRegions.Clear();
			RCEvents.Clear();
			RCVariableNames.Clear();
			playerVariables.Clear();
			titanVariables.Clear();
			RCRegionTriggers.Clear();
			hashtable.Add(PhotonPlayerProperty.statACL, 100);
			hashtable.Add(PhotonPlayerProperty.statBLA, 100);
			hashtable.Add(PhotonPlayerProperty.statGAS, 100);
			hashtable.Add(PhotonPlayerProperty.statSPD, 100);
			restartingTitan = false;
			restartingMC = false;
			restartingHorse = false;
			restartingEren = false;
			restartingBomb = false;
		}
		PhotonNetwork.player.SetCustomProperties(hashtable);
		resetGameSettings();
		banHash = new ExitGames.Client.Photon.Hashtable();
		imatitan = new ExitGames.Client.Photon.Hashtable();
		oldScript = string.Empty;
		ignoreList = new List<int>();
		restartCount = new List<float>();
		heroHash = new ExitGames.Client.Photon.Hashtable();
	}

	private IEnumerator respawnE(float seconds)
	{
		while (true)
		{
			yield return new WaitForSeconds(seconds);
			if (isLosing || isWinning)
			{
				continue;
			}
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
				if (photonPlayer.customProperties[PhotonPlayerProperty.RCteam] == null && RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) != 2)
				{
					base.photonView.RPC("respawnHeroInNewRound", photonPlayer);
				}
			}
		}
	}

	[RPC]
	private void respawnHeroInNewRound()
	{
		if (!needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
		{
			SpawnPlayer(myLastHero, myLastRespawnTag);
			GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
			ShowHUDInfoCenter(string.Empty);
		}
	}

	public IEnumerator restartE(float time)
	{
		yield return new WaitForSeconds(time);
		restartGame2();
	}

	public void restartGame2(bool masterclientSwitched = false)
	{
		if (!gameTimesUp)
		{
			PVPtitanScore = 0;
			PVPhumanScore = 0;
			startRacing = false;
			endRacing = false;
			checkpoint = null;
			timeElapse = 0f;
			roundTime = 0f;
			isWinning = false;
			isLosing = false;
			isPlayer1Winning = false;
			isPlayer2Winning = false;
			wave = 1;
			myRespawnTime = 0f;
			kicklist = new ArrayList();
			killInfoGO = new ArrayList();
			racingResult = new ArrayList();
			ShowHUDInfoCenter(string.Empty);
			isRestarting = true;
			DestroyAllExistingCloths();
			PhotonNetwork.DestroyAll();
			ExitGames.Client.Photon.Hashtable hashtable = checkGameGUI();
			base.photonView.RPC("settingRPC", PhotonTargets.Others, hashtable);
			base.photonView.RPC("RPCLoadLevel", PhotonTargets.All);
			setGameSettings(hashtable);
			if (masterclientSwitched)
			{
				sendChatContentInfo("<color=#A8FF24>MasterClient has switched to </color>" + ((string)PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]).hexColor());
			}
		}
	}

	[RPC]
	private void restartGameByClient()
	{
	}

	public void restartGameSingle2()
	{
		startRacing = false;
		endRacing = false;
		checkpoint = null;
		single_kills = 0;
		single_maxDamage = 0;
		single_totalDamage = 0;
		timeElapse = 0f;
		roundTime = 0f;
		timeTotalServer = 0f;
		isWinning = false;
		isLosing = false;
		isPlayer1Winning = false;
		isPlayer2Winning = false;
		wave = 1;
		myRespawnTime = 0f;
		ShowHUDInfoCenter(string.Empty);
		DestroyAllExistingCloths();
		Application.LoadLevel(Application.loadedLevel);
	}

	public void restartRC()
	{
		intVariables.Clear();
		boolVariables.Clear();
		stringVariables.Clear();
		floatVariables.Clear();
		playerVariables.Clear();
		titanVariables.Clear();
		if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
		{
			endGameInfectionRC();
		}
		else
		{
			endGameRC();
		}
	}

	public RCActionHelper returnHelper(string str)
	{
		string[] array = str.Split('.');
		if (float.TryParse(str, out var _))
		{
			array = new string[1] { str };
		}
		List<RCActionHelper> list = new List<RCActionHelper>();
		int sentType = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (list.Count == 0)
			{
				string text = array[i];
				int result2;
				float result3;
				if (text.StartsWith("\"") && text.EndsWith("\""))
				{
					RCActionHelper item = new RCActionHelper(0, 0, text.Substring(1, text.Length - 2));
					list.Add(item);
					sentType = 2;
				}
				else if (int.TryParse(text, out result2))
				{
					RCActionHelper item = new RCActionHelper(0, 0, result2);
					list.Add(item);
					sentType = 0;
				}
				else if (float.TryParse(text, out result3))
				{
					RCActionHelper item = new RCActionHelper(0, 0, result3);
					list.Add(item);
					sentType = 3;
				}
				else if (text.ToLower() == "true" || text.ToLower() == "false")
				{
					RCActionHelper item = new RCActionHelper(0, 0, Convert.ToBoolean(text.ToLower()));
					list.Add(item);
					sentType = 1;
				}
				else if (text.StartsWith("Variable"))
				{
					int num = text.IndexOf('(');
					int num2 = text.LastIndexOf(')');
					if (text.StartsWith("VariableInt"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 0, returnHelper(text));
						list.Add(item);
						sentType = 0;
					}
					else if (text.StartsWith("VariableBool"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 1, returnHelper(text));
						list.Add(item);
						sentType = 1;
					}
					else if (text.StartsWith("VariableString"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 2, returnHelper(text));
						list.Add(item);
						sentType = 2;
					}
					else if (text.StartsWith("VariableFloat"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 3, returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
					else if (text.StartsWith("VariablePlayer"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 4, returnHelper(text));
						list.Add(item);
						sentType = 4;
					}
					else if (text.StartsWith("VariableTitan"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 5, returnHelper(text));
						list.Add(item);
						sentType = 5;
					}
				}
				else if (text.StartsWith("Region"))
				{
					int num = text.IndexOf('(');
					int num2 = text.LastIndexOf(')');
					if (text.StartsWith("RegionRandomX"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(4, 0, returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
					else if (text.StartsWith("RegionRandomY"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(4, 1, returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
					else if (text.StartsWith("RegionRandomZ"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(4, 2, returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
				}
			}
			else
			{
				if (list.Count <= 0)
				{
					continue;
				}
				string text = array[i];
				if (list[list.Count - 1].helperClass == 1)
				{
					switch (list[list.Count - 1].helperType)
					{
					case 4:
						if (text.StartsWith("GetTeam()"))
						{
							RCActionHelper item = new RCActionHelper(2, 1, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetType()"))
						{
							RCActionHelper item = new RCActionHelper(2, 0, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetIsAlive()"))
						{
							RCActionHelper item = new RCActionHelper(2, 2, null);
							list.Add(item);
							sentType = 1;
						}
						else if (text.StartsWith("GetTitan()"))
						{
							RCActionHelper item = new RCActionHelper(2, 3, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetKills()"))
						{
							RCActionHelper item = new RCActionHelper(2, 4, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetDeaths()"))
						{
							RCActionHelper item = new RCActionHelper(2, 5, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetMaxDmg()"))
						{
							RCActionHelper item = new RCActionHelper(2, 6, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetTotalDmg()"))
						{
							RCActionHelper item = new RCActionHelper(2, 7, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetCustomInt()"))
						{
							RCActionHelper item = new RCActionHelper(2, 8, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetCustomBool()"))
						{
							RCActionHelper item = new RCActionHelper(2, 9, null);
							list.Add(item);
							sentType = 1;
						}
						else if (text.StartsWith("GetCustomString()"))
						{
							RCActionHelper item = new RCActionHelper(2, 10, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("GetCustomFloat()"))
						{
							RCActionHelper item = new RCActionHelper(2, 11, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionX()"))
						{
							RCActionHelper item = new RCActionHelper(2, 14, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionY()"))
						{
							RCActionHelper item = new RCActionHelper(2, 15, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionZ()"))
						{
							RCActionHelper item = new RCActionHelper(2, 16, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetName()"))
						{
							RCActionHelper item = new RCActionHelper(2, 12, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("GetGuildName()"))
						{
							RCActionHelper item = new RCActionHelper(2, 13, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("GetSpeed()"))
						{
							RCActionHelper item = new RCActionHelper(2, 17, null);
							list.Add(item);
							sentType = 3;
						}
						break;
					case 5:
						if (text.StartsWith("GetType()"))
						{
							RCActionHelper item = new RCActionHelper(3, 0, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetSize()"))
						{
							RCActionHelper item = new RCActionHelper(3, 1, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetHealth()"))
						{
							RCActionHelper item = new RCActionHelper(3, 2, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetPositionX()"))
						{
							RCActionHelper item = new RCActionHelper(3, 3, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionY()"))
						{
							RCActionHelper item = new RCActionHelper(3, 4, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionZ()"))
						{
							RCActionHelper item = new RCActionHelper(3, 5, null);
							list.Add(item);
							sentType = 3;
						}
						break;
					default:
						if (text.StartsWith("ConvertToInt()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("ConvertToBool()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 1;
						}
						else if (text.StartsWith("ConvertToString()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("ConvertToFloat()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 3;
						}
						break;
					}
				}
				else if (text.StartsWith("ConvertToInt()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 0;
				}
				else if (text.StartsWith("ConvertToBool()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 1;
				}
				else if (text.StartsWith("ConvertToString()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 2;
				}
				else if (text.StartsWith("ConvertToFloat()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 3;
				}
			}
		}
		for (int i = list.Count - 1; i > 0; i--)
		{
			list[i - 1].setNextHelper(list[i]);
		}
		return list[0];
	}

	public static PeerStates returnPeerState(int peerstate)
	{
		return peerstate switch
		{
			0 => PeerStates.Authenticated, 
			1 => PeerStates.ConnectedToMaster, 
			2 => PeerStates.DisconnectingFromMasterserver, 
			3 => PeerStates.DisconnectingFromGameserver, 
			4 => PeerStates.DisconnectingFromNameServer, 
			_ => PeerStates.ConnectingToMasterserver, 
		};
	}

	[RPC]
	private void RPCLoadLevel(PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			DestroyAllExistingCloths();
			PhotonNetwork.LoadLevel(LevelInfo.getInfo(level).mapName);
		}
		else if (PhotonNetwork.isMasterClient)
		{
			kickPlayerRC(info.sender, ban: true, "false restart.");
		}
		else
		{
			if (masterRC)
			{
				return;
			}
			restartCount.Add(Time.time);
			foreach (float item in restartCount)
			{
				if (Time.time - item > 60f)
				{
					restartCount.Remove(item);
				}
			}
			if (restartCount.Count < 6)
			{
				DestroyAllExistingCloths();
				PhotonNetwork.LoadLevel(LevelInfo.getInfo(level).mapName);
			}
		}
	}

	public void sendChatContentInfo(string content)
	{
		object[] parameters = new object[2]
		{
			content,
			string.Empty
		};
		base.photonView.RPC("Chat", PhotonTargets.All, parameters);
	}

	public void sendKillInfo(bool t1, string killer, bool t2, string victim, int dmg = 0)
	{
		object[] parameters = new object[5] { t1, killer, t2, victim, dmg };
		base.photonView.RPC("updateKillInfo", PhotonTargets.All, parameters);
	}

	public static void ServerCloseConnection(PhotonPlayer targetPlayer, bool requestIpBan, string inGameName = null)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
		raiseEventOptions.TargetActors = new int[1] { targetPlayer.ID };
		RaiseEventOptions options = raiseEventOptions;
		if (requestIpBan)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = true;
			if (inGameName != null && inGameName.Length > 0)
			{
				hashtable[(byte)1] = inGameName;
			}
			PhotonNetwork.RaiseEvent(203, hashtable, sendReliable: true, options);
		}
		else
		{
			PhotonNetwork.RaiseEvent(203, null, sendReliable: true, options);
		}
	}

	public static void ServerRequestAuthentication(string authPassword)
	{
		if (!string.IsNullOrEmpty(authPassword))
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = authPassword;
			PhotonNetwork.RaiseEvent(198, hashtable, sendReliable: true, new RaiseEventOptions());
		}
	}

	public static void ServerRequestUnban(string bannedAddress)
	{
		if (!string.IsNullOrEmpty(bannedAddress))
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = bannedAddress;
			PhotonNetwork.RaiseEvent(199, hashtable, sendReliable: true, new RaiseEventOptions());
		}
	}

	private void setGameSettings(ExitGames.Client.Photon.Hashtable hash)
	{
		restartingEren = false;
		restartingBomb = false;
		restartingHorse = false;
		restartingTitan = false;
		LegacyGameSettings legacyGameSettings = SettingsManager.LegacyGameSettings;
		if (hash.ContainsKey("bomb"))
		{
			if (!legacyGameSettings.BombModeEnabled.Value)
			{
				legacyGameSettings.BombModeEnabled.Value = true;
				chatRoom.addLINE("<color=#FFCC00>PVP Bomb Mode enabled.</color>");
			}
		}
		else if (legacyGameSettings.BombModeEnabled.Value)
		{
			legacyGameSettings.BombModeEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>PVP Bomb Mode disabled.</color>");
			if (PhotonNetwork.isMasterClient)
			{
				restartingBomb = true;
			}
		}
		if (legacyGameSettings.BombModeEnabled.Value && (!hash.ContainsKey("bombCeiling") || (int)hash["bombCeiling"] == 1))
		{
			MapCeiling.CreateMapCeiling();
		}
		if (!hash.ContainsKey("bombInfiniteGas") || (int)hash["bombInfiniteGas"] == 1)
		{
			legacyGameSettings.BombModeInfiniteGas.Value = true;
		}
		else
		{
			legacyGameSettings.BombModeInfiniteGas.Value = false;
		}
		legacyGameSettings.GlobalHideNames.Value = hash.ContainsKey("globalHideNames");
		if (hash.ContainsKey("globalDisableMinimap"))
		{
			if (!legacyGameSettings.GlobalMinimapDisable.Value)
			{
				legacyGameSettings.GlobalMinimapDisable.Value = true;
				chatRoom.addLINE("<color=#FFCC00>Minimaps are not allowed.</color>");
			}
		}
		else if (legacyGameSettings.GlobalMinimapDisable.Value)
		{
			legacyGameSettings.GlobalMinimapDisable.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Minimaps are allowed.</color>");
		}
		if (hash.ContainsKey("globalDisableMinimap"))
		{
			if (!legacyGameSettings.GlobalMinimapDisable.Value)
			{
				legacyGameSettings.GlobalMinimapDisable.Value = true;
				chatRoom.addLINE("<color=#FFCC00>Minimaps are not allowed.</color>");
			}
		}
		else if (legacyGameSettings.GlobalMinimapDisable.Value)
		{
			legacyGameSettings.GlobalMinimapDisable.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Minimaps are allowed.</color>");
		}
		if (hash.ContainsKey("horse"))
		{
			if (!legacyGameSettings.AllowHorses.Value)
			{
				legacyGameSettings.AllowHorses.Value = true;
				chatRoom.addLINE("<color=#FFCC00>Horses enabled.</color>");
			}
		}
		else if (legacyGameSettings.AllowHorses.Value)
		{
			legacyGameSettings.AllowHorses.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Horses disabled.</color>");
			if (PhotonNetwork.isMasterClient)
			{
				restartingHorse = true;
			}
		}
		if (hash.ContainsKey("punkWaves"))
		{
			if (legacyGameSettings.PunksEveryFive.Value)
			{
				legacyGameSettings.PunksEveryFive.Value = false;
				chatRoom.addLINE("<color=#FFCC00>Punks every 5 waves disabled.</color>");
			}
		}
		else if (!legacyGameSettings.PunksEveryFive.Value)
		{
			legacyGameSettings.PunksEveryFive.Value = true;
			chatRoom.addLINE("<color=#FFCC00>Punks ever 5 waves enabled.</color>");
		}
		if (hash.ContainsKey("ahssReload"))
		{
			if (legacyGameSettings.AHSSAirReload.Value)
			{
				legacyGameSettings.AHSSAirReload.Value = false;
				chatRoom.addLINE("<color=#FFCC00>AHSS Air-Reload disabled.</color>");
			}
		}
		else if (!legacyGameSettings.AHSSAirReload.Value)
		{
			legacyGameSettings.AHSSAirReload.Value = true;
			chatRoom.addLINE("<color=#FFCC00>AHSS Air-Reload allowed.</color>");
		}
		if (hash.ContainsKey("team"))
		{
			if (legacyGameSettings.TeamMode.Value != (int)hash["team"])
			{
				legacyGameSettings.TeamMode.Value = (int)hash["team"];
				string text = string.Empty;
				if (legacyGameSettings.TeamMode.Value == 1)
				{
					text = "no sort";
				}
				else if (legacyGameSettings.TeamMode.Value == 2)
				{
					text = "locked by size";
				}
				else if (legacyGameSettings.TeamMode.Value == 3)
				{
					text = "locked by skill";
				}
				chatRoom.addLINE("<color=#FFCC00>Team Mode enabled (" + text + ").</color>");
				if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 0)
				{
					setTeam(3);
				}
			}
		}
		else if (legacyGameSettings.TeamMode.Value != 0)
		{
			legacyGameSettings.TeamMode.Value = 0;
			setTeam(0);
			chatRoom.addLINE("<color=#FFCC00>Team mode disabled.</color>");
		}
		if (hash.ContainsKey("point"))
		{
			if (!legacyGameSettings.PointModeEnabled.Value || legacyGameSettings.PointModeAmount.Value != (int)hash["point"])
			{
				legacyGameSettings.PointModeEnabled.Value = true;
				legacyGameSettings.PointModeAmount.Value = (int)hash["point"];
				chatRoom.addLINE("<color=#FFCC00>Point limit enabled (" + Convert.ToString(legacyGameSettings.PointModeAmount.Value) + ").</color>");
			}
		}
		else if (legacyGameSettings.PointModeEnabled.Value)
		{
			legacyGameSettings.PointModeEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Point limit disabled.</color>");
		}
		if (hash.ContainsKey("rock"))
		{
			if (legacyGameSettings.RockThrowEnabled.Value)
			{
				legacyGameSettings.RockThrowEnabled.Value = false;
				chatRoom.addLINE("<color=#FFCC00>Punk rock throwing disabled.</color>");
			}
		}
		else if (!legacyGameSettings.RockThrowEnabled.Value)
		{
			legacyGameSettings.RockThrowEnabled.Value = true;
			chatRoom.addLINE("<color=#FFCC00>Punk rock throwing enabled.</color>");
		}
		if (hash.ContainsKey("explode"))
		{
			if (!legacyGameSettings.TitanExplodeEnabled.Value || legacyGameSettings.TitanExplodeRadius.Value != (int)hash["explode"])
			{
				legacyGameSettings.TitanExplodeEnabled.Value = true;
				legacyGameSettings.TitanExplodeRadius.Value = (int)hash["explode"];
				chatRoom.addLINE("<color=#FFCC00>Titan Explode Mode enabled (Radius " + Convert.ToString(legacyGameSettings.TitanExplodeRadius.Value) + ").</color>");
			}
		}
		else if (legacyGameSettings.TitanExplodeEnabled.Value)
		{
			legacyGameSettings.TitanExplodeEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Titan Explode Mode disabled.</color>");
		}
		if (hash.ContainsKey("healthMode") && hash.ContainsKey("healthLower") && hash.ContainsKey("healthUpper"))
		{
			if (legacyGameSettings.TitanHealthMode.Value != (int)hash["healthMode"] || legacyGameSettings.TitanHealthMin.Value != (int)hash["healthLower"] || legacyGameSettings.TitanHealthMax.Value != (int)hash["healthUpper"])
			{
				legacyGameSettings.TitanHealthMode.Value = (int)hash["healthMode"];
				legacyGameSettings.TitanHealthMin.Value = (int)hash["healthLower"];
				legacyGameSettings.TitanHealthMax.Value = (int)hash["healthUpper"];
				string text = "Static";
				if (legacyGameSettings.TitanHealthMode.Value == 2)
				{
					text = "Scaled";
				}
				chatRoom.addLINE("<color=#FFCC00>Titan Health (" + text + ", " + legacyGameSettings.TitanHealthMin.Value + " to " + legacyGameSettings.TitanHealthMax.Value + ") enabled.</color>");
			}
		}
		else if (legacyGameSettings.TitanHealthMode.Value > 0)
		{
			legacyGameSettings.TitanHealthMode.Value = 0;
			chatRoom.addLINE("<color=#FFCC00>Titan Health disabled.</color>");
		}
		if (hash.ContainsKey("infection"))
		{
			if (!legacyGameSettings.InfectionModeEnabled.Value)
			{
				legacyGameSettings.InfectionModeEnabled.Value = true;
				legacyGameSettings.InfectionModeAmount.Value = (int)hash["infection"];
				name = LoginFengKAI.player.name;
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.RCteam, 0);
				PhotonNetwork.player.SetCustomProperties(hashtable);
				chatRoom.addLINE("<color=#FFCC00>Infection mode (" + Convert.ToString(legacyGameSettings.InfectionModeAmount.Value) + ") enabled. Make sure your first character is human.</color>");
			}
		}
		else if (legacyGameSettings.InfectionModeEnabled.Value)
		{
			legacyGameSettings.InfectionModeEnabled.Value = false;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.isTitan, 1);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			chatRoom.addLINE("<color=#FFCC00>Infection Mode disabled.</color>");
			if (PhotonNetwork.isMasterClient)
			{
				restartingTitan = true;
			}
		}
		if (hash.ContainsKey("eren"))
		{
			if (!legacyGameSettings.KickShifters.Value)
			{
				legacyGameSettings.KickShifters.Value = true;
				chatRoom.addLINE("<color=#FFCC00>Anti-Eren enabled. Using eren transform will get you kicked.</color>");
				if (PhotonNetwork.isMasterClient)
				{
					restartingEren = true;
				}
			}
		}
		else if (legacyGameSettings.KickShifters.Value)
		{
			legacyGameSettings.KickShifters.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Anti-Eren disabled. Eren transform is allowed.</color>");
		}
		if (hash.ContainsKey("titanc"))
		{
			if (!legacyGameSettings.TitanNumberEnabled.Value || legacyGameSettings.TitanNumber.Value != (int)hash["titanc"])
			{
				legacyGameSettings.TitanNumberEnabled.Value = true;
				legacyGameSettings.TitanNumber.Value = (int)hash["titanc"];
				chatRoom.addLINE("<color=#FFCC00>" + Convert.ToString(legacyGameSettings.TitanNumber.Value) + " titans will spawn each round.</color>");
			}
		}
		else if (legacyGameSettings.TitanNumberEnabled.Value)
		{
			legacyGameSettings.TitanNumberEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Default titans will spawn each round.</color>");
		}
		if (hash.ContainsKey("damage"))
		{
			if (!legacyGameSettings.TitanArmorEnabled.Value || legacyGameSettings.TitanArmor.Value != (int)hash["damage"])
			{
				legacyGameSettings.TitanArmorEnabled.Value = true;
				legacyGameSettings.TitanArmor.Value = (int)hash["damage"];
				chatRoom.addLINE("<color=#FFCC00>Nape minimum damage (" + Convert.ToString(legacyGameSettings.TitanArmor.Value) + ") enabled.</color>");
			}
		}
		else if (legacyGameSettings.TitanArmorEnabled.Value)
		{
			legacyGameSettings.TitanArmorEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Nape minimum damage disabled.</color>");
		}
		if (hash.ContainsKey("sizeMode") && hash.ContainsKey("sizeLower") && hash.ContainsKey("sizeUpper"))
		{
			if (!legacyGameSettings.TitanSizeEnabled.Value || legacyGameSettings.TitanSizeMin.Value != (float)hash["sizeLower"] || legacyGameSettings.TitanSizeMax.Value != (float)hash["sizeUpper"])
			{
				legacyGameSettings.TitanSizeEnabled.Value = true;
				legacyGameSettings.TitanSizeMin.Value = (float)hash["sizeLower"];
				legacyGameSettings.TitanSizeMax.Value = (float)hash["sizeUpper"];
				chatRoom.addLINE("<color=#FFCC00>Custom titan size (" + legacyGameSettings.TitanSizeMin.Value.ToString("F2") + "," + legacyGameSettings.TitanSizeMax.Value.ToString("F2") + ") enabled.</color>");
			}
		}
		else if (legacyGameSettings.TitanSizeEnabled.Value)
		{
			legacyGameSettings.TitanSizeEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Custom titan size disabled.</color>");
		}
		if (hash.ContainsKey("spawnMode") && hash.ContainsKey("nRate") && hash.ContainsKey("aRate") && hash.ContainsKey("jRate") && hash.ContainsKey("cRate") && hash.ContainsKey("pRate"))
		{
			if (!legacyGameSettings.TitanSpawnEnabled.Value || legacyGameSettings.TitanSpawnNormal.Value != (float)hash["nRate"] || legacyGameSettings.TitanSpawnAberrant.Value != (float)hash["aRate"] || legacyGameSettings.TitanSpawnJumper.Value != (float)hash["jRate"] || legacyGameSettings.TitanSpawnCrawler.Value != (float)hash["cRate"] || legacyGameSettings.TitanSpawnPunk.Value != (float)hash["pRate"])
			{
				legacyGameSettings.TitanSpawnEnabled.Value = true;
				legacyGameSettings.TitanSpawnNormal.Value = (float)hash["nRate"];
				legacyGameSettings.TitanSpawnAberrant.Value = (float)hash["aRate"];
				legacyGameSettings.TitanSpawnJumper.Value = (float)hash["jRate"];
				legacyGameSettings.TitanSpawnCrawler.Value = (float)hash["cRate"];
				legacyGameSettings.TitanSpawnPunk.Value = (float)hash["pRate"];
				chatRoom.addLINE("<color=#FFCC00>Custom spawn rate enabled (" + legacyGameSettings.TitanSpawnNormal.Value.ToString("F2") + "% Normal, " + legacyGameSettings.TitanSpawnAberrant.Value.ToString("F2") + "% Abnormal, " + legacyGameSettings.TitanSpawnJumper.Value.ToString("F2") + "% Jumper, " + legacyGameSettings.TitanSpawnCrawler.Value.ToString("F2") + "% Crawler, " + legacyGameSettings.TitanSpawnPunk.Value.ToString("F2") + "% Punk </color>");
			}
		}
		else if (legacyGameSettings.TitanSpawnEnabled.Value)
		{
			legacyGameSettings.TitanSpawnEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Custom spawn rate disabled.</color>");
		}
		if (hash.ContainsKey("waveModeOn") && hash.ContainsKey("waveModeNum"))
		{
			if (!legacyGameSettings.TitanPerWavesEnabled.Value || legacyGameSettings.TitanPerWaves.Value != (int)hash["waveModeNum"])
			{
				legacyGameSettings.TitanPerWavesEnabled.Value = true;
				legacyGameSettings.TitanPerWaves.Value = (int)hash["waveModeNum"];
				chatRoom.addLINE("<color=#FFCC00>Custom wave mode (" + legacyGameSettings.TitanPerWaves.Value + ") enabled.</color>");
			}
		}
		else if (legacyGameSettings.TitanPerWavesEnabled.Value)
		{
			legacyGameSettings.TitanPerWavesEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Custom wave mode disabled.</color>");
		}
		if (hash.ContainsKey("friendly"))
		{
			if (!legacyGameSettings.FriendlyMode.Value)
			{
				legacyGameSettings.FriendlyMode.Value = true;
				chatRoom.addLINE("<color=#FFCC00>PVP is prohibited.</color>");
			}
		}
		else if (legacyGameSettings.FriendlyMode.Value)
		{
			legacyGameSettings.FriendlyMode.Value = false;
			chatRoom.addLINE("<color=#FFCC00>PVP is allowed.</color>");
		}
		if (hash.ContainsKey("pvp"))
		{
			if (legacyGameSettings.BladePVP.Value != (int)hash["pvp"])
			{
				legacyGameSettings.BladePVP.Value = (int)hash["pvp"];
				string text = string.Empty;
				if (legacyGameSettings.BladePVP.Value == 1)
				{
					text = "Team-Based";
				}
				else if (legacyGameSettings.BladePVP.Value == 2)
				{
					text = "FFA";
				}
				chatRoom.addLINE("<color=#FFCC00>Blade/AHSS PVP enabled (" + text + ").</color>");
			}
		}
		else if (legacyGameSettings.BladePVP.Value != 0)
		{
			legacyGameSettings.BladePVP.Value = 0;
			chatRoom.addLINE("<color=#FFCC00>Blade/AHSS PVP disabled.</color>");
		}
		if (hash.ContainsKey("maxwave"))
		{
			if (!legacyGameSettings.TitanMaxWavesEnabled.Value || legacyGameSettings.TitanMaxWaves.Value != (int)hash["maxwave"])
			{
				legacyGameSettings.TitanMaxWavesEnabled.Value = true;
				legacyGameSettings.TitanMaxWaves.Value = (int)hash["maxwave"];
				chatRoom.addLINE("<color=#FFCC00>Max wave is " + legacyGameSettings.TitanMaxWaves.Value + ".</color>");
			}
		}
		else if (legacyGameSettings.TitanMaxWavesEnabled.Value)
		{
			legacyGameSettings.TitanMaxWavesEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Max wave set to default.</color>");
		}
		if (hash.ContainsKey("endless"))
		{
			if (!legacyGameSettings.EndlessRespawnEnabled.Value || legacyGameSettings.EndlessRespawnTime.Value != (int)hash["endless"])
			{
				legacyGameSettings.EndlessRespawnEnabled.Value = true;
				legacyGameSettings.EndlessRespawnTime.Value = (int)hash["endless"];
				chatRoom.addLINE("<color=#FFCC00>Endless respawn enabled (" + legacyGameSettings.EndlessRespawnTime.Value + " seconds).</color>");
			}
		}
		else if (legacyGameSettings.EndlessRespawnEnabled.Value)
		{
			legacyGameSettings.EndlessRespawnEnabled.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Endless respawn disabled.</color>");
		}
		if (hash.ContainsKey("motd"))
		{
			if (legacyGameSettings.Motd.Value != (string)hash["motd"])
			{
				legacyGameSettings.Motd.Value = (string)hash["motd"];
				chatRoom.addLINE("<color=#FFCC00>MOTD:" + legacyGameSettings.Motd.Value + "</color>");
			}
		}
		else if (legacyGameSettings.Motd.Value != string.Empty)
		{
			legacyGameSettings.Motd.Value = string.Empty;
		}
		if (hash.ContainsKey("deadlycannons"))
		{
			if (!legacyGameSettings.CannonsFriendlyFire.Value)
			{
				legacyGameSettings.CannonsFriendlyFire.Value = true;
				chatRoom.addLINE("<color=#FFCC00>Cannons will now kill players.</color>");
			}
		}
		else if (legacyGameSettings.CannonsFriendlyFire.Value)
		{
			legacyGameSettings.CannonsFriendlyFire.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Cannons will no longer kill players.</color>");
		}
		if (hash.ContainsKey("asoracing"))
		{
			if (!legacyGameSettings.RacingEndless.Value)
			{
				legacyGameSettings.RacingEndless.Value = true;
				chatRoom.addLINE("<color=#FFCC00>Racing will not restart on win.</color>");
			}
		}
		else if (legacyGameSettings.RacingEndless.Value)
		{
			legacyGameSettings.RacingEndless.Value = false;
			chatRoom.addLINE("<color=#FFCC00>Racing will restart on win.</color>");
		}
		if (hash.ContainsKey("racingStartTime"))
		{
			legacyGameSettings.RacingStartTime.Value = (float)hash["racingStartTime"];
		}
		else
		{
			legacyGameSettings.RacingStartTime.Value = 20f;
		}
		foreach (HERO hero in heroes)
		{
			if (hero != null)
			{
				hero.SetName();
			}
		}
	}

	private IEnumerator setGuildFeng()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("name", LoginFengKAI.player.name);
		wWWForm.AddField("guildname", LoginFengKAI.player.guildname);
		yield return (!Application.isWebPlayer) ? new WWW("http://fenglee.com/game/aog/change_guild_name.php", wWWForm) : new WWW("http://aotskins.com/version/guild.php", wWWForm);
	}

	[RPC]
	private void setMasterRC(PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			masterRC = true;
		}
	}

	private void setTeam(int setting)
	{
		switch (setting)
		{
		case 0:
		{
			name = LoginFengKAI.player.name;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.RCteam, 0);
			hashtable.Add(PhotonPlayerProperty.name, name);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			break;
		}
		case 1:
		{
			ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
			hashtable3.Add(PhotonPlayerProperty.RCteam, 1);
			string text2 = LoginFengKAI.player.name;
			while (text2.Contains("[") && text2.Length >= text2.IndexOf("[") + 8)
			{
				int startIndex2 = text2.IndexOf("[");
				text2 = text2.Remove(startIndex2, 8);
			}
			if (!text2.StartsWith("[00FFFF]"))
			{
				text2 = "[00FFFF]" + text2;
			}
			name = text2;
			hashtable3.Add(PhotonPlayerProperty.name, name);
			PhotonNetwork.player.SetCustomProperties(hashtable3);
			break;
		}
		case 2:
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
			hashtable2.Add(PhotonPlayerProperty.RCteam, 2);
			string text = LoginFengKAI.player.name;
			while (text.Contains("[") && text.Length >= text.IndexOf("[") + 8)
			{
				int startIndex = text.IndexOf("[");
				text = text.Remove(startIndex, 8);
			}
			if (!text.StartsWith("[FF00FF]"))
			{
				text = "[FF00FF]" + text;
			}
			name = text;
			hashtable2.Add(PhotonPlayerProperty.name, name);
			PhotonNetwork.player.SetCustomProperties(hashtable2);
			break;
		}
		case 3:
		{
			int num = 0;
			int num2 = 0;
			int team = 1;
			PhotonPlayer[] array = PhotonNetwork.playerList;
			for (int i = 0; i < array.Length; i++)
			{
				switch (RCextensions.returnIntFromObject(array[i].customProperties[PhotonPlayerProperty.RCteam]))
				{
				case 1:
					num++;
					break;
				case 2:
					num2++;
					break;
				}
			}
			if (num > num2)
			{
				team = 2;
			}
			setTeam(team);
			break;
		}
		}
		if (setting != 0 && setting != 1 && setting != 2)
		{
			return;
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject go in array2)
		{
			if (go.GetPhotonView().isMine)
			{
				base.photonView.RPC("labelRPC", PhotonTargets.All, go.GetPhotonView().viewID);
			}
		}
	}

	[RPC]
	private void setTeamRPC(int setting, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient || info.sender.isLocal)
		{
			setTeam(setting);
		}
	}

	[RPC]
	private void settingRPC(ExitGames.Client.Photon.Hashtable hash, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			setGameSettings(hash);
		}
	}

	private void showChatContent(string content)
	{
		chatContent.Add(content);
		if (chatContent.Count > 10)
		{
			chatContent.RemoveAt(0);
		}
		GameObject.Find("LabelChatContent").GetComponent<UILabel>().text = string.Empty;
		for (int i = 0; i < chatContent.Count; i++)
		{
			GameObject.Find("LabelChatContent").GetComponent<UILabel>().text += chatContent[i];
		}
	}

	public void ShowHUDInfoCenter(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	public void ShowHUDInfoCenterADD(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text += content;
		}
	}

	private void ShowHUDInfoTopCenter(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void ShowHUDInfoTopCenterADD(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text += content;
		}
	}

	private void ShowHUDInfoTopLeft(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopLeft");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void ShowHUDInfoTopRight(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopRight");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void ShowHUDInfoTopRightMAPNAME(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopRight");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text += content;
		}
	}

	[RPC]
	private void showResult(string text0, string text1, string text2, string text3, string text4, string text6, PhotonMessageInfo t)
	{
		if (!gameTimesUp && t.sender.isMasterClient)
		{
			gameTimesUp = true;
			GameObject obj = GameObject.Find("UI_IN_GAME");
			NGUITools.SetActive(obj.GetComponent<UIReferArray>().panels[0], state: false);
			NGUITools.SetActive(obj.GetComponent<UIReferArray>().panels[1], state: false);
			NGUITools.SetActive(obj.GetComponent<UIReferArray>().panels[2], state: true);
			NGUITools.SetActive(obj.GetComponent<UIReferArray>().panels[3], state: false);
			GameObject.Find("LabelName").GetComponent<UILabel>().text = text0;
			GameObject.Find("LabelKill").GetComponent<UILabel>().text = text1;
			GameObject.Find("LabelDead").GetComponent<UILabel>().text = text2;
			GameObject.Find("LabelMaxDmg").GetComponent<UILabel>().text = text3;
			GameObject.Find("LabelTotalDmg").GetComponent<UILabel>().text = text4;
			GameObject.Find("LabelResultTitle").GetComponent<UILabel>().text = text6;
			IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
			gameStart = false;
		}
		else if (!t.sender.isMasterClient && PhotonNetwork.player.isMasterClient)
		{
			kickPlayerRC(t.sender, ban: true, "false game end.");
		}
	}

	private void SingleShowHUDInfoTopCenter(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void SingleShowHUDInfoTopLeft(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopLeft");
		if (gameObject != null)
		{
			content = content.Replace("[0]", "[*^_^*]");
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	[RPC]
	public void someOneIsDead(int id = -1)
	{
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			if (id != 0)
			{
				PVPtitanScore += 2;
			}
			checkPVPpts();
			object[] parameters = new object[2] { PVPhumanScore, PVPtitanScore };
			base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
		{
			titanScore++;
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.TROST)
		{
			if (isPlayerAllDead2())
			{
				gameLose2();
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS && SettingsManager.LegacyGameSettings.BladePVP.Value == 0 && !SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
		{
			if (isPlayerAllDead2())
			{
				gameLose2();
				teamWinner = 0;
			}
			if (isTeamAllDead2(1))
			{
				teamWinner = 2;
				gameWin2();
			}
			if (isTeamAllDead2(2))
			{
				teamWinner = 1;
				gameWin2();
			}
		}
	}

	public void SpawnNonAITitan(string id, string tag = "titanRespawn")
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		GameObject gameObject = array[UnityEngine.Random.Range(0, array.Length)];
		myLastHero = id.ToUpper();
		GameObject gameObject2 = ((IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE) ? PhotonNetwork.Instantiate("TITAN_VER3.1", gameObject.transform.position, gameObject.transform.rotation, 0) : PhotonNetwork.Instantiate("TITAN_VER3.1", checkpoint.transform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20)), checkpoint.transform.rotation, 0));
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(gameObject2);
		gameObject2.GetComponent<TITAN>().nonAI = true;
		gameObject2.GetComponent<TITAN>().speed = 30f;
		gameObject2.GetComponent<TITAN_CONTROLLER>().enabled = true;
		if (id == "RANDOM" && UnityEngine.Random.Range(0, 100) < 7)
		{
			gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
		}
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
		GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", false } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			2
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		ShowHUDInfoCenter(string.Empty);
	}

	public void SpawnNonAITitan2(string id, string tag = "titanRespawn")
	{
		if (logicLoaded && customLevelLoaded)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
			GameObject gameObject = array[UnityEngine.Random.Range(0, array.Length)];
			Vector3 position = gameObject.transform.position;
			if (level.StartsWith("Custom") && titanSpawns.Count > 0)
			{
				position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
			}
			myLastHero = id.ToUpper();
			GameObject gameObject2 = ((IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE) ? PhotonNetwork.Instantiate("TITAN_VER3.1", position, gameObject.transform.rotation, 0) : PhotonNetwork.Instantiate("TITAN_VER3.1", checkpoint.transform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20)), checkpoint.transform.rotation, 0));
			GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(gameObject2);
			gameObject2.GetComponent<TITAN>().nonAI = true;
			gameObject2.GetComponent<TITAN>().speed = 30f;
			gameObject2.GetComponent<TITAN_CONTROLLER>().enabled = true;
			if (id == "RANDOM" && UnityEngine.Random.Range(0, 100) < 7)
			{
				gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
			}
			GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
			GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
			GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
			GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", false } };
			PhotonNetwork.player.SetCustomProperties(customProperties);
			customProperties = new ExitGames.Client.Photon.Hashtable { 
			{
				PhotonPlayerProperty.isTitan,
				2
			} };
			PhotonNetwork.player.SetCustomProperties(customProperties);
			ShowHUDInfoCenter(string.Empty);
		}
		else
		{
			NOTSpawnNonAITitanRC(id);
		}
	}

	public void SpawnPlayer(string id, string tag = "playerRespawn")
	{
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			SpawnPlayerAt2(id, checkpoint);
			return;
		}
		myLastRespawnTag = tag;
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		GameObject pos = array[UnityEngine.Random.Range(0, array.Length)];
		SpawnPlayerAt2(id, pos);
	}

	public void SpawnPlayerAt2(string id, GameObject pos)
	{
		if (!logicLoaded || !customLevelLoaded)
		{
			NOTSpawnPlayerRC(id);
			return;
		}
		Vector3 position = pos.transform.position;
		if (racingSpawnPointSet)
		{
			position = racingSpawnPoint;
		}
		else if (level.StartsWith("Custom"))
		{
			if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 0)
			{
				List<Vector3> list = new List<Vector3>();
				foreach (Vector3 item in playerSpawnsC)
				{
					list.Add(item);
				}
				foreach (Vector3 item2 in playerSpawnsM)
				{
					list.Add(item2);
				}
				if (list.Count > 0)
				{
					position = list[UnityEngine.Random.Range(0, list.Count)];
				}
			}
			else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 1)
			{
				if (playerSpawnsC.Count > 0)
				{
					position = playerSpawnsC[UnityEngine.Random.Range(0, playerSpawnsC.Count)];
				}
			}
			else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 2 && playerSpawnsM.Count > 0)
			{
				position = playerSpawnsM[UnityEngine.Random.Range(0, playerSpawnsM.Count)];
			}
		}
		IN_GAME_MAIN_CAMERA component = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
		myLastHero = id.ToUpper();
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (IN_GAME_MAIN_CAMERA.singleCharacter == "TITAN_EREN")
			{
				component.setMainObject((GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), pos.transform.position, pos.transform.rotation));
			}
			else
			{
				component.setMainObject((GameObject)UnityEngine.Object.Instantiate(Resources.Load("AOTTG_HERO 1"), pos.transform.position, pos.transform.rotation));
				if (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 1" || IN_GAME_MAIN_CAMERA.singleCharacter == "SET 2" || IN_GAME_MAIN_CAMERA.singleCharacter == "SET 3")
				{
					HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(IN_GAME_MAIN_CAMERA.singleCharacter);
					heroCostume.checkstat();
					CostumeConeveter.HeroCostumeToLocalData(heroCostume, IN_GAME_MAIN_CAMERA.singleCharacter);
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
					if (heroCostume != null)
					{
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume;
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = heroCostume.stat;
					}
					else
					{
						heroCostume = HeroCostume.costumeOption[3];
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume;
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(heroCostume.name.ToUpper());
					}
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
					component.main_object.GetComponent<HERO>().setStat2();
					component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
				}
				else
				{
					for (int i = 0; i < HeroCostume.costume.Length; i++)
					{
						if (HeroCostume.costume[i].name.ToUpper() == IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper())
						{
							int num = HeroCostume.costume[i].id + CheckBoxCostume.costumeSet - 1;
							if (HeroCostume.costume[num].name != HeroCostume.costume[i].name)
							{
								num = HeroCostume.costume[i].id + 1;
							}
							component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
							component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[num];
							component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num].name.ToUpper());
							component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
							component.main_object.GetComponent<HERO>().setStat2();
							component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
							break;
						}
					}
				}
			}
		}
		else
		{
			component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, pos.transform.rotation, 0));
			id = id.ToUpper();
			switch (id)
			{
			case "SET 1":
			case "SET 2":
			case "SET 3":
			{
				HeroCostume heroCostume2 = CostumeConeveter.LocalDataToHeroCostume(id);
				heroCostume2.checkstat();
				CostumeConeveter.HeroCostumeToLocalData(heroCostume2, id);
				if (heroCostume2.uniform_type == UNIFORM_TYPE.CasualAHSS && SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
				{
					heroCostume2 = HeroCostume.costume[6];
				}
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
				if (heroCostume2 != null)
				{
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume2;
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = heroCostume2.stat;
				}
				else
				{
					heroCostume2 = HeroCostume.costumeOption[3];
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume2;
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(heroCostume2.name.ToUpper());
				}
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
				component.main_object.GetComponent<HERO>().setStat2();
				component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
				break;
			}
			default:
			{
				for (int j = 0; j < HeroCostume.costume.Length; j++)
				{
					if (HeroCostume.costume[j].name.ToUpper() == id.ToUpper())
					{
						int num2 = HeroCostume.costume[j].id;
						if (id.ToUpper() != "AHSS")
						{
							num2 += CheckBoxCostume.costumeSet - 1;
						}
						if (HeroCostume.costume[num2].name != HeroCostume.costume[j].name)
						{
							num2 = HeroCostume.costume[j].id + 1;
						}
						if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && id.ToUpper() == "AHSS")
						{
							num2 = 6;
						}
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[num2];
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num2].name.ToUpper());
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
						component.main_object.GetComponent<HERO>().setStat2();
						component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
						break;
					}
				}
				break;
			}
			}
			CostumeConeveter.HeroCostumeToPhotonData2(component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
			{
				component.main_object.transform.position += new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20));
			}
			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", false } };
			PhotonNetwork.player.SetCustomProperties(customProperties);
			customProperties = new ExitGames.Client.Photon.Hashtable { 
			{
				PhotonPlayerProperty.isTitan,
				1
			} };
			PhotonNetwork.player.SetCustomProperties(customProperties);
		}
		component.enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
		GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
		GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
		component.gameOver = false;
		isLosing = false;
		ShowHUDInfoCenter(string.Empty);
	}

	[RPC]
	public void spawnPlayerAtRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient || !logicLoaded || !customLevelLoaded || needChooseSide || !Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
		{
			return;
		}
		Vector3 position = new Vector3(posX, posY, posZ);
		IN_GAME_MAIN_CAMERA component = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();
		component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, new Quaternion(0f, 0f, 0f, 1f), 0));
		string text = myLastHero.ToUpper();
		switch (text)
		{
		case "SET 1":
		case "SET 2":
		case "SET 3":
		{
			HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(text);
			heroCostume.checkstat();
			CostumeConeveter.HeroCostumeToLocalData(heroCostume, text);
			if (heroCostume.uniform_type == UNIFORM_TYPE.CasualAHSS && SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
			{
				heroCostume = HeroCostume.costume[6];
			}
			component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
			if (heroCostume != null)
			{
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume;
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = heroCostume.stat;
			}
			else
			{
				heroCostume = HeroCostume.costumeOption[3];
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume;
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(heroCostume.name.ToUpper());
			}
			component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
			component.main_object.GetComponent<HERO>().setStat2();
			component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
			break;
		}
		default:
		{
			for (int i = 0; i < HeroCostume.costume.Length; i++)
			{
				if (HeroCostume.costume[i].name.ToUpper() == text.ToUpper())
				{
					int num = HeroCostume.costume[i].id;
					if (text.ToUpper() != "AHSS")
					{
						num += CheckBoxCostume.costumeSet - 1;
					}
					if (HeroCostume.costume[num].name != HeroCostume.costume[i].name)
					{
						num = HeroCostume.costume[i].id + 1;
					}
					if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && text.ToUpper() == "AHSS")
					{
						num = 6;
					}
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[num];
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num].name.ToUpper());
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
					component.main_object.GetComponent<HERO>().setStat2();
					component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
					break;
				}
			}
			break;
		}
		}
		CostumeConeveter.HeroCostumeToPhotonData2(component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			component.main_object.transform.position += new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20));
		}
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", false } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			1
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		component.enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
		GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
		GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
		component.gameOver = false;
		isLosing = false;
		ShowHUDInfoCenter(string.Empty);
	}

	private void spawnPlayerCustomMap()
	{
		if (!needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
		{
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
			if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
			{
				SpawnNonAITitan2(myLastHero);
			}
			else
			{
				SpawnPlayer(myLastHero, myLastRespawnTag);
			}
			ShowHUDInfoCenter(string.Empty);
		}
	}

	public GameObject spawnTitan(int rate, Vector3 position, Quaternion rotation, bool punk = false)
	{
		GameObject gameObject = spawnTitanRaw(position, rotation);
		if (punk)
		{
			gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
		}
		else if (UnityEngine.Random.Range(0, 100) < rate)
		{
			if (IN_GAME_MAIN_CAMERA.difficulty == 2)
			{
				if (UnityEngine.Random.Range(0f, 1f) < 0.7f || LevelInfo.getInfo(level).noCrawler)
				{
					gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				}
				else
				{
					gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
				}
			}
		}
		else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
		{
			if (UnityEngine.Random.Range(0f, 1f) < 0.7f || LevelInfo.getInfo(level).noCrawler)
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
			}
			else
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
			}
		}
		else if (UnityEngine.Random.Range(0, 100) < rate)
		{
			if (UnityEngine.Random.Range(0f, 1f) < 0.8f || LevelInfo.getInfo(level).noCrawler)
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
			}
			else
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
			}
		}
		else if (UnityEngine.Random.Range(0f, 1f) < 0.8f || LevelInfo.getInfo(level).noCrawler)
		{
			gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
		}
		else
		{
			gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
		}
		GameObject gameObject2 = ((IN_GAME_MAIN_CAMERA.gametype != 0) ? PhotonNetwork.Instantiate("FX/FXtitanSpawn", gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f), 0) : ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanSpawn"), gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f))));
		gameObject2.transform.localScale = gameObject.transform.localScale;
		return gameObject;
	}

	public void spawnTitanAction(int type, float size, int health, int number)
	{
		Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
		Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
		if (titanSpawns.Count > 0)
		{
			position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
		}
		else
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("titanRespawn");
			if (array.Length != 0)
			{
				int num = UnityEngine.Random.Range(0, array.Length);
				GameObject obj = array[num];
				position = obj.transform.position;
				rotation = obj.transform.rotation;
			}
		}
		for (int i = 0; i < number; i++)
		{
			GameObject gameObject = spawnTitanRaw(position, rotation);
			gameObject.GetComponent<TITAN>().resetLevel(size);
			gameObject.GetComponent<TITAN>().hasSetLevel = true;
			if ((float)health > 0f)
			{
				gameObject.GetComponent<TITAN>().currentHealth = health;
				gameObject.GetComponent<TITAN>().maxHealth = health;
			}
			switch (type)
			{
			case 0:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
				break;
			case 1:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
				break;
			case 2:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				break;
			case 3:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
				break;
			case 4:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
				break;
			}
		}
	}

	public void spawnTitanAtAction(int type, float size, int health, int number, float posX, float posY, float posZ)
	{
		Vector3 position = new Vector3(posX, posY, posZ);
		Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
		for (int i = 0; i < number; i++)
		{
			GameObject gameObject = spawnTitanRaw(position, rotation);
			gameObject.GetComponent<TITAN>().resetLevel(size);
			gameObject.GetComponent<TITAN>().hasSetLevel = true;
			if ((float)health > 0f)
			{
				gameObject.GetComponent<TITAN>().currentHealth = health;
				gameObject.GetComponent<TITAN>().maxHealth = health;
			}
			switch (type)
			{
			case 0:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
				break;
			case 1:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
				break;
			case 2:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				break;
			case 3:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
				break;
			case 4:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
				break;
			}
		}
	}

	public void spawnTitanCustom(string type, int abnormal, int rate, bool punk)
	{
		int num = rate;
		if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
		{
			punk = false;
		}
		if (level.StartsWith("Custom"))
		{
			num = 5;
			if (SettingsManager.LegacyGameSettings.GameType.Value == 1)
			{
				num = 3;
			}
			else if (SettingsManager.LegacyGameSettings.GameType.Value == 2 || SettingsManager.LegacyGameSettings.GameType.Value == 3)
			{
				num = 0;
			}
		}
		if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value || (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value && level.StartsWith("Custom") && SettingsManager.LegacyGameSettings.GameType.Value >= 2))
		{
			num = SettingsManager.LegacyGameSettings.TitanNumber.Value;
			if (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
			{
				num = 0;
			}
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
		{
			if (punk)
			{
				num = rate;
			}
			else if (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
			{
				int num2 = 1;
				if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
				{
					num2 = SettingsManager.LegacyGameSettings.TitanPerWaves.Value;
				}
				num += (wave - 1) * (num2 - 1);
			}
			else if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
			{
				int num2 = 1;
				if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
				{
					num2 = SettingsManager.LegacyGameSettings.TitanPerWaves.Value;
				}
				num += (wave - 1) * num2;
			}
		}
		num = Math.Min(100, num);
		if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
		{
			float num3 = SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value;
			float num4 = SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value;
			float num5 = SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value;
			float num6 = SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value;
			float num7 = SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value;
			if (punk)
			{
				num3 = 0f;
				num4 = 0f;
				num5 = 0f;
				num6 = 0f;
				num7 = 100f;
				num = rate;
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("titanRespawn");
			List<GameObject> list = new List<GameObject>(array);
			for (int i = 0; i < num; i++)
			{
				Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
				Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
				if (titanSpawns.Count > 0)
				{
					position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
				}
				else if (array.Length != 0)
				{
					if (list.Count <= 0)
					{
						list = new List<GameObject>(array);
					}
					int index = UnityEngine.Random.Range(0, list.Count);
					GameObject obj = list[index];
					position = obj.transform.position;
					rotation = obj.transform.rotation;
					list.RemoveAt(index);
				}
				float num8 = UnityEngine.Random.Range(0f, 100f);
				if (num8 <= num3 + num4 + num5 + num6 + num7)
				{
					GameObject gameObject = spawnTitanRaw(position, rotation);
					if (num8 < num3)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
					}
					else if (num8 >= num3 && num8 < num3 + num4)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
					}
					else if (num8 >= num3 + num4 && num8 < num3 + num4 + num5)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
					}
					else if (num8 >= num3 + num4 + num5 && num8 < num3 + num4 + num5 + num6)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
					}
					else if (num8 >= num3 + num4 + num5 + num6 && num8 < num3 + num4 + num5 + num6 + num7)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
					}
					else
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
					}
				}
				else
				{
					spawnTitan(abnormal, position, rotation, punk);
				}
			}
		}
		else if (level.StartsWith("Custom"))
		{
			GameObject[] array2 = GameObject.FindGameObjectsWithTag("titanRespawn");
			List<GameObject> list2 = new List<GameObject>(array2);
			for (int i = 0; i < num; i++)
			{
				Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
				Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
				if (titanSpawns.Count > 0)
				{
					position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
				}
				else if (array2.Length != 0)
				{
					if (list2.Count <= 0)
					{
						list2 = new List<GameObject>(array2);
					}
					int index2 = UnityEngine.Random.Range(0, list2.Count);
					GameObject obj2 = list2[index2];
					position = obj2.transform.position;
					rotation = obj2.transform.rotation;
					list2.RemoveAt(index2);
				}
				spawnTitan(abnormal, position, rotation, punk);
			}
		}
		else
		{
			randomSpawnTitan("titanRespawn", abnormal, num, punk);
		}
	}

	private GameObject spawnTitanRaw(Vector3 position, Quaternion rotation)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			return (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_VER3.1"), position, rotation);
		}
		return PhotonNetwork.Instantiate("TITAN_VER3.1", position, rotation, 0);
	}

	[RPC]
	private void spawnTitanRPC(PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient)
		{
			return;
		}
		foreach (TITAN titan in titans)
		{
			if (titan.photonView.isMine && (!PhotonNetwork.isMasterClient || titan.nonAI))
			{
				PhotonNetwork.Destroy(titan.gameObject);
			}
		}
		SpawnNonAITitan2(myLastHero);
	}

	private void Start()
	{
		instance = this;
		base.gameObject.name = "MultiplayerManager";
		HeroCostume.init2();
		CharacterMaterials.init();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		heroes = new ArrayList();
		eT = new ArrayList();
		titans = new ArrayList();
		fT = new ArrayList();
		cT = new ArrayList();
		hooks = new ArrayList();
		name = string.Empty;
		if (nameField == null)
		{
			nameField = "GUEST" + UnityEngine.Random.Range(0, 100000);
		}
		if (privateServerField == null)
		{
			privateServerField = string.Empty;
		}
		if (privateLobbyField == null)
		{
			privateLobbyField = string.Empty;
		}
		usernameField = string.Empty;
		passwordField = string.Empty;
		resetGameSettings();
		banHash = new ExitGames.Client.Photon.Hashtable();
		imatitan = new ExitGames.Client.Photon.Hashtable();
		oldScript = string.Empty;
		currentLevel = string.Empty;
		titanSpawns = new List<Vector3>();
		playerSpawnsC = new List<Vector3>();
		playerSpawnsM = new List<Vector3>();
		playersRPC = new List<PhotonPlayer>();
		levelCache = new List<string[]>();
		titanSpawners = new List<TitanSpawner>();
		restartCount = new List<float>();
		ignoreList = new List<int>();
		groundList = new List<GameObject>();
		noRestart = false;
		masterRC = false;
		isSpawning = false;
		intVariables = new ExitGames.Client.Photon.Hashtable();
		heroHash = new ExitGames.Client.Photon.Hashtable();
		boolVariables = new ExitGames.Client.Photon.Hashtable();
		stringVariables = new ExitGames.Client.Photon.Hashtable();
		floatVariables = new ExitGames.Client.Photon.Hashtable();
		globalVariables = new ExitGames.Client.Photon.Hashtable();
		RCRegions = new ExitGames.Client.Photon.Hashtable();
		RCEvents = new ExitGames.Client.Photon.Hashtable();
		RCVariableNames = new ExitGames.Client.Photon.Hashtable();
		RCRegionTriggers = new ExitGames.Client.Photon.Hashtable();
		playerVariables = new ExitGames.Client.Photon.Hashtable();
		titanVariables = new ExitGames.Client.Photon.Hashtable();
		logicLoaded = false;
		customLevelLoaded = false;
		oldScriptLogic = string.Empty;
		customMapMaterials = new Dictionary<string, Material>();
		retryTime = 0f;
		playerList = string.Empty;
		updateTime = 0f;
		if (textureBackgroundBlack == null)
		{
			textureBackgroundBlack = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			textureBackgroundBlack.SetPixel(0, 0, new Color(0f, 0f, 0f, 1f));
			textureBackgroundBlack.Apply();
		}
		if (textureBackgroundBlue == null)
		{
			textureBackgroundBlue = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			textureBackgroundBlue.SetPixel(0, 0, new Color(0.08f, 0.3f, 0.4f, 1f));
			textureBackgroundBlue.Apply();
		}
		loadconfig();
		List<string> list = new List<string> { "PanelLogin", "LOGIN", "VERSION", "LabelNetworkStatus" };
		List<string> collection = new List<string> { "AOTTG_HERO", "Colossal", "Icosphere", "Cube", "colossal", "CITY", "city", "rock" };
		if (!SettingsManager.GraphicsSettings.AnimatedIntro.Value)
		{
			list.AddRange(collection);
		}
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			foreach (string item in list)
			{
				if (gameObject.name.Contains(item))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}
	}

	public void titanGetKill(PhotonPlayer player, int Damage, string name)
	{
		Damage = Mathf.Max(10, Damage);
		object[] parameters = new object[1] { Damage };
		base.photonView.RPC("netShowDamage", player, parameters);
		object[] parameters2 = new object[2] { name, false };
		base.photonView.RPC("oneTitanDown", PhotonTargets.MasterClient, parameters2);
		sendKillInfo(t1: false, (string)player.customProperties[PhotonPlayerProperty.name], t2: true, name, Damage);
		playerKillInfoUpdate(player, Damage);
	}

	public void titanGetKillbyServer(int Damage, string name)
	{
		Damage = Mathf.Max(10, Damage);
		sendKillInfo(t1: false, LoginFengKAI.player.name, t2: true, name, Damage);
		netShowDamage(Damage);
		oneTitanDown(name, onPlayerLeave: false);
		playerKillInfoUpdate(PhotonNetwork.player, Damage);
	}

	private void tryKick(KickState tmp)
	{
		sendChatContentInfo("kicking #" + tmp.name + ", " + tmp.getKickCount() + "/" + (int)((float)PhotonNetwork.playerList.Length * 0.5f) + "vote");
		if (tmp.getKickCount() >= (int)((float)PhotonNetwork.playerList.Length * 0.5f))
		{
			kickPhotonPlayer(tmp.name.ToString());
		}
	}

	public void unloadAssets(bool immediate = false)
	{
		if (immediate)
		{
			Resources.UnloadUnusedAssets();
		}
		else if (!isUnloading)
		{
			isUnloading = true;
			StartCoroutine(unloadAssetsE(10f));
		}
	}

	public IEnumerator unloadAssetsE(float time)
	{
		yield return new WaitForSeconds(time);
		Resources.UnloadUnusedAssets();
		isUnloading = false;
	}

	public void unloadAssetsEditor()
	{
		if (!isUnloading)
		{
			isUnloading = true;
			StartCoroutine(unloadAssetsE(30f));
		}
	}

	private void Update()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && GameObject.Find("LabelNetworkStatus") != null)
		{
			GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>().text = PhotonNetwork.connectionStateDetailed.ToString();
			if (PhotonNetwork.connected)
			{
				UILabel component = GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>();
				component.text = component.text + " ping:" + PhotonNetwork.GetPing();
			}
		}
		if (!gameStart)
		{
			return;
		}
		foreach (HERO hero in heroes)
		{
			hero.update2();
		}
		foreach (Bullet hook in hooks)
		{
			hook.update();
		}
		foreach (TITAN_EREN item in eT)
		{
			item.update();
		}
		foreach (TITAN titan in titans)
		{
			titan.update2();
		}
		foreach (FEMALE_TITAN item2 in fT)
		{
			item2.update();
		}
		foreach (COLOSSAL_TITAN item3 in cT)
		{
			item3.update2();
		}
		if (mainCamera != null)
		{
			mainCamera.update2();
		}
	}

	[RPC]
	private void updateKillInfo(bool t1, string killer, bool t2, string victim, int dmg)
	{
		GameObject gameObject = GameObject.Find("UI_IN_GAME");
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/KillInfo"));
		for (int i = 0; i < killInfoGO.Count; i++)
		{
			GameObject gameObject3 = (GameObject)killInfoGO[i];
			if (gameObject3 != null)
			{
				gameObject3.GetComponent<KillInfoComponent>().moveOn();
			}
		}
		if (killInfoGO.Count > 4)
		{
			GameObject gameObject3 = (GameObject)killInfoGO[0];
			if (gameObject3 != null)
			{
				gameObject3.GetComponent<KillInfoComponent>().destory();
			}
			killInfoGO.RemoveAt(0);
		}
		gameObject2.transform.parent = gameObject.GetComponent<UIReferArray>().panels[0].transform;
		gameObject2.GetComponent<KillInfoComponent>().show(t1, killer, t2, victim, dmg);
		killInfoGO.Add(gameObject2);
		ReportKillToChatFeed(killer, victim, dmg);
	}

	public void ReportKillToChatFeed(string killer, string victim, int damage)
	{
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			string text = "<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> " + killer.hexColor() + " killed ";
			string newLine = text + victim.hexColor() + " for " + damage + " damage.";
			chatRoom.addLINE(newLine);
		}
	}

	[RPC]
	public void verifyPlayerHasLeft(int ID, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient && PhotonPlayer.Find(ID) != null)
		{
			PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
			string empty = string.Empty;
			empty = RCextensions.returnStringFromObject(photonPlayer.customProperties[PhotonPlayerProperty.name]);
			banHash.Add(ID, empty);
		}
	}

	public IEnumerator WaitAndRecompilePlayerList(float time)
	{
		yield return new WaitForSeconds(time);
		string text = string.Empty;
		if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0)
		{
			PhotonPlayer[] array = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer in array)
			{
				if (photonPlayer.customProperties[PhotonPlayerProperty.dead] == null)
				{
					continue;
				}
				if (ignoreList.Contains(photonPlayer.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!photonPlayer.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(photonPlayer.ID) + "] ";
				if (photonPlayer.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.team]);
					if (num < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				_ = string.Empty;
				string text3 = RCextensions.returnStringFromObject(photonPlayer.customProperties[PhotonPlayerProperty.name]);
				int num2 = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.kills]);
				int num3 = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.deaths]);
				int num4 = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]);
				int num5 = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]);
				object[] array2 = new object[11]
				{
					text2,
					string.Empty,
					text3,
					"[ffffff]:",
					num2,
					"/",
					num3,
					"/",
					num4,
					"/",
					num5
				};
				text = string.Concat(array2);
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
		}
		else
		{
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			int num13 = 0;
			Dictionary<int, PhotonPlayer> dictionary = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
			PhotonPlayer[] array = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer2 in array)
			{
				if (photonPlayer2.customProperties[PhotonPlayerProperty.dead] != null && !ignoreList.Contains(photonPlayer2.ID))
				{
					switch (RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.RCteam]))
					{
					case 0:
						dictionary3.Add(photonPlayer2.ID, photonPlayer2);
						break;
					case 1:
						dictionary.Add(photonPlayer2.ID, photonPlayer2);
						num6 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.kills]);
						num8 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.deaths]);
						num10 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.max_dmg]);
						num12 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.total_dmg]);
						break;
					case 2:
						dictionary2.Add(photonPlayer2.ID, photonPlayer2);
						num7 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.kills]);
						num9 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.deaths]);
						num11 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.max_dmg]);
						num13 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.total_dmg]);
						break;
					}
				}
			}
			cyanKills = num6;
			magentaKills = num7;
			if (PhotonNetwork.isMasterClient)
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value != 2)
				{
					if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
					{
						array = PhotonNetwork.playerList;
						foreach (PhotonPlayer photonPlayer3 in array)
						{
							int num14 = 0;
							int num15 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.RCteam]);
							if (num15 <= 0)
							{
								continue;
							}
							switch (num15)
							{
							case 1:
							{
								int num17 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.kills]);
								if (num7 + num17 + 7 < num6 - num17)
								{
									num14 = 2;
									num7 += num17;
									num6 -= num17;
								}
								break;
							}
							case 2:
							{
								int num16 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.kills]);
								if (num6 + num16 + 7 < num7 - num16)
								{
									num14 = 1;
									num6 += num16;
									num7 -= num16;
								}
								break;
							}
							}
							if (num14 > 0)
							{
								base.photonView.RPC("setTeamRPC", photonPlayer3, num14);
							}
						}
					}
				}
				else
				{
					array = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer4 in array)
					{
						int num18 = 0;
						if (dictionary.Count > dictionary2.Count + 1)
						{
							num18 = 2;
							if (dictionary.ContainsKey(photonPlayer4.ID))
							{
								dictionary.Remove(photonPlayer4.ID);
							}
							if (!dictionary2.ContainsKey(photonPlayer4.ID))
							{
								dictionary2.Add(photonPlayer4.ID, photonPlayer4);
							}
						}
						else if (dictionary2.Count > dictionary.Count + 1)
						{
							num18 = 1;
							if (!dictionary.ContainsKey(photonPlayer4.ID))
							{
								dictionary.Add(photonPlayer4.ID, photonPlayer4);
							}
							if (dictionary2.ContainsKey(photonPlayer4.ID))
							{
								dictionary2.Remove(photonPlayer4.ID);
							}
						}
						if (num18 > 0)
						{
							base.photonView.RPC("setTeamRPC", photonPlayer4, num18);
						}
					}
				}
			}
			text = text + "[00FFFF]TEAM CYAN" + "[ffffff]:" + cyanKills + "/" + num8 + "/" + num10 + "/" + num12 + "\n";
			foreach (PhotonPlayer value in dictionary.Values)
			{
				int num15 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.RCteam]);
				if (value.customProperties[PhotonPlayerProperty.dead] == null || num15 != 1)
				{
					continue;
				}
				if (ignoreList.Contains(value.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value.ID) + "] ";
				if (value.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.team]);
					if (num < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text4 = text;
				_ = string.Empty;
				string text3 = RCextensions.returnStringFromObject(value.customProperties[PhotonPlayerProperty.name]);
				int num2 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.kills]);
				int num3 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.deaths]);
				int num4 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.max_dmg]);
				int num5 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text4 + string.Empty + text3 + "[ffffff]:" + num2 + "/" + num3 + "/" + num4 + "/" + num5;
				if (RCextensions.returnBoolFromObject(value.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = text + " \n" + "[FF00FF]TEAM MAGENTA" + "[ffffff]:" + magentaKills + "/" + num9 + "/" + num11 + "/" + num13 + "\n";
			foreach (PhotonPlayer value2 in dictionary2.Values)
			{
				int num15 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.RCteam]);
				if (value2.customProperties[PhotonPlayerProperty.dead] == null || num15 != 2)
				{
					continue;
				}
				if (ignoreList.Contains(value2.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value2.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value2.ID) + "] ";
				if (value2.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value2.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.team]);
					if (num < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text4 = text;
				_ = string.Empty;
				string text3 = RCextensions.returnStringFromObject(value2.customProperties[PhotonPlayerProperty.name]);
				int num2 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.kills]);
				int num3 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.deaths]);
				int num4 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.max_dmg]);
				int num5 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text4 + string.Empty + text3 + "[ffffff]:" + num2 + "/" + num3 + "/" + num4 + "/" + num5;
				if (RCextensions.returnBoolFromObject(value2.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = string.Concat(new object[3] { text, " \n", "[00FF00]INDIVIDUAL\n" });
			foreach (PhotonPlayer value3 in dictionary3.Values)
			{
				int num15 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.RCteam]);
				if (value3.customProperties[PhotonPlayerProperty.dead] == null || num15 != 0)
				{
					continue;
				}
				if (ignoreList.Contains(value3.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value3.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value3.ID) + "] ";
				if (value3.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value3.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.team]);
					if (num < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text4 = text;
				_ = string.Empty;
				string text3 = RCextensions.returnStringFromObject(value3.customProperties[PhotonPlayerProperty.name]);
				int num2 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.kills]);
				int num3 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.deaths]);
				int num4 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.max_dmg]);
				int num5 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text4 + string.Empty + text3 + "[ffffff]:" + num2 + "/" + num3 + "/" + num4 + "/" + num5;
				if (RCextensions.returnBoolFromObject(value3.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
		}
		playerList = text;
		if (PhotonNetwork.isMasterClient && !isWinning && !isLosing && roundTime >= 5f)
		{
			if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
			{
				int num19 = 0;
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					PhotonPlayer photonPlayer5 = PhotonNetwork.playerList[j];
					if (ignoreList.Contains(photonPlayer5.ID) || photonPlayer5.customProperties[PhotonPlayerProperty.dead] == null || photonPlayer5.customProperties[PhotonPlayerProperty.isTitan] == null)
					{
						continue;
					}
					if (RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.isTitan]) == 1)
					{
						if (RCextensions.returnBoolFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.deaths]) > 0)
						{
							if (!imatitan.ContainsKey(photonPlayer5.ID))
							{
								imatitan.Add(photonPlayer5.ID, 2);
							}
							ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
							hashtable.Add(PhotonPlayerProperty.isTitan, 2);
							photonPlayer5.SetCustomProperties(hashtable);
							base.photonView.RPC("spawnTitanRPC", photonPlayer5);
						}
						else
						{
							if (!imatitan.ContainsKey(photonPlayer5.ID))
							{
								continue;
							}
							for (int k = 0; k < heroes.Count; k++)
							{
								HERO hERO = (HERO)heroes[k];
								if (hERO.photonView.owner == photonPlayer5)
								{
									hERO.markDie();
									hERO.photonView.RPC("netDie2", PhotonTargets.All, -1, "no switching in infection");
								}
							}
						}
					}
					else if (RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.isTitan]) == 2 && !RCextensions.returnBoolFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.dead]))
					{
						num19++;
					}
				}
				if (num19 <= 0 && IN_GAME_MAIN_CAMERA.gamemode != 0)
				{
					gameWin2();
				}
			}
			else if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
				{
					if (cyanKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
					{
						object[] parameters = new object[2]
						{
							"<color=#00FFFF>Team Cyan wins! </color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters);
						gameWin2();
					}
					else if (magentaKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
					{
						object[] array2 = new object[2]
						{
							"<color=#FF00FF>Team Magenta wins! </color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, array2);
						gameWin2();
					}
				}
				else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0)
				{
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer6 = PhotonNetwork.playerList[j];
						if (RCextensions.returnIntFromObject(photonPlayer6.customProperties[PhotonPlayerProperty.kills]) >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
						{
							object[] parameters2 = new object[2]
							{
								"<color=#FFCC00>" + RCextensions.returnStringFromObject(photonPlayer6.customProperties[PhotonPlayerProperty.name]).hexColor() + " wins!</color>",
								string.Empty
							};
							base.photonView.RPC("Chat", PhotonTargets.All, parameters2);
							gameWin2();
						}
					}
				}
			}
			else if (!SettingsManager.LegacyGameSettings.PointModeEnabled.Value && (SettingsManager.LegacyGameSettings.BombModeEnabled.Value || SettingsManager.LegacyGameSettings.BladePVP.Value > 0))
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.playerList.Length > 1)
				{
					int num20 = 0;
					int num21 = 0;
					int num22 = 0;
					int num23 = 0;
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer7 = PhotonNetwork.playerList[j];
						if (ignoreList.Contains(photonPlayer7.ID) || photonPlayer7.customProperties[PhotonPlayerProperty.RCteam] == null || photonPlayer7.customProperties[PhotonPlayerProperty.dead] == null)
						{
							continue;
						}
						if (RCextensions.returnIntFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.RCteam]) == 1)
						{
							num22++;
							if (!RCextensions.returnBoolFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.dead]))
							{
								num20++;
							}
						}
						else if (RCextensions.returnIntFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.RCteam]) == 2)
						{
							num23++;
							if (!RCextensions.returnBoolFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.dead]))
							{
								num21++;
							}
						}
					}
					if (num22 > 0 && num23 > 0)
					{
						if (num20 == 0)
						{
							object[] parameters3 = new object[2]
							{
								"<color=#FF00FF>Team Magenta wins! </color>",
								string.Empty
							};
							base.photonView.RPC("Chat", PhotonTargets.All, parameters3);
							gameWin2();
						}
						else if (num21 == 0)
						{
							object[] parameters4 = new object[2]
							{
								"<color=#00FFFF>Team Cyan wins! </color>",
								string.Empty
							};
							base.photonView.RPC("Chat", PhotonTargets.All, parameters4);
							gameWin2();
						}
					}
				}
				else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0 && PhotonNetwork.playerList.Length > 1)
				{
					int num24 = 0;
					string text5 = "Nobody";
					PhotonPlayer player = PhotonNetwork.playerList[0];
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer8 = PhotonNetwork.playerList[j];
						if (photonPlayer8.customProperties[PhotonPlayerProperty.dead] != null && !RCextensions.returnBoolFromObject(photonPlayer8.customProperties[PhotonPlayerProperty.dead]))
						{
							text5 = RCextensions.returnStringFromObject(photonPlayer8.customProperties[PhotonPlayerProperty.name]).hexColor();
							player = photonPlayer8;
							num24++;
						}
					}
					if (num24 <= 1)
					{
						string text6 = " 5 points added.";
						if (text5 == "Nobody")
						{
							text6 = string.Empty;
						}
						else
						{
							for (int j = 0; j < 5; j++)
							{
								playerKillInfoUpdate(player, 0);
							}
						}
						object[] parameters5 = new object[2]
						{
							"<color=#FFCC00>" + text5.hexColor() + " wins." + text6 + "</color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters5);
						gameWin2();
					}
				}
			}
		}
		isRecompiling = false;
	}

	public IEnumerator WaitAndReloadKDR(PhotonPlayer player)
	{
		yield return new WaitForSeconds(5f);
		string key = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
		if (PreservedPlayerKDR.ContainsKey(key))
		{
			int[] array = PreservedPlayerKDR[key];
			PreservedPlayerKDR.Remove(key);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.kills, array[0]);
			hashtable.Add(PhotonPlayerProperty.deaths, array[1]);
			hashtable.Add(PhotonPlayerProperty.max_dmg, array[2]);
			hashtable.Add(PhotonPlayerProperty.total_dmg, array[3]);
			player.SetCustomProperties(hashtable);
		}
	}

	public IEnumerator WaitAndResetRestarts()
	{
		yield return new WaitForSeconds(10f);
		restartingBomb = false;
		restartingEren = false;
		restartingHorse = false;
		restartingMC = false;
		restartingTitan = false;
	}

	public IEnumerator WaitAndRespawn1(float time, string str)
	{
		yield return new WaitForSeconds(time);
		SpawnPlayer(myLastHero, str);
	}

	public IEnumerator WaitAndRespawn2(float time, GameObject pos)
	{
		yield return new WaitForSeconds(time);
		SpawnPlayerAt2(myLastHero, pos);
	}
}
