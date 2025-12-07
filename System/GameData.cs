using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

public static class GameData
{
    public static KeyboardState previous {get;set;}
    public static bool Exit {get;set;}
    public static bool Cube1 {get;set;}
    public static bool Cube2 {get;set;}
    public static bool Cube3 {get;set;}
    public static bool Cube4 {get;set;}
    public static Color[] CubeColor = new Color[4];
}