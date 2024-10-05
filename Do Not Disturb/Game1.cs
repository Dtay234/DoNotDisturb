using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Do_Not_Disturb.Classes;

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

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState kbs;
        private KeyboardState prevKBS;
        private Player player;

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
            new Geometry(new Rectangle(0, 700, 1000, 100));
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.ApplyChanges();
            player = new Player(new Vector2(0, 0), new Rectangle(0, 0, 32, 32));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Player.spriteSheet = Content.Load<Texture2D>("Images/RedPanda");
            font = Content.Load<SpriteFont>("Fonts/File");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            prevKBS = kbs;
            kbs = Keyboard.GetState();
           
           

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
            player.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
