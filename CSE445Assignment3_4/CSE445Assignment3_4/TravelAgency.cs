using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CSE445Assignment3_4
{
    public class TravelAgency
    {
        static Random rng = new Random();               // used to start async

        public static void RunTravelAgency(int id)      // 5 TravelAgency threads will run here
        {
            while (true)
            {
                Order newOrder = new Order();
                MainClass._priceCutEventPool.WaitOne();         // wait for a sale to happen

                MainClass._bufferPool.WaitOne();              // sale has happened, now need to wait for buffer spot to open   
                Thread.Sleep(rng.Next(50, 350));        // so the two threads don't enter at the same time    
                //Console.WriteLine("Thread {0} enters the semaphore.", id);

                lock (MainClass.bufferCellRef.dataCells)
                {
                    bool finished = false;
                    while (finished == false)
                    {
                        Order tempOrder = MainClass.bufferCellRef.getCurrentOrder();
                        newOrder.setAmount(tempOrder.getAmount());
                        newOrder.setUnitPrice(tempOrder.getUnitPrice());
                        newOrder.setSenderId(id);
                        newOrder.setCreditCardNumber(rng.Next(3000, 8000));
                        newOrder.isAvailableToWrite = false;
                        
                        if (MainClass.bufferCellRef.dataCells[0].isAvailableToWrite)
                        {
                            //Console.WriteLine("Thread {0} enters first buffer.", id);
                            MainClass.bufferCellRef.setOneCell(newOrder, 0);
                            finished = true;
                        }
                        else if (MainClass.bufferCellRef.dataCells[1].isAvailableToWrite)
                        {
                            //Console.WriteLine("Thread {0} enters second buffer.", id);
                            MainClass.bufferCellRef.setOneCell(newOrder, 1);
                            finished = true;
                        }
                    }
                    Monitor.Pulse(MainClass.bufferCellRef.dataCells);       // signal airplane thread to read
                }
            }
        }

        public void ProcessOrder(double saleValue)
        {
            int amountOfTickets = (int)saleValue / 10;
            switch (amountOfTickets)
            {
                case 5:     // major sale $50-79
                case 6:
                case 7:
                    amountOfTickets = 30;
                    break;
                case 8:     // normal sale $80-109
                case 9:
                case 10:
                    amountOfTickets = 20;
                    break;
                case 11:    // minor sale $110-139
                case 12:
                case 13:
                    amountOfTickets = 10;
                    break;
                case 14:    // very minor sale $140-159
                case 15:
                    amountOfTickets = 5;
                    break;
                default:    // $160-200 high price
                    amountOfTickets = 1;
                    break; 
            }

            Order newOrder = new Order();
            newOrder.setAmount(amountOfTickets);
            newOrder.setUnitPrice(saleValue);

            if (MainClass.bufferCellRef.currentOrder == null)
            {
                MainClass.bufferCellRef.setCurrentOrder(newOrder);      // global variable of most current sale price
            }
            lock (MainClass.bufferCellRef.currentOrder)
            {
                MainClass.bufferCellRef.setCurrentOrder(newOrder);      // global variable of most current sale price
            }

            MainClass._priceCutEventPool.Release(5);        // signal 5 threads that a priceCutEvent has happened
        }
    }
}
