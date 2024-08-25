// Decompiled with JetBrains decompiler
// Type: ScriptTool.BuildMobRoster
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#nullable disable
namespace ScriptTool
{
  internal class BuildMobRoster : ScriptSection
  {
    private const string defaultMobRoster = "function buildMobRoster(_mobList){\r\n    //You can copy the roster of an existing level by calling :\r\n    addMobRosterFrom(\"PrisonStart\", _mobList); // Valid values for level name are : PrisonStart, PrisonCourtyard, PrisonDepths, SewerShort, PrisonRoof,\r\n                                                   //Ossuary, SewerDepths, Bridge, BeholderPit, PrisonCorrupt, StiltVillage, AncienTemple, Crypt, \r\n                                                   //ClockTower, TopClockTower, Castle, Throne\r\n    //Add here mobs to the roster passed as argument\r\n    //ex to add a mob : copy/paste this block for each different type of mob you want (mobName)\r\n    //var mobZombie = new Mob();\r\n    //{\r\n    //    mobZombie.mobName = \"Zombie\"; //Valid values are: Zombie, FastZombie, FlyZombie, S_Fly, WormZombie, S_Worm, S_WallEggWorm, EliteSideKick,\r\n                                          //Scorpio, Hammer, Grenader, ClusterGrenader, Archer, Ninja, Runner, LeapingDuelyst, Shield, SipkedStatyr,\r\n                                          //Worm, BatDasher, BatKamikaze, Fly, Shielder, Comboter, Hooker, AxeThrower, Spawner, Spawnling, Spinner,\r\n                                          //PirateChief, Spiker, Shoker, Mage360, OrbLauncher, Golem (careful with room sizes), Fogger, CastleKnight,\r\n                                          //Lancer\r\n    //    mobZombie.quantityFactor = 1.0;\r\n    //    mobZombie.singleRoom = false;\r\n    //    mobZombie.singleRoomRatio = -1; //-1 is default\r\n    //    mobZombie.minCombatRoomsBefore = 0;\r\n    //    mobZombie.maxCombatRoomsBefore = 0;\r\n    //    mobZombie.minDifficulty = 0;\r\n    //    mobZombie.maxDifficulty = 5; //Be careful that max difficulty < current difficulty will not spawn the mob\r\n    //}\r\n    //_mobList.push(mobZombie);\r\n    //Uncomment next line to deactivate the automatic mob scaling (activated by default is nothing is called)\r\n    //setAutomaticMobScaling(false);\r\n}\r\n";

    public override bool isOptional => false;

    public override string ToString()
    {
      if (LevelMobForm.mobRoster.Count <= 0)
        return "function buildMobRoster(_mobList){\r\n    //You can copy the roster of an existing level by calling :\r\n    addMobRosterFrom(\"PrisonStart\", _mobList); // Valid values for level name are : PrisonStart, PrisonCourtyard, PrisonDepths, SewerShort, PrisonRoof,\r\n                                                   //Ossuary, SewerDepths, Bridge, BeholderPit, PrisonCorrupt, StiltVillage, AncienTemple, Crypt, \r\n                                                   //ClockTower, TopClockTower, Castle, Throne\r\n    //Add here mobs to the roster passed as argument\r\n    //ex to add a mob : copy/paste this block for each different type of mob you want (mobName)\r\n    //var mobZombie = new Mob();\r\n    //{\r\n    //    mobZombie.mobName = \"Zombie\"; //Valid values are: Zombie, FastZombie, FlyZombie, S_Fly, WormZombie, S_Worm, S_WallEggWorm, EliteSideKick,\r\n                                          //Scorpio, Hammer, Grenader, ClusterGrenader, Archer, Ninja, Runner, LeapingDuelyst, Shield, SipkedStatyr,\r\n                                          //Worm, BatDasher, BatKamikaze, Fly, Shielder, Comboter, Hooker, AxeThrower, Spawner, Spawnling, Spinner,\r\n                                          //PirateChief, Spiker, Shoker, Mage360, OrbLauncher, Golem (careful with room sizes), Fogger, CastleKnight,\r\n                                          //Lancer\r\n    //    mobZombie.quantityFactor = 1.0;\r\n    //    mobZombie.singleRoom = false;\r\n    //    mobZombie.singleRoomRatio = -1; //-1 is default\r\n    //    mobZombie.minCombatRoomsBefore = 0;\r\n    //    mobZombie.maxCombatRoomsBefore = 0;\r\n    //    mobZombie.minDifficulty = 0;\r\n    //    mobZombie.maxDifficulty = 5; //Be careful that max difficulty < current difficulty will not spawn the mob\r\n    //}\r\n    //_mobList.push(mobZombie);\r\n    //Uncomment next line to deactivate the automatic mob scaling (activated by default is nothing is called)\r\n    //setAutomaticMobScaling(false);\r\n}\r\n";
      string str1 = "function buildMobRoster(_mobList){\r\n    ";
      HashSet<string> stringSet = new HashSet<string>();
      foreach (Mob mob in (Collection<Mob>) LevelMobForm.mobRoster)
      {
        if (mob.mobName != "null" && stringSet.Add(mob.mobName))
        {
          List<string> list = ((IEnumerable<string>) JsonConvert.SerializeObject((object) mob).Split(',')).ToList<string>();
          string str2 = "mob" + mob.mobName;
          int index = 0;
          while (index < list.Count)
          {
            list[index] = list[index].Replace("\"", "");
            list[index] = list[index].Replace(":", " = ");
            string str3 = list[index].Trim();
            if (str3.IndexOf('{') == 0)
            {
              list[index] = list[index].Substring(list[index].IndexOf('{') + 1);
              list[index] = "    " + str2 + "." + list[index].Replace(" = ", " = \"") + "\";";
              list.Insert(index, "{");
              list.Insert(index, "var " + str2 + " = new Mob();");
              index += 3;
            }
            else if (str3.IndexOf('}') == str3.Length - 1)
            {
              list[index] = "    " + str2 + "." + list[index].Substring(0, list[index].IndexOf('}')) + ";";
              list.Insert(index + 1, "}");
              index = list.Count;
            }
            else
            {
              list[index] = "    " + str2 + "." + list[index] + ";";
              ++index;
            }
          }
          list.Add("_mobList.push(" + str2 + ");\r\n");
          foreach (string str4 in list)
            str1 = str1 + "\r\n    " + str4;
        }
      }
      return str1 + "\n}";
    }
  }
}
