using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class RigidbodyFirstPersonController : MonoBehaviour
    {
        private static float speed = 30.0f; //Base speed for the player
		
        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 30.0f;   // Speed when walking forward
            public float BackwardSpeed = 4.0f;  // Speed when walking backwards
            public float StrafeSpeed = 4.0f;    // Speed when walking sideways
            public float RunMultiplier = 2.0f;   // Speed when sprinting
	        public KeyCode RunKey = KeyCode.LeftShift;
            public float JumpForce = 30f;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = speed;

#if !MOBILE_INPUT
            private bool m_Running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
	            if (input == Vector2.zero) return;
				if (input.x > 0 || input.x < 0)
				{
					//strafe
					CurrentTargetSpeed = StrafeSpeed;
				}
				if (input.y < 0)
				{
					//backwards
					CurrentTargetSpeed = BackwardSpeed;
				}
				if (input.y > 0)
				{
                    //forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = speed;
				}
#if !MOBILE_INPUT
	            if (Input.GetKey(RunKey))
	            {
		            CurrentTargetSpeed *= RunMultiplier;
		            m_Running = true;
	            }
	            else
	            {
		            m_Running = false;
	            }
#endif
            }

#if !MOBILE_INPUT
            public bool Running
            {
                get { return m_Running; }
            }
#endif
        }

        [Serializable]
        public class AdvancedSettings
        {
            public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float stickToGroundHelperDistance = 0.5f; // stops the character
            public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
            public bool airControl; // can the user control the direction that is being moved in the air
            [Tooltip("set it to 0.1 or more if you get stuck in wall")]
            public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
        }


        public Camera cam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();


        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private Vector3 m_GroundContactNormal;
        private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;


        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }

        public bool Jumping
        {
            get { return m_Jumping; }
        }

        public bool Running
        {
            get
            {
 #if !MOBILE_INPUT
				return movementSettings.Running;
#else
	            return false;
#endif
            }
        }

        private void Start()
        {
			
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init (transform, cam.transform);
			CardboardOnGUI.onGUICallback += this.OnGUI; //cardboard
			CardboardOnGUI.IsGUIVisible = true;
			//print(this.OnGUI);
        }
		
		void OnDestroy() {
			CardboardOnGUI.onGUICallback -= this.OnGUI;
		}

        //Camera Control
        private void RotateView()
        {
			//changeNeedle(50.0f);
			
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation(transform, cam.transform);

			//Points the needle in the direction of...
			

            if (m_IsGrounded || advancedSettings.airControl)
            {
				
                // Rotate the rigidbody velocity to match the new direction that the character is looking
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
            }
        }

        /*
         *-----------------------------------------------------------------------------------------------------------------------------------
         * Edited Code
         *
         *
         * @date 12/31/2015
         * @author Michael Chin
         */
        
        /* Game Invarients */
        private string message = ""; //Test GUI Message for the player
        private Vector3 forwardMovement = new Vector3(0.0f, 1.0f); //Forward Vector
        private Vector3 backwardMovement = new Vector3(0.0f, 0.0f); //Backward-Stop Vector
        private static int waitTime = 20; //this seems like a reasonable amount of wait time between inputs
        private Vector3 startPos = new Vector3(490.0f, 3.0f, 59.0f); //The starting position for the player

        /* Game Specific Variables */
        private float time = 0; //Clock counter
        private bool gameStarted = false; //Did the game start?
        private bool isMoving = false; //Is the player currently moving?
        private int wait = 0; //Wait counter

        /*
         * changeText
         * 
         * Changes the GUI message for the player
         *
         * @mutator
         */
        public void changeText(string text) {
            message = text;
        }
		
		void OnEnable(){
			Cardboard.SDK.OnTrigger += TriggerPulled;
		}
		
		void TriggerPulled() {
			print("The trigger was pulled!");
			if (wait == 0) {
				if (!gameStarted) gameStarted = true; //Looks like the game started
				isMoving = !isMoving; //Toggle our move state
				wait = waitTime;
			}
		}

		static int ansHeight = 60;
		static int ansX = (Screen.width / 2) + 40;
		static int ansX2 = Screen.width - 10;
		static int arrowDif = 75;
		
		//public Texture2D compass; // compass image
		public Texture2D needle; // needle image (same size of compass)
		private static Rect r = new Rect(ansX + arrowDif, ansHeight + 30, 50, 50); // rect where to draw compass
		float angle; // angle to rotate the needle
		Vector2 p = new Vector2(r.x+r.width/2,r.y+r.height/2); // find the center
		
		private static Rect r2 = new Rect(ansX2 + arrowDif, ansHeight + 30, 50, 50); // rect where to draw compass
		float angle2; // angle to rotate the needle
		Vector2 p2 = new Vector2(r2.x+r2.width/2,r2.y+r2.height/2); // find the center
		
		string[] ans;
		
		bool arrowVis = false;
		
		
		
        /*
         * OnGUI
         *
         * GUI in front of the player screen
         */
        void OnGUI() {
			if (!CardboardOnGUI.OKToDraw(this)) return;
			//print("HI?");
            //Format the clock timer
            var minutes = time / 60; //Divide the guiTime by sixty to get the minutes.
            var seconds = time % 60;//Use the euclidean division for the seconds.
            var fraction = (time * 100) % 100;
            string timerLabel = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);

            //Centered Text Style 
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            //Centered Text with Big Font Style
            var centeredStyleBig = GUI.skin.GetStyle("Label");
            centeredStyleBig.fontSize = 20;
            centeredStyleBig.alignment = TextAnchor.UpperCenter;
			
            //Display Message
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 3, 200, 200), message, centeredStyle);
			GUI.Label(new Rect(ansX, ansHeight, 200, 50), ans[0], centeredStyle);
			GUI.Label(new Rect(ansX2, ansHeight, 200, 50), ans[1], centeredStyle);
            //Display Time
            GUI.Label(new Rect(Screen.width / 2 - 50, 25, 200, 50), timerLabel, centeredStyleBig);

			//Compass
			//GUI.DrawTexture(r, compass); // draw the compass...
			 var svMat = GUI.matrix; // save gui matrix
			 GUIUtility.RotateAroundPivot(angle,p); // prepare matrix to rotate
			 if(arrowVis) GUI.DrawTexture(r,needle); // draw the needle rotated by angle
			 GUI.matrix = svMat; // restore gui matrix
			 
			 svMat = GUI.matrix; // save gui matrix
			 GUIUtility.RotateAroundPivot(angle2,p2); // prepare matrix to rotate
			 if(arrowVis) GUI.DrawTexture(r2,needle); // draw the needle rotated by angle
			 GUI.matrix = svMat; // restore gui matrix
        }
		
		float answerDir = 0.0f;
		float inDir = 0.0f;
		
		public void changeAns(string[] answ) {
			ans = answ;
		}
		
		public void changeVis(bool vis) {
			arrowVis = vis;
		}
		
		public void changeArrow(float angle) {
			answerDir = angle - 90.0f;
		}
		
		public void changeArrow2(float angle) {
			inDir = angle - 90.0f;
		}
		
		/*
		 *
		 Needle compass works by aiming the correct and incorrect arrows at their
		 respective positions.  Then in the RotateView(), as the character moves we
		 need to minus their new angle as they rotate from the correct and incorrect 
		 arrow angles to keep the correct angle.
		 
		 These angles are set in Unity on the cube objects themselves
		 
		 */
		public void changeNeedle(float nangle) {
			angle = answerDir - nangle;
			angle2 = inDir - nangle;
		}
		
		/*
		var dirVector = destinationTransform.position - player.position;
		dirVector.y = 0; // remove the vertical component, if any
		var rot = Quaternion.FromToRotation(northVector, dirVector);
		var angle: float; // angle is what we want
		var axis: Vector3; // but an axis variable must be provided as well
		rot.ToAngleAxis(angle, axis); // get the angle*/

        //Watch the camera rotation every update
        private void Update() {
            //RotateView();

            //If the game is in progress, we need to be counting the clock
            if (gameStarted) time += Time.deltaTime;

            //@not needed for our game
            if (CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
            {
                m_Jump = true;
            }
        }

     
        /*
         * restart
         *
         * Restarts the game to its initial state
         *
         * @author Michael Chin
         * @date 12/31/2015
         */
        protected void restart() {
            //Send us back to the start
            m_RigidBody.position = startPos;
            
            //Reset all game variables back to their initial
            time = 0;
            gameStarted = false;
            isMoving = false;
            wait = 0;
        }
		/*
		public Transform target; //Target to point at (you could set this to any gameObject dynamically)
		private Vector3 targetPos; //Target position on screen
		private Vector3 screenMiddle;//Middle of the screen */

        /*
         * --FixedUpdate occurs slightly before Update()
         * 
         Auto-movement 
         *
         * Character movement is toggled on and off with the up-arrow on the keyboard
         * Note to self-- delete the other input keys such as left,right,shift keys
         *
         * @author Michael Chin
         * @date 12/29/2015
         * 
         */
        private void FixedUpdate() {
		/*
			//Get the targets position on screen into a Vector3
			targetPos = cam.WorldToScreenPoint (target.transform.position);
			//Get the middle of the screen into a Vector3
			screenMiddle = new Vector3(Screen.width/2, Screen.height/2, 0); 
			//Compute the angle from screenMiddle to targetPos
			var tarAngle = (Mathf.Atan2(targetPos.x-screenMiddle.x,Screen.height-targetPos.y-screenMiddle.y) * Mathf.Rad2Deg)+90;
			if (tarAngle < 0) tarAngle +=360;


			 //Calculate the angle from the camera to the target
			 var targetDir = target.transform.position - cam.transform.position;
			 var forward = cam.transform.forward;
			 var angle = Vector3.Angle(targetDir, forward);
			 //If the angle exceeds 90deg inverse the rotation to point correctly
			 if(angle < 90){
				 transform.localRotation = Quaternion.Euler(-tarAngle,90,270);
			 } else {
				 transform.localRotation = Quaternion.Euler(tarAngle,270,90);
			 } */
		
            GroundCheck(); //@not mine
            //Vector2 userInput = GetInput(); //Take in user input

            Vector2 input = backwardMovement; //Default to no movement
            //Make sure time has passed between the last input
            if (wait > 0) {
                wait--;
            }
            //If the up-key was pressed
			/*
            if (userInput.y > 0) {
                if (wait == 0) {
                    if (!gameStarted) gameStarted = true; //Looks like the game started
                    isMoving = !isMoving; //Toggle our move state
                    wait = waitTime;
                } 
            }*/
            //Test restart button ~~ on the left/right key
			/*
            if(userInput.x > 0)
            {
                restart();
            } */

            //Start moving forward if we should be..
            if(isMoving) {
                input = forwardMovement;
            }

            //IGNORE ALL THIS CODE BELOW --------------------------------------------------------------------------------------
            //This does everything
			
            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
            {
                // always move along the camera forward as it is the direction that it being aimed at
				Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;
				print(movementSettings.CurrentTargetSpeed);
                desiredMove.x = desiredMove.x*speed;
                desiredMove.z = desiredMove.z*speed;
                desiredMove.y = desiredMove.y*speed;
                if (m_RigidBody.velocity.sqrMagnitude <
                    (speed*speed))
                {
                    m_RigidBody.AddForce(desiredMove*SlopeMultiplier(), ForceMode.Impulse);
                }
            } 

            //Some kind of jumping code? Probably safe to delete
            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;

                if (m_Jump)
                {
                    m_RigidBody.drag = 0f;
                    m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                    m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                    m_Jumping = true;
                }

                if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
                {
                    m_RigidBody.Sleep();
                }
            }
            else
            {
                m_RigidBody.drag = 0f;
                if (m_PreviouslyGrounded && !m_Jumping)
                {
                    StickToGroundHelper();
                }
            }
            m_Jump = false;
        }


        private float SlopeMultiplier()
        {
            float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
            return movementSettings.SlopeCurveModifier.Evaluate(angle);
        }


        private void StickToGroundHelper()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) +
                                   advancedSettings.stickToGroundHelperDistance, ~0, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
                }
            }
        }


        private Vector2 GetInput()
        {
            
            Vector2 input = new Vector2
                {
                    x = CrossPlatformInputManager.GetAxis("Horizontal"),
                    y = CrossPlatformInputManager.GetAxis("Vertical")
                };
			movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }

        /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
        private void GroundCheck()
        {
            m_PreviouslyGrounded = m_IsGrounded;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, ~0, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                m_GroundContactNormal = hitInfo.normal;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundContactNormal = Vector3.up;
            }
            if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
            {
                m_Jumping = false;
            }
        }
    }
}
