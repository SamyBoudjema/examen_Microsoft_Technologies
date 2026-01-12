namespace ExamM2.Maze.Tests;

public class MazeShortestPathTests
{
    [Fact]
    public void GetShortestPath_SimpleMaze_ReturnsCorrectPath()
    {
        var maze = new Maze(@"D..
...
..S");

        maze.GetDistance(); // Calcule les distances
        var path = maze.GetShortestPath();

        // Vérifions que le chemin commence au Start et termine à l'Exit
        Assert.Equal((0, 0), path.First()); // Start
        Assert.Equal((2, 2), path.Last());  // Exit
        Assert.Equal(5, path.Count); // 5 cases total (D inclus, S inclus)
    }

    [Fact]
    public void GetShortestPath_MazeWithWalls_ReturnsCorrectPath()
    {
        var maze = new Maze(@"D.#
..#
..S");

        maze.GetDistance();
        var path = maze.GetShortestPath();

        Assert.Equal((0, 0), path.First()); // Start
        Assert.Equal((2, 2), path.Last());  // Exit
        Assert.Equal(5, path.Count); // 5 cases
        
        // Vérifions qu'il n'y a pas de murs dans le chemin (Grid[y][x] doit être true)
        foreach (var (x, y) in path)
        {
            Assert.True(maze.Grid[y][x]);
        }
    }

    [Fact]
    public void GetShortestPath_PathIsSequential()
    {
        var maze = new Maze(@"D..
...
..S");

        maze.GetDistance();
        var path = maze.GetShortestPath();

        // Vérifions que chaque case est adjacente à la suivante
        for (int i = 0; i < path.Count - 1; i++)
        {
            var (x1, y1) = path[i];
            var (x2, y2) = path[i + 1];
            
            int distance = Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
            Assert.Equal(1, distance); // Voisins orthogonaux
        }
    }
}
