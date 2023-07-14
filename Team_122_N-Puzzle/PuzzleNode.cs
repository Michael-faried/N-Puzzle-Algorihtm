using System;
using System.Collections.Generic;
using System.Text;

namespace Team_122_N_Puzzle
{
    class PuzzleNode
    {
        public int[,] Puzzle;   // 2D Board
        public int[] Puzzle_1D;

        public int S;           // Puzzle Size
        public int DepthLevel;  // Tree Branch Level (# of Moves)
        public int Dist_value;  // Manhattan or Hamming
        public int Empty_i_Pos;
        public int Empty_j_Pos;
        public int Moves = 0;
        public int Move_cost;
        public string Move_dir; // The Last Direction that the Empty Cell Moved
        public string MainKey;
        public int TotalCost;   // Distance Function + Number of Moves

        public PuzzleNode Parent;
        public List<PuzzleNode> Children;   // List of Possible Puzzle Nodes Combinations

        public bool CanMoveUp = false;
        public bool CanMoveRight = false;
        public bool CanMoveDown = false;
        public bool CanMoveLeft = false;

        //Main Constructor
        public PuzzleNode(int[,] Puzzle, int S, int Empty_i_Pos, int Empty_j_Pos, int Dist_value, int DepthLevel, PuzzleNode P)
        {
            this.S = S;
            this.Puzzle = Puzzle;

            this.DepthLevel = DepthLevel;
            this.Empty_i_Pos = Empty_i_Pos;
            this.Empty_j_Pos = Empty_j_Pos;
            this.Move_dir = "Initial";
            this.Dist_value = Dist_value;
            this.Parent = P;
            Children = new List<PuzzleNode>();
        }
        // Secondry Copy Constructor
        public PuzzleNode(PuzzleNode pn)
        {
            this.S = pn.S;
            this.Puzzle = new int[S, S];
            this.Puzzle_1D = new int[S * S];

            for (int i = 0; i < this.S * this.S; i++)
            {
                this.Puzzle[i / this.S, i % this.S] = pn.Puzzle[i / this.S, i % this.S];
                this.Puzzle_1D[i] = pn.Puzzle[i / this.S, i % this.S];
                this.MainKey = this.MainKey + Puzzle[i / this.S, i % this.S];
            }

            this.DepthLevel = pn.DepthLevel + 1;
            this.Empty_i_Pos = pn.Empty_i_Pos;
            this.Empty_j_Pos = pn.Empty_j_Pos;
            this.Move_dir = "To Be Determined";
            this.Move_cost = pn.Move_cost;
            this.MainKey = pn.MainKey;
            this.Parent = pn.Parent;
            this.Dist_value = pn.Dist_value;
        }

        public void NextMove(bool IsManhattan)
        {
            this.FeasibleMoves();   // Calculating the Possible Moves for the Current Board
            if (this.CanMoveUp)
                this.UpDirection(IsManhattan);  // Creating a New Board After Moving the Empty Cell Upwards
            if (this.CanMoveRight)
                this.RightDirection(IsManhattan);   // Creating a New Board After Moving the Empty Cell to the Right
            if (this.CanMoveDown)
                this.DownDirection(IsManhattan);    // Creating a New Board After Moving the Empty Cell Downwards
            if (this.CanMoveLeft)
                this.LeftDirection(IsManhattan);    // Creating a New Board After Moving the Empty Cell to the Left
        }
        public void FeasibleMoves()
        {
            if (this.Empty_i_Pos > 0)       // Not at the Top Row
                this.CanMoveUp = true;
            if (this.Empty_j_Pos + 1 < S)   // Not at the Right Most Column
                this.CanMoveRight = true;   
            if (this.Empty_i_Pos + 1 < S)   // Not at the Bottom Row
                this.CanMoveDown = true;    
            if (this.Empty_j_Pos > 0)       // Not at the Left Most Column
                this.CanMoveLeft = true;    
        }

        public void UpDirection(bool IsManhattan)
        {
            int[,] UpBranch = new int[S, S];
            // Coping the Values of the Current Board to the New Board
            for (int i = 0; i < Puzzle.Length; i++)
                UpBranch[i / S, i % S] = Puzzle[i / S, i % S];

            PuzzleNode CurrNode = this;     // Parent Node Before Moving Up
            // Getting the Number Above the Empty Cell
            int IndexAbove_0 = UpBranch[Empty_i_Pos - 1, Empty_j_Pos];

            int DistanceFunction = 0;
            // Returning the Zero Index of the Current Node
            if (IsManhattan)
                DistanceFunction = ManCheckOptimal(CurrNode, IndexAbove_0 - 1, Empty_i_Pos - 1, Empty_j_Pos);
            else
                DistanceFunction = HamCheckOptimal(CurrNode, IndexAbove_0, Empty_i_Pos - 1, Empty_j_Pos);

            int Move = CurrNode.DepthLevel + 1; // New Board Move (Next Level)
            // Creating the New Puzzle Board with the New Move
            PuzzleNode UpBoard = new PuzzleNode(UpBranch, S, Empty_i_Pos - 1, Empty_j_Pos, DistanceFunction, Move, CurrNode);
            UpBoard.Move_dir = "Upwards <↑>";

            // Substituting the Empty Cell with the Number Above it
            int temp = UpBranch[Empty_i_Pos, Empty_j_Pos];
            UpBranch[Empty_i_Pos, Empty_j_Pos] = UpBranch[Empty_i_Pos - 1, Empty_j_Pos];
            UpBranch[Empty_i_Pos - 1, Empty_j_Pos] = temp;

            // Checking that the New Node Doesn't Return Back to the Old Position
            if ((CurrNode.Parent == null || CurrNode.Parent.Empty_i_Pos != Empty_i_Pos - 1 || CurrNode.Parent.Empty_j_Pos != Empty_j_Pos))
                Children.Add(UpBoard);
        }
        public void RightDirection(bool IsManhattan)
        {
            int[,] RightBranch = new int[S, S];
            // Coping the Values of the Current Board to the New Board
            for (int i = 0; i < Puzzle.Length; i++)
                RightBranch[i / S, i % S] = Puzzle[i / S, i % S];

            PuzzleNode CurrNode = this;
            // Getting the Number To the Right of the Empty Cell
            int IndexRight_0 = RightBranch[Empty_i_Pos, Empty_j_Pos + 1];

            int DistanceFunction = 0;
            if (IsManhattan)
                DistanceFunction = ManCheckOptimal(CurrNode, IndexRight_0 - 1, Empty_i_Pos, Empty_j_Pos + 1);
            else
                DistanceFunction = HamCheckOptimal(CurrNode, IndexRight_0, Empty_i_Pos, Empty_j_Pos + 1);

            int Move = CurrNode.DepthLevel + 1;
            // Creating the New Puzzle Board with the New Move
            PuzzleNode RightBoard = new PuzzleNode(RightBranch, S, Empty_i_Pos, Empty_j_Pos + 1, DistanceFunction, Move, CurrNode);
            RightBoard.Move_dir = "Right -> ";

            // Substituting the Empty Cell with the Number to the Right of it
            int temp = RightBranch[Empty_i_Pos, Empty_j_Pos];
            RightBranch[Empty_i_Pos, Empty_j_Pos] = RightBranch[Empty_i_Pos, Empty_j_Pos + 1];
            RightBranch[Empty_i_Pos, Empty_j_Pos + 1] = temp;

            // Checking that the New Node Doesn't Return Back to the Old Position
            if ((CurrNode.Parent == null || CurrNode.Parent.Empty_i_Pos != Empty_i_Pos || CurrNode.Parent.Empty_j_Pos != Empty_j_Pos + 1))
                Children.Add(RightBoard);
        }
        public void DownDirection(bool IsManhattan)
        {
            int[,] DownBranch = new int[S, S];
            // Coping the Values of the Current Board to the New Board
            for (int i = 0; i < Puzzle.Length; i++)
                DownBranch[i / S, i % S] = Puzzle[i / S, i % S];

            PuzzleNode CurrNode = this;
            // Getting the Number Below the Empty Cell
            int IndexBelow_0 = DownBranch[Empty_i_Pos + 1, Empty_j_Pos];

            int DistanceFunction = 0;
            if (IsManhattan)
                DistanceFunction = ManCheckOptimal(CurrNode, IndexBelow_0 - 1, Empty_i_Pos + 1, Empty_j_Pos);
            else
                DistanceFunction = HamCheckOptimal(CurrNode, IndexBelow_0, Empty_i_Pos + 1, Empty_j_Pos);

            int Move = CurrNode.DepthLevel + 1;
            // Creating the New Puzzle Board with the New Move
            PuzzleNode DownBoard = new PuzzleNode(DownBranch, S, Empty_i_Pos + 1, Empty_j_Pos, DistanceFunction, Move, CurrNode);
            DownBoard.Move_dir = "Downwards <↓>";

            // Substituting the Empty Cell with the Number Below it
            int temp = DownBranch[Empty_i_Pos + 1, Empty_j_Pos];
            DownBranch[Empty_i_Pos + 1, Empty_j_Pos] = DownBranch[Empty_i_Pos, Empty_j_Pos];
            DownBranch[Empty_i_Pos, Empty_j_Pos] = temp;

            // Checking that the New Node Doesn't Return Back to the Old Position
            if ((CurrNode.Parent == null || CurrNode.Parent.Empty_i_Pos != Empty_i_Pos + 1 || CurrNode.Parent.Empty_j_Pos != Empty_j_Pos))
                Children.Add(DownBoard);
        }
        public void LeftDirection(bool IsManhattan)
        {
            int[,] LeftBranch = new int[S, S];
            // Coping the Values of the Current Board to the New Board
            for (int i = 0; i < Puzzle.Length; i++)
                LeftBranch[i / S, i % S] = Puzzle[i / S, i % S];

            PuzzleNode CurrNode = this;
            // Getting the Number to the Left the Empty Cell
            int IndexLeft_0 = LeftBranch[Empty_i_Pos, Empty_j_Pos - 1];

            int DistanceFunction = 0;
            if (IsManhattan)
                DistanceFunction = ManCheckOptimal(CurrNode, IndexLeft_0 - 1, Empty_i_Pos, Empty_j_Pos - 1);
            else
                DistanceFunction = HamCheckOptimal(CurrNode, IndexLeft_0, Empty_i_Pos, Empty_j_Pos - 1);

            int Move = CurrNode.DepthLevel + 1;
            // Creating the New Puzzle Board with the New Move
            PuzzleNode LeftBoard = new PuzzleNode(LeftBranch, S, Empty_i_Pos, Empty_j_Pos - 1, DistanceFunction, Move, CurrNode);
            LeftBoard.Move_dir = "Left <-";

            // Substituting the Empty Cell with the Number to the Left it
            int temp = LeftBranch[Empty_i_Pos, Empty_j_Pos];
            LeftBranch[Empty_i_Pos, Empty_j_Pos] = LeftBranch[Empty_i_Pos, Empty_j_Pos - 1];
            LeftBranch[Empty_i_Pos, Empty_j_Pos - 1] = temp;

            // Checking that the New Node Doesn't Return Back to the Old Position
            if ((CurrNode.Parent == null || CurrNode.Parent.Empty_i_Pos != Empty_i_Pos || CurrNode.Parent.Empty_j_Pos != Empty_j_Pos - 1))
                Children.Add(LeftBoard);
        }

        public int ManCheckOptimal(PuzzleNode pn, int N, int LastEmpty_i_Pos, int LastEmpty_j_Pos)
        {
            int Manhatten = pn.Dist_value;
            // Parent Empty Cell Position
            int ParentCorrectPositon = LastEmpty_i_Pos * pn.S + LastEmpty_j_Pos;
            // Checking the Parent Empty Cell Position Relative to the Next Move
            if (ParentCorrectPositon != N)
                Manhatten += ManCal(pn, N, pn.Empty_i_Pos, pn.Empty_j_Pos) - ManCal(pn, N, LastEmpty_i_Pos, LastEmpty_j_Pos);
            else
                Manhatten++;
            return Manhatten;
        }
        public int ManCal(PuzzleNode pn, int N, int LastEmpty_i_Pos, int LastEmpty_j_Pos)
        {
            int i_Pos = N / pn.S;   // Current Row Index
            int j_Pos = N % pn.S;   // Current Column Index
            // Distance Between Current Row Index & Correct Row Index
            int CorrectRowPosition = Math.Abs(i_Pos - LastEmpty_i_Pos);
            // Distance Between Current Column Index & Correct Column Index
            int CorrectColumnPosition = Math.Abs(j_Pos - LastEmpty_j_Pos);
            return CorrectRowPosition + CorrectColumnPosition;
        }
        public int HamCheckOptimal(PuzzleNode pn, int N, int LastEmpty_i_Pos, int LastEmpty_j_Pos)
        {
            int Ham = pn.Dist_value;
            // Current Empty Cell Position
            int ChildCorrectPosition = pn.Empty_i_Pos * pn.S + pn.Empty_j_Pos;
            // Parrent Empty Cell Position
            int ParentCorrectPositon = LastEmpty_i_Pos * pn.S + LastEmpty_j_Pos;
            // Checking the Parent Empty Cell Positon Relative to the Next Move
            if (ParentCorrectPositon == N - 1)
                Ham++;
            // Checking the Child Empty Cell Positon Relative to the Next Move
            else if (ChildCorrectPosition == N - 1)
                Ham--;
            return Ham;
        }

    }
}
