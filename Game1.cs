#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace TileMaps
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map map = new Map();
        Player player;

        float delay = 0;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.map.TileSheet = Content.Load<Texture2D>("Tiles");
            this.map.LoadMapFromImage(Content.Load<Texture2D>("Map_2"));
            //this.map.LoadMapFromTextfile("Content/Map.txt", 10, 3);
            this.player = new Player(Content.Load<Texture2D>("Player"));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            this.ProcessInput(gameTime);
            base.Update(gameTime);
            Console.WriteLine(delay);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            this.map.RenderMap(spriteBatch);
            this.player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ProcessInput(GameTime gametime)
        {
            delay = gametime.ElapsedGameTime.Milliseconds;

            // Keyboard
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 moveDirection = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.D))
            {
                moveDirection.X++;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                moveDirection.X--;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                moveDirection.Y--;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                moveDirection.Y++;
            }

            if (moveDirection != Vector2.Zero)
            {
                this.MovePlayer(moveDirection);
            }

            // Mouse
            MouseState mouseState = Mouse.GetState();
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                Vector2 target = this.ConvertScreenToWorldPoint(mouseState.X, mouseState.Y);
                Vector2 direction = checkDistance(target, this.player);


                if ((int)target.X == player.Position.X + (int)direction.X && (int)target.Y == player.Position.Y || (int)target.X == player.Position.X - (int)direction.X && (int)target.Y == player.Position.Y || (int)target.Y == player.Position.Y + (int)direction.Y && (int)target.X == player.Position.X || (int)target.Y == player.Position.Y - (int)direction.Y && (int)target.X == player.Position.X)
                {
                    delay += gametime.ElapsedGameTime.Milliseconds;

                    if (delay >= 23)
                    {
                        this.MovePlayer(direction);
                        delay = 0;
                    }
                    else
                    {
                        Console.WriteLine("not possible");
                        Console.WriteLine(delay);
                    }
                }
            }
        }
        private void MovePlayer(Vector2 moveDirection)
        {
            Tile nextTile = this.map.GetTile(this.player.Position + moveDirection);
            if (nextTile != null)
            {
                if (nextTile.Type == Tile.Types.Path)
                {
                    this.player.Move(moveDirection);
                }
            }
        }
        private Vector2 checkDistance(Vector2 target, Player player)
        {
            Vector2 distance = target - this.player.Position;

            return distance;
        }

        private Vector2 ConvertScreenToWorldPoint(int x, int y)
        {
            int tileX = x / Map.TileWidth;
            int tileY = y / Map.TileHeight;

            return new Vector2(tileX, tileY);
        }
    }
}
