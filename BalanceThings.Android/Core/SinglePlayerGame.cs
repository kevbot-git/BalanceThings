using BalanceThings.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using BalanceThings.Physics;
using BalanceThings.Drawing;
using BalanceThings.Items;
using FarseerPhysics;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace BalanceThings.Core
{
    class SinglePlayerGame : Game
    {
        private World _world;

        private Effect _effectBlackout;

        private GameObject _hand;
        private GameObject _baseballBat;

        public SinglePlayerGame(AccelerometerHandlerDelegate accelerometerHandler)
            : base(accelerometerHandler) { }

        // My alternative to LoadContent; makes progress bars easier
        protected override ContentManager Load()
        {
            ContentManager c = new ContentManager();

            c.AddTask(new LoadTask("effects", delegate ()
            {
                _effectBlackout = Content.Load<Effect>("fx/effect_blackout");
            }));

            c.AddTask(new LoadTask("physics", delegate ()
            {
                TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.DragComplete;
                ConvertUnits.SetDisplayUnitToSimUnitRatio(10f);

                _world = new World(new Vector2(0, 9.98f));

                _hand = new Hand(_world, Content, new Vector2(0, 26));
                _baseballBat = new BaseballBat(_world, Content, new Vector2(0, -24));

                Camera = new View.Camera(this, new Vector2(0, 0), 1f);
            }));

            return c;
        }

        protected override void Start()
        {
            _baseballBat.Position = new Vector2(0, -24);
            _baseballBat.Body.LinearVelocity = Vector2.Zero;
            _baseballBat.Body.AngularVelocity = 0f;
            _baseballBat.Body.Rotation = 0f;

            Camera.Position = Vector2.Zero;
            Camera.Zoom = 1f;

            base.Start();
        }

        protected override void Update(GameTime gameTime)
        {
            GestureSample? touch = null;
            if (TouchPanel.IsGestureAvailable)
                touch = TouchPanel.ReadGesture();

            switch (_currentGameState)
            {
                case GameState.PLAYING:
                    _world.Step((float) 1 / 60);

                    if (touch != null)
                    {
                        if (touch.Value.GestureType == GestureType.Tap)
                            _baseballBat.Body.LinearVelocity = new Vector2(0f, -5f);
                        else if (touch.Value.GestureType == GestureType.FreeDrag)
                            _hand.Position = new Vector2(touch.Value.Position.X / 12 - 32, _hand.Sprite.Position.Y);
                    }

                    if (_baseballBat.Position.Y > 128 && Camera.Zoom < 3f)
                    {
                        Camera.Position = (Camera.Position * 15 + _hand.Position) / 16f;
                        Camera.Zoom = 2f - (1f / Math.Abs((Camera.Position.X + Camera.Position.Y) / 2f));
                        if (Camera.Zoom > 2f || Camera.Zoom < 1f)
                            Log.D("Zoom got to " + Camera.Zoom);
                    }
                    if (_baseballBat.Position.Y > 480f)
                        Restart();

                    _hand.Update(gameTime);

                    _baseballBat.Update(gameTime);

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Background);

            switch (_currentGameState)
            {
                case GameState.PLAYING:

                    Matrix? transform = null;
                    if (Camera != null)
                        transform = Camera.Transform;

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, transform);

                    _hand.Draw(gameTime, spriteBatch);
                    _baseballBat.Draw(gameTime, spriteBatch);

                    spriteBatch.End();

                    break;
            }

            base.Draw(gameTime);
        }
    }
}