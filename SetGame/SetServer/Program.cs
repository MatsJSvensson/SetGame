using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;

namespace SetServer
{
    class Program
    {
        static TcpListener listener;
        static int[] deckOrder;
        static int counter;
        static int counter2;
        static string[] scores;
        static StreamWriter[] theWriters;
        const int nPlayers = 2;

        static void Main(string[] args)
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[1];
            listener = new TcpListener(ip, 7777);
            Console.WriteLine(((IPEndPoint)listener.LocalEndpoint).Address.ToString());
            listener.Start();
            
            deckOrder = new int[81];
            for (int i = 0; i < 81; i++) { deckOrder[i] = i; }

            Shuffle();

            theWriters = new StreamWriter[nPlayers];
            scores = new string[nPlayers];
            counter = 0; counter2 = 0;

            for (int i = 0; i < nPlayers; i++)
            {
                Thread t = new Thread(new ThreadStart(Service));
                t.Start();
            }
        }

        private static void Service()
        {
            while (true)
            {
                Socket soc = listener.AcceptSocket();
                try
                {
                    Stream s = new NetworkStream(soc);
                    StreamReader sr = new StreamReader(s);
                    StreamWriter sw = new StreamWriter(s);
                    theWriters[counter] = sw; counter++;
                    sw.AutoFlush = true;
                    sw.WriteLine("Connected!"); Console.WriteLine("Connected!");
                    
                    string cards = "";
                    for (int i = 0; i < 12; i++)
                    {
                        cards += deckOrder[i].ToString() + " ";
                    }
                    cards += "81";
                    sw.WriteLine(cards);

                    while (true)
                    {
                        string incoming = sr.ReadLine();
                        string[] splitIn = incoming.Split(' ');
                        if(splitIn[0] == "Score")
                        {
                            scores[counter2] = splitIn[1];
                            counter2++;
                            if(counter2 >= counter)
                            {
                                string temp = "Score ";
                                for (int i = 0; i < counter; i++) { temp += scores[i] + " "; }
                                for (int i = 0; i < counter; i++) { theWriters[i].WriteLine(temp); theWriters[i].Close(); }
                                break;
                            }
                        }
                        if (splitIn[0] == "End")
                        {
                            for (int i = 0; i < counter; i++) { theWriters[i].WriteLine("End"); }
                        }
                        else
                        {
                            if (incoming == "Shuffle") { Shuffle(); }
                            else
                            {
                                int[] numbers = new int[3];
                                if (splitIn.Length == 4)
                                {
                                    for (int i = 0; i < 3; i++) { numbers[i] = Convert.ToInt32(splitIn[i]); }
                                }
                                else { Console.WriteLine("Something went horribly wrong"); break; }

                                for (int i = 0; i < deckOrder.Length; i++)
                                {
                                    if (numbers[0] == deckOrder[i]) { deckOrder[i] = -1; }
                                    if (numbers[1] == deckOrder[i]) { deckOrder[i] = -1; }
                                    if (numbers[2] == deckOrder[i]) { deckOrder[i] = -1; }
                                }
                                Console.WriteLine("{0} {1} {2}", numbers[0], numbers[1], numbers[2]);
                                Console.WriteLine("{0}", numbers[0] - 2 * numbers[1] + numbers[2]);
                            }

                            cards = "";
                            int c = 0;
                            for (int i = 0; i < deckOrder.Length; i++)
                            {
                                if (deckOrder[i] != -1 && c < 12)
                                {
                                    cards += deckOrder[i].ToString() + " ";
                                    c++;
                                }
                            }
                            cards += deckOrder.Count(x => x != -1);
                            for (int i = 0; i < counter; i++) { theWriters[i].WriteLine(cards); }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                soc.Close();
            }
        }

        private static void Shuffle()
        {
            int filled = deckOrder.Count(x => x >= 0);
            int c = 0;
            
            int[] tempDeck = new int[filled];
            for(int i = 0; i < deckOrder.Length; i++)
            {
                if(deckOrder[i] >= 0)
                {
                    tempDeck[c] = deckOrder[i];
                    c++;
                }
            }
            deckOrder = tempDeck;

            int r1; int r2; int temp;
            Random r = new Random();
            for(int i = 0; i < filled; i++)
            {
                r1 = r.Next(filled); r2 = r.Next(filled);
                temp = deckOrder[r1];
                deckOrder[r1] = deckOrder[r2];
                deckOrder[r2] = temp;
            }

        }
    }
}
