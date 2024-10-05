﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Do_Not_Disturb.Classes;
using Do_Not_Disturb.Classes.Puzzle;
using System.Collections.Generic;

namespace Do_Not_Disturb
{
    enum GameStates
    {
        Menu,
        Game,
        PauseScreen,
        Loading,
    }
    public class Game1 : Game
    {
        public static SpriteFont font;
        public static Texture2D pixel;
        public static List<GameObject> objects = new();
        public static Texture2D title;
        public static Texture2D loading;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kbs;
        private KeyboardState prevKBS;
        private GameStates gameState = GameStates.Menu;
        
     

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            

            //remove this later

            objects.Add(new Block(new Vector2(500, 500), new Rectangle(0, 0, 100, 100), BlockTypes.ARedBlock));

            //objects.Add(new Bubble(new Vector2(300, 600), new Rectangle(100000, 10000, 20, 20)));

            
            Level level = new Level("../../../Content/levels/level0.csv");
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.ApplyChanges();
            new Player(new Vector2(500, 350));

            Camera.globalOffset = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Player.spriteSheet = Content.Load<Texture2D>("Images/RedPanda");
            pixel = Content.Load<Texture2D>("Images/WhitePixel");
            font = Content.Load<SpriteFont>("Fonts/File");
            title = Content.Load<Texture2D>("Images/Title");
            loading = Content.Load<Texture2D>("Images/LoadingSpriteSheet");
            Geometry.LoadBlocks(Content);

            Vector2 loadingScreenPosition = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

           
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            prevKBS = kbs;
            kbs = Keyboard.GetState();
            
            switch (gameState){
                case GameStates.Menu:
                {
                    kbs = Keyboard.GetState();
                        if (kbs.IsKeyDown(Keys.Enter))
                        {
                            gameState = GameStates.Game;
                        }
                     prevKBS = kbs;
                    break;
                }

                case GameStates.PauseScreen:
                {
                    break;
                }

                case GameStates.Loading:
                    {
                        
                        break;
                    }

                case GameStates.Game:
                {
                        
                        foreach (GameObject obj in objects)
                        {
                            if (obj is Player)
                            {
                                ((Player)obj).Update(gameTime, kbs, prevKBS);
                            }
                            
                            else
                            {
                                obj.Update(gameTime);
                            }

                        }

                        GameObject temp;
                        if ((temp = objects.Find(x => x is Bubble && ((Bubble)x).popped)) != null)
                        {
                            objects.Remove(temp);
                        }

                        break;
                }
               
            }
           


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            _spriteBatch.Begin();

            switch (gameState)
            {
                case GameStates.Menu:
                {
                        _spriteBatch.Draw(title, new Rectangle(150,150,652,260), Color.White);
                        break;
                }
                case GameStates.Loading:
                    {
                        
                        break;
                    }
                case GameStates.Game:
                    {
                        foreach (Geometry box in Geometry.map)
                        {
                            box.Draw(_spriteBatch);
                        }
                        foreach (GameObject obj in objects)
                        {
                            obj.Draw(_spriteBatch);
                            _spriteBatch.Draw(pixel, new Rectangle(
                                Camera.RelativePosition(obj.Hitbox.Location.ToVector2()).ToPoint(),
                                obj.Hitbox.Size), Color.White);
                        }
                        break;
                    }
                case GameStates.PauseScreen:
                    {
                        break;
                    }
            }

            // TODO: Add your drawing code here
            

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public static bool Collide(Rectangle rect)
        {
            if (GeometryCollide(rect) != null || ObjectCollide(rect) != null)
            {
                return true;
            }

            return false;
        }

        public static bool Collide(Rectangle rect, out Geometry geo, out GameObject obj)
        {
            geo = GeometryCollide(rect);
            obj = ObjectCollide(rect);

            if (geo != null || obj != null)
            {
                return true;
            }

            geo = null;
            obj = null;
            return false;
        }

        public static Geometry GeometryCollide(Rectangle rect)
        {
            Geometry returned = null;

            foreach (Geometry box in Geometry.map)
            {
                if(box.BoundBox.Intersects(rect))
                {
                    returned = box;
                }
            }

            return returned;
        }

        public static GameObject ObjectCollide(Rectangle rect)
        {
            GameObject returned = null;

            foreach (GameObject box in objects)
            {
                if (box.Hitbox != rect && box.Hitbox.Intersects(rect))
                {
                    returned = box;
                }
            }

            return returned;
        }

        public static bool CheckGrounded(GameObject obj)
        {
            bool temp1 = GeometryCollide(new Rectangle(obj.Hitbox.Location + new Point(0, 5), obj.Hitbox.Size)) != null;
            bool temp2 = false;
            
            foreach (GameObject box in objects)
            {
                if (!obj.Equals(box) && (box.Hitbox.Intersects(new Rectangle(obj.Hitbox.Location + new Point(0, 5), obj.Hitbox.Size))))
                {
                    temp2 = true;
                }
            }

            return temp1 || temp2;
        }
    }
}
