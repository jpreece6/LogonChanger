using ChangerCore;

namespace LogonChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            Changer.InitialiseSettings(args);

            Changer.Update();
        }
    }
}
