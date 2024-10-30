using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monogame;

public class WallSpawner
{
    private static Random rand = new Random();
    private static int minHeight = 20;
    //static int minGap = 85;
    //static int maxGap = 125;

    public List<Object> Spawn(int ScreenWidth, int ScreenHeight){
        List<Object> objectList = new List<Object>();
        Object obj1 = new Object();
        Object obj2 = new Object();
        
        int gap = 128;
        int maxHeight = ScreenHeight - gap;
        int height = (int)Math.Floor(rand.NextDouble() * (maxHeight - minHeight + 1) + minHeight);
        
        //int gap = (int)Math.Floor(rand.NextDouble() * (maxGap - minGap + 1) + minGap);

        obj1.X = ScreenWidth;
        obj1.Y = 0;
        obj1.Width = 16;
        obj1.Height = height;

        obj2.X = ScreenWidth;
        obj2.Y = height + gap;
        obj2.Width = 16;
        obj2.Height = ScreenHeight - height - gap;

        objectList.Add(obj1);
        objectList.Add(obj2);
        return objectList;
    }

}