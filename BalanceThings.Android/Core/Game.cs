using BalanceThings.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace BalanceThings.Core
{
    internal abstract class Game : Microsoft.Xna.Framework.Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        // Delegate handler for accelerometer sensor
        internal delegate Vector3 AccelerometerHandlerDelegate();

        // Handler for aboce delegate method
        protected AccelerometerHandlerDelegate GetAccelerometerVector;
        protected GameState _currentGameState;

        private ContentManager _contentManager;
        private Thread _loadingThread;

        private Texture2D _loaderLeft, _loaderRight, _loaderEmpty, _loaderFull;

        internal Game(AccelerometerHandlerDelegate accelerometerHandler)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.Portrait;

            GetAccelerometerVector = accelerometerHandler;

            _currentGameState = GameState.LOADING;
        }

        // Used to reset game objects' postions etc.
        protected virtual void Restart()
        {
            Init();
        }

        protected abstract void Init();
        protected abstract ContentManager Load();
        
        protected sealed override void Initialize()
        {
            Background = Color.White;

            Init();
            base.Initialize();
        }

        private void preLoad()
        {
            _loaderLeft = Content.Load<Texture2D>("load_chunk_left");
            _loaderRight = Content.Load<Texture2D>("load_chunk_right");
            _loaderEmpty = Content.Load<Texture2D>("load_chunk_empty");
            _loaderFull = Content.Load<Texture2D>("load_chunk_full");
        }

        protected sealed override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            preLoad();

            _contentManager = Load();

            // Begin loading:
            _loadingThread = new Thread(new ThreadStart(_contentManager.LoadAll) + OnLoaded);
            _loadingThread.Start();
            Log.D("Finished native LoadContent() method.");
        }

        protected virtual void OnLoaded()
        {
            _currentGameState = GameState.IN_MAIN_MENU;
        }

        protected override void UnloadContent()
        {
            
        }

        private void drawLoadingBar(int nChunks)
        {
            Vector2 center = GraphicsDevice.Viewport.Bounds.Center.ToVector2();

            for (int i = 0; i < nChunks; i++)
            {
                Texture2D texture;

                // If this particular chunk is loaded
                if (_contentManager.LoadProgress > ((float)i / nChunks))
                    texture = _loaderFull;
                else
                {
                    if (i == 0)
                        texture = _loaderLeft;
                    else if (i == nChunks - 1)
                        texture = _loaderRight;
                    else
                        texture = _loaderEmpty;
                }

                Vector2 position = center + new Vector2(-nChunks * texture.Width * GlobalScale / 2 + i * texture.Width * GlobalScale, 0);

                spriteBatch.Draw(texture, position , null, Color.White,
                    0f, Vector2.Zero, GlobalScale, SpriteEffects.None, 0f);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Background);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, null);

            switch (_currentGameState)
            {
                case GameState.LOADING:

                    drawLoadingBar(10);

                    break;

            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected Color Background { set; get; }

        protected float GlobalScale { get { return 8f; } } //
    }
}
