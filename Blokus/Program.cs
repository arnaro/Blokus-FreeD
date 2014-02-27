using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;

namespace Blokus
{
    class Program
    {
        private static ILog sLogger = LogManager.GetLogger(typeof(Program));
        public static byte[] Board = new byte[400];
        private static int numberOfGames = 1;
        //public byte[,] Board = new byte[20, 20];
        static void Main(string[] args)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // To skip player selection, enable this
                //args = new[] {"BLerminator", "BLerminator", "PentiumPlayer", "PentiumPlayer", "4"};
            }


            // Get some players from dll
            Console.ResetColor();
            var v = GetPlayersFromDll();
            List<IBlokusPlayer> playas = SelectPlayers(v, args);

            int gameCounter = 0;

            Dictionary<IBlokusPlayer, int> winCounter = new Dictionary<IBlokusPlayer, int>();
            Dictionary<IBlokusPlayer, int> pointCounter = new Dictionary<IBlokusPlayer, int>();
            playas.ForEach(a => { winCounter[a] = 0; pointCounter[a] = 0; });

            while (gameCounter < numberOfGames)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();

                ShufflePlayers(playas);


                BlokusGame g = new BlokusGame(playas);
                g.PrintGameState();

                while (!g.IsGameOver())
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = 0;
                    g.NextMove();
                    g.PrintGameState();
                    //val = Console.ReadKey(true).KeyChar;
                    Thread.Sleep(100);
                }
                g.GetWinners().ForEach(a => winCounter[a] += 1);
                playas.ForEach(a=> pointCounter[a] += g.ScoreGame(a.Id));
                ++gameCounter;
                sLogger.InfoFormat("Result: {0}", string.Join("",playas.Select(a=> string.Format("{0, -30}" ,a.Name+": "+g.ScoreGame(a.Id)+"pts"))));
            }

            Console.WriteLine("Game over");
            Console.WriteLine("                 ");
            Console.WriteLine("                 ");
            Console.WriteLine("                 ");
            Console.WriteLine("                 ");

            Console.WriteLine("WINNER(S)");

            foreach (var winner in pointCounter.OrderByDescending(a => a.Value))
            {
                Console.WriteLine(winner.Key + "      " + winner.Value + " pts (" + winCounter[winner.Key] + ") wins");
            }

            //if (!g.IsGameOver())
            //{
            //    Console.WriteLine("Game terminated");
            //}
            //else
            //{
            //    Console.WriteLine("Game over");
            //    Console.WriteLine("                 ");
            //    Console.WriteLine("                 ");
            //    Console.WriteLine("                 ");
            //    Console.WriteLine("                 ");

            //    Console.WriteLine("WINNER(S)");
                
            //    foreach (var winner in pointCounter.OrderByDescending(a => a.Value))
            //    {
            //        Console.WriteLine(winner.Key + "      "+winner.Value+" pts ("+winCounter[winner.Key]+") wins");    
            //    }

            //    // WINNER IS!!
            //    //List<IBlokusPlayer> winners = g.GetWinners();
            //    //Console.WriteLine("WINNER(S)");
            //    //winners.ForEach(a=> Console.WriteLine(a.Name+"      "));
            //}
            Console.ReadKey();
        }

        public static List<IBlokusPlayer> SelectPlayers(List<Type> types, string[] args)
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
                while (players.Count < 4 && pressed != 'd' && pressed != 'D')
                {
                    int nextId = players.Count + 1;
                    int count = players.Count;
                    if (args.Length > count)
                    {
                        IBlokusPlayer p = (IBlokusPlayer)Activator.CreateInstance(types.Single(a=> a.Name == args[count]));
                        p.Name = args[count] + " " + nextId;
                        p.Initialize(nextId);
                        players.Add(p);
                        continue;
                    }

                    Console.Clear();
                    Console.WriteLine(" ===== Blokus player selection ===== ");

                    Console.WriteLine(string.Format("Current players:"));
                    if (players.Count == 0)
                    {
                        Console.WriteLine(indent + "No players selected");
                    }
                    else
                    {
                        for (int i = 1; i <= players.Count; i++)
                        {
                            Console.WriteLine(indent + string.Format("{0}. {1}", players[i - 1].Id, players[i - 1].Name));
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
                            if (string.IsNullOrEmpty(name))
                            {
                                name = types[iPress - 1].Name + " " + nextId;
                            }
                            IBlokusPlayer p = (IBlokusPlayer)Activator.CreateInstance(types[iPress - 1]);
                            p.Name = name;
                            p.Initialize(nextId);
                            players.Add(p);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.ReadKey();
                        }
                    }
                }
                Console.CursorLeft = 0;

                if (args.Length < 5 || !int.TryParse(args[4], out numberOfGames))
                {
                    Console.Write("How many games? ");
                    string manys = Console.ReadLine();
                    int.TryParse(manys, out numberOfGames);
                }
            }


            return players;
        }

        public static List<Type> GetPlayersFromDll()
        {
            List<Type> types = new List<Type>();
            string dir = Directory.GetCurrentDirectory();
            List<string> fileNames = Directory.GetFiles(dir, "*.dll").OrderBy(a => a).ToList();
            foreach (string fileName in fileNames.Where(a => !a.EndsWith("BlokusInterface.dll")))
            {
                Assembly ass = Assembly.LoadFrom(fileName);
                List<Type> parsers = ass.GetTypes().Where(a => a.IsClass && typeof(IBlokusPlayer).IsAssignableFrom(a)).ToList();
                types.AddRange(parsers);
            }

            return types;
        }

        public static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void ShufflePlayers(List<IBlokusPlayer> players )
        {
            Shuffle(players);
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Initialize(i + 1);
            }
        }

    }
}
