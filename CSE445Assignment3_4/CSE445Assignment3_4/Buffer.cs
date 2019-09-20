using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class Buffer
    {
        // n=2 data cells -> references to Order Object? 
        public int numberOfCells = 2;
        //public Order[] dataCells = new Order[2];
        public double cell = -1;
        public static double currentPrice = 0;

        public Buffer()
        {
            for(int i = 0; i < numberOfCells; i++)
            {
                //Order dataCell = new Order();
                //dataCells[i] = dataCell;
            }
        }

        // semaphore --> allows travel agency to see open cells *********PAGE 86****
        // lock --> allowa travel agency to gain read and write access to cells *** pAGE 85 ********
        // synchronization / monitor req. for read/write and write/write overlap
        // Airline can read cells at the same time

        //public void setOneCell(Order newOrder)  // synchronized
        //{

        //}

        //public Order getOneCell() // synchronized
        //{
        //    return null;
        //}

        public double getCurrentPrice()
        {
            return currentPrice;
        }

        public void setCurrentPrice(double price)
        {
            currentPrice = price;
        }

        public double getOneCell() // synchronized
        {
            lock(this)
            {
                return cell;
            }
        }

        public void setOneCell(double newCell)  // synchronized
        {
            lock(this)
            {
                cell = newCell;
            }
        }
    }
}
