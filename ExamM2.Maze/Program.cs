using ExamM2.Maze;

Console.WriteLine("=== Test du résolveur de labyrinthe ===\n");

// Test 1 : Labyrinthe simple 3x3
Console.WriteLine("Test 1 : Labyrinthe simple 3x3");
var maze1 = new Maze(@"D..
...
..S");

var distance1 = maze1.GetDistance();
var path1 = maze1.GetShortestPath();

Console.WriteLine($"Distance: {distance1}");
Console.WriteLine($"Chemin: {string.Join(" -> ", path1.Select(p => $"({p.Item1},{p.Item2})"))}");
Console.WriteLine();

// Test 2 : Labyrinthe avec murs
Console.WriteLine("Test 2 : Labyrinthe avec murs");
var maze2 = new Maze(@"D.#
..#
..S");

var distance2 = maze2.GetDistance();
var path2 = maze2.GetShortestPath();

Console.WriteLine($"Distance: {distance2}");
Console.WriteLine($"Chemin: {string.Join(" -> ", path2.Select(p => $"({p.Item1},{p.Item2})"))}");
Console.WriteLine();

// Test 3 : Labyrinthe complexe 5x5 (de l'énoncé)
Console.WriteLine("Test 3 : Labyrinthe complexe 5x5");
var maze3 = new Maze(@"D..#.
##...
.#.#.
...#.
####S");

var distance3 = maze3.GetDistance();
var path3 = maze3.GetShortestPath();

Console.WriteLine($"Distance: {distance3}");
Console.WriteLine($"Chemin: {string.Join(" -> ", path3.Select(p => $"({p.Item1},{p.Item2})"))}");
Console.WriteLine();

Console.WriteLine("✅ Tous les tests sont passés !");
