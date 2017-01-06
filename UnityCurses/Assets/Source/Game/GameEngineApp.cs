using System;
using System.Collections.Generic;
using Assets.Source.Engine;

namespace Assets.Source.Game
{
    public class GameEngineApp : EngineApp
    {
        /// <summary>
        ///     Determines what windows the simulation will be capable of using and creating using the window managers factory.
        /// </summary>
        public override IEnumerable<Type> AllowedWindows
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Fired when the ticker receives the first system tick event.
        /// </summary>
        protected override void OnFirstTick()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Called by the text user interface scene graph renderer before it asks the active window to render itself out for
        ///     display.
        /// </summary>
        public override string OnPreRender()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Called when simulation is about to destroy itself, but right before it actually does it.
        /// </summary>
        protected override void OnShutdown()
        {
            throw new NotImplementedException();
        }
    }
}