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
        private Effect _effectGreyscale;

        private GameObject _hand;
        private GameObject _baseballBat;

        private bool _isFailing;
        private double _lastFail;

        public SinglePlayerGame(AccelerometerHandlerDelegate accelerometerHandler)
            : base(accelerometerHandler) { }

        // My alternative to LoadContent; makes progress bars easier
        protected override ContentManager Load()
        {
            ContentManager c = new ContentManager();

            c.AddTask(new LoadTask("effects", delegate ()
            {
                _effectBlackout = Content.Load<Effect>("fx/effect_blackout");
                _effectGreyscale = Content.Load<Effect>("fx/effect_greyscale");
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
            GlobalEffect = null;

            _baseballBat.Position = new Vector2(0, -24);
            _baseballBat.Body.LinearVelocity = Vector2.Zero;
            _baseballBat.Body.AngularVelocity = 0f;
            _baseballBat.Body.Rotation = 0f;

            int choice = (new Random().Next(0, 2));
            Log.D("Picked: " + choice);
            _hand.Position = new Vector2((choice - 0.5f), 26f);

            Camera.Position = Vector2.Zero;
            Camera.Zoom = 1f;

            _isFailing = false;

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
                            _hand.Position = new Vector2(touch.Value.Position.X / SCALED_ZOOM - _hand.Sprite.Texture.Width, _hand.Sprite.Position.Y);
                    }

                    if (_baseballBat.Position.Y > 128)
                    {
                        double failTime = 2.0;

                        if (!_isFailing)
                        {
                            _lastFail = gameTime.TotalGameTime.TotalSeconds;
                            failTime = 2;
                            _isFailing = true;
                        }
                        if (gameTime.TotalGameTime.TotalSeconds - _lastFail < failTime)
                            animateFail();
                        else
                            Restart();
                    }

                    _hand.Update(gameTime);

                    _baseballBat.Update(gameTime);

                    break;
            }

            base.Update(gameTime);
        }

        private void animateFail()
        {
            Camera.Position = (Camera.Position * 20 + _hand.Position) / new Vector2(21, 21);
            float animProgress = 1f / ((Math.Abs(Camera.Position.X) + Math.Abs(Camera.Position.Y) + 1f) / 2f);
            Camera.Zoom = 2f - animProgress;
            GlobalEffect = _effectGreyscale;
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

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap,
                        null, null, GlobalEffect, transform);

                    _hand.Draw(gameTime, spriteBatch);
                    _baseballBat.Draw(gameTime, spriteBatch);

                    spriteBatch.End();

                    break;
            }

            base.Draw(gameTime);
        }
    }
}