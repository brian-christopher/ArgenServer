using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Argen.Demo;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SocketWrapper _wrapper;

    private Texture2D _texture;
    private List<Vector2> _players = [];

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _wrapper = new SocketWrapper();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
        _wrapper.OnConnected += OnConnected;
        _wrapper.OnDisconnected += OnDisconnected;
        _wrapper.OnDataReceived += OnDataReceived;

        _wrapper.ConnectToHost("127.0.0.1", 7666);
    }

    private void OnDataReceived(string obj)
    {
    }

    private void OnDisconnected()
    {
    }

    private void OnConnected()
    {
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _texture = new Texture2D(GraphicsDevice, 32, 32, false, SurfaceFormat.Color);

        Color[] data = new Color[32 * 32];
        
        for (int i = 0; i < data.Length; ++i)
        {
            data[i] = Color.Red;
        }

        _texture.SetData(data);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _wrapper.PoolEvents();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        foreach (var position in _players)
        {
            _spriteBatch.Draw(_texture, position, Color.White);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
