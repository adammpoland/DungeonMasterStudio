using System;
namespace DungeonMasterStudio.Helpers
{
    public class Dice
    {
        Random rnd = new Random();
        private int Sides;
        public Dice(int Sides)
        {
            this.Sides = Sides;
        }

        public int Roll()
        {
            return rnd.Next(1,Sides+1);
        }
    }
}
