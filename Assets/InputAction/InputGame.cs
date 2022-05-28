// GENERATED AUTOMATICALLY FROM 'Assets/InputAction/InputGame.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputGame : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputGame()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputGame"",
    ""maps"": [
        {
            ""name"": ""Game"",
            ""id"": ""9b574be7-e62c-4cf2-bfbf-0e317dac9a69"",
            ""actions"": [
                {
                    ""name"": ""NextTitle"",
                    ""type"": ""Button"",
                    ""id"": ""b80138b6-ad24-4f11-b8ca-547c3eba0c6a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MarkDone"",
                    ""type"": ""Button"",
                    ""id"": ""8892888d-5f86-4914-80f0-090750be6d68"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MarkNotDone"",
                    ""type"": ""Button"",
                    ""id"": ""6b1028a4-a493-4eb2-b64c-553eda7556f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""df1dffbc-43a1-426d-8062-5044e8495047"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Option1"",
                    ""type"": ""Button"",
                    ""id"": ""176e5bbc-ceac-4eaf-bde3-c62030627f3a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Option2"",
                    ""type"": ""Button"",
                    ""id"": ""b24dc47c-23d9-486c-bc56-66740ddf7c85"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Option3"",
                    ""type"": ""Button"",
                    ""id"": ""833479f0-315c-4047-be50-8873086cef82"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Option4"",
                    ""type"": ""Button"",
                    ""id"": ""6695db2e-0e3d-4014-b999-faab94949224"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Option5"",
                    ""type"": ""Button"",
                    ""id"": ""b9554edd-f1de-46c5-a4a3-2483740feff8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fireworks"",
                    ""type"": ""Button"",
                    ""id"": ""cce9d752-3af3-435e-98f5-5e46d0fbb853"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Flash"",
                    ""type"": ""Button"",
                    ""id"": ""bdb7f036-8846-43f8-aeec-4bf090601d14"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TeamsTitle"",
                    ""type"": ""Button"",
                    ""id"": ""57a98669-f94c-42bd-a63a-fc2bd7971cfb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayUntilPauseMark"",
                    ""type"": ""Button"",
                    ""id"": ""3db4dda2-6817-4542-81a9-d6e088abff1e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayAfterPauseMark"",
                    ""type"": ""Button"",
                    ""id"": ""f8d9cc04-94f4-42ec-87c1-dabff8524bf1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayFull"",
                    ""type"": ""Button"",
                    ""id"": ""f4ccfcc5-10ce-41a1-a2ff-27ba575cbaf6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerPause"",
                    ""type"": ""Button"",
                    ""id"": ""c5a1431f-79a2-4aaf-a662-df5c0fbaec96"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerLoop"",
                    ""type"": ""Button"",
                    ""id"": ""adc6eae5-07c7-45aa-9a8c-895499f65bb8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""6a1fa82e-50d0-4e3c-9029-84d1411037f9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fullscreen"",
                    ""type"": ""Button"",
                    ""id"": ""5bac81a3-093a-4d08-ac15-0f517757b8fd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StartCountdown"",
                    ""type"": ""Button"",
                    ""id"": ""51ad0583-2341-478a-9a2f-c3358592b5e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchVisibleBGElements"",
                    ""type"": ""Button"",
                    ""id"": ""679a7fa2-1605-46df-a56c-df0aed3eab37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlusFieldOfView"",
                    ""type"": ""Button"",
                    ""id"": ""5df3342a-1f0f-49b7-9614-3429ea768a2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MinusFieldOfView"",
                    ""type"": ""Button"",
                    ""id"": ""bb9b0ed8-c99f-4a1a-a4e9-c01c37524bd0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""567e09e4-69aa-46a7-9a89-5040d1c98300"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextTitle"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f0a128c4-2555-4e82-9f14-379db8d0335d"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""NextTitle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""810f73ed-ee98-4313-a4e5-2396cbeedbf7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""NextTitle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8479eb1f-13aa-4928-8d9f-939627adda16"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MarkDone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48385c5a-cc7a-4eee-9aea-77dad5397ecb"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8cbd3ae4-0244-4f13-a0f0-b6a6e33eaa8c"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MarkNotDone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68969426-8a12-4487-8289-c0e537668b54"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Option1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8e006ff-e228-4086-88db-e21ac7ee392b"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Option2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""674a8e02-6db2-4ee7-8c1c-db0f37a7bf7f"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Option3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d79ff5d-5bb5-4ff7-b204-d22da8584add"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Option4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5df1f043-0f2b-4e64-8d3a-b45c18d9189e"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Fireworks"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75661983-5ed9-481c-849b-f8e110cd115d"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TeamsTitle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""800f5d79-064e-46e3-a020-dc6982b3ebe0"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Option5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5becd01e-e46e-4088-84c9-b1956f71b854"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayUntilPauseMark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f6d8c72-25f2-4a19-8f5a-96ceaee7f73c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayAfterPauseMark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9fc1718c-8f0f-4c9a-9ea5-d86709e1f2ad"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayFull"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44741acb-9c54-47f2-9d47-8e01d3edc89f"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerPause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5af344ef-14ea-4e6c-b450-5aff8d4b2afc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlayerLoop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21ad779a-8320-4a9f-a6c5-972236e9ab0c"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96842752-88fc-4bb9-be72-739c4fc6fbb9"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Fullscreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a3d1ce5-3a20-4e75-be54-665e3ad8f296"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""StartCountdown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b255d3b-c97f-49e5-904c-d630e81d9031"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SwitchVisibleBGElements"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e02871bc-8d62-4085-93c6-ec794f5bf239"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Flash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f6f206b-7644-477e-aa70-90219ad5072f"",
                    ""path"": ""<Keyboard>/numpadPlus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PlusFieldOfView"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36dfa04d-9729-4e7e-a817-908a61dfc90c"",
                    ""path"": ""<Keyboard>/numpadMinus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MinusFieldOfView"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_NextTitle = m_Game.FindAction("NextTitle", throwIfNotFound: true);
        m_Game_MarkDone = m_Game.FindAction("MarkDone", throwIfNotFound: true);
        m_Game_MarkNotDone = m_Game.FindAction("MarkNotDone", throwIfNotFound: true);
        m_Game_Pause = m_Game.FindAction("Pause", throwIfNotFound: true);
        m_Game_Option1 = m_Game.FindAction("Option1", throwIfNotFound: true);
        m_Game_Option2 = m_Game.FindAction("Option2", throwIfNotFound: true);
        m_Game_Option3 = m_Game.FindAction("Option3", throwIfNotFound: true);
        m_Game_Option4 = m_Game.FindAction("Option4", throwIfNotFound: true);
        m_Game_Option5 = m_Game.FindAction("Option5", throwIfNotFound: true);
        m_Game_Fireworks = m_Game.FindAction("Fireworks", throwIfNotFound: true);
        m_Game_Flash = m_Game.FindAction("Flash", throwIfNotFound: true);
        m_Game_TeamsTitle = m_Game.FindAction("TeamsTitle", throwIfNotFound: true);
        m_Game_PlayUntilPauseMark = m_Game.FindAction("PlayUntilPauseMark", throwIfNotFound: true);
        m_Game_PlayAfterPauseMark = m_Game.FindAction("PlayAfterPauseMark", throwIfNotFound: true);
        m_Game_PlayFull = m_Game.FindAction("PlayFull", throwIfNotFound: true);
        m_Game_PlayerPause = m_Game.FindAction("PlayerPause", throwIfNotFound: true);
        m_Game_PlayerLoop = m_Game.FindAction("PlayerLoop", throwIfNotFound: true);
        m_Game_Menu = m_Game.FindAction("Menu", throwIfNotFound: true);
        m_Game_Fullscreen = m_Game.FindAction("Fullscreen", throwIfNotFound: true);
        m_Game_StartCountdown = m_Game.FindAction("StartCountdown", throwIfNotFound: true);
        m_Game_SwitchVisibleBGElements = m_Game.FindAction("SwitchVisibleBGElements", throwIfNotFound: true);
        m_Game_PlusFieldOfView = m_Game.FindAction("PlusFieldOfView", throwIfNotFound: true);
        m_Game_MinusFieldOfView = m_Game.FindAction("MinusFieldOfView", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Game
    private readonly InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private readonly InputAction m_Game_NextTitle;
    private readonly InputAction m_Game_MarkDone;
    private readonly InputAction m_Game_MarkNotDone;
    private readonly InputAction m_Game_Pause;
    private readonly InputAction m_Game_Option1;
    private readonly InputAction m_Game_Option2;
    private readonly InputAction m_Game_Option3;
    private readonly InputAction m_Game_Option4;
    private readonly InputAction m_Game_Option5;
    private readonly InputAction m_Game_Fireworks;
    private readonly InputAction m_Game_Flash;
    private readonly InputAction m_Game_TeamsTitle;
    private readonly InputAction m_Game_PlayUntilPauseMark;
    private readonly InputAction m_Game_PlayAfterPauseMark;
    private readonly InputAction m_Game_PlayFull;
    private readonly InputAction m_Game_PlayerPause;
    private readonly InputAction m_Game_PlayerLoop;
    private readonly InputAction m_Game_Menu;
    private readonly InputAction m_Game_Fullscreen;
    private readonly InputAction m_Game_StartCountdown;
    private readonly InputAction m_Game_SwitchVisibleBGElements;
    private readonly InputAction m_Game_PlusFieldOfView;
    private readonly InputAction m_Game_MinusFieldOfView;
    public struct GameActions
    {
        private @InputGame m_Wrapper;
        public GameActions(@InputGame wrapper) { m_Wrapper = wrapper; }
        public InputAction @NextTitle => m_Wrapper.m_Game_NextTitle;
        public InputAction @MarkDone => m_Wrapper.m_Game_MarkDone;
        public InputAction @MarkNotDone => m_Wrapper.m_Game_MarkNotDone;
        public InputAction @Pause => m_Wrapper.m_Game_Pause;
        public InputAction @Option1 => m_Wrapper.m_Game_Option1;
        public InputAction @Option2 => m_Wrapper.m_Game_Option2;
        public InputAction @Option3 => m_Wrapper.m_Game_Option3;
        public InputAction @Option4 => m_Wrapper.m_Game_Option4;
        public InputAction @Option5 => m_Wrapper.m_Game_Option5;
        public InputAction @Fireworks => m_Wrapper.m_Game_Fireworks;
        public InputAction @Flash => m_Wrapper.m_Game_Flash;
        public InputAction @TeamsTitle => m_Wrapper.m_Game_TeamsTitle;
        public InputAction @PlayUntilPauseMark => m_Wrapper.m_Game_PlayUntilPauseMark;
        public InputAction @PlayAfterPauseMark => m_Wrapper.m_Game_PlayAfterPauseMark;
        public InputAction @PlayFull => m_Wrapper.m_Game_PlayFull;
        public InputAction @PlayerPause => m_Wrapper.m_Game_PlayerPause;
        public InputAction @PlayerLoop => m_Wrapper.m_Game_PlayerLoop;
        public InputAction @Menu => m_Wrapper.m_Game_Menu;
        public InputAction @Fullscreen => m_Wrapper.m_Game_Fullscreen;
        public InputAction @StartCountdown => m_Wrapper.m_Game_StartCountdown;
        public InputAction @SwitchVisibleBGElements => m_Wrapper.m_Game_SwitchVisibleBGElements;
        public InputAction @PlusFieldOfView => m_Wrapper.m_Game_PlusFieldOfView;
        public InputAction @MinusFieldOfView => m_Wrapper.m_Game_MinusFieldOfView;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @NextTitle.started -= m_Wrapper.m_GameActionsCallbackInterface.OnNextTitle;
                @NextTitle.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnNextTitle;
                @NextTitle.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnNextTitle;
                @MarkDone.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMarkDone;
                @MarkDone.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMarkDone;
                @MarkDone.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnMarkDone;
                @MarkNotDone.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMarkNotDone;
                @MarkNotDone.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMarkNotDone;
                @MarkNotDone.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnMarkNotDone;
                @Pause.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPause;
                @Option1.started -= m_Wrapper.m_GameActionsCallbackInterface.OnOption1;
                @Option1.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnOption1;
                @Option1.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnOption1;
                @Option2.started -= m_Wrapper.m_GameActionsCallbackInterface.OnOption2;
                @Option2.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnOption2;
                @Option2.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnOption2;
                @Option3.started -= m_Wrapper.m_GameActionsCallbackInterface.OnOption3;
                @Option3.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnOption3;
                @Option3.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnOption3;
                @Option4.started -= m_Wrapper.m_GameActionsCallbackInterface.OnOption4;
                @Option4.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnOption4;
                @Option4.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnOption4;
                @Option5.started -= m_Wrapper.m_GameActionsCallbackInterface.OnOption5;
                @Option5.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnOption5;
                @Option5.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnOption5;
                @Fireworks.started -= m_Wrapper.m_GameActionsCallbackInterface.OnFireworks;
                @Fireworks.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnFireworks;
                @Fireworks.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnFireworks;
                @Flash.started -= m_Wrapper.m_GameActionsCallbackInterface.OnFlash;
                @Flash.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnFlash;
                @Flash.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnFlash;
                @TeamsTitle.started -= m_Wrapper.m_GameActionsCallbackInterface.OnTeamsTitle;
                @TeamsTitle.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnTeamsTitle;
                @TeamsTitle.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnTeamsTitle;
                @PlayUntilPauseMark.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayUntilPauseMark;
                @PlayUntilPauseMark.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayUntilPauseMark;
                @PlayUntilPauseMark.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayUntilPauseMark;
                @PlayAfterPauseMark.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayAfterPauseMark;
                @PlayAfterPauseMark.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayAfterPauseMark;
                @PlayAfterPauseMark.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayAfterPauseMark;
                @PlayFull.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayFull;
                @PlayFull.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayFull;
                @PlayFull.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayFull;
                @PlayerPause.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayerPause;
                @PlayerPause.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayerPause;
                @PlayerPause.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayerPause;
                @PlayerLoop.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayerLoop;
                @PlayerLoop.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayerLoop;
                @PlayerLoop.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPlayerLoop;
                @Menu.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnMenu;
                @Fullscreen.started -= m_Wrapper.m_GameActionsCallbackInterface.OnFullscreen;
                @Fullscreen.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnFullscreen;
                @Fullscreen.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnFullscreen;
                @StartCountdown.started -= m_Wrapper.m_GameActionsCallbackInterface.OnStartCountdown;
                @StartCountdown.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnStartCountdown;
                @StartCountdown.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnStartCountdown;
                @SwitchVisibleBGElements.started -= m_Wrapper.m_GameActionsCallbackInterface.OnSwitchVisibleBGElements;
                @SwitchVisibleBGElements.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnSwitchVisibleBGElements;
                @SwitchVisibleBGElements.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnSwitchVisibleBGElements;
                @PlusFieldOfView.started -= m_Wrapper.m_GameActionsCallbackInterface.OnPlusFieldOfView;
                @PlusFieldOfView.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnPlusFieldOfView;
                @PlusFieldOfView.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnPlusFieldOfView;
                @MinusFieldOfView.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMinusFieldOfView;
                @MinusFieldOfView.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMinusFieldOfView;
                @MinusFieldOfView.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnMinusFieldOfView;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @NextTitle.started += instance.OnNextTitle;
                @NextTitle.performed += instance.OnNextTitle;
                @NextTitle.canceled += instance.OnNextTitle;
                @MarkDone.started += instance.OnMarkDone;
                @MarkDone.performed += instance.OnMarkDone;
                @MarkDone.canceled += instance.OnMarkDone;
                @MarkNotDone.started += instance.OnMarkNotDone;
                @MarkNotDone.performed += instance.OnMarkNotDone;
                @MarkNotDone.canceled += instance.OnMarkNotDone;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Option1.started += instance.OnOption1;
                @Option1.performed += instance.OnOption1;
                @Option1.canceled += instance.OnOption1;
                @Option2.started += instance.OnOption2;
                @Option2.performed += instance.OnOption2;
                @Option2.canceled += instance.OnOption2;
                @Option3.started += instance.OnOption3;
                @Option3.performed += instance.OnOption3;
                @Option3.canceled += instance.OnOption3;
                @Option4.started += instance.OnOption4;
                @Option4.performed += instance.OnOption4;
                @Option4.canceled += instance.OnOption4;
                @Option5.started += instance.OnOption5;
                @Option5.performed += instance.OnOption5;
                @Option5.canceled += instance.OnOption5;
                @Fireworks.started += instance.OnFireworks;
                @Fireworks.performed += instance.OnFireworks;
                @Fireworks.canceled += instance.OnFireworks;
                @Flash.started += instance.OnFlash;
                @Flash.performed += instance.OnFlash;
                @Flash.canceled += instance.OnFlash;
                @TeamsTitle.started += instance.OnTeamsTitle;
                @TeamsTitle.performed += instance.OnTeamsTitle;
                @TeamsTitle.canceled += instance.OnTeamsTitle;
                @PlayUntilPauseMark.started += instance.OnPlayUntilPauseMark;
                @PlayUntilPauseMark.performed += instance.OnPlayUntilPauseMark;
                @PlayUntilPauseMark.canceled += instance.OnPlayUntilPauseMark;
                @PlayAfterPauseMark.started += instance.OnPlayAfterPauseMark;
                @PlayAfterPauseMark.performed += instance.OnPlayAfterPauseMark;
                @PlayAfterPauseMark.canceled += instance.OnPlayAfterPauseMark;
                @PlayFull.started += instance.OnPlayFull;
                @PlayFull.performed += instance.OnPlayFull;
                @PlayFull.canceled += instance.OnPlayFull;
                @PlayerPause.started += instance.OnPlayerPause;
                @PlayerPause.performed += instance.OnPlayerPause;
                @PlayerPause.canceled += instance.OnPlayerPause;
                @PlayerLoop.started += instance.OnPlayerLoop;
                @PlayerLoop.performed += instance.OnPlayerLoop;
                @PlayerLoop.canceled += instance.OnPlayerLoop;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
                @Fullscreen.started += instance.OnFullscreen;
                @Fullscreen.performed += instance.OnFullscreen;
                @Fullscreen.canceled += instance.OnFullscreen;
                @StartCountdown.started += instance.OnStartCountdown;
                @StartCountdown.performed += instance.OnStartCountdown;
                @StartCountdown.canceled += instance.OnStartCountdown;
                @SwitchVisibleBGElements.started += instance.OnSwitchVisibleBGElements;
                @SwitchVisibleBGElements.performed += instance.OnSwitchVisibleBGElements;
                @SwitchVisibleBGElements.canceled += instance.OnSwitchVisibleBGElements;
                @PlusFieldOfView.started += instance.OnPlusFieldOfView;
                @PlusFieldOfView.performed += instance.OnPlusFieldOfView;
                @PlusFieldOfView.canceled += instance.OnPlusFieldOfView;
                @MinusFieldOfView.started += instance.OnMinusFieldOfView;
                @MinusFieldOfView.performed += instance.OnMinusFieldOfView;
                @MinusFieldOfView.canceled += instance.OnMinusFieldOfView;
            }
        }
    }
    public GameActions @Game => new GameActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    public interface IGameActions
    {
        void OnNextTitle(InputAction.CallbackContext context);
        void OnMarkDone(InputAction.CallbackContext context);
        void OnMarkNotDone(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnOption1(InputAction.CallbackContext context);
        void OnOption2(InputAction.CallbackContext context);
        void OnOption3(InputAction.CallbackContext context);
        void OnOption4(InputAction.CallbackContext context);
        void OnOption5(InputAction.CallbackContext context);
        void OnFireworks(InputAction.CallbackContext context);
        void OnFlash(InputAction.CallbackContext context);
        void OnTeamsTitle(InputAction.CallbackContext context);
        void OnPlayUntilPauseMark(InputAction.CallbackContext context);
        void OnPlayAfterPauseMark(InputAction.CallbackContext context);
        void OnPlayFull(InputAction.CallbackContext context);
        void OnPlayerPause(InputAction.CallbackContext context);
        void OnPlayerLoop(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
        void OnFullscreen(InputAction.CallbackContext context);
        void OnStartCountdown(InputAction.CallbackContext context);
        void OnSwitchVisibleBGElements(InputAction.CallbackContext context);
        void OnPlusFieldOfView(InputAction.CallbackContext context);
        void OnMinusFieldOfView(InputAction.CallbackContext context);
    }
}
