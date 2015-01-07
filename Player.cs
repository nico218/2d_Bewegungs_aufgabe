using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileMaps
{
    class Player
    {
        public Vector2 Position { get; private set; }
        public Texture2D Image { get; private set; }

        public Player(Texture2D image)
        {
            this.Image = image;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 mapPosition = new Vector2(this.Position.X * Map.TileWidth, this.Position.Y * Map.TileHeight);
            spriteBatch.Draw(this.Image, mapPosition, Color.White);
        }

        public void Move(Vector2 moveDirection)
        {
            this.Position += moveDirection;
        }
    }
}
