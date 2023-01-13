using System;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public static class NetUtility
{
	public static void OnData(DataStreamReader stream, NetworkConnection networkConnection, Server server = null)
	{
		NetMessage msg = null;

		var opCode = (OpCode)stream.ReadByte();

		switch (opCode)
		{
			case OpCode.KEEP_ALIVE:
				msg = new NetKeepAlive(stream);
				break;
			case OpCode.WELCOME:
				msg = new NetWelcome(stream);
				break;
			case OpCode.WRITE_LOG:
				msg = new NetWriteLog(stream);
				break;
			case OpCode.SCREENSHOT:
				msg = new NetScreenshot(stream);
				break;
			case OpCode.NEXT_TITLE:
				msg = new NetNextTitle(stream);
				break;
			case OpCode.MAIN_TITLE:
				msg = new NetMainTitle(stream);
				break;
			case OpCode.MAIN_TITLE_STATE_BUTTON:
				msg = new NetMainTitleStateButton(stream);
				break;
			case OpCode.TEAMS_TITLE:
				msg = new NetTeamsTitle(stream);
				break;
			case OpCode.RIGHT_ANSWER:
				msg = new NetRightAnswer(stream);
				break;
			case OpCode.WRONG_ANSWER:
				msg = new NetWrongAnswer(stream);
				break;
			case OpCode.FLASH_LIGHT:
				msg = new NetFlashLight(stream);
				break;
			case OpCode.FIREWORKS:
				msg = new NetFireworks(stream);
				break;
			case OpCode.FORCE_ON_PRIMITIVES:
				msg = new NetForceOnPrimitives(stream);
				break;
			case OpCode.RESTART_PRIMITIVES:
				msg = new NetRestartPrimitives(stream);
				break;
			case OpCode.START_COUNTDOWN:
				msg = new NetStartCountdown(stream);
				break;
			case OpCode.OPTION:
				msg = new NetOption(stream);
				break;
			case OpCode.MONETARY_UNIT:
				msg = new NetMonetaryUnit(stream);
				break;
			case OpCode.INTERACTABLE_REPLACE_CURRENT_TEAM:
				msg = new NetInteractableReplaceCurrentTeam(stream);
				break;
			case OpCode.REFRESH_TEAMS_DROPDOWN:
				msg = new NetRefreshTeamsDropdown(stream);
				break;
			case OpCode.REPLACE_CURRENT_TEAM:
				msg = new NetReplaceCurrentTeam(stream);
				break;
			case OpCode.CHANGE_TEAM_NAME:
				msg = new NetChangeTeamName(stream);
				break;
			case OpCode.ADD_MONEY_TO_TEAM:
				msg = new NetAddMoneyToTeam(stream);
				break;
			case OpCode.PLAY_UNTIL_MARK:
				msg = new NetPlayUntilMark(stream);
				break;
			case OpCode.PLAY_AFTER_MARK:
				msg = new NetPlayAfterMark(stream);
				break;
			case OpCode.PLAY_FULL:
				msg = new NetPlayFull(stream);
				break;
			case OpCode.PLAYER_PAUSE:
				msg = new NetPlayerPause(stream);
				break;
			case OpCode.PLAYER_LOOP:
				msg = new NetPlayerLoop(stream);
				break;
			case OpCode.DISPLAY_PLAYER_PLAYBACK:
				msg = new NetDisplayPlayerPlayback(stream);
				break;
			case OpCode.DISPLAY_PAUSE_MARK:
				msg = new NetDisplayPauseMark(stream);
				break;
			case OpCode.SET_PAUSE_MARK:
				msg = new NetSetPauseMark(stream);
				break;
			case OpCode.SET_GAME_RESOLUTION:
				msg = new NetSetGameDisplayResolution(stream);
				break;
			case OpCode.GAME_FULLSCREEN:
				msg = new NetGameDisplayFullscreen(stream);
				break;
			case OpCode.GAME_DISPLAY_INFO:
				msg = new NetGameDisplayInfo(stream);
				break;
			case OpCode.REFRESH_PALETTES:
				msg = new NetRefreshPalettes(stream);
				break;
			case OpCode.COLOR_PALETTES:
				msg = new NetColorPalettes(stream);
				break;
			case OpCode.CHANGE_PALETTE:
				msg = new NetChangePalette(stream);
				break;
			case OpCode.CHANGE_COLOR_IN_PALETTE:
				msg = new NetChangeColorInPalette(stream);
				break;
			case OpCode.SET_AUDIO_CHANNEL:
				msg = new NetSetAudioChannel(stream);
				break;
			case OpCode.AUDIO_CHANNEL_DOUBLE_CLICKED:
				msg = new NetDoubleClickAudioChannel(stream);
				break;
			case OpCode.UPDATE_ALL_AUDIO_CHANNELS:
				msg = new NetUpdateAllAudioChannels(stream);
				break;
			case OpCode.GAME_TEXTS:
				msg = new NetGameTextsWindow(stream);
				break;
			case OpCode.CHANGED_GAME_TEXT:
				msg = new NetChangedGameTexts(stream);
				break;	
			case OpCode.REFRESH_LOGO:
				msg = new NetRefreshLogo(stream);
				break;
			case OpCode.USER_LOADED_LOGO:
				msg = new NetUserLoadedLogo(stream);
				break;
			case OpCode.USER_DELETED_LOGO:
				msg = new NetUserDeletedLogo(stream);
				break;
			case OpCode.REQUEST_PREVIEW_TEXTURE:
				msg = new NetRequestPreviewTexture(stream);
				break;
			case OpCode.SEND_PREVIEW_TEXTURE:
				msg = new NetSendPreviewTexture(stream);
				break;
			case OpCode.TAB_REFRESH:
				msg = new NetTabRefresh(stream);
				break;
			case OpCode.TAB_INTERACTABLES:
				msg = new NetTabInteractables(stream);
				break;
			default:
				Debug.LogError("Message received had no OpCode");
				break;
		}

		if (server != null)
			msg.RecievedOnServer(networkConnection);
		else
			msg.RecievedOnClient();
	}

	//Net messages
	public static Action<NetMessage> C_KEEP_ALIVE;
	public static Action<NetMessage> C_WELCOME;
	public static Action<NetMessage> C_WRITE_LOG;
	public static Action<NetMessage> C_NEXT_TITLE;
	public static Action<NetMessage> C_SCREENSHOT;
	public static Action<NetMessage> C_MAIN_TITLE;
	public static Action<NetMessage> C_MAIN_TITLE_STATE_BUTTON;
	public static Action<NetMessage> C_TEAM_TITLE;
	public static Action<NetMessage> C_RIGHT_ANSWER;
	public static Action<NetMessage> C_WRONG_ANSWER;
	public static Action<NetMessage> C_FLASH_LIGHT;
	public static Action<NetMessage> C_FIREWORKS;
	public static Action<NetMessage> C_FORCE_ON_PRIMITIVES;
	public static Action<NetMessage> C_RESTART_PRIMITIVES;
	public static Action<NetMessage> C_START_COUNTDOWN;
	public static Action<NetMessage> C_OPTION;
	public static Action<NetMessage> C_MONETARY_UNIT;
	public static Action<NetMessage> C_INTERACTABLE_REPLACE_CURRENT_TEAM;
	public static Action<NetMessage> C_REFRESH_TEAMS_DROPDOWN;
	public static Action<NetMessage> C_REPLACE_CURRENT_TEAM;
	public static Action<NetMessage> C_CHANGE_TEAM_NAME;
	public static Action<NetMessage> C_ADD_MONEY_TO_TEAM;
	public static Action<NetMessage> C_PLAY_UNTIL_MARK;
	public static Action<NetMessage> C_PLAY_AFTER_MARK;
	public static Action<NetMessage> C_PLAY_FULL;
	public static Action<NetMessage> C_PLAYER_PAUSE;
	public static Action<NetMessage> C_PLAYER_LOOP;
	public static Action<NetMessage> C_DISPLAY_PLAYER_PLAYBACK;
	public static Action<NetMessage> C_DISPLAY_PAUSE_MARK;
	public static Action<NetMessage> C_SET_PAUSE_MARK;
	public static Action<NetMessage> C_SET_GAME_RESOLUTION;
	public static Action<NetMessage> C_GAME_FULLSCREEN;
	public static Action<NetMessage> C_GAME_DISPLAY_INFO;
	public static Action<NetMessage> C_REFRESH_PALETTES;
	public static Action<NetMessage> C_COLOR_PALETTES;
	public static Action<NetMessage> C_CHANGE_PALETTE;
	public static Action<NetMessage> C_CHANGE_COLOR_IN_PALETTE;
	public static Action<NetMessage> C_SET_AUDIO_CHANNEL;
	public static Action<NetMessage> C_AUDIO_CHANNEL_DOUBLE_CLICKED;
	public static Action<NetMessage> C_UPDATE_ALL_AUDIO_CHANNELS;
	public static Action<NetMessage> C_GAME_TEXTS;
	public static Action<NetMessage> C_CHANGED_GAME_TEXTS;
	public static Action<NetMessage> C_REFRESH_LOGO;
	public static Action<NetMessage> C_USER_LOADED_LOGO;
	public static Action<NetMessage> C_USER_DELETED_LOGO;
	public static Action<NetMessage> C_REQUEST_PREVIEW_TEXTURE;
	public static Action<NetMessage> C_SEND_PREVIEW_TEXTURE;
	public static Action<NetMessage> C_TAB_REFRESH;
	public static Action<NetMessage> C_TAB_INTERACTABLES;

	public static Action<NetMessage, NetworkConnection> S_KEEP_ALIVE;
	public static Action<NetMessage, NetworkConnection> S_WELCOME;
	public static Action<NetMessage, NetworkConnection> S_WRITE_LOG;
	public static Action<NetMessage, NetworkConnection> S_NEXT_TITLE;
	public static Action<NetMessage, NetworkConnection> S_SCREENSHOT;
	public static Action<NetMessage, NetworkConnection> S_MAIN_TITLE;
	public static Action<NetMessage, NetworkConnection> S_MAIN_TITLE_STATE_BUTTON;
	public static Action<NetMessage, NetworkConnection> S_TEAM_TITLE;
	public static Action<NetMessage, NetworkConnection> S_RIGHT_ANSWER;
	public static Action<NetMessage, NetworkConnection> S_WRONG_ANSWER;
	public static Action<NetMessage, NetworkConnection> S_FLASH_LIGHT;
	public static Action<NetMessage, NetworkConnection> S_FIREWORKS;
	public static Action<NetMessage, NetworkConnection> S_START_COUNTDOWN;
	public static Action<NetMessage, NetworkConnection> S_FORCE_ON_PRIMITIVES;
	public static Action<NetMessage, NetworkConnection> S_RESTART_PRIMITIVES;
	public static Action<NetMessage, NetworkConnection> S_OPTION;
	public static Action<NetMessage, NetworkConnection> S_MONETARY_UNIT;
	public static Action<NetMessage, NetworkConnection> S_INTERACTABLE_REPLACE_CURRENT_TEAM;
	public static Action<NetMessage, NetworkConnection> S_REFRESH_TEAMS_DROPDOWN;
	public static Action<NetMessage, NetworkConnection> S_REPLACE_CURRENT_TEAM;
	public static Action<NetMessage, NetworkConnection> S_CHANGE_TEAM_NAME;
	public static Action<NetMessage, NetworkConnection> S_ADD_MONEY_TO_TEAM;
	public static Action<NetMessage, NetworkConnection> S_PLAY_UNTIL_MARK;
	public static Action<NetMessage, NetworkConnection> S_PLAY_AFTER_MARK;
	public static Action<NetMessage, NetworkConnection> S_PLAY_FULL;
	public static Action<NetMessage, NetworkConnection> S_PLAYER_PAUSE;
	public static Action<NetMessage, NetworkConnection> S_PLAYER_LOOP;
	public static Action<NetMessage, NetworkConnection> S_DISPLAY_PLAYER_PLAYBACK;
	public static Action<NetMessage, NetworkConnection> S_DISPLAY_PAUSE_MARK;
	public static Action<NetMessage, NetworkConnection> S_SET_PAUSE_MARK;
	public static Action<NetMessage, NetworkConnection> S_SET_GAME_RESOLUTION;
	public static Action<NetMessage, NetworkConnection> S_GAME_FULLSCREEN;
	public static Action<NetMessage, NetworkConnection> S_GAME_DISPLAY_INFO;
	public static Action<NetMessage, NetworkConnection> S_REFRESH_PALETTES;
	public static Action<NetMessage, NetworkConnection> S_COLOR_PALETTES;
	public static Action<NetMessage, NetworkConnection> S_CHANGE_PALETTE;
	public static Action<NetMessage, NetworkConnection> S_CHANGE_COLOR_IN_PALETTE;
	public static Action<NetMessage, NetworkConnection> S_SET_AUDIO_CHANNEL;
	public static Action<NetMessage, NetworkConnection> S_AUDIO_CHANNEL_DOUBLE_CLICKED;
	public static Action<NetMessage, NetworkConnection> S_UPDATE_ALL_AUDIO_CHANNELS;
	public static Action<NetMessage, NetworkConnection> S_GAME_TEXTS;
	public static Action<NetMessage, NetworkConnection> S_CHANGED_GAME_TEXTS;
	public static Action<NetMessage, NetworkConnection> S_REFRESH_LOGO;
	public static Action<NetMessage, NetworkConnection> S_USER_LOADED_LOGO;
	public static Action<NetMessage, NetworkConnection> S_USER_DELETED_LOGO;
	public static Action<NetMessage, NetworkConnection> S_REQUEST_PREVIEW_TEXTURE;
	public static Action<NetMessage, NetworkConnection> S_SEND_PREVIEW_TEXTURE;
	public static Action<NetMessage, NetworkConnection> S_TAB_REFRESH;
	public static Action<NetMessage, NetworkConnection> S_TAB_INTERACTABLES;
}