using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Do_Not_Disturb.Classes;
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
        public static List<Collidable> collidableList = new();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kbs;
        private KeyboardState prevKBS;
        private GameStates gameState = GameStates.Game;
        
     

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
            new Geometry(new Rectangle(0, 700, 1000, 100), BlockTypes.NormalLongBlock);
            new Geometry(new Rectangle(900, 0, 100, 1000), BlockTypes.NormalLongBlock);

            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.ApplyChanges();
            collidableList.Add(new Player(new Vector2(0, 0), new Rectangle(0, 0, 96, 96)));

            Camera.globalOffset = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Player.spriteSheet = Content.Load<Texture2D>("Images/RedPanda");
            pixel = Content.Load<Texture2D>("Images/WhitePixel");
            font = Content.Load<SpriteFont>("Fonts/File");
            Geometry.LoadBlocks(Content);
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
                    break;
                }

                case GameStates.PauseScreen:
                {
                    break;
                }

                case GameStates.Game:
                {
                        foreach (Collidable collidable in collidableList)
                        {
                            if (collidable is Player)
                            {
                                ((Player)collidable).Update(gameTime, kbs);
                            }
                            else
                            {
                                collidable.Update(gameTime);
                            }

                        }

                        break;
                }

                case GameStates.Loading:
                {
                        break;
                }
            }
           


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // TODO: Add your drawing code here
            foreach (Geometry box in Geometry.map)
            {
                box.Draw(_spriteBatch);
            }
            foreach (Collidable collidable in collidableList)
            {
                collidable.Draw(_spriteBatch);
                _spriteBatch.Draw(pixel, new Rectangle(
                    Camera.RelativePosition(collidable.Hitbox.Location.ToVector2()).ToPoint(),
                    collidable.Hitbox.Size), Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
