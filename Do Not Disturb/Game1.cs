﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Do_Not_Disturb.Classes;
using Do_Not_Disturb.Classes.Puzzle;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Reflection;

namespace Do_Not_Disturb
{
    enum GameStates
    {
        Menu,
        Game,
        PauseScreen,
        Loading,
        Information
    }
    public class Game1 : Game
    {
        private enum Animations
        {
            Default,
            None
        }
        public static SpriteFont font;
        public static Texture2D pixel;
        public static List<GameObject> objects = new();
        public static Texture2D title;
        public static Texture2D loading;
        public static float Scale = 1.5f;
 
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kbs;
        private KeyboardState prevKBS;
        private GameStates gameState = GameStates.Menu;
        private Level lastLevel;
        private List<Level> levelList = new List<Level>();
        private Animation<Animations> anim;

        private RED levelCompleteCondition;

        private Texture2D background;
        private Texture2D pressEnter;
        private Texture2D Paused;
        private Song titleSong;
        private Song gameSong;
        public static SoundEffect toyCar;
        public static SoundEffectInstance toyCarInstance;

        private int levelIndex;
        private int maxLevelIndex;
        private MouseState prevMS;
        MouseState ms;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        protected override void Initialize()
        {
            loading = Content.Load<Texture2D>("Images/LoadingSpriteSheet");
            anim = new Animation<Animations>("loadScreen.txt", loading);
            //anim.ChangeAnimation(Animations.None, 0, false);

            try
            {
                int i = 1;
                while (true)
                {
                    StreamReader reader = new StreamReader("../../../Content/levels/level" + i + ".csv");
                    Level level = new Level("../../../Content/levels/level" + i + ".csv", "../../../Content/levels/level" + i + "_object.csv");
                    levelList.Add(level);
                    i++;
                }
            }
            catch
            {

            }

            Player.spriteSheet = Content.Load<Texture2D>("Images/RedPanda");
            Car.sheet = Content.Load<Texture2D>("Images/CarSheet");
            levelIndex = -1;
            NextLevel();
            
            
            


            lastLevel = levelList[levelIndex];
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.ApplyChanges();

            
            

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
            Bubble.sprite = Content.Load<Texture2D>("Images/BubbleSprite");
            Geometry.tileset = Content.Load<Texture2D>("Images/AllBlocks");
            background = Content.Load<Texture2D>("Images/BackGround");
            titleSong = Content.Load<Song>("Audio/TitlleMusic");
            Car.sheet = Content.Load<Texture2D>("Images/CarSheet");
            gameSong = Content.Load<Song>("Audio/GameMusic");
            toyCar = Content.Load<SoundEffect>("Audio/ToyCar");
            toyCarInstance = toyCar.CreateInstance();




            pressEnter = Content.Load<Texture2D>("Images/PressEnter");
            Paused = Content.Load<Texture2D>("Images/Paused");

            Vector2 loadingScreenPosition = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            
        }

        public void TitleSong(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(titleSong);
        }

        public void GameSong(object sender, System.EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(gameSong);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            anim.Update(gameTime);


            prevMS = ms;
            ms = Mouse.GetState();

            prevKBS = kbs;
            kbs = Keyboard.GetState();
            
             
            
            switch (gameState){
                case GameStates.Menu:
                {       
                        if(titleSong.PlayCount == 0)
                        {
                            
                            TitleSong(gameTime, System.EventArgs.Empty);
                        }
                      
                        kbs = Keyboard.GetState();
                        if (kbs.IsKeyDown(Keys.Enter))
                        {
                            if(levelIndex <= 0)
                            {
                                gameState = GameStates.Information;
                            } else
                            {
                                gameState = GameStates.Game;
                                
                            }
                            MediaPlayer.Stop();

                        }
                     prevKBS = kbs;
                    break;
                }

                case GameStates.PauseScreen:
                {
                        if (kbs.IsKeyDown(Keys.P) && prevKBS.IsKeyUp(Keys.P))
                        {
                            gameState = GameStates.Game;
                            MediaPlayer.Resume();
                        }

                        break;
                }

                case GameStates.Information:
                    if( kbs.IsKeyDown(Keys.I) && prevKBS.IsKeyUp(Keys.I))
                    {
                        gameState = GameStates.Game;
                        MediaPlayer.Resume();
                    }
                    break;

                

                case GameStates.Loading:
                    {
                        
                        break;
                    }

                case GameStates.Game:
                {
                        Bubble.cooldown -= gameTime.ElapsedGameTime.TotalSeconds;

                        if (gameSong.PlayCount == 0)
                        {
                            GameSong(gameTime, System.EventArgs.Empty);
                        }

                        if (kbs.IsKeyDown(Keys.P) && prevKBS.IsKeyUp(Keys.P))
                        {
                            gameState = GameStates.PauseScreen;
                            MediaPlayer.Pause();
                            
                        }

                        if(kbs.IsKeyDown(Keys.I) && prevKBS.IsKeyUp(Keys.I))
                        {
                            gameState = GameStates.Information;
                            MediaPlayer.Pause();
                            
                        }
                        
                        if (kbs.IsKeyDown(Keys.R) && prevKBS.IsKeyUp(Keys.R))
                        {
                           ResetLevel();
                        }


                            for (int i = 0; i < objects.Count; i++) 
                        {
                            GameObject obj = objects[i];

                            if (obj is Player)
                            {
                                ((Player)obj).Update(gameTime, kbs, prevKBS, ms, prevMS);
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

                        levelCompleteCondition.Update(gameTime);

                        break;
                }
               
            }
           


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            

            switch (gameState)
            {

                case GameStates.Menu:

                {
                        _spriteBatch.Draw(background, new Rectangle(0, 0, 1920, 1080), Color.White);

                        _spriteBatch.Draw(title, new Rectangle(665,600,652,260), Color.White);

                        _spriteBatch.Draw(pressEnter, new Rectangle(740, 900, 226 * 2, 72 * 2), Color.White);
                        
                        break;
                }
                case GameStates.Loading:
                    {
                        
                        break;
                    }
                case GameStates.Game:
                    {
                        Point parallaxOffset = Camera.Parallax(8).ToPoint();
                        _spriteBatch.Draw(background,
                                new Rectangle(
                                    _graphics.PreferredBackBufferWidth / 2 - (int)(background.Width * 1.5f + parallaxOffset.X),
                                    _graphics.PreferredBackBufferHeight / 2 - (int)(background.Height * 1.5f + parallaxOffset.Y),
                                    background.Width * 3, background.Height * 3),
                                background.Bounds,
                                Color.White,
                                0f, Vector2.Zero,
                                SpriteEffects.None,
                                0);

                        

                        foreach (Geometry box in Geometry.map)
                        {
                            box.Draw(_spriteBatch);
                        }
                        foreach (GameObject obj in objects)
                        {
                            
                            
                            obj.Draw(_spriteBatch);
                            /*
                            _spriteBatch.Draw(pixel, new Rectangle(
                                Camera.RelativePosition(obj.Hitbox.Location.ToVector2()).ToPoint(),
                                obj.Hitbox.Size), Color.White);
                            */

                            
                        }
                        break;
                    }

                case GameStates.Information:
                    





                            foreach (Geometry box in Geometry.map)
                            {
                                box.Draw(_spriteBatch);
                            }
                            foreach (GameObject obj in objects)
                            {


                                obj.Draw(_spriteBatch);
                                /*
                                _spriteBatch.Draw(pixel, new Rectangle(
                                    Camera.RelativePosition(obj.Hitbox.Location.ToVector2()).ToPoint(),
                                    obj.Hitbox.Size), Color.White);
                                */


                            }

                          

                            _spriteBatch.Draw(pixel, new Rectangle(0, 0, 2000, 2000), Color.Gray);

                            _spriteBatch.Draw(pixel, new Rectangle(300, 50, 1250, 1000), Color.Black);

                            _spriteBatch.DrawString(font, "\n   NO PARENTS ALLOWED!! \n   All you need to have fun is your panda friend Red. \n" +
                                "   Spell his name with blocks to complete the level." + "\n\n   No one can make you leave your world of whimsy." + 
                                "\n\n" +
                                "   Controls:" +
                                "\n   A : Move Left" +
                                "\n   D : Move Right" +
                                "\n   S : Crouch Down" +
                                "\n   Space : Jump" +
                                "\n   Left-click : Form a bubble to bounce around" +
                                "\n   P : Pause" +
                                "\n   R : Reset" +
                                "\n\n   I : Open and close information page" +
                                "\n   Esc : Quit" +
                                "\n   ", new Vector2(300, 50), Color.White);;
                                break;
                case GameStates.PauseScreen:
                    {
                        





                        foreach (Geometry box in Geometry.map)
                        {
                            box.Draw(_spriteBatch);
                        }
                        foreach (GameObject obj in objects)
                        {


                            obj.Draw(_spriteBatch);
                            /*
                            _spriteBatch.Draw(pixel, new Rectangle(
                                Camera.RelativePosition(obj.Hitbox.Location.ToVector2()).ToPoint(),
                                obj.Hitbox.Size), Color.White);
                            */


                        }

                        _spriteBatch.Draw(pixel, new Rectangle(0, 0, 2000, 2000), Color.Gray);
                        _spriteBatch.Draw(Paused, new Rectangle(960 - (116 * 2), 540 - (47 * 2) - 30, 116 * 4, 47 * 4), Color.White);
                        break;
                    }
            }

            anim.DrawScreen(_spriteBatch, new Rectangle(0, 0, 1920, 1080), 0);

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

        public void NextLevel()
        {
            anim.ChangeAnimation(Animations.Default, 0, true);
            //anim.ChangeAnimation(Animations.None, 0, false);

            //Thread.Sleep(1000);
            objects.Clear();
            Geometry.map.Clear();

            if(levelIndex >= levelList.Count - 1)
            {
                levelIndex = -1;
                gameState = GameStates.Menu;
                return;
            }

            levelIndex++;
            levelList[levelIndex].loadWorld();
            levelCompleteCondition = levelList[levelIndex].loadActualObjects();



            levelCompleteCondition.LevelComplete += NextLevel;

        }
        public void ResetLevel()
        {
            levelIndex--;

            NextLevel();

        }
    }
}
