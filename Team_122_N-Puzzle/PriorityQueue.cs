using System;
using System.Collections.Generic;
using System.Text;

namespace Team_122_N_Puzzle
{
    class PriorityQueue
    {
        PuzzleNode[] Combinations;
        int NodeCount = 0;
        public PriorityQueue()
        {
            // Initializing the Puzzle Boards Queue
            Combinations = new PuzzleNode[(int)1e7];
        }
        public void Enqueue(PuzzleNode pn)
        {
            NodeCount++;    // Incrementing the Number of Nodes in the Queue
            Combinations[NodeCount] = pn;   // Setting the New Node to the New Puzzle Board
            UpHeapSort();   // Sorting the Queue After adding the New Puzzle Board
        }
        public PuzzleNode Dequeue()
        {
            int FirstIndex = 1;         // The Index of the Puzzle Board with the Minimum Distance Value
            int LastIndex = NodeCount;  // The Index of the Last Puzzle Board
            PuzzleNode First = Combinations[FirstIndex];    // The First Puzzle Board in the Queue
            // Updating the Value of the First Value to be Equal the Last Value
            Combinations[FirstIndex] = Combinations[LastIndex];
            NodeCount--;    // Decrementing the Number of Nodes in the Queue
            DownHeapSort(NodeCount, 1); // Sorting the Queue After removing the First Value
            return First;
        }

        void UpHeapSort()
        {
            int Index = NodeCount;  // The Index of the Last Element
            // Checking if the Cost of the Parent is Larger than the New Node
            while (BigParent(Index))
            {
                // Swapping the Child with the Parent
                PuzzleNode temp = Combinations[Index / 2];  // Index / 2 = Parent
                Combinations[Index / 2] = Combinations[Index];
                Combinations[Index] = temp;
                // Swapping the Child Index with the Parent Index
                Index /= 2;
            }
        }
        bool BigParent(int Index)
        {
            if (Index > 1)  // Index / 2 = Parent
                return (Combinations[Index / 2].TotalCost >= Combinations[Index].TotalCost);
            return false;
        }

        void DownHeapSort(int N, int Index)
        {
            int Min;
            int LeftChild = Index * 2;
            int RightChild = LeftChild + 1;

            if (BigChild(N, LeftChild, Index))  // Left Child Cost Less than Parent Cost
                Min = LeftChild;
            else                                // Left Child Cost More than Parent Cost
                Min = Index;
            if (BigChild(N, RightChild, Min))   // Right Child Cost Less than Parent Cost & Left Child Cost
                Min = RightChild;
            if (Index != Min)       // Check that it is not the Last Puzzle in the Queue
            {
                // Swaping the Index with Min Index (Parent / Left Child / Right Child)
                PuzzleNode temp = Combinations[Index];
                Combinations[Index] = Combinations[Min];
                Combinations[Min] = temp;
                DownHeapSort(N, Min);   // Recursively Sort the Array
            }
        }
        bool BigChild(int N, int Child, int Comp)
        {
            if (Child <= N)
                return (Combinations[Child].TotalCost < Combinations[Comp].TotalCost);
            return false;
        }
    }
}
