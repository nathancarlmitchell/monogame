using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace monogame
{
    /// <summary>
    /// Provides extension methods for the TouchCollection type.
    /// </summary>
    public static class TouchCollectionExtensions
    {
        /// <summary>
        /// Determines if there are any touches on the screen.
        /// </summary>
        /// <param name="touchState">The current TouchCollection.</param>
        /// <returns>True if there are any touches in the Pressed or Moved state, false otherwise</returns>
        public static bool AnyTouch(this TouchCollection touchState)
        {
            foreach (TouchLocation location in touchState)
            {
                if (location.State == TouchLocationState.Pressed || location.State == TouchLocationState.Moved)
                {
                    return true;
                }
            }
            return false;
        }

        public static Vector2 GetPosition(this TouchCollection touchState)
        {
            foreach (TouchLocation location in touchState)
            {
                if (location.State == TouchLocationState.Pressed || location.State == TouchLocationState.Moved)
                {
                    return location.Position;
                }
            }
            return new Vector2(0,0);
        }
    }
}
