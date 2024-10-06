using Microsoft.Xna.Framework;
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

        private Song titleSong;
        private Song gameSong;
        private int levelIndex;
        private int maxLevelIndex;


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
            levelIndex = -1;
            NextLevel();
            
            
            Car.sheet = Content.Load<Texture2D>("Images/CarSheet");


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

            Vector2 loadingScreenPosition = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            anim.Update(gameTime);
            

            prevKBS = kbs;
            kbs = Keyboard.GetState();
            
            switch (gameState){
                case GameStates.Menu:
                {       
                        if(titleSong.PlayCount == 0)
                        {
                            MediaPlayer.Play(titleSong);
                            MediaPlayer.IsRepeating = true;
                        }
                     
                        kbs = Keyboard.GetState();
                        if (kbs.IsKeyDown(Keys.Enter))
                        {
                            gameState = GameStates.Game;
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

                case GameStates.Loading:
                    {
                        
                        break;
                    }

                case GameStates.Game:
                {
                        if (gameSong.PlayCount == 0)
                        {
                            MediaPlayer.Play(gameSong);
                            MediaPlayer.IsRepeating = true;
                        }
                       
                        if (kbs.IsKeyDown(Keys.R))
                        {
                            ResetLevel();
                        }

                        if (kbs.IsKeyDown(Keys.P) && prevKBS.IsKeyUp(Keys.P))
                        {
                            gameState = GameStates.PauseScreen;
                            MediaPlayer.Pause();
                            
                        }

                        for (int i = 0; i < objects.Count; i++) 
                        {
                            GameObject obj = objects[i];

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

                        _spriteBatch.Draw(title, new Rectangle(750,600,652,260), Color.White);
                        
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
                case GameStates.PauseScreen:
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

                        _spriteBatch.Draw(pixel, new Rectangle(0, 0, 2000, 2000), Color.Gray);
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
