using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class MainClass
    {
        public static Buffer bufferCellRef = new Buffer();
        public static Semaphore _pool;
        public static ReaderWriterLock rwlock = new ReaderWriterLock();
        public static int padding = 0;
        private static int totalThreads = 5;
        // public static double currentPrice;

        public static void Main(string[] args)
        {
            _pool = new Semaphore(0, 2);

            Airline airline = new Airline();
            Thread airlineThread = airline.CreateThread();
            airline.StartThread(airlineThread);

            TravelAgency travelAgency = new TravelAgency();
            Airline.priceCut += new Airline.priceCutEvent(travelAgency.ProcessOrder);    // this is a callback

            Thread[] travelAgencies = new Thread[totalThreads];
            for(int i = 0; i < totalThreads; i++)
            {
                travelAgencies[i] = new Thread(() => TravelAgency.RunTravelAgency(i));
                Thread.Sleep(200);
                travelAgencies[i].Name = (i + 1).ToString();
                travelAgencies[i].Start();
            }
            Thread.Sleep(500);
            _pool.Release(2);


            //List<Thread> travelAgencyThreads = travelAgency.CreateThreads();
            //travelAgency.StartThreads();

            airlineThread.Join();
            Console.WriteLine("Finished waiting");
        }
    }
}
