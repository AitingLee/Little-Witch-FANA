// GENERATED AUTOMATICALLY FROM 'Assets/MyAsset/Script/PlayerControl/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player Movement"",
            ""id"": ""49393d96-3b2a-4ae8-a134-c94334009a51"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3892a723-3c49-49a4-a335-e4a931ad48b7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""481e09d2-f41f-4bf7-a9ca-adfca200a809"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""df1d287e-2d88-42e5-9334-2bdb2c1f29e2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""3c21c73f-223d-47c3-b56c-22b4a17c2380"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""97b5d302-f64b-416e-a599-3df2b2e111ba"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1ae14ce6-166e-4abc-8ff2-c7447c445e1d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7b06b8c7-92e1-45cd-9d3e-1028bb8f2836"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""662f371e-4a4a-404f-b65f-e0b240253c1a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4a691e81-ba98-4377-972e-6b3f47cb68c4"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24dd75dd-4cff-48bc-bdf7-b616ac694909"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player Actions"",
            ""id"": ""12f20a40-2e4a-43e7-b293-6f99d7de3199"",
            ""actions"": [
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""2157212f-d1a7-4c19-a8ea-ee16df83c76a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""9a241ba6-4c0f-46dd-871d-c08805e4fb3e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""7af77e0e-0250-4436-8b47-a5387eb3e974"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AttackA"",
                    ""type"": ""Button"",
                    ""id"": ""6b096897-83c9-45db-9642-9d5d1205680e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AttackB"",
                    ""type"": ""Button"",
                    ""id"": ""f85e9f0a-c286-4df8-941c-cb61e84ca19f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""f2a1989b-0626-468b-9549-1800c6d50800"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cbf5ad4b-c5f7-4daf-b61b-5dfbcd8f1c10"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d79ad96-a12b-4132-b1d4-fff3c7db9e19"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""284811b8-4083-476a-bb9f-8787ad7aacef"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75e37370-a9d1-42f8-a002-5085b2c87981"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackA"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdc3fc62-1efd-43cc-8177-7ae60026b773"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9897333-e7c3-4766-b3fa-306b974c7ef2"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ShortCuts"",
            ""id"": ""5e2966a4-7a2a-4320-abab-b5c660da5a99"",
            ""actions"": [
                {
                    ""name"": ""One"",
                    ""type"": ""Button"",
                    ""id"": ""3d7aa6e1-0842-41a3-a788-ccf577d197f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Two"",
                    ""type"": ""Button"",
                    ""id"": ""aba95b43-a634-4204-ab27-9218626e13f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Three"",
                    ""type"": ""Button"",
                    ""id"": ""f259bde6-c4ea-4654-bfe1-fe1eebbf4baf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Four"",
                    ""type"": ""Button"",
                    ""id"": ""ecc86f38-b204-46bc-91f8-5a62126bf418"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""E"",
                    ""type"": ""Button"",
                    ""id"": ""a1cad3bc-c43b-4995-8b6c-2e9cc9fd131d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tab"",
                    ""type"": ""Button"",
                    ""id"": ""76d2d1a7-4e91-4162-bd2e-48c9750414e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""R"",
                    ""type"": ""Button"",
                    ""id"": ""dc28e4f4-5bed-487a-8c68-0e9912695e31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""M"",
                    ""type"": ""Button"",
                    ""id"": ""5b12d26e-abf1-479e-8416-8b4b6b5b46a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""788bde85-abd5-4352-b630-e22ff698b2cc"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""One"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""604ddd36-edf2-42ab-bc8f-f756f9b4633f"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3dd1f3a8-d386-487e-a5ae-9d92fd6e29e4"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d88fe8d2-54f9-45e0-a2f2-288faaf76d10"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Four"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c0eb320-a0e5-4927-858c-531bb90ba195"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""E"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be39e0e2-53f9-4a93-8c8e-c2a7122e1fe6"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5b35b8d-29f5-488a-828d-74a100777172"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""R"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f61cc6ed-2521-45bd-8167-9176f4dfdf9d"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""M"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player Movement
        m_PlayerMovement = asset.FindActionMap("Player Movement", throwIfNotFound: true);
        m_PlayerMovement_Movement = m_PlayerMovement.FindAction("Movement", throwIfNotFound: true);
        m_PlayerMovement_Camera = m_PlayerMovement.FindAction("Camera", throwIfNotFound: true);
        m_PlayerMovement_Zoom = m_PlayerMovement.FindAction("Zoom", throwIfNotFound: true);
        // Player Actions
        m_PlayerActions = asset.FindActionMap("Player Actions", throwIfNotFound: true);
        m_PlayerActions_Sprint = m_PlayerActions.FindAction("Sprint", throwIfNotFound: true);
        m_PlayerActions_Jump = m_PlayerActions.FindAction("Jump", throwIfNotFound: true);
        m_PlayerActions_Dodge = m_PlayerActions.FindAction("Dodge", throwIfNotFound: true);
        m_PlayerActions_AttackA = m_PlayerActions.FindAction("AttackA", throwIfNotFound: true);
        m_PlayerActions_AttackB = m_PlayerActions.FindAction("AttackB", throwIfNotFound: true);
        m_PlayerActions_Aim = m_PlayerActions.FindAction("Aim", throwIfNotFound: true);
        // ShortCuts
        m_ShortCuts = asset.FindActionMap("ShortCuts", throwIfNotFound: true);
        m_ShortCuts_One = m_ShortCuts.FindAction("One", throwIfNotFound: true);
        m_ShortCuts_Two = m_ShortCuts.FindAction("Two", throwIfNotFound: true);
        m_ShortCuts_Three = m_ShortCuts.FindAction("Three", throwIfNotFound: true);
        m_ShortCuts_Four = m_ShortCuts.FindAction("Four", throwIfNotFound: true);
        m_ShortCuts_E = m_ShortCuts.FindAction("E", throwIfNotFound: true);
        m_ShortCuts_Tab = m_ShortCuts.FindAction("Tab", throwIfNotFound: true);
        m_ShortCuts_R = m_ShortCuts.FindAction("R", throwIfNotFound: true);
        m_ShortCuts_M = m_ShortCuts.FindAction("M", throwIfNotFound: true);
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

    // Player Movement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_Movement;
    private readonly InputAction m_PlayerMovement_Camera;
    private readonly InputAction m_PlayerMovement_Zoom;
    public struct PlayerMovementActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovement_Movement;
        public InputAction @Camera => m_Wrapper.m_PlayerMovement_Camera;
        public InputAction @Zoom => m_Wrapper.m_PlayerMovement_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Camera.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCamera;
                @Zoom.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnZoom;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // Player Actions
    private readonly InputActionMap m_PlayerActions;
    private IPlayerActionsActions m_PlayerActionsActionsCallbackInterface;
    private readonly InputAction m_PlayerActions_Sprint;
    private readonly InputAction m_PlayerActions_Jump;
    private readonly InputAction m_PlayerActions_Dodge;
    private readonly InputAction m_PlayerActions_AttackA;
    private readonly InputAction m_PlayerActions_AttackB;
    private readonly InputAction m_PlayerActions_Aim;
    public struct PlayerActionsActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Sprint => m_Wrapper.m_PlayerActions_Sprint;
        public InputAction @Jump => m_Wrapper.m_PlayerActions_Jump;
        public InputAction @Dodge => m_Wrapper.m_PlayerActions_Dodge;
        public InputAction @AttackA => m_Wrapper.m_PlayerActions_AttackA;
        public InputAction @AttackB => m_Wrapper.m_PlayerActions_AttackB;
        public InputAction @Aim => m_Wrapper.m_PlayerActions_Aim;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActionsActions instance)
        {
            if (m_Wrapper.m_PlayerActionsActionsCallbackInterface != null)
            {
                @Sprint.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSprint;
                @Jump.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnJump;
                @Dodge.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnDodge;
                @Dodge.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnDodge;
                @Dodge.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnDodge;
                @AttackA.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAttackA;
                @AttackA.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAttackA;
                @AttackA.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAttackA;
                @AttackB.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAttackB;
                @AttackB.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAttackB;
                @AttackB.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAttackB;
                @Aim.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnAim;
            }
            m_Wrapper.m_PlayerActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Dodge.started += instance.OnDodge;
                @Dodge.performed += instance.OnDodge;
                @Dodge.canceled += instance.OnDodge;
                @AttackA.started += instance.OnAttackA;
                @AttackA.performed += instance.OnAttackA;
                @AttackA.canceled += instance.OnAttackA;
                @AttackB.started += instance.OnAttackB;
                @AttackB.performed += instance.OnAttackB;
                @AttackB.canceled += instance.OnAttackB;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
            }
        }
    }
    public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);

    // ShortCuts
    private readonly InputActionMap m_ShortCuts;
    private IShortCutsActions m_ShortCutsActionsCallbackInterface;
    private readonly InputAction m_ShortCuts_One;
    private readonly InputAction m_ShortCuts_Two;
    private readonly InputAction m_ShortCuts_Three;
    private readonly InputAction m_ShortCuts_Four;
    private readonly InputAction m_ShortCuts_E;
    private readonly InputAction m_ShortCuts_Tab;
    private readonly InputAction m_ShortCuts_R;
    private readonly InputAction m_ShortCuts_M;
    public struct ShortCutsActions
    {
        private @PlayerControls m_Wrapper;
        public ShortCutsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @One => m_Wrapper.m_ShortCuts_One;
        public InputAction @Two => m_Wrapper.m_ShortCuts_Two;
        public InputAction @Three => m_Wrapper.m_ShortCuts_Three;
        public InputAction @Four => m_Wrapper.m_ShortCuts_Four;
        public InputAction @E => m_Wrapper.m_ShortCuts_E;
        public InputAction @Tab => m_Wrapper.m_ShortCuts_Tab;
        public InputAction @R => m_Wrapper.m_ShortCuts_R;
        public InputAction @M => m_Wrapper.m_ShortCuts_M;
        public InputActionMap Get() { return m_Wrapper.m_ShortCuts; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShortCutsActions set) { return set.Get(); }
        public void SetCallbacks(IShortCutsActions instance)
        {
            if (m_Wrapper.m_ShortCutsActionsCallbackInterface != null)
            {
                @One.started -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnOne;
                @One.performed -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnOne;
                @One.canceled -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnOne;
                @Two.started -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnTwo;
                @Two.performed -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnTwo;
                @Two.canceled -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnTwo;
                @Three.started -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnThree;
                @Three.performed -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnThree;
                @Three.canceled -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnThree;
                @Four.started -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnFour;
                @Four.performed -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnFour;
                @Four.canceled -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnFour;
                @E.started -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnE;
                @E.performed -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnE;
                @E.canceled -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnE;
                @Tab.started -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnTab;
                @Tab.performed -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnTab;
                @Tab.canceled -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnTab;
                @R.started -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnR;
                @R.performed -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnR;
                @R.canceled -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnR;
                @M.started -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnM;
                @M.performed -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnM;
                @M.canceled -= m_Wrapper.m_ShortCutsActionsCallbackInterface.OnM;
            }
            m_Wrapper.m_ShortCutsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @One.started += instance.OnOne;
                @One.performed += instance.OnOne;
                @One.canceled += instance.OnOne;
                @Two.started += instance.OnTwo;
                @Two.performed += instance.OnTwo;
                @Two.canceled += instance.OnTwo;
                @Three.started += instance.OnThree;
                @Three.performed += instance.OnThree;
                @Three.canceled += instance.OnThree;
                @Four.started += instance.OnFour;
                @Four.performed += instance.OnFour;
                @Four.canceled += instance.OnFour;
                @E.started += instance.OnE;
                @E.performed += instance.OnE;
                @E.canceled += instance.OnE;
                @Tab.started += instance.OnTab;
                @Tab.performed += instance.OnTab;
                @Tab.canceled += instance.OnTab;
                @R.started += instance.OnR;
                @R.performed += instance.OnR;
                @R.canceled += instance.OnR;
                @M.started += instance.OnM;
                @M.performed += instance.OnM;
                @M.canceled += instance.OnM;
            }
        }
    }
    public ShortCutsActions @ShortCuts => new ShortCutsActions(this);
    public interface IPlayerMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
    public interface IPlayerActionsActions
    {
        void OnSprint(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnAttackA(InputAction.CallbackContext context);
        void OnAttackB(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
    }
    public interface IShortCutsActions
    {
        void OnOne(InputAction.CallbackContext context);
        void OnTwo(InputAction.CallbackContext context);
        void OnThree(InputAction.CallbackContext context);
        void OnFour(InputAction.CallbackContext context);
        void OnE(InputAction.CallbackContext context);
        void OnTab(InputAction.CallbackContext context);
        void OnR(InputAction.CallbackContext context);
        void OnM(InputAction.CallbackContext context);
    }
}
