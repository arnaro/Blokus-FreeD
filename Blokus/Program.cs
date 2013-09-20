using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blokus
{
    class Program
    {
        public static byte[] Board = new byte[400];
        //public byte[,] Board = new byte[20, 20];
        static void Main(string[] args)
        {

            var v = GetPlayersFromDll();
            List<IBlokusPlayer> playas = SelectPlayers(v);
            List<IBlokusPlayer> Players = new List<IBlokusPlayer>();
            // Get some players from dll
            //Players.Add(new BlokusTestPlayer());
            //Players.Add(new BlokusTestPlayer());
            //Players.Add(new BlokusTestPlayer());
            //Players.Add(new BlokusTestPlayer());

            // Display Map
            BlokusGame g = new BlokusGame(Players);
            g.PrintGameState();
            char val = 'n';
            while (val == 'n')
            {
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                g.NextMove();
                g.PrintGameState();
                val = Console.ReadKey(true).KeyChar;
            }

        }

        public static List<IBlokusPlayer> SelectPlayers(List<Type> types)
        {
            string indent = " ";
            List<IBlokusPlayer> players = new List<IBlokusPlayer>();
            if (types == null || types.Count == 0)
            {
                Console.WriteLine("No Players found. Make sure you put the dll in the same folder the executable.");
            }
            else
            {
                char pressed = '_';
                while (pressed != 'd' && pressed != 'D')
                {
                    Console.Clear();
                    Console.WriteLine(" ===== CodeWars Player selection ===== ");

                    Console.WriteLine(string.Format("Current players:"));
                    if (players.Count == 0)
                    {
                        Console.WriteLine(indent + "No players selected");
                    }
                    else
                    {
                        for (int i = 1; i <= players.Count; i++)
                        {
                            Console.WriteLine(indent + string.Format("{0}. {1}", i, players[i - 1].Name));
                        }
                    }
                    Console.WriteLine("Select new player or 'D' when done");
                    for (int i = 1; i <= types.Count; i++)
                    {
                        Console.WriteLine(indent + string.Format("{0}. {1}", i, types[i - 1].Name));
                    }
                    pressed = Console.ReadKey().KeyChar;
                    int iPress = 0;
                    if (int.TryParse(pressed.ToString(), out iPress))
                    {
                        try
                        {
                            Console.CursorLeft = 0;
                            Console.Write(indent + string.Format("Enter the {0}'s name: ", types[iPress - 1].Name));
                            string name = Console.ReadLine();
                            IBlokusPlayer p = (IBlokusPlayer)Activator.CreateInstance(types[iPress - 1]);
                            p.Name = name;
                            players.Add(p);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.ReadKey();
                        }
                    }
                }
            }


            return players;
        }

        public static List<Type> GetPlayersFromDll()
        {
            List<Type> types = new List<Type>();
            string dir = Directory.GetCurrentDirectory();
            List<string> fileNames = Directory.GetFiles(dir, "*.dll").OrderBy(a => a).ToList();
            foreach (string fileName in fileNames)
            {
                Assembly ass = Assembly.LoadFrom(fileName);
                List<Type> parsers = ass.GetTypes().Where(a => a.IsClass && typeof(IBlokusPlayer).IsAssignableFrom(a)).ToList();
                types.AddRange(parsers);
            }

            return types;
        }

    }
}
