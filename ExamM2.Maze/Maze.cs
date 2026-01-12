namespace ExamM2.Maze;

/// <summary>
/// Résolveur de labyrinthe utilisant l'algorithme BFS (Breadth-First Search)
/// </summary>
public class Maze
{
    public int[][] Distances { get; init; }
    public bool[][] Grid { get; init; }
    public Queue<(int x, int y, int distance)> ToVisit { get; init; }
    public (int x, int y) Start { get; init; }
    public (int x, int y) Exit { get; init; }

    /// <summary>
    /// Parse le labyrinthe : D=départ, S=sortie, .=vide, #=mur
    /// </summary>
    public Maze(string maze)
    {
        var lines = maze.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Select(l => l.Trim())
                        .ToArray();

        int height = lines.Length;
        int width = lines[0].Length;

        Grid = new bool[height][];
        Distances = new int[height][];

        for (int y = 0; y < height; y++)
        {
            Grid[y] = new bool[width];
            Distances[y] = new int[width];

            for (int x = 0; x < width; x++)
            {
                char cell = lines[y][x];

                if (cell == 'D')
                {
                    Start = (x, y);
                    Grid[y][x] = true;
                }
                else if (cell == 'S')
                {
                    Exit = (x, y);
                    Grid[y][x] = true;
                }
                else if (cell == '.')
                {
                    Grid[y][x] = true;
                }
                else if (cell == '#')
                {
                    Grid[y][x] = false;
                }

                Distances[y][x] = 0;
            }
        }

        ToVisit = new Queue<(int x, int y, int distance)>();
        ToVisit.Enqueue((Start.x, Start.y, 0));
    }

    /// <summary>
    /// Calcule et retourne la distance minimale du départ à la sortie
    /// </summary>
    public int GetDistance()
    {
        int maxIterations = Grid.Length * Grid[0].Length * 2;
        int iterations = 0;
        
        while (!Fill())
        {
            iterations++;
            if (iterations > maxIterations)
                throw new InvalidOperationException("Aucun chemin trouvé vers la sortie");
        }
        
        return Distances[Exit.y][Exit.x];
    }

    /// <summary>
    /// Retourne les voisins orthogonaux valides (exclut murs, hors limites et départ)
    /// </summary>
    public IList<(int, int)> GetNeighbours(int x, int y)
    {
        var neighbours = new List<(int, int)>();
        int height = Grid.Length;
        int width = Grid[0].Length;

        var directions = new[] { (x, y - 1), (x, y + 1), (x - 1, y), (x + 1, y) };

        foreach (var (nx, ny) in directions)
        {
            if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                continue;

            if (!Grid[ny][nx])
                continue;

            if (nx == Start.x && ny == Start.y)
                continue;

            neighbours.Add((nx, ny));
        }

        return neighbours;
    }

    /// <summary>
    /// Traite une cellule de la queue BFS et retourne true si on atteint la sortie
    /// </summary>
    public bool Fill()
    {
        if (ToVisit.Count == 0)
            return false;

        var (x, y, distance) = ToVisit.Dequeue();

        if (x == Exit.x && y == Exit.y)
        {
            Distances[y][x] = distance;
            return true;
        }

        if (Distances[y][x] != 0 && !(x == Start.x && y == Start.y))
            return false;

        Distances[y][x] = distance;

        var neighbours = GetNeighbours(x, y);
        foreach (var (nx, ny) in neighbours)
        {
            ToVisit.Enqueue((nx, ny, distance + 1));
        }

        return false;
    }

    /// <summary>
    /// Reconstruit le chemin optimal en remontant depuis la sortie jusqu'au départ
    /// </summary>
    public IList<(int, int)> GetShortestPath()
    {
        var path = new List<(int, int)>();
        var (currentX, currentY) = Exit;
        
        path.Add((currentX, currentY));
        
        while (currentX != Start.x || currentY != Start.y)
        {
            int currentDistance = Distances[currentY][currentX];
            var directions = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };
            bool found = false;
            
            foreach (var (dx, dy) in directions)
            {
                int nx = currentX + dx;
                int ny = currentY + dy;
                
                if (nx < 0 || ny < 0 || nx >= Grid[0].Length || ny >= Grid.Length)
                    continue;
                
                if (!Grid[ny][nx])
                    continue;
                
                if (Distances[ny][nx] == currentDistance - 1)
                {
                    path.Add((nx, ny));
                    currentX = nx;
                    currentY = ny;
                    found = true;
                    break;
                }
            }
            
            if (!found)
                break;
        }
        
        path.Reverse();
        return path;
    }
}
