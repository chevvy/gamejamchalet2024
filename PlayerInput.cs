using System.Collections.Generic;
using System.Linq;
using Godot;

public enum PlayerID
{
  P1, P2, P3, P4, P5
}
public enum InputAction
{
  MoveLeft, MoveRight, MoveUp, MoveDown, Interact, Start
}
public partial class PlayerInput
{
  public static Dictionary<PlayerID, string> PlayerTagByID = new(){
    {PlayerID.P1, "p1"},
    {PlayerID.P2, "p2"},
    {PlayerID.P3, "p3"},
    {PlayerID.P4, "p4"},
    {PlayerID.P5, "p5"}
  };

  private static readonly Dictionary<InputAction, string> InputByName = new() {
    { InputAction.MoveLeft, "_move_left" },
    { InputAction.MoveRight, "_move_right" },
    { InputAction.MoveUp, "_move_up"},
    { InputAction.MoveDown, "_move_down"},
    { InputAction.Interact, "_interact" },
    { InputAction.Start, "_start" },
  };
  private Dictionary<InputAction, string> _inputs;

  private readonly PlayerID _id;

  public PlayerInput(PlayerID id)
  {
    _id = id;
    var playerName = PlayerTagByID[id];
    (InputAction action, string actionName) getInputInfo(InputAction action) => (action, actionName: playerName + InputByName[action]);

    _inputs = new();
    foreach (var inputWithName in InputByName.ToList())
    {
      var (action, actionName) = getInputInfo(inputWithName.Key);
      _inputs[action] = actionName;
    }
  }

  public string GetInputKey(InputAction action) => _inputs[action];
}
