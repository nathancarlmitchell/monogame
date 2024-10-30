using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame;

public class Object
{

    public int X { get; set; }
    public int Y { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }

    public bool CheckForCollisions(Object obj) {
        //if ((Y+Height < obj.Y) || (Y > obj.Y+obj.Height) || (X+Width < obj.X) || (X > obj.Width))
        //    return false;
        //return true;
        var myleft = this.X - this.Width/2; // devide by 2 because origin settings on player
        var myright = this.X + (this.Width)/2;
        var mytop = this.Y - this.Height/2;
        var mybottom = this.Y + (this.Height)/2;
        var otherleft = obj.X;
        var otherright = obj.X + (obj.Width);
        var othertop = obj.Y;
        var otherbottom = obj.Y + (obj.Height);
        var crash = true;
        if ((mybottom < othertop) || (mytop > otherbottom) || (myright < otherleft) || (myleft > otherright)) {
            crash = false;
        }
        return crash;
    }

    public bool CheckIsNear(Object obj, int ammount) {
        //if ((Y+Height < obj.Y) || (Y > obj.Y+obj.Height) || (X+Width < obj.X) || (X > obj.Width))
        //    return false;
        //return true;
        var myleft = this.X - this.Width/2; // devide by 2 because origin settings on player
        var myright = this.X + (this.Width)/2;
        var mytop = this.Y - this.Height/2;
        var mybottom = this.Y + (this.Height)/2;
        var otherleft = obj.X;
        var otherright = obj.X + (obj.Width);
        var othertop = obj.Y;
        var otherbottom = obj.Y + (obj.Height);
        var crash = true;
        if ((mybottom < othertop - ammount) || (mytop - ammount > otherbottom) || (myright < otherleft - ammount) || (myleft -ammount > otherright)) {
            crash = false;
        }
        return crash;
    }
    
}