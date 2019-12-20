using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace pp
{
    class UI : MonoBehaviour
    {
        //private bool changed = false;
        private string cmgName = GameManager.CurrentMinigameDef.minigameName;
        private string state;
        private string controller;
        BarnBrawlPlayer localBarnPlayer;
        TreasureHuntPlayer localtHuntPlayer;
        IEnumerable<BarnBrawlPlayer> barnPlayers;
        IEnumerable<TreasureHuntObject> tHuntCollectibles;
        TreasureHuntPlayer p0;

        IEnumerable<SpeedDemonPlayer> saberPlayers;
        SpeedDemonPlayer localSaberPlayer;
        SpeedDemonController saberController;
        float saberPunch;

        IEnumerable<TreasureHuntPlayer> tHuntPlayers;
        int hitcount;
        int timesCalled;
        int collectibles;
        TreasureHuntController tHuntCon;
        static Assembly assembly = Assembly.LoadFile(@"C:\Program Files (x86)\Steam\steamapps\common\Pummel Party\PummelParty_Data\Managed\Assembly-CSharp.dll");
        //static Type bpPlayerType = assembly.GetType("BarnBrawlPlayer");

        Type bombType;
        FieldInfo intervalInfo;
        FieldInfo bombInfo;
        float[] bTimers;

        Type bbp;
        FieldInfo xhairInfo;
        MethodInfo shoot;
        Color hitColor;
        BarnBrawlController bbCon;

        SpookySpikesPlayer localSpikePlayer;
        SpookySpikesController spikeCon;
        Type ssc;
        FieldInfo spikesInfo;
        Type ssp;
        MethodInfo jumpInfo;
        MethodInfo crouchInfo;
        FieldInfo stateInfo;
        public static bool QDown;
        public static bool SPACEDown;

        SpellingPlayer localSpellPlayer;
        SpellingController spellCon;
        Type sp;
        FieldInfo curWord;
        


        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);
        public struct INPUT
        {
            public int type;
            public KEYBDINPUT ki;

        }
        internal struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        public void Start()
        {
            cmgName = "none";
            state = "no state";
            controller = "no controller";
            localBarnPlayer = null;
            localtHuntPlayer = null;
            barnPlayers = null;
            tHuntPlayers = null;
            tHuntCollectibles = null;
            hitcount = 0;
            timesCalled = 0;
            collectibles = 0;
            tHuntCon = null;
            p0 = null;

            saberPlayers = null;
            localSaberPlayer = null;
            saberController = null;
            saberPunch = 0;

            bombType = typeof(PassTheBombController);
            intervalInfo = bombType.GetField("intervalIndex", BindingFlags.NonPublic | BindingFlags.Instance);
            bombInfo = bombType.GetField("bombTimers", BindingFlags.NonPublic | BindingFlags.Instance);
            bTimers = new float[3] {-1f, -1f, -1f };

            bbp = typeof(BarnBrawlPlayer);
            xhairInfo = bbp.GetField("crossHair", BindingFlags.NonPublic | BindingFlags.Instance);
            hitColor = new Color(1f, 0f, 0f, 0.4f);
            shoot = bbp.GetMethod("DoShotgunShot", BindingFlags.NonPublic | BindingFlags.Instance);
            bbCon = null;

            localSpikePlayer = null;
            spikeCon = null;
            ssc = typeof(SpookySpikesController);
            spikesInfo = ssc.GetField("spikes", BindingFlags.NonPublic | BindingFlags.Instance);
            ssp = typeof(SpookySpikesPlayer);
            jumpInfo = ssp.GetMethod("Jump", BindingFlags.NonPublic | BindingFlags.Instance);
            crouchInfo = ssp.GetMethod("Crouch", BindingFlags.NonPublic | BindingFlags.Instance);
            stateInfo = ssp.GetField("curState", BindingFlags.NonPublic | BindingFlags.Instance);
            QDown = false;
            SPACEDown = false;

            localSpellPlayer = null;
            spellCon = null;
            sp = typeof(SpellingPlayer);

            curWord = sp.GetField("m_curWord", BindingFlags.NonPublic | BindingFlags.Instance);
            
        }

        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 150, 25), "Ampulex Cheat v1.0");
            GUI.Label(new Rect(200, 10, 300, 25), cmgName);
            GUI.Label(new Rect(200, 35, 300, 25), state);
            GUI.Label(new Rect(200, 60, 300, 25), controller);
            //GUI.Label(new Rect(1600, 10, 300, 25), "!" + cmgName + "!");
            //GUI.Label(new Rect(200, 60, 300, 25), state);

            switch (cmgName)
            {
                case "Acidic Atoll":
                    break;
                case "Altitude Attack":
                    break;
                case "Animal Arithmetic":
                   // GUI.Label(new Rect(1600, 10, 300, 25), "found aa");
                    AnimalArithmeticHelper();
                    break;
                case "Barn Brawl":
                    BarnBrawlHelper();
                    break;
                case "Bouncing Balls":
                    break;
                case "Breaking Blocks":
                    break;
                case "Bullet Barrage":
                    break;
                case "Cannon Circle":
                    break;
                case "Crown Capture":
                    break;
                case "Daring Dogfight":
                    break;
                case "Elemental Escalation":
                    //eeHelper();
                    break;
                case "Explosive Exchange":
                    bombHelper();
                    break;
                case "Gift Grab": //presents controller
                    break;
                case "Grifting Gifts":
                    break;
                case "Laser Leap":
                    break;
                case "Magma & Mages":
                    break;
                case "Miniature Motors":
                    break;
                case "Morphing Maze":
                    break;
                case "Mortar Mayhem":
                    break;
                case "Mystery Maze":
                    break;
                case "Rockin Rhythem":
                    break;
                case "Rotor Race":
                    break;
                case "Rusty Racers":
                    break;
                case "Sandy Search":
                    TreasureHuntHelper();
                    break;
                case "Searing Spotlights":
                    spotlightHelper();
                    break;
                case "Sidestep Slope":
                    break;
                case "Slippery Sprint":
                    break;
                case "Snowy Spin":
                    break;
                case "Sorcerers Sprint":
                    break;
                case "Speedy Sabers":
                    //saberHelper();
                    break;
                case "Spooky Spikes":
                    SpikesHelper();
                    break;
                case "Strategic Shockwave":
                    break;
                case "Swift Shooters":
                    shooterHelper();
                    break;
                case "Temporal Trails":
                    break;
                case "Thunderous Trench":
                    break;
                case "Traffic Theft":
                    break;
                case "Tunneling Tanks":
                    break;
                case "Word Wars":
                    SpellingHelper();
                    break;
            }
        }

        public void Update()
        {
            cmgName = GameManager.CurrentMinigameDef.minigameName;
            state = GameManager.Minigame.State.ToString();
            controller = GameManager.Minigame.ToString();
        }

        private void BarnBrawlHelper()
        {
            if (GameManager.Minigame.State == MinigameControllerState.EnablePlayers || GameManager.Minigame.State == MinigameControllerState.Countdown)
            {

                var bbCon = FindObjectOfType<BarnBrawlController>();
                barnPlayers = FindObjectsOfType<BarnBrawlPlayer>();
                foreach (BarnBrawlPlayer player in barnPlayers)
                {
                    if (player.GamePlayer.IsLocalPlayer && !player.GamePlayer.IsAI)
                    {
                        localBarnPlayer = player;
                        break;
                    }
                }
            }

            else if (GameManager.Minigame.State == MinigameControllerState.Playing)
            {
                
                UnityEngine.UI.Image xhair = (UnityEngine.UI.Image)xhairInfo.GetValue(localBarnPlayer);
                if(localBarnPlayer.HoldingShotgun && xhair.color.Equals(hitColor))
                {
                    shoot.Invoke(localBarnPlayer, new object[] { GameManager.rand.Next(0, int.MaxValue), null });
                }
                var cam = FindObjectOfType<Camera>();
                foreach (BarnBrawlPlayer p in barnPlayers)
                {

                    if (!p.gameObject.Equals(localBarnPlayer.gameObject))
                    {
                        var pos = p.transform.position;
                        var w2s = cam.WorldToScreenPoint(pos);
                        if (w2s.z > 0)
                        {
                            var c = p.GamePlayer.Color.uiColor;
                            if (p.HoldingShotgun)
                            {
                                c = Color.black;
                            }
                            DrawBox(w2s.x, Screen.height - w2s.y, 100, 100, c, p.GamePlayer.Name);
                        }
                    }

                }

                foreach (BarnBrawlShotgunPickup t in bbCon.ShotgunPickups)
                {
                    var w2s = cam.WorldToScreenPoint(t.lootSignal.transform.position);
                    if (w2s.z > 0)
                    {
                        var c = Color.magenta;
                        DrawBox(w2s.x, Screen.height - w2s.y, 50, 50, c);
                    }
                }
            }

            else if (GameManager.Minigame.State == MinigameControllerState.RoundResetWait)
            {
                bbCon = null;
            }
        }

        private void TreasureHuntHelper()
        {
            if (GameManager.Minigame.State == MinigameControllerState.RoundStartWait || GameManager.Minigame.State == MinigameControllerState.Countdown)
            {

                tHuntCon = FindObjectOfType<TreasureHuntController>();
                tHuntPlayers = FindObjectsOfType<TreasureHuntPlayer>();

                foreach (TreasureHuntPlayer t in tHuntPlayers)
                {
                    if (t.GamePlayer.IsLocalPlayer && !t.GamePlayer.IsAI)
                    {
                        p0 = t;
                        break;
                    }
                }
            }

            else if (GameManager.Minigame.State == MinigameControllerState.Playing)
            {
                var treasure = FindObjectOfType<TreasureHuntTreasure>();
                var cam = p0.cam;
                foreach (TreasureHuntObject c in tHuntCon.objects)
                {
                    var pos = c.transform.position;
                    var w2s = cam.WorldToScreenPoint(pos);
                    int d = (int)Vector3.Distance(p0.transform.position, pos);
                    if (w2s.z > 0)
                    {
                        if (c.gameObject.Equals(treasure.gameObject))
                        {
                            DrawBox(w2s.x, Screen.height - w2s.y, 40, 40, Color.red, "treasure");
                        }
                        else
                        {
                            DrawBox(w2s.x, Screen.height - w2s.y, 40, 40, Color.cyan, d.ToString());
                        }
                        //GUI.DrawTexture(new Rect(w2s.x, w2s.y, 20, 20), Texture2D.whiteTexture, ScaleMode.ScaleToFit);
                    }
                }

            }

            else if (GameManager.Minigame.State == MinigameControllerState.RoundResetWait)
            {
                tHuntCon = null;
                tHuntPlayers = null;
            }
        }

        private void eeHelper()
        {
            var eeCon = FindObjectOfType<ElementalMagesController>();
            var cam = eeCon.MinigameCamera;

            foreach(ElementalMagesController.Crystal crystal in eeCon.crystals)
            {
                var crystalPos = crystal.gameObject.transform.position;
                var w2s = cam.WorldToScreenPoint(crystalPos);
                DrawBox(w2s.x, Screen.height-w2s.y, 40, 40, Color.red);
            }
        }

        private void bombHelper()
        {
            var bombCon = FindObjectOfType<PassTheBombController>();
            GUI.Label(new Rect(200, 88, 300, 25), bombCon.State.ToString());
            if (bombCon.State == MinigameControllerState.Playing)
            {
                int interval = (int)intervalInfo.GetValue(bombCon);
                if (bTimers[0] == -1f)
                {
                    Array bombs = (Array)bombInfo.GetValue(bombCon);
                    for (int i = 0; i < 3; i++)
                    {
                        bTimers[i] = (float)bombs.GetValue(i);
                    }
                }
                var cam = bombCon.MinigameCamera;
                var target = bombCon.GetBombPlayer().transform.position;
                var w2s = cam.WorldToScreenPoint(target);
                var timer = (int)bTimers[interval] - bombCon.GetBombTimer();
                DrawBox(w2s.x, Screen.height - w2s.y, 200, 200, bombCon.GetBombPlayer().GamePlayer.Color.uiColor, timer.ToString());
            }

            if (bombCon.State == MinigameControllerState.RoundResetWait)
            {
                bTimers[0] = -1;
                bTimers[1] = -1;
                bTimers[2] = -1;
                bombCon = null;
            }
        }

        private void spotlightHelper()
        {
            var spotlightCon = FindObjectOfType<SpeedySpotlightsController>();
            if (spotlightCon.State == MinigameControllerState.Countdown || spotlightCon.State == MinigameControllerState.Playing)
            {
                var players = FindObjectsOfType<SpeedySpotlightsPlayer>();
                var cam = spotlightCon.MinigameCamera;
                foreach (SpeedySpotlightsPlayer t in players)
                {
                    var target = t.transform.position;
                    var w2s = cam.WorldToScreenPoint(target);
                    if (w2s.z > 0)
                    {
                        DrawBox(w2s.x, Screen.height - w2s.y, 100, 100, t.GamePlayer.Color.uiColor, t.GamePlayer.Name + " " + ((int)t.Health).ToString());
                    }
                }
            }
        }

        private void saberHelper()
        {
            if (saberController == null)
            {
                saberController = FindObjectOfType<SpeedDemonController>();
            }
            if(saberPlayers == null)
            {
                saberPlayers = FindObjectsOfType<SpeedDemonPlayer>();
            }

            if (localSaberPlayer == null)
            {
                foreach (SpeedDemonPlayer p in saberPlayers)
                {
                    if (p.GamePlayer.IsLocalPlayer && !p.GamePlayer.IsAI)
                    {
                        localSaberPlayer = p;
                        break;
                    }
                }
            }

            if(saberController.State == MinigameControllerState.Playing)
            {
                GUI.Label(new Rect(200, 60, 300, 25), Time.time.ToString());
                GUI.Label(new Rect(200, 85, 150, 25), saberPunch.ToString());
                foreach (SpeedDemonPlayer p in saberPlayers)
                {
                   // GUI.Label(new Rect(350, 85+(saberController.players.IndexOf(p.gameObject.)), 300, 25), Vector3.Distance(localSaberPlayer.transform.position, p.transform.position).ToString());
                    if (Vector3.Distance(localSaberPlayer.transform.position, p.transform.position) < 2.5f)
                    {
                        if (saberPunch == 0)
                        {
                            saberPunch = Time.time;
                            LeftClick();
                        }

                        else if(Time.time - saberPunch > 0.15f)
                        {
                            saberPunch = Time.time;
                            LeftClick();
                        }
                        
                    }
                }
            }
        }

        private void SpikesHelper()
        {
            if (GameManager.Minigame.State == MinigameControllerState.Countdown)
            {
                if (localSpikePlayer == null)
                {
                    var players = FindObjectsOfType<SpookySpikesPlayer>();
                    foreach (SpookySpikesPlayer p in players)
                    {
                        if (p.GamePlayer.IsLocalPlayer && !p.GamePlayer.IsAI)
                        {
                            localSpikePlayer = p;
                            break;
                        }
                    }
                }
                if (spikeCon == null)
                {
                    spikeCon = FindObjectOfType<SpookySpikesController>();
                }
            }

            else if (GameManager.Minigame.State == MinigameControllerState.Playing && localSpikePlayer.IsAlive)
            {
                List<GameObject> spikes = (List<GameObject>)spikesInfo.GetValue(spikeCon);
                var cam = spikeCon.MinigameCamera;

                foreach (GameObject s in spikes)
                {
                    if(s.transform.position.x == 0f)
                    {
                        if (s.transform.position.y == 1.8f)
                        {
                            if((int)stateInfo.GetValue(localSpikePlayer) == 0) //Q duck
                            {
                                
                            }
                            
                        }
                        else if (s.transform.position.y == 0.4f) //SPACE jump
                        {
                            if ((int)stateInfo.GetValue(localSpikePlayer) == 0)
                            {
                                ;
                            }
                        }
                    }
                    
                    var pos = s.transform.position;
                    var w2s = cam.WorldToScreenPoint(pos); //1.8 high  0.4 low
                    if (w2s.z > 0)
                    {
                        var info = s.transform.position.x.ToString() + ", " + s.transform.position.y.ToString()
                            + " state: " + ((int)stateInfo.GetValue(localSpikePlayer)).ToString();
                        DrawBox(w2s.x, Screen.height - w2s.y, 100, 100, Color.cyan, info);
                    }
                    
                }

            }
        }

        private void shooterHelper()
        {
            var shooters = FindObjectsOfType<SwiftShootersPlayer>();
            SwiftShootersPlayer localShooter = null;
            foreach(SwiftShootersPlayer s in shooters)
            {
                if (s.GamePlayer.IsLocalPlayer && !s.GamePlayer.IsAI)
                {
                    localShooter = s;
                    break;
                }
            }

            
        }

        private void SpellingHelper()
        {
            if(spellCon == null)
            {
                spellCon = FindObjectOfType<SpellingController>();
            }
            if(GameManager.Minigame.State == MinigameControllerState.Initializing)
            {
                if(localSpellPlayer == null)
                {
                    var spellPlayers = FindObjectsOfType<SpellingPlayer>();
                    foreach(SpellingPlayer p in spellPlayers)
                    {
                        if(p.GamePlayer.IsLocalPlayer && !p.GamePlayer.IsAI)
                        {
                            localSpellPlayer = p;
                            break;
                        }
                    }
                }
            }
            else if (GameManager.Minigame.State == MinigameControllerState.Playing)
            {
                var cam = spellCon.MinigameCamera;
                if (cam != null)
                {
                    string word = (string)curWord.GetValue(localSpellPlayer);
                    int prog = localSpellPlayer.WordProgress;
                    int index = word[prog] - 'a';
                    int index2 = -1;
                    if (prog < word.Length)
                    {
                        index2 = word[prog + 1] - 'a';
                    }
                    var pos = spellCon.GetSpellingButton(index);
                    var pos2 = spellCon.GetSpellingButton(index2);
                    var w2s = cam.WorldToScreenPoint(pos.transform.position);
                    DrawBox(w2s.x, Screen.height - w2s.y, 100, 100, Color.green);
                    if (index2 > -1)
                    {
                        var w2s2 = cam.WorldToScreenPoint(pos2.transform.position);
                        DrawBox(w2s2.x, Screen.height - w2s2.y, 100, 100, Color.yellow);
                    }
                }
            }
        }
        private void AnimalArithmeticHelper()
        {
            var countCon = FindObjectOfType<CountingController>();

            if (GameManager.Minigame.State == MinigameControllerState.Playing)
            {
                Display(countCon.curCorrectCount.ToString(), 0);
                
                var cam = countCon.MinigameCamera;
                var plist = FindObjectsOfType<CountingPlayer>();
                if(cam != null && plist != null)
                {
                    foreach(CountingPlayer p in plist)
                    {
                        var pos = p.transform.position;
                        var w2s = cam.WorldToScreenPoint(pos);
                        if (w2s.z > 0)
                        {
                            var c = p.GamePlayer.Color.uiColor;
                            DrawBox(w2s.x, Screen.height - w2s.y, 100, 100, c, p.guessCount.Value.ToString(), false);
                        }
                    }
                }
            }
        }
        public static void DrawBox(float x, float y, float h, float w, Color color, string text=null, bool box=true)
        {
            //Vector2 start = new Vector2(x, y
            GUI.color = color;
            if (box)
            {
                GUI.DrawTexture(new Rect(x - (h / 2), y - (w / 2), w, 1), Texture2D.whiteTexture, ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(x + (h / 2), y - (w / 2), 1, h + 1), Texture2D.whiteTexture, ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(x - (h / 2), y - (w / 2), 1, h), Texture2D.whiteTexture, ScaleMode.StretchToFill);
                GUI.DrawTexture(new Rect(x - (h / 2), y + (w / 2), w + 1, 1), Texture2D.whiteTexture, ScaleMode.StretchToFill);
            }
            if(text != null)
            {
                GUI.Label(new Rect(x - (text.Length)*3, y + h + 5, 150, 25), text);
            }
        }

        public static void Display(string str, int pos)
        {
            GUI.Label(new Rect(1600, (pos*25) + 10, 300, 25), str);

        }

        public static void LeftClick() //issue -  send left down - wait - left up
        {
        }

        public static void PressKey(ushort keycode, int i)
        {
            INPUT Input = new INPUT();
            Input.type = 1;
            Input.ki.Vk = keycode;
            if (keycode == 0x51) //Q
            {
                if (i == 0)
                {
                    Input.ki.Flags = 0;
                    QDown = true;
                }
                if (i == 1)
                {
                    Input.ki.Flags = 0x0002;
                    QDown = false;
                }
            }
            else if(keycode == 0x20) //SPACE
            {
                if (i == 0)
                {
                    Input.ki.Flags = 0;
                    SPACEDown = true;
                }
                if (i == 1)
                {
                    Input.ki.Flags = 0x0002;
                    SPACEDown = false;
                }
            }
            SendInput(1, ref Input, Marshal.SizeOf(Input));
        }
    }

}
