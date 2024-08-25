// Decompiled with JetBrains decompiler
// Type: ScriptTool.BuildMainRooms
// Assembly: ScriptTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F2726570-3B7A-4F0B-A465-A10C11BE2151
// Assembly location: D:\SteamLibrary\steamapps\common\Dead Cells\ModTools\ScriptTool.exe

#nullable disable
namespace ScriptTool
{
  internal class BuildMainRooms : ScriptSection
  {
    private const string defaultBuildMainRooms = "function buildMainRooms(){\r\n    //Add here the main structure of your level (ex : entrance, exits, shops, treasures etc)\r\n    //Creating an entrance and calling it \"start\" is mandatory.\r\n    //Example:\r\n    Struct.createRoomWithType(\"Entrance\").setName(\"start\")\r\n        .chain(Struct.createRoomWithType(\"Combat\"))\r\n        .chain(Struct.createExit(\"Main\").setTitleAndColor(\"My Awesome Level\", 8888888).setName(\"Main\");\r\n}\r\n";

    public override bool isOptional => false;

    public override string ToString()
    {
      return "function buildMainRooms(){\r\n    //Add here the main structure of your level (ex : entrance, exits, shops, treasures etc)\r\n    //Creating an entrance and calling it \"start\" is mandatory.\r\n    //Example:\r\n    Struct.createRoomWithType(\"Entrance\").setName(\"start\")\r\n        .chain(Struct.createRoomWithType(\"Combat\"))\r\n        .chain(Struct.createExit(\"Main\").setTitleAndColor(\"My Awesome Level\", 8888888).setName(\"Main\");\r\n}\r\n";
    }
  }
}
