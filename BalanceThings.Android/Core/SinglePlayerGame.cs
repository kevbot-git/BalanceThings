using Microsoft.Xna.Framework;

namespace BalanceThings.Core
{
    class SinglePlayerGame : Game
    {
        public SinglePlayerGame(AccelerometerHandlerDelegate accelerometerHandler)
            : base(accelerometerHandler) { }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Restart()
        {
            base.Restart();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}