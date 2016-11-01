using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BalanceThings
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        // Delegate handler for accelerometer sensor
        public delegate Vector3 AccelerometerHandlerDelegate();

        // Handler for aboce delegate method
        protected AccelerometerHandlerDelegate GetAccelerometerVector;

        public Game(AccelerometerHandlerDelegate accelerometerHandler)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;

            GetAccelerometerVector = accelerometerHandler;
        }

        protected virtual void Restart()
        {
            Log.D("Restarting...");
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here

            Vector3 v = GetAccelerometerVector();

            Log.D("X: " + v.X + " Y: " + v.Y + " Z: " + v.Z);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
