using BalanceThings.Util;
using BalanceThings.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace BalanceThings.Core
{
    internal abstract class Game : Microsoft.Xna.Framework.Game
    {
        internal float SCALED_ZOOM;

        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        internal delegate void OnGamePause();
        internal delegate void OnGameResume();
        // Delegate handler for accelerometer sensor
        internal delegate Vector3 AccelerometerHandlerDelegate();

        // Handler for above delegate method
        protected AccelerometerHandlerDelegate GetAccelerometerVector;

        protected OnGamePause _onGamePause;
        protected OnGameResume _onGameResume;
        protected GameState _currentGameState;

        private ContentManager _contentManager;
        private Thread _loadingThread;

        private Effect _globalEffect;
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
            _onGamePause = null;
            _onGameResume = null;

            _currentGameState = GameState.LOADING;
        }

        // Used to reset game objects' postions etc.
        protected void Restart()
        {
            Start();
        }

        protected virtual void Pause(GameTime gameTime)
        {
            _currentGameState = GameState.PAUSED;
            // If _onGamePause is set, run it
            _onGamePause?.Invoke();
        }

        protected virtual void Resume(GameTime gameTime)
        {
            _currentGameState = GameState.PLAYING;
            // If _onGameResume is set, run it
            _onGameResume?.Invoke();
        }

        protected virtual void Start()
        {
            _currentGameState = GameState.PLAYING;
        }

        protected abstract ContentManager Load();
        
        protected sealed override void Initialize()
        {
            base.Initialize();
        }

        private void preLoad()
        {
            _loaderLeft = Content.Load<Texture2D>("img/load_chunk_left");
            _loaderRight = Content.Load<Texture2D>("img/load_chunk_right");
            _loaderEmpty = Content.Load<Texture2D>("img/load_chunk_empty");
            _loaderFull = Content.Load<Texture2D>("img/load_chunk_full");
        }

        protected sealed override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SCALED_ZOOM = GraphicsDevice.Viewport.Width / 60f;

            Background = Color.White;
            GlobalEffect = null;
            Camera = null;

            preLoad();

            _contentManager = Load();

            // Begin loading:
            _loadingThread = new Thread(new ThreadStart(_contentManager.LoadAll) + Start);
            _loadingThread.Start();
        }

        protected override void UnloadContent()
        {
            
        }

        private void drawLoadingBar(int nChunks)
        {
            float scale = 4f;
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

                Vector2 position = center + new Vector2(-nChunks * texture.Width * scale / 2 + i * texture.Width * scale, 0);

                spriteBatch.Draw(texture, position , null, Color.White,
                    0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (_currentGameState == GameState.PAUSED)
                    Resume(gameTime);
                else if (_currentGameState == GameState.PLAYING || _currentGameState == GameState.FAILING)
                    Pause(gameTime);
            }
            

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            switch (_currentGameState)
            {
                case GameState.LOADING:
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, null);
                    drawLoadingBar(10);
                    spriteBatch.End();
                    break;

            }
            base.Draw(gameTime);
        }

        internal OnGamePause GamePause
        {
            get { return _onGamePause; }
            set { _onGamePause = value; }
        }

        internal OnGameResume GameResume
        {
            get { return _onGameResume; }
            set { _onGameResume = value; }
        }

        protected Color Background { set; get; }
        protected Camera Camera { get; set; }

        protected Effect GlobalEffect
        {
            get { return _globalEffect; }
            set
            {
                if (value == null)
                    _globalEffect = null;
                else
                    _globalEffect = value.Clone();
            }
        }
    }
}
