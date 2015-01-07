using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileMaps
{
    class Map
    {
        public const int TileWidth = 32;
        public const int TileHeight = 32;

        public int MapWidth = 10;
        public int MapHeight = 10;
        public Texture2D TileSheet;

        private Tile[,] tiles;

        public void RenderMap(SpriteBatch spriteBatch)
        {
            if (TileSheet == null)
            {
                throw new Exception("TileSheet is null");
            }

            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    Tile tile = this.tiles[x, y];
                    Vector2 position = new Vector2(x * TileWidth, y * TileHeight);
                    Rectangle sourceRect = new Rectangle((int)tile.Type * TileWidth, 0, TileWidth, TileHeight);
                    spriteBatch.Draw(this.TileSheet, position, sourceRect, Color.White);
                }
            }
        }

        public void LoadMapFromTextfile(string mapPath, int width, int height)
        {
            this.InitMapSize(width, height);
            string cleanContent = this.LoadAndClean(mapPath);
            this.FillTileData(cleanContent);
        }

        public void LoadMapFromImage(Texture2D image)
        {
            this.InitMapSize(image.Width, image.Height);
            Color[] data = this.GetColorData(image);
            this.FillTileData(data);
        }

        public Tile GetTile(Vector2 position)
        {
            if (position.X >= 0 && position.X < MapWidth && position.Y >= 0 && position.Y < MapHeight)
            {
                return this.tiles[(int)position.X, (int)position.Y];
            }
            return null;
        }

        private string LoadAndClean(string path)
        {
            StreamReader reader = new StreamReader(path);
            string fileContent = reader.ReadToEnd();
            reader.Close();

            return fileContent.Replace("\r\n", "");
        }

        private void InitMapSize(int width, int height)
        {
            this.MapWidth = width;
            this.MapHeight = height;
            this.tiles = new Tile[width, height];
        }

        private void FillTileData(string data)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int index = y * MapWidth + x;
                    char tileType = data[index];
                    this.tiles[x, y] = this.GetTileByType(tileType);
                }
            }
        }

        private void FillTileData(Color[] data)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int index = y * MapWidth + x;
                    Color tileType = data[index];
                    this.tiles[x, y] = this.GetTileByType(tileType);
                }
            }
        }

        private Tile GetTileByType(char type)
        {
            int tileId = type - '0';
            return new Tile((Tile.Types)tileId);
        }

        private Tile GetTileByType(Color color)
        {
            // green
            if (color == new Color(0, 255, 0))
            {
                return new Tile(Tile.Types.Path);
            } // blue
            else if (color == new Color(0, 0, 255))
            {
                return new Tile(Tile.Types.Water);
            }
            else
            {
                return null;
            }
        }

        private Color[] GetColorData(Texture2D image)
        {
            Color[] data = new Color[image.Width * image.Height];
            image.GetData<Color>(data);

            return data;
        }
    }
}
