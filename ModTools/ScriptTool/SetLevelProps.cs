﻿// Decompiled with JetBrains decompiler
// Type: ScriptTool.SetLevelProps
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

#nullable disable
namespace ScriptTool
{
  internal class SetLevelProps : ScriptSection
  {
    private const string defaultLevelInfo = "\r\n//Optional\r\nfunction SetLevelProps(_levelProps){\r\n    //Use a preset from existing level by using the \"setLevelPropsFrom\" and changing the LevelName to a valid levelName.\r\n    setLevelPropsFrom(\"PrisonStart\", _levelProps); //Valid levelName are : PrisonStart, PrisonCourtyard, PrisonDepths, SewerShort, PrisonRoof,\r\n                                //Ossuary, SewerDepths, Bridge, BeholderPit, PrisonCorrupt, StiltVillage, AncienTemple, Crypt, ClockTower, TopClockTower,\r\n                                //Castle, Throne\r\n\r\n    //Change here the props parameters you want:\r\n    //_levelProps.brutalityTier = 5;\r\n    //_levelProps.timedDoor = 2; //in minutes\r\n    //_levelProps.timedBluprint = \"DashShield\"; \r\n    //_levelProps.timedGoldMul = 1.0;\r\n    //_levelProps.timedScrolls = 2;\r\n    //_levelProps.survivalTier = 1;\r\n    //_levelProps.wind = 0.2;\r\n    //_levelProps.musicIntro = \"music/prisonstart_intro.ogg\";\r\n    //_levelProps.musicLoop = \"music/prisonstart_loop.ogg\";\r\n    //_levelProps.doorColor = 10110973;\r\n    //_levelProps.zDoorColor = 10110973;\r\n    //_levelProps.chromaColor = 10110973;\r\n    //_levelProps.loadingColor = 10110973;\r\n    //_levelProps.loadingDescColor = 10110973;\r\n}";

    public override bool isOptional => true;

    public override string ToString()
    {
      return "\r\n//Optional\r\nfunction SetLevelProps(_levelProps){\r\n    //Use a preset from existing level by using the \"setLevelPropsFrom\" and changing the LevelName to a valid levelName.\r\n    setLevelPropsFrom(\"PrisonStart\", _levelProps); //Valid levelName are : PrisonStart, PrisonCourtyard, PrisonDepths, SewerShort, PrisonRoof,\r\n                                //Ossuary, SewerDepths, Bridge, BeholderPit, PrisonCorrupt, StiltVillage, AncienTemple, Crypt, ClockTower, TopClockTower,\r\n                                //Castle, Throne\r\n\r\n    //Change here the props parameters you want:\r\n    //_levelProps.brutalityTier = 5;\r\n    //_levelProps.timedDoor = 2; //in minutes\r\n    //_levelProps.timedBluprint = \"DashShield\"; \r\n    //_levelProps.timedGoldMul = 1.0;\r\n    //_levelProps.timedScrolls = 2;\r\n    //_levelProps.survivalTier = 1;\r\n    //_levelProps.wind = 0.2;\r\n    //_levelProps.musicIntro = \"music/prisonstart_intro.ogg\";\r\n    //_levelProps.musicLoop = \"music/prisonstart_loop.ogg\";\r\n    //_levelProps.doorColor = 10110973;\r\n    //_levelProps.zDoorColor = 10110973;\r\n    //_levelProps.chromaColor = 10110973;\r\n    //_levelProps.loadingColor = 10110973;\r\n    //_levelProps.loadingDescColor = 10110973;\r\n}";
    }
  }
}
