using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using TiledSharp;

namespace TheShadow;

public class Map : IScene
{
    private GraphicsDevice _graphics;
    private SceneManager _sceneManager;
    private ContentManager _content;
    private Texture2D _playerTexture;
    private Texture2D _ghostTexture;
    private MapManager _mapmanager;
    
    private TmxMap _map;

    private Player _player;
    private Player _ghost;

    private Camera2D _camera;

    private List<Rectangle> _solidTiles;

    private List<Rectangle> _button1;
    private List<Rectangle> _button2;
    private List<Rectangle> _button3;
    private List<Rectangle> _button4;

    private List<Rectangle> _door1;
    private List<Rectangle> _door2;
    private List<Rectangle> _door3;
    private List<Rectangle> _door4;

    private List<Rectangle> _redCube;
    private List<Rectangle> _blueCube;
    private List<Rectangle> _yellowCube;
    private List<Rectangle> _greenCube;

    private List<Rectangle> _state1;
    private bool _state1Status = false;

    private List<Rectangle> _state2;
    private bool _state2Status = false;
    
    private List<Rectangle> _state3;
    private bool _state3Status = false;

    private List<Rectangle> _state4;
    private bool _state4Status = false;

    private List<Rectangle> _trashLayer;
    private List<Rectangle> _controlLayer;

    private Color?[] _colorArray = new Color?[4];

    private Texture2D _hand;

    private bool _inHand = false;
    private int _handId = 0;

    private Dictionary<string, bool> _layerVisible = new();

    private bool _playerControl = true;

    private List<Rectangle> LoadListObject(string path, string name)
    {
        TmxMap map = new TmxMap(path);
        var solidTiles = new List<Rectangle>();

        foreach(var objectGroups in map.ObjectGroups)
        {
            if(objectGroups.Name == name)
            {
                foreach(var obj in objectGroups.Objects)
                {
                    if(obj.Width > 0 && obj.Height > 0)
                    {
                        var rect = new Rectangle(
                            (int)obj.X,
                            (int)obj.Y,
                            (int)obj.Width,
                            (int)obj.Height
                        );

                        solidTiles.Add(rect);
                    }
                }
            }
        }

        return solidTiles;
    }

    public Map(GraphicsDevice _graphics, SceneManager _sceneManager, ContentManager _content)
    {
        this._graphics = _graphics;
        this._sceneManager = _sceneManager;
        this._content = _content;
    }

    public void LoadContent()
    {
        _map = new TmxMap("Content/map.tmx");
        
        var dict = new Dictionary<int, Texture2D>();

        foreach(var ts in _map.Tilesets)
        {
            string asset = Path.GetFileNameWithoutExtension(ts.Image.Source);
            dict[ts.FirstGid] = _content.Load<Texture2D>(asset);
        }

        foreach(var layer in _map.Layers)
        {
            _layerVisible[layer.Name] = _map.Layers[layer.Name].Visible;
        }

        _playerTexture = _content.Load<Texture2D>("player");
        _ghostTexture = _content.Load<Texture2D>("ghost");

        _mapmanager = new MapManager(_map, dict);

        _solidTiles = LoadListObject("Content/map.tmx", "Collision");

        _button1 = LoadListObject("Content/map.tmx", "Button1");
        _button2 = LoadListObject("Content/map.tmx", "Button2");
        _button3 = LoadListObject("Content/map.tmx", "Button3");
        _button4 = LoadListObject("Content/map.tmx", "Button4");

        _door1 = LoadListObject("Content/map.tmx", "Door1");
        _door2 = LoadListObject("Content/map.tmx", "Door2");
        _door3 = LoadListObject("Content/map.tmx", "Door3");
        _door4 = LoadListObject("Content/map.tmx", "Door4");

        _redCube = LoadListObject("Content/map.tmx", "RedCube");
        _blueCube = LoadListObject("Content/map.tmx", "BlueCube");
        _yellowCube = LoadListObject("Content/map.tmx", "YellowCube");
        _greenCube = LoadListObject("Content/map.tmx", "GreenCube");

        _state1 = LoadListObject("Content/map.tmx", "State1");
        _state2 = LoadListObject("Content/map.tmx", "State2");
        _state3 = LoadListObject("Content/map.tmx", "State3");
        _state4 = LoadListObject("Content/map.tmx", "State4");

        _trashLayer = LoadListObject("Content/map.tmx", "Trash");
        _controlLayer = LoadListObject("Content/map.tmx", "Control");

        _hand = new Texture2D(_graphics, 1, 1);
        _hand.SetData(new[] {Color.White});

        _camera = new Camera2D(_graphics.Viewport);
        
        _player = new Player(new Vector2(704, 704));
        _ghost = new Player(new Vector2(720, 704));
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();

        bool Button1 = false;
        bool Button2 = false;
        bool Button3 = false;
        bool Button4 = false;

        List<Rectangle> solidTiles = new();

        foreach(Rectangle solid in _solidTiles)
        {
            solidTiles.Add(solid);
        }

        if(state.IsKeyDown(Keys.E) && !GameData.previous.IsKeyDown(Keys.E) && _playerControl)
        {
            foreach(Rectangle solid in _yellowCube)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    _inHand = true;
                    _handId = 2;
                }
            }

            foreach(Rectangle solid in _blueCube)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    _inHand = true;
                    _handId = 4;
                }
            }

            foreach(Rectangle solid in _redCube)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    _inHand = true;
                    _handId = 1;
                }
            }

            foreach(Rectangle solid in _greenCube)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    _inHand = true;
                    _handId = 3;
                }
            }

            foreach(Rectangle solid in _trashLayer)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    _inHand = false;
                    _handId = 0;
                }
            }

            foreach(Rectangle solid in _controlLayer)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    _sceneManager.ChangeScene("control");
                }
            }

            foreach(Rectangle solid in _state1)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    if(_state1Status)
                    {
                        if(_inHand == false && _handId == 0)
                        {
                            if(GameData.Cube1)
                            {
                                if(GameData.CubeColor[0] == Color.Red)
                                {
                                    _handId = 1;
                                }

                                if(GameData.CubeColor[0] == Color.Yellow)
                                {
                                    _handId = 2;
                                }

                                if(GameData.CubeColor[0] == Color.Green)
                                {
                                    _handId = 3;
                                }

                                if(GameData.CubeColor[0] == Color.Blue)
                                {
                                    _handId = 4;
                                }

                            } else {
                                
                                if(_colorArray[0] == Color.Red)
                                {
                                    _handId = 1;
                                }

                                if(_colorArray[0] == Color.Yellow)
                                {
                                    _handId = 2;
                                }

                                if(_colorArray[0] == Color.Green)
                                {
                                    _handId = 3;
                                }

                                if(_colorArray[0] == Color.Blue)
                                {
                                    _handId = 4;
                                }

                                _colorArray[0] = Color.White;

                            }
                            
                            _layerVisible["State1"] = true;
                            _layerVisible["State1Red"] = false;
                            _layerVisible["State1Yellow"] = false;
                            _layerVisible["State1Green"] = false;
                            _layerVisible["State1Blue"] = false;

                            _inHand = true;
                            _state1Status = false;
                            GameData.Cube1 = false;
                            } 
                        
                        } else {

                            if((GameData.CubeColor[0] == Color.Red && _handId == 1))
                            {
                                _layerVisible["State1"] = false;
                                _layerVisible["State1Red"] = true;
                                GameData.Cube1 = true;
                                _state1Status = true;
                            } else if(_handId == 1)
                            {
                                _layerVisible["State1"] = false;
                                _layerVisible["State1Red"] = true;
                                _colorArray[0] = Color.Red;
                                _state1Status = true;
                            }

                            if((GameData.CubeColor[0] == Color.Yellow && _handId == 2))
                            {
                                _layerVisible["State1"] = false;
                                _layerVisible["State1Yellow"] = true;
                                GameData.Cube1 = true;
                                _state1Status = true;
                            } else if(_handId == 2)
                            {
                                _layerVisible["State1"] = false;
                                _layerVisible["State1Yellow"] = true;
                                _colorArray[0] = Color.Yellow;
                                _state1Status = true;
                            }

                            if((GameData.CubeColor[0] == Color.Green && _handId == 3))
                            {
                                _layerVisible["State1"] = false;
                                _layerVisible["State1Green"] = true;
                                GameData.Cube1 = true;
                                _state1Status = true;
                            } else if(_handId == 3)
                            {
                                _layerVisible["State1"] = false;
                                _layerVisible["State1Green"] = true;
                                _colorArray[0] = Color.Green;
                                _state1Status = true;
                            }

                            if((GameData.CubeColor[0] == Color.Blue && _handId == 4))
                            {
                                _layerVisible["State1"] = false;
                                _layerVisible["State1Blue"] = true;
                                GameData.Cube1 = true;
                                _state1Status = true;
                            } else if(_handId == 4)
                            {
                                _layerVisible["State1"] = false;
                                _layerVisible["State1Blue"] = true;
                                _colorArray[0] = Color.Blue;
                                _state1Status = true;
                            }

                            _handId = 0;
                            _inHand = false;
                    }
                }
            }

            foreach(Rectangle solid in _state2)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    if(_state2Status)
                    {
                        if(_inHand == false && _handId == 0)
                        {
                            if(GameData.Cube2)
                            {
                                if(GameData.CubeColor[1] == Color.Red)
                                {
                                    _handId = 1;
                                }

                                if(GameData.CubeColor[1] == Color.Yellow)
                                {
                                    _handId = 2;
                                }

                                if(GameData.CubeColor[1] == Color.Green)
                                {
                                    _handId = 3;
                                }

                                if(GameData.CubeColor[1] == Color.Blue)
                                {
                                    _handId = 4;
                                }

                            } else {
                                
                                if(_colorArray[1] == Color.Red)
                                {
                                    _handId = 1;
                                }

                                if(_colorArray[1] == Color.Yellow)
                                {
                                    _handId = 2;
                                }

                                if(_colorArray[1] == Color.Green)
                                {
                                    _handId = 3;
                                }

                                if(_colorArray[1] == Color.Blue)
                                {
                                    _handId = 4;
                                }

                                _colorArray[1] = Color.White;

                            }
                            
                            _layerVisible["State2"] = true;
                            _layerVisible["State2Red"] = false;
                            _layerVisible["State2Yellow"] = false;
                            _layerVisible["State2Green"] = false;
                            _layerVisible["State2Blue"] = false;

                            _inHand = true;
                            _state2Status = false;
                            GameData.Cube2 = false;
                            } 
                        
                        } else {

                            if((GameData.CubeColor[1] == Color.Red && _handId == 1))
                            {
                                _layerVisible["State2"] = false;
                                _layerVisible["State2Red"] = true;
                                GameData.Cube2 = true;
                                _state2Status = true;
                            } else if(_handId == 1)
                            {
                                _layerVisible["State2"] = false;
                                _layerVisible["State2Red"] = true;
                                _colorArray[1] = Color.Red;
                                _state2Status = true;
                            }

                            if((GameData.CubeColor[1] == Color.Yellow && _handId == 2))
                            {
                                _layerVisible["State2"] = false;
                                _layerVisible["State2Yellow"] = true;
                                GameData.Cube2 = true;
                                _state2Status = true;
                            } else if(_handId == 2)
                            {
                                _layerVisible["State2"] = false;
                                _layerVisible["State2Yellow"] = true;
                                _colorArray[1] = Color.Yellow;
                                _state2Status = true;
                            }

                            if((GameData.CubeColor[1] == Color.Green && _handId == 3))
                            {
                                _layerVisible["State2"] = false;
                                _layerVisible["State2Green"] = true;
                                GameData.Cube2 = true;
                                _state2Status = true;
                            } else if(_handId == 3)
                            {
                                _layerVisible["State2"] = false;
                                _layerVisible["State2Green"] = true;
                                _colorArray[1] = Color.Green;
                                _state2Status = true;
                            }

                            if((GameData.CubeColor[1] == Color.Blue && _handId == 4))
                            {
                                _layerVisible["State2"] = false;
                                _layerVisible["State2Blue"] = true;
                                GameData.Cube2 = true;
                                _state2Status = true;
                            } else if(_handId == 4)
                            {
                                _layerVisible["State2"] = false;
                                _layerVisible["State2Blue"] = true;
                                _colorArray[1] = Color.Blue;
                                _state2Status = true;
                            }

                            _handId = 0;
                            _inHand = false;
                    }
                }
            }

            foreach(Rectangle solid in _state3)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    if(_state3Status)
                    {
                        if(_inHand == false && _handId == 0)
                        {
                            if(GameData.Cube3)
                            {
                                if(GameData.CubeColor[2] == Color.Red)
                                {
                                    _handId = 1;
                                }

                                if(GameData.CubeColor[2] == Color.Yellow)
                                {
                                    _handId = 2;
                                }

                                if(GameData.CubeColor[2] == Color.Green)
                                {
                                    _handId = 3;
                                }

                                if(GameData.CubeColor[2] == Color.Blue)
                                {
                                    _handId = 4;
                                }

                            } else {
                                
                                if(_colorArray[2] == Color.Red)
                                {
                                    _handId = 1;
                                }

                                if(_colorArray[2] == Color.Yellow)
                                {
                                    _handId = 2;
                                }

                                if(_colorArray[2] == Color.Green)
                                {
                                    _handId = 3;
                                }

                                if(_colorArray[2] == Color.Blue)
                                {
                                    _handId = 4;
                                }

                                _colorArray[2] = Color.White;

                            }
                            
                            _layerVisible["State3"] = true;
                            _layerVisible["State3Red"] = false;
                            _layerVisible["State3Yellow"] = false;
                            _layerVisible["State3Green"] = false;
                            _layerVisible["State3Blue"] = false;

                            _inHand = true;
                            _state3Status = false;
                            GameData.Cube3 = false;
                            } 
                        
                        } else {

                            if((GameData.CubeColor[2] == Color.Red && _handId == 1))
                            {
                                _layerVisible["State3"] = false;
                                _layerVisible["State3Red"] = true;
                                GameData.Cube3 = true;
                                _state3Status = true;
                            } else if(_handId == 1)
                            {
                                _layerVisible["State3"] = false;
                                _layerVisible["State3Red"] = true;
                                _colorArray[2] = Color.Red;
                                _state3Status = true;
                            }

                            if((GameData.CubeColor[2] == Color.Yellow && _handId == 2))
                            {
                                _layerVisible["State3"] = false;
                                _layerVisible["State3Yellow"] = true;
                                GameData.Cube3 = true;
                                _state3Status = true;
                            } else if(_handId == 2)
                            {
                                _layerVisible["State3"] = false;
                                _layerVisible["State3Yellow"] = true;
                                _colorArray[2] = Color.Yellow;
                                _state3Status = true;
                            }

                            if((GameData.CubeColor[2] == Color.Green && _handId == 3))
                            {
                                _layerVisible["State3"] = false;
                                _layerVisible["State3Green"] = true;
                                GameData.Cube3 = true;
                                _state3Status = true;
                            } else if(_handId == 3)
                            {
                                _layerVisible["State3"] = false;
                                _layerVisible["State3Green"] = true;
                                _colorArray[2] = Color.Green;
                                _state3Status = true;
                            }

                            if((GameData.CubeColor[2] == Color.Blue && _handId == 4))
                            {
                                _layerVisible["State3"] = false;
                                _layerVisible["State3Blue"] = true;
                                GameData.Cube3 = true;
                                _state3Status = true;
                            } else if(_handId == 4)
                            {
                                _layerVisible["State3"] = false;
                                _layerVisible["State3Blue"] = true;
                                _colorArray[2] = Color.Blue;
                                _state3Status = true;
                            }

                            _handId = 0;
                            _inHand = false;
                    }
                }
            }

            foreach(Rectangle solid in _state4)
            {
                if(_player.Hitbox.Intersects(solid))
                {
                    if(_state4Status)
                    {
                        if(_inHand == false && _handId == 0)
                        {
                            if(GameData.Cube4)
                            {
                                if(GameData.CubeColor[3] == Color.Red)
                                {
                                    _handId = 1;
                                }

                                if(GameData.CubeColor[3] == Color.Yellow)
                                {
                                    _handId = 2;
                                }

                                if(GameData.CubeColor[3] == Color.Green)
                                {
                                    _handId = 3;
                                }

                                if(GameData.CubeColor[3] == Color.Blue)
                                {
                                    _handId = 4;
                                }

                            } else {
                                
                                if(_colorArray[3] == Color.Red)
                                {
                                    _handId = 1;
                                }

                                if(_colorArray[3] == Color.Yellow)
                                {
                                    _handId = 2;
                                }

                                if(_colorArray[3] == Color.Green)
                                {
                                    _handId = 3;
                                }

                                if(_colorArray[3] == Color.Blue)
                                {
                                    _handId = 4;
                                }

                                _colorArray[3] = Color.White;

                            }
                            
                            _layerVisible["State4"] = true;
                            _layerVisible["State4Red"] = false;
                            _layerVisible["State4Yellow"] = false;
                            _layerVisible["State4Green"] = false;
                            _layerVisible["State4Blue"] = false;

                            _inHand = true;
                            _state4Status = false;
                            GameData.Cube4 = false;
                            } 
                        
                        } else {

                            if((GameData.CubeColor[3] == Color.Red && _handId == 1))
                            {
                                _layerVisible["State4"] = false;
                                _layerVisible["State4Red"] = true;
                                GameData.Cube4 = true;
                                _state4Status = true;
                            } else if(_handId == 1)
                            {
                                _layerVisible["State4"] = false;
                                _layerVisible["State4Red"] = true;
                                _colorArray[3] = Color.Red;
                                _state4Status = true;
                            }

                            if((GameData.CubeColor[3] == Color.Yellow && _handId == 2))
                            {
                                _layerVisible["State4"] = false;
                                _layerVisible["State4Yellow"] = true;
                                GameData.Cube4 = true;
                                _state4Status = true;
                            } else if(_handId == 2)
                            {
                                _layerVisible["State4"] = false;
                                _layerVisible["State4Yellow"] = true;
                                _colorArray[3] = Color.Yellow;
                                _state4Status = true;
                            }

                            if((GameData.CubeColor[3] == Color.Green && _handId == 3))
                            {
                                _layerVisible["State4"] = false;
                                _layerVisible["State4Green"] = true;
                                GameData.Cube4 = true;
                                _state4Status = true;
                            } else if(_handId == 3)
                            {
                                _layerVisible["State4"] = false;
                                _layerVisible["State4Green"] = true;
                                _colorArray[3] = Color.Green;
                                _state4Status = true;
                            }

                            if((GameData.CubeColor[3] == Color.Blue && _handId == 4))
                            {
                                _layerVisible["State4"] = false;
                                _layerVisible["State4Blue"] = true;
                                GameData.Cube4 = true;
                                _state4Status = true;
                            } else if(_handId == 4)
                            {
                                _layerVisible["State4"] = false;
                                _layerVisible["State4Blue"] = true;
                                _colorArray[3] = Color.Blue;
                                _state4Status = true;
                            }

                            _handId = 0;
                            _inHand = false;
                    }
                }
            }
        }

        if(state.IsKeyDown(Keys.T) && !GameData.previous.IsKeyDown(Keys.T))
        {
            _playerControl = !_playerControl;
        }

        foreach(Rectangle button in _button1)
        {
            if(_player.Hitbox.Intersects(button) || _ghost.Hitbox.Intersects(button))
            {
                Button1 = true;
            }
        }

        foreach(Rectangle button in _button2)
        {
            if(_player.Hitbox.Intersects(button) || _ghost.Hitbox.Intersects(button))
            {
                Button2 = true;
            }
        }

        foreach(Rectangle button in _button3)
        {
            if(_player.Hitbox.Intersects(button) || _ghost.Hitbox.Intersects(button))
            {
                Button3 = true;
            }
        }

        foreach(Rectangle button in _button4)
        {
            if(_player.Hitbox.Intersects(button) || _ghost.Hitbox.Intersects(button))
            {
                Button4 = true;
            }
        }

        if(!Button1)
        {
            foreach(Rectangle solid in _door1)
            {
                solidTiles.Add(solid);
            }
            
            _layerVisible["door_1"] = true;

        } else {
            _layerVisible["door_1"] = false;
        }

        if(!Button2)
        {
            foreach(Rectangle solid in _door2)
            {
                solidTiles.Add(solid);
            }

            _layerVisible["door_2"] = true;

        } else {
            _layerVisible["door_2"] = false;
        }

        if(!Button3)
        {
            foreach(Rectangle solid in _door3)
            {
                solidTiles.Add(solid);
            }

            _layerVisible["door_3"] = true;

        } else {
            _layerVisible["door_3"] = false;
        }

        if(!Button4)
        {
            foreach(Rectangle solid in _door4)
            {
                solidTiles.Add(solid);
            }

            _layerVisible["door_4"] = true;

        } else {
            _layerVisible["door_4"] = false;
        }

        if(_playerControl)
        {
            _player.Update(gameTime, solidTiles, _camera);
            _camera.Follow(_player.Position, new Vector2(_map.Width * 16, _map.Height * 16));
        } else
        {
            _ghost.Update(gameTime, solidTiles, _camera);
            _camera.Follow(_ghost.Position, new Vector2(_map.Width * 16, _map.Height * 16));
        }

        GameData.previous = state;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        float LayerDeep = 0.01f;

        foreach(var layer in _map.Layers)
        {
            if(_layerVisible[layer.Name])
            {
                _mapmanager.DrawLayer(layer, spriteBatch, LayerDeep, _camera);
                LayerDeep+= 0.01f;
            }
        }

        if(_inHand)
        {
            if(_handId == 1)
            {
                spriteBatch.Draw(_hand, new Rectangle((int)(_player.screenPos.X + 70), (int)(_player.screenPos.Y + 10), 20, 20), (Rectangle?)null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            }

            if(_handId == 2)
            {
                spriteBatch.Draw(_hand, new Rectangle((int)(_player.screenPos.X + 70), (int)(_player.screenPos.Y + 10), 20, 20), (Rectangle?)null, Color.Yellow, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            }

            if(_handId == 3)
            {
                spriteBatch.Draw(_hand, new Rectangle((int)(_player.screenPos.X + 70), (int)(_player.screenPos.Y + 10), 20, 20), (Rectangle?)null, Color.Green, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            }

            if(_handId == 4)
            {
                spriteBatch.Draw(_hand, new Rectangle((int)(_player.screenPos.X + 70), (int)(_player.screenPos.Y + 10), 20, 20), (Rectangle?)null, Color.Blue, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
            }
        }

        _player.Draw(spriteBatch, _playerTexture, _camera);
        _ghost.Draw(spriteBatch, _ghostTexture, _camera);
    }
}