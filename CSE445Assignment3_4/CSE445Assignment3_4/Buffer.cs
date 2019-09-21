using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE445Assignment3_4
{
    public class Buffer
    {
        public int numberOfCells = 2;
        public Order[] dataCells = new Order[2];
        public Order currentOrder = new Order();    // global variable for most recent priceCutEvent

        public Buffer()
        {
            for(int i = 0; i < numberOfCells; i++)
            {
                Order dataCell = new Order();
                dataCell.isAvailableToWrite = true;
                dataCells[i] = dataCell;
            }
        }

        public Order getCurrentOrder()
        {
            return currentOrder;
        }

        public void setCurrentOrder(Order newOrder)
        {
            currentOrder = newOrder;
        }

        public Order getOneCell(int index)                      // return a cell by index
        {
            return dataCells[index];
        }

        public void setOneCell(Order newOrder, int index)       // set a cell by index
        {
            dataCells[index] = newOrder;
            dataCells[index].isAvailableToWrite = false;        // don't overwrite this cell until it has been read by Airline thread
        }

        public void resetCells()
        {
            dataCells[0].isAvailableToWrite = true;             // allow both cells to be overwritten
            dataCells[1].isAvailableToWrite = true;
        }      
    }
}
