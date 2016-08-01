using System;
using ChangerCore;
using ChangerCore.Exceptions;

namespace LogonChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            Changer.InitialiseSettings(args);

            try
            {
                Changer.Update();
            }
            catch (UnsupportedOSException uoe)
            {
                Console.WriteLine("Error: Could not start program.\n\n" + uoe.Message);
                Console.ReadKey();
            }
        }
    }
}
