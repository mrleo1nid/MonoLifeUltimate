using System;

namespace MonoLifeUltimate
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new LifeGame())
                game.Run();
        }
    }
}
