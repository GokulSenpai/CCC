using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    [CustomEditor(typeof(Player))]
    public class CustomInspector : UnityEditor.Editor
    {
        private AnimBool _crouchValue;
        private AnimBool _headBobValue;
        private AnimBool _jumpValue;
        private AnimBool _peekValue;
        private AnimBool _runValue;
        private AnimBool _tiltValue;
        private AnimBool _zoomValue;

        private Player _myTarget;
        private SerializedObject _soTarget;

        public Text stateText;
        
        // Speed
        private SerializedProperty walkSpeed;
        private SerializedProperty runSpeed;
        
        // Jump
        private SerializedProperty jumpHeight;
        private SerializedProperty jumpAirTime;
        private SerializedProperty jumpEffectRecoil;
        private SerializedProperty walkJumpEffectRecoil;
        private SerializedProperty runJumpEffectRecoil;
        private SerializedProperty smoothJumpRecoilFactor;
        private SerializedProperty jumpRecoilCurve;
        private SerializedProperty walkJumpRecoilCurve;
        private SerializedProperty runJumpRecoilCurve;
        
        // Ground
        private SerializedProperty groundDistance;
        private SerializedProperty ceilingCheck;    
        private SerializedProperty groundCheck;
        private SerializedProperty groundMask;
        
        // HeadBob
        private SerializedProperty idleXIntensity;
        private SerializedProperty idleYIntensity;
        private SerializedProperty walkXIntensity;
        private SerializedProperty walkYIntensity;
        private SerializedProperty runXIntensity;
        private SerializedProperty runYIntensity;
        private SerializedProperty crouchXIntensity;
        private SerializedProperty crouchYIntensity;
        private SerializedProperty runHeadBobOffset;
        private SerializedProperty idleFactorA;
        private SerializedProperty idleFactorB;
        private SerializedProperty walkFactorA;
        private SerializedProperty walkFactorB;
        private SerializedProperty runFactorA;
        private SerializedProperty runFactorB;
        private SerializedProperty crouchFactorA;
        private SerializedProperty crouchFactorB;
        
        // Zoom
        private SerializedProperty zoomFov;
        private SerializedProperty smoothZoomFactor;
        private SerializedProperty zoomCurve;

        // Run
        private SerializedProperty runFovMultiplier;
        private SerializedProperty runFovFactor;
        private SerializedProperty lookBackRotation;
        private SerializedProperty smoothLookBackFactor;
        private SerializedProperty lookBackCurve;
        
        // Crouch
        private SerializedProperty crouchValues;
        private SerializedProperty collisionOverlapRadius;
        private SerializedProperty crouchCurve;
        private SerializedProperty smoothCrouchFactor;
        private SerializedProperty crouchControllerHeight;
        private SerializedProperty controllerCentreOffset;

        // Peek
        private SerializedProperty peekPosition;
        private SerializedProperty peekAngle;
        private SerializedProperty peekCurve;
        private SerializedProperty smoothPeekFactor;
        private SerializedProperty mousePeekClampMin;
        private SerializedProperty mousePeekClampMax;
        
        // Camera Tilt
        private SerializedProperty cameraTiltPos;
        private SerializedProperty cameraTiltFactor;
        
        // Forces
        private SerializedProperty gravity;
        private SerializedProperty drag;

        // References
        private SerializedProperty mouseClampMin;
        private SerializedProperty mouseClampMax;
        private SerializedProperty theCamera;
        private SerializedProperty playerAnimations;
        private SerializedProperty walkAudioSource;
        private SerializedProperty walkAudioClip;

        
        private bool _crouchExpanded;
        private bool _headBobExpanded;
        private bool _jumpExpanded;
        private bool _peekExpanded;
        private bool _runExpanded;
        private bool _tiltExpanded;
        private bool _zoomExpanded;
        
        private void OnEnable()
        {
            _myTarget = (Player) target;
            _soTarget = new SerializedObject(target);

            _crouchValue = new AnimBool(true);
            _crouchValue.valueChanged.AddListener(Repaint);
            
            _headBobValue = new AnimBool(true);
            _headBobValue.valueChanged.AddListener(Repaint);
            
            _jumpValue = new AnimBool(true);
            _jumpValue.valueChanged.AddListener(Repaint);
            
            _peekValue = new AnimBool(true);
            _peekValue.valueChanged.AddListener(Repaint);
            
            _runValue = new AnimBool(true);
            _runValue.valueChanged.AddListener(Repaint);
            
            _tiltValue = new AnimBool(true);
            _tiltValue.valueChanged.AddListener(Repaint);
            
            _zoomValue = new AnimBool(true);
            _zoomValue.valueChanged.AddListener(Repaint);
            
            // Speed
            walkSpeed = _soTarget.FindProperty("walkSpeed");
            runSpeed = _soTarget.FindProperty("runSpeed");
        
            // Jump
            jumpHeight = _soTarget.FindProperty("jumpHeight");
            jumpAirTime = _soTarget.FindProperty("jumpAirTime");
            jumpEffectRecoil = _soTarget.FindProperty("jumpEffectRecoil");
            walkJumpEffectRecoil = _soTarget.FindProperty("walkJumpEffectRecoil");
            runJumpEffectRecoil = _soTarget.FindProperty("runJumpEffectRecoil");
            smoothJumpRecoilFactor = _soTarget.FindProperty("smoothJumpRecoilFactor");
            jumpRecoilCurve = _soTarget.FindProperty("jumpRecoilCurve");
            walkJumpRecoilCurve = _soTarget.FindProperty("walkJumpRecoilCurve");
            runJumpRecoilCurve = _soTarget.FindProperty("runJumpRecoilCurve");
        
            // Ground
            groundDistance = _soTarget.FindProperty("groundDistance");
            ceilingCheck = _soTarget.FindProperty("ceilingCheck");
            groundCheck = _soTarget.FindProperty("groundCheck");
            groundMask = _soTarget.FindProperty("groundMask");
            
            // Headbob
            idleXIntensity = _soTarget.FindProperty("idleXIntensity");
            idleYIntensity = _soTarget.FindProperty("idleYIntensity");
            walkXIntensity = _soTarget.FindProperty("walkXIntensity");
            walkYIntensity = _soTarget.FindProperty("walkYIntensity");
            runXIntensity = _soTarget.FindProperty("runXIntensity");
            runYIntensity = _soTarget.FindProperty("runYIntensity");
            crouchXIntensity = _soTarget.FindProperty("crouchXIntensity");
            crouchYIntensity = _soTarget.FindProperty("crouchYIntensity");
            runHeadBobOffset = _soTarget.FindProperty("runHeadBobOffset");
            idleFactorA = _soTarget.FindProperty("idleFactorA");
            idleFactorB = _soTarget.FindProperty("idleFactorB");
            walkFactorA = _soTarget.FindProperty("walkFactorA");
            walkFactorB = _soTarget.FindProperty("walkFactorB");
            runFactorA = _soTarget.FindProperty("runFactorA");
            runFactorB = _soTarget.FindProperty("runFactorB");
            crouchFactorA = _soTarget.FindProperty("crouchFactorA");
            crouchFactorB = _soTarget.FindProperty("crouchFactorB");
        
            // Zoom
            zoomFov = _soTarget.FindProperty("zoomFov");
            smoothZoomFactor = _soTarget.FindProperty("smoothZoomFactor");
            zoomCurve = _soTarget.FindProperty("zoomCurve");
        
            // Run
            runFovMultiplier = _soTarget.FindProperty("runFovMultiplier");
            runFovFactor = _soTarget.FindProperty("runFovFactor");
            lookBackRotation = _soTarget.FindProperty("lookBackRotation");
            smoothLookBackFactor = _soTarget.FindProperty("smoothLookBackFactor");
            lookBackCurve = _soTarget.FindProperty("lookBackCurve");
            
            // Crouch
            crouchValues = _soTarget.FindProperty("crouchValues");
            collisionOverlapRadius = _soTarget.FindProperty("collisionOverlapRadius");
            crouchCurve = _soTarget.FindProperty("crouchCurve");
            smoothCrouchFactor = _soTarget.FindProperty("smoothCrouchFactor");
            crouchControllerHeight = _soTarget.FindProperty("crouchControllerHeight");
            controllerCentreOffset = _soTarget.FindProperty("controllerCentreOffset");
            
            // Peek
            peekPosition = _soTarget.FindProperty("peekPosition");
            peekAngle = _soTarget.FindProperty("peekAngle");
            peekCurve = _soTarget.FindProperty("peekCurve");
            smoothPeekFactor = _soTarget.FindProperty("smoothPeekFactor");
            mousePeekClampMin = _soTarget.FindProperty("mousePeekClampMin");
            mousePeekClampMax = _soTarget.FindProperty("mousePeekClampMax");
            
            // Camera Tilt
            cameraTiltPos = _soTarget.FindProperty("cameraTiltPos");
            cameraTiltFactor = _soTarget.FindProperty("cameraTiltFactor");
           
            // Forces
            gravity = _soTarget.FindProperty("gravity");
            drag = _soTarget.FindProperty("drag");
            
            // References
            mouseClampMin = _soTarget.FindProperty("mouseClampMin");
            mouseClampMax = _soTarget.FindProperty("mouseClampMax");
            theCamera = _soTarget.FindProperty("theCamera");
            playerAnimations = _soTarget.FindProperty("playerAnimations");
            walkAudioSource = _soTarget.FindProperty("walkAudioSource");
            walkAudioClip = _soTarget.FindProperty("walkAudioClip");

        }

        public override void OnInspectorGUI()
        {
            _soTarget.Update();
            
            EditorGUILayout.Space();

            GUILayout.Label("Can I ?", EditorStyles.boldLabel);
            
            EditorGUILayout.Space();

            #region Crouch Toggle

            _crouchValue.target = EditorGUILayout.Toggle("Crouch", _crouchValue.target);
            
            _myTarget.crouch = _crouchValue.target.Equals(true);

            if (EditorGUILayout.BeginFadeGroup(_crouchValue.faded))
            {
                EditorGUILayout.BeginVertical();
       
                _crouchExpanded = GUILayout.Toggle( _crouchExpanded, (_crouchExpanded ? "Collapse" : "Customize"), "Foldout", GUILayout.ExpandWidth(false));

                if (_crouchExpanded)
                {               
                    EditorGUI.indentLevel++;
                
                    EditorGUILayout.PropertyField(crouchValues);
                    EditorGUILayout.PropertyField(collisionOverlapRadius);
                    EditorGUILayout.PropertyField(crouchCurve);
                    EditorGUILayout.PropertyField(smoothCrouchFactor);
                    EditorGUILayout.PropertyField(crouchControllerHeight);
                    EditorGUILayout.PropertyField(controllerCentreOffset);
                
                    EditorGUI.indentLevel--;      
                }      
       
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndFadeGroup();
            
            EditorGUILayout.Space();

            #endregion
        
            #region HeadBob Toggle

            _headBobValue.target = EditorGUILayout.Toggle("Head Bob", _headBobValue.target);
            
            _myTarget.headBob = _headBobValue.target.Equals(true);

            if (EditorGUILayout.BeginFadeGroup(_headBobValue.faded))
            {
                EditorGUILayout.BeginVertical();
       
                _headBobExpanded = GUILayout.Toggle( _headBobExpanded, (_headBobExpanded ? "Collapse" : "Customize"), "Foldout", GUILayout.ExpandWidth(false));

                if (_headBobExpanded)
                {           
                    EditorGUI.indentLevel++;
                
                    EditorGUILayout.PropertyField(idleXIntensity);
                    EditorGUILayout.PropertyField(idleYIntensity);
                    EditorGUILayout.PropertyField(walkXIntensity);
                    EditorGUILayout.PropertyField(walkYIntensity);
                    EditorGUILayout.PropertyField(runXIntensity);
                    EditorGUILayout.PropertyField(runYIntensity);
                    EditorGUILayout.PropertyField(crouchXIntensity);
                    EditorGUILayout.PropertyField(crouchYIntensity);
                    EditorGUILayout.PropertyField(runHeadBobOffset);
                    EditorGUILayout.PropertyField(idleFactorA);
                    EditorGUILayout.PropertyField(idleFactorB);
                    EditorGUILayout.PropertyField(walkFactorA);
                    EditorGUILayout.PropertyField(walkFactorB);
                    EditorGUILayout.PropertyField(runFactorA);
                    EditorGUILayout.PropertyField(runFactorB);
                    EditorGUILayout.PropertyField(crouchFactorA);
                    EditorGUILayout.PropertyField(crouchFactorB);

                    EditorGUI.indentLevel--;    
                }      
       
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndFadeGroup();
        
            EditorGUILayout.Space();

            #endregion
        
            #region Jump Toggle

            _jumpValue.target = EditorGUILayout.Toggle("Jump", _jumpValue.target);
        
            _myTarget.jump = _jumpValue.target.Equals(true);

            if (EditorGUILayout.BeginFadeGroup(_jumpValue.faded))
            {
                EditorGUILayout.BeginVertical();
       
                _jumpExpanded = GUILayout.Toggle( _jumpExpanded, (_jumpExpanded ? "Collapse" : "Customize"), "Foldout", GUILayout.ExpandWidth(false));

                if (_jumpExpanded)
                {           
                    EditorGUI.indentLevel++;
                
                    EditorGUILayout.PropertyField(jumpHeight);
                    EditorGUILayout.PropertyField(jumpAirTime);
                    EditorGUILayout.PropertyField(jumpEffectRecoil);
                    EditorGUILayout.PropertyField(walkJumpEffectRecoil);
                    EditorGUILayout.PropertyField(runJumpEffectRecoil);
                    EditorGUILayout.PropertyField(jumpRecoilCurve);
                    EditorGUILayout.PropertyField(walkJumpRecoilCurve);
                    EditorGUILayout.PropertyField(runJumpRecoilCurve);
                    EditorGUILayout.PropertyField(smoothJumpRecoilFactor);

                    EditorGUI.indentLevel--;
                }      
       
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndFadeGroup();
        
            EditorGUILayout.Space();

            #endregion
        
            #region Peek Toggle

            _peekValue.target = EditorGUILayout.Toggle("Peek", _peekValue.target);
            
            _myTarget.peek = _peekValue.target.Equals(true);

            if (EditorGUILayout.BeginFadeGroup(_peekValue.faded))
            {
                
                EditorGUILayout.BeginVertical();
       
                _peekExpanded = GUILayout.Toggle( _peekExpanded, (_peekExpanded ? "Collapse" : "Customize"), "Foldout", GUILayout.ExpandWidth(false));

                if (_peekExpanded)
                {           
                    EditorGUI.indentLevel++;
                
                    EditorGUILayout.PropertyField(peekPosition);
                    EditorGUILayout.PropertyField(peekAngle);
                    EditorGUILayout.PropertyField(peekCurve);
                    EditorGUILayout.PropertyField(smoothPeekFactor);
                    EditorGUILayout.PropertyField(mousePeekClampMin);
                    EditorGUILayout.PropertyField(mousePeekClampMax);
                
                    EditorGUI.indentLevel--;
                }      
       
                EditorGUILayout.EndVertical();
               
            }
            EditorGUILayout.EndFadeGroup();
        
            EditorGUILayout.Space();

            #endregion
        
            #region Run Toggle

            _runValue.target = EditorGUILayout.Toggle("Run", _runValue.target);
            
            _myTarget.run = _runValue.target.Equals(true);

            if (EditorGUILayout.BeginFadeGroup(_runValue.faded))
            {
                EditorGUILayout.BeginVertical();
       
                _runExpanded = GUILayout.Toggle( _runExpanded, (_runExpanded ? "Collapse" : "Customize"), "Foldout", GUILayout.ExpandWidth(false));

                if (_runExpanded)
                {           
                    EditorGUI.indentLevel++;
                
                    EditorGUILayout.PropertyField(runFovMultiplier);
                    EditorGUILayout.PropertyField(runFovFactor);
                    EditorGUILayout.PropertyField(lookBackRotation);
                    EditorGUILayout.PropertyField(lookBackCurve);
                    EditorGUILayout.PropertyField(smoothLookBackFactor);

                    EditorGUI.indentLevel--;
                }      
       
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndFadeGroup();
        
            EditorGUILayout.Space();

            #endregion
        
            #region Tilt Toggle

            _tiltValue.target = EditorGUILayout.Toggle("Tilt", _tiltValue.target);
            
            _myTarget.cameraTilt = _tiltValue.target.Equals(true);

            if (EditorGUILayout.BeginFadeGroup(_tiltValue.faded))
            {
                EditorGUILayout.BeginVertical();
       
                _tiltExpanded = GUILayout.Toggle( _tiltExpanded, (_tiltExpanded ? "Collapse" : "Customize"), "Foldout", GUILayout.ExpandWidth(false));

                if (_tiltExpanded)
                {           
                    EditorGUI.indentLevel++;
                
                    EditorGUILayout.PropertyField(cameraTiltPos);
                    EditorGUILayout.PropertyField(cameraTiltFactor);

                    EditorGUI.indentLevel--;
                }      
       
                EditorGUILayout.EndVertical();

            }
            EditorGUILayout.EndFadeGroup();
        
            EditorGUILayout.Space();

            #endregion
        
            #region Zoom Toggle

            _zoomValue.target = EditorGUILayout.Toggle("Zoom", _zoomValue.target);
            
            _myTarget.zoom = _zoomValue.target.Equals(true);

            if (EditorGUILayout.BeginFadeGroup(_zoomValue.faded))
            {
                EditorGUILayout.BeginVertical();
       
                _zoomExpanded = GUILayout.Toggle( _zoomExpanded, (_zoomExpanded ? "Collapse" : "Customize"), "Foldout", GUILayout.ExpandWidth(false));

                if (_zoomExpanded)
                {           
                    EditorGUI.indentLevel++;
                
                    EditorGUILayout.PropertyField(zoomFov);
                    EditorGUILayout.PropertyField(zoomCurve);
                    EditorGUILayout.PropertyField(smoothZoomFactor);

                    EditorGUI.indentLevel--;
                }      
       
                EditorGUILayout.EndVertical();
                
            }
            EditorGUILayout.EndFadeGroup();
        
            EditorGUILayout.Space();

            #endregion
            
            EditorGUI.BeginChangeCheck();
            
            _myTarget.toolBar = GUILayout.Toolbar(_myTarget.toolBar,
                new string[] {"Speed", "Ground", "Forces", "References"});

            switch (_myTarget.toolBar)
            {
                case 0:
                    _myTarget.currentTab = "Speed";
                    break;
            
                case 1:
                    _myTarget.currentTab = "Ground";
                    break;
            
                case 2:
                    _myTarget.currentTab = "Forces";
                    break;
            
                case 3:
                    _myTarget.currentTab = "References";
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                _soTarget.ApplyModifiedProperties();
                GUI.FocusControl(null);
            }
        
            EditorGUI.BeginChangeCheck();

            switch (_myTarget.currentTab)
            {
                case "Speed": 
                    EditorGUILayout.PropertyField(walkSpeed);
                    EditorGUILayout.PropertyField(runSpeed);
                    break;
            
                case "Ground":
                    EditorGUILayout.PropertyField(groundDistance);
                    EditorGUILayout.PropertyField(ceilingCheck);
                    EditorGUILayout.PropertyField(groundCheck);
                    EditorGUILayout.PropertyField(groundMask);
                    break;
            
                case "Forces":
                    EditorGUILayout.PropertyField(gravity);
                    EditorGUILayout.PropertyField(drag);
                    break;
            
                case "References":
                    EditorGUILayout.PropertyField(mouseClampMin);
                    EditorGUILayout.PropertyField(mouseClampMax);
                    EditorGUILayout.PropertyField(theCamera);
                    EditorGUILayout.PropertyField(playerAnimations);
                    EditorGUILayout.PropertyField(walkAudioSource);
                    EditorGUILayout.PropertyField(walkAudioClip);
                    break;
            }
        
            if (EditorGUI.EndChangeCheck())
            {
                _soTarget.ApplyModifiedProperties();
            }
        }
    }
}
