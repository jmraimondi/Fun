using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;

namespace ROR
{
    class UI : MonoBehaviour
    {
        Camera cam;
        RoR2.LocalUser local;
        List<RoR2.PurchaseInteraction> activeChests;
        List<RoR2.PurchaseInteraction> chests;
        RoR2.TeleporterInteraction tele;

        RoR2.Stage stage;
        Type stageType = typeof(RoR2.Stage);
        FieldInfo nextStage;
        string curStage;

        Type BI;
        FieldInfo BIopened;
        List<RoR2.BarrelInteraction> barrels;

        List<RoR2.ItemDisplay> items;

        public void Start()
        {  
            cam = null;
            local = null;
            activeChests = new List<RoR2.PurchaseInteraction>();
            tele = null;
            stage = null;
            items = null;


            BI = typeof(RoR2.BarrelInteraction);
            BIopened = BI.GetField("opened", BindingFlags.NonPublic | BindingFlags.Instance);
            barrels = new List<RoR2.BarrelInteraction>();

            nextStage = stageType.GetField("nextStage", BindingFlags.NonPublic | BindingFlags.Instance);
            curStage = "";
        }

        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 150, 25), "Ampulex Cheat v1.0");
            //GUI.Label(new Rect(200, 10, 150, 20), "local: " + local.userProfile.name + " ID: " + local.id.ToString());
            GUI.Label(new Rect(200, 10, 150, 20), "chests: " + chests.Count.ToString());
            //GUI.Label(new Rect(200, 50, 150, 20), "cam: " + cam.name.ToString());
            GUI.Label(new Rect(200, 50, 150, 20), local.cachedMasterController.crosshairPosition.ToString());
            if (chests.Count != 0)
            {
                foreach (RoR2.PurchaseInteraction pi in chests)
                {
                    if (!stage.completed)
                    {
                        if (!activeChests.Contains(pi) && pi.available)
                        {
                            activeChests.Add(pi);
                        }
                        else if (activeChests.Contains(pi) && !pi.available)
                        {
                            activeChests.Remove(pi);
                        }
                    }
                    if(local.cachedBody.master.money == 0)
                    {
                        //curStage = (string)nextStage.GetValue(stage);
                        activeChests.Clear();
                        barrels.Clear();
                    }

                    if (tele != null && cam != null)
                    {
                        var telew2s = cam.WorldToScreenPoint(tele.transform.position);
                        if (telew2s.z > 0)
                        {
                            DrawBox(telew2s.x, Screen.height - telew2s.y, 40, 40, Color.black, "Teleporter");
                        }

                    }

                    var tar = pi.transform.position;
                    if (cam != null)
                    {
                        var w2s = cam.WorldToScreenPoint(tar);
                        if (w2s.z > 0 && pi.available)
                        {
                            var str = "";
                            var money = (int)local.cachedBody.master.money;
                            var cost = pi.cost;
                            var color = Color.white;
                            var name = pi.GetDisplayName();

                            switch(name)
                            {
                                case "Large Chest":
                                    {
                                        color = Color.green;
                                        break;
                                    }
                                case "Legendary Chest":
                                    {
                                        color = new Color(212f, 175f, 55f); //Gold
                                        break;
                                    }
                                case "Equipment Barrel":
                                    {
                                        color = new Color(255f, 140f, 0f); //dark orange
                                        break;
                                    }
                                case "Fan":
                                    {
                                        color = Color.gray;
                                        break;
                                    }
                                case "Newt Altar":
                                    {
                                        color = Color.cyan;
                                        break;
                                    }
                                case "Radio Scanner":
                                case "Broken Missile Drone":
                                default:
                                    {
                                        color = Color.white;
                                        break;
                                    }
                            }

                            if (pi.costType == RoR2.CostType.Money)
                            {
                                str = "$" + money + "/" + cost;
                                if(money < cost)
                                {
                                    color = Color.red;
                                }
                            }
                            str += " " + name;
                            DrawBox(w2s.x, Screen.height - w2s.y, 20, 20, color, str);
                        }
                    }
                    
                }
                
            }

            if(barrels.Count > 0)
            {
                foreach(RoR2.BarrelInteraction b in barrels)
                {
                    if((bool)BIopened.GetValue(b))
                    {
                        barrels.Remove(b);
                        continue;
                    }
                    if(cam != null)
                    {
                        var tar = b.transform.position;
                        var w2s = cam.WorldToScreenPoint(tar);
                        if(w2s.z > 0)
                        {
                            DrawBox(w2s.x, Screen.height - w2s.y, 10, 10, Color.gray, b.goldReward.ToString());
                        }
                    }
                }
            }

            /*
            if(items.Count > 0)
            {
                foreach (RoR2.ItemDisplay i in items)
                {
                    if(cam != null)
                    {
                        var tar = i.transform.position;
                        var w2s = cam.WorldToScreenPoint(tar);
                        if(w2s.z > 0)
                        {
                            DrawBox(w2s.x, Screen.height - w2s.y, 20, 20, Color.blue);
                        }
                    }
                }
            }
            */
        }


        public void Update()
        {

            if(chests.Count == 0)
            {
                var c = FindObjectsOfType<RoR2.PurchaseInteraction>();
                foreach(RoR2.PurchaseInteraction p in c)
                {
                    chests.Add(p);
                }
            }
            if (cam == null || !cam.Equals(Camera.main))
            {
                if(local == null)
                {
                    local = RoR2.LocalUserManager.GetFirstLocalUser();
                }
                cam = Camera.main;
            }
            if(local != null)
            {
                local.cachedBody.SetSpreadBloom(float.MinValue, false);
                //local.cachedBody.
            }
            if(tele == null)
            {
                tele = RoR2.TeleporterInteraction.instance;
            }
            if(stage == null)
            {
                //var stg = FindObjectOfType<RoR2.Stage>();
                stage = RoR2.Stage.instance;
                //if(curStage == "")
                //{
                //    curStage = (string)nextStage.GetValue(stage);
                //}
            }
            if(barrels.Count == 0)
            {
                var b = FindObjectsOfType<RoR2.BarrelInteraction>();
                foreach(RoR2.BarrelInteraction bi in b)
                {
                    if(!(bool)BIopened.GetValue(bi))
                    {
                        barrels.Add(bi);
                    }
                }
            }

            /*
            if(items == null)
            {
                var i = FindObjectsOfType<RoR2.ItemDisplay>();
                foreach(RoR2.ItemDisplay ii in i)
                {
                    items.Add(ii);
                }
            }
            
            */
        }

        public static void DrawBox(float x, float y, float h, float w, Color color, string text = null)
        {
            //Vector2 start = new Vector2(x, y
            GUI.color = color;
            GUI.DrawTexture(new Rect(x - (h / 2), y - (w / 2), w, 1), Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.DrawTexture(new Rect(x + (h / 2), y - (w / 2), 1, h + 1), Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.DrawTexture(new Rect(x - (h / 2), y - (w / 2), 1, h), Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.DrawTexture(new Rect(x - (h / 2), y + (w / 2), w + 1, 1), Texture2D.whiteTexture, ScaleMode.StretchToFill);
            if (text != null)
            {
                GUI.Label(new Rect(x - (text.Length) * 3, y + h + 2, 150, 100), text);
            }
        }
    }
}
