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
        public static Semaphore _bufferPool;
        public static ReaderWriterLock rwlock = new ReaderWriterLock();
        public static int padding = 0;
        private static int totalThreads = 5;
        public static Semaphore _saleEventPool;
        // public static double currentPrice;

        public static void Main(string[] args)
        {
            _bufferPool = new Semaphore(0, 2);                  // the bufferPool semaphore will be used to manage availability of the buffer
            _saleEventPool = new Semaphore(0, 5);               // the saleEventPool semaphore will be used to notify all 5 threads of a priceCut

            Airline airline = new Airline();                    
            Thread airlineThread = airline.CreateThread();
            airline.StartThread(airlineThread);                 // start airline thread

            TravelAgency travelAgency = new TravelAgency();
            Airline.priceCut += new Airline.priceCutEvent(travelAgency.ProcessOrder);    // set up the delegate-> when a priceCutEvent is emitted, call travelAgency.ProcessOrder
            Thread[] travelAgencies = new Thread[totalThreads];
            for(int i = 0; i < totalThreads; i++)               // start 5 travel agency threads
            {
                travelAgencies[i] = new Thread(() => TravelAgency.RunTravelAgency(i));
                travelAgencies[i].Name = (i + 1).ToString();
                Thread.Sleep(100);
                travelAgencies[i].Start();
            }
            _bufferPool.Release(2);                             // the buffer now has 2 open spots, but threads will be blocked by saleEventPool until a priceCut event occurs

            airlineThread.Join();                               // wait for airline thread to finish (after n priceCut events)
            Console.WriteLine("Terminating TravelAgency Threads");
            for (int i = 0; i < totalThreads; i++)
            {
                travelAgencies[i].Abort();
            }
            Console.WriteLine("Finished program- hit any key to exit.");
            Console.ReadLine();
        }
    }
}
