public enum OpCode
{
	KEEP_ALIVE,
	WELCOME,
	WRITE_LOG,
	//BLOCK_HOTKEY,
	SCREENSHOT,

	//TabUpdates
	TAB_REFRESH,
	TAB_INTERACTABLES,

	//Preview Menu
	REQUEST_PREVIEW_TEXTURE,
	SEND_PREVIEW_TEXTURE,

	//Game Tab
	NEXT_TITLE,
	MAIN_TITLE,
	MAIN_TITLE_STATE_BUTTON,
	TEAMS_TITLE,
	RIGHT_ANSWER,
	WRONG_ANSWER,
	START_COUNTDOWN,
	OPTION,
	FLASH_LIGHT,
	FIREWORKS,
	FORCE_ON_PRIMITIVES,
	RESTART_PRIMITIVES,
	MONETARY_UNIT,

	//Teams Tab
	REFRESH_TEAMS_DROPDOWN,
	REPLACE_CURRENT_TEAM,
	CHANGE_TEAM_NAME,
	ADD_MONEY_TO_TEAM,

	//Player Tab
	PLAY_UNTIL_MARK,
	PLAY_AFTER_MARK,
	PLAY_FULL,
	PLAYER_PAUSE,
	PLAYER_LOOP,
	DISPLAY_PLAYER_PLAYBACK,
	DISPLAY_PAUSE_MARK,
	SET_PAUSE_MARK,

	//Game Display Tab
	SET_GAME_RESOLUTION,
	GAME_FULLSCREEN,
	GAME_DISPLAY_INFO,
	//REFRESH_DISPLAY_INFO,
	
	//Properties Tab
	REFRESH_PALETTES,
	COLOR_PALETTES,
	CHANGE_PALETTE,
	CHANGE_COLOR_IN_PALETTE,

	GAME_TEXTS,
	CHANGED_GAME_TEXT,

	REFRESH_LOGO,
	USER_LOADED_LOGO,
	USER_DELETED_LOGO,

	SET_AUDIO_CHANNEL,
	AUDIO_CHANNEL_DOUBLE_CLICKED,
	UPDATE_ALL_AUDIO_CHANNELS,

	//NEED_REFRESH_GAME_TEXT,
	//NEED_REFRESH_MONETARY_UNITS,
	//REFRESH_MONETARY_UNITS,

	INTERACTABLE_REPLACE_CURRENT_TEAM,
	INTERACTABLE_ADD_MONEY_TO_TEAM,
	INTERACTABLE_CHANGE_NAME,
	//INTERACTABLE
	//INTERACTABLE_NEXT_BUTTON,
	//INTERACTABLE_ANSWER_BUTTONS,
	//INTERACTABLE_COUNTDOWN_BUTTON,

	ENABLE_OPTIONS_BUTTONS,
	DISABLE_OPTIONS_BUTTONS,
	ENABLE_PLAYER_BUTTONS,
	DISABLE_PLAYER_BUTTONS
}
