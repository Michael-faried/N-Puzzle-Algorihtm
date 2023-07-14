# N-Puzzle_Algorihtm
The N-Puzzle is known in finite versions such as the 8-puzzle (a 3x3 board) and the 15-puzzle (a 4x4 board), and with various names like "sliding block", "tile puzzle", etc. The N-Puzzle is a board game for a single player. It consists of (N) numbered squared tiles in random order, and one blank space ("a missing tile"). 

The objective of the puzzle is to rearrange the tiles in order by making sliding moves that use the empty space, using the fewest moves. Moves of the puzzle are made by sliding an adjacent tile into the empty space. Only tiles that are horizontally or vertically adjacent to the blank space (not diagonally adjacent) may be moved. 

The N-puzzle (N = 8, 15...) is a classical problem for modeling algorithms involving heuristics. Commonly used heuristics for this problem include counting the number of misplaced tiles (Hamming Distance) OR finding the sum of the Manhattan distances between each block and its position in the goal configuration. Note that both are admissible, i.e. they never overestimate the number of moves left, which ensures optimality for certain search algorithms such as A*. A* is one of the informed search algorithms that can be used in such problems to get the optimal solution. 

1.	First, detect whether the board is solvable or not. If not solvable, then no feasible solution for the given puzzle.
2.	If solvable, then your objective is to get the shortest path to get the final solution from the initial board using A* as a search algorithm as the following: 
  a.	Convert the given puzzle into a tree while applying the A* search algorithm on it to get the least number of movements that can be needed while the searching process to get the goal state. Where each node in the search tree represents an arrangement of tiles (one board state), for example, the initial given state will be the graph’s root.
  b.	The success of this approach hinges on the choice of priority function for a state (Heuristic value). You will search for the goal based on the following two priority functions:
1.	Hamming priority function: The number of blocks in the wrong position, plus the number of moves made so far to get to the state. Intuitively, a state with a small number of blocks in the wrong position is close to the goal state, and we prefer a state that has been reached using a small number of moves.
2.	Manhattan priority function: The sum of the distances (sum of the vertical and horizontal distance) from the blocks to their goal positions, plus the number of moves made so far to get to the state.
For example, the Hamming and Manhattan priorities of the initial state below are 5 and 10, respectively.
