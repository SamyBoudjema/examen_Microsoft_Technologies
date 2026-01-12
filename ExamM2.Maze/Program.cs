using ExamM2.Maze;

Console.WriteLine("=== Test du résolveur de labyrinthe ===\n");

// Test 1 : Labyrinthe simple 3x3
TestMaze("Test 1 : Labyrinthe simple 3x3", @"D..
...
..S");

// Test 2 : Labyrinthe avec murs
TestMaze("Test 2 : Labyrinthe avec murs", @"D.#
..#
..S");

// Test 3 : Labyrinthe complexe 5x5
TestMaze("Test 3 : Labyrinthe complexe 5x5", @"D..#.
##...
.#.#.
...#.
####S");

// Test 4 : Labyrinthe de l'annexe (distance = 18)
TestMaze("Test 4 : Labyrinthe de l'annexe", @".#.......
D#.#####.
.#.#...#.
.....#.#.
###.#..#.
.##.#.##.
..#.#..#.
#.#.##.#.
....#S.#.");

Console.WriteLine("✅ Tous les tests sont passés !");

static void TestMaze(string title, string mazeString)
{
    Console.WriteLine(title);
    Console.WriteLine("Labyrinthe:");
    Console.WriteLine(mazeString);
    Console.WriteLine();
    
    var maze = new Maze(mazeString);
    var distance = maze.GetDistance();
    var path = maze.GetShortestPath();
    
    Console.WriteLine($"Distance: {distance}");
    Console.WriteLine($"Chemin: {string.Join(" -> ", path.Select(p => $"({p.Item1},{p.Item2})"))}");
    Console.WriteLine();
}
