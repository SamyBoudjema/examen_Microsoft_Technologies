namespace ExamM2.Maze;

public class Maze
{
    public int[][] Distances { get; init; }
    public bool[][] Grid { get; init; }
    public Queue<(int x, int y, int distance)> ToVisit { get; init; }
    public (int x, int y) Start { get; init; }
    public (int x, int y) Exit { get; init; }

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

    public int GetDistance()
    {
        while (!Fill())
        {
            // Continue à traiter les cellules jusqu'à atteindre la sortie
        }

        return Distances[Exit.y][Exit.x];
    }

    public IList<(int, int)> GetNeighbours(int x, int y)
    {
        var neighbours = new List<(int, int)>();
        int height = Grid.Length;
        int width = Grid[0].Length;

        var directions = new[]
        {
            (x, y - 1),
            (x, y + 1),
            (x - 1, y),
            (x + 1, y)
        };

        foreach (var (nx, ny) in directions)
        {
            if (nx < 0 || ny < 0 || nx >= width || ny >= height)
            {
                continue;
            }

            if (!Grid[ny][nx])
            {
                continue;
            }

            if (nx == Start.x && ny == Start.y)
            {
                continue;
            }

            neighbours.Add((nx, ny));
        }

        return neighbours;
    }

    public bool Fill()
    {
        if (ToVisit.Count == 0)
        {
            return false;
        }

        var (x, y, distance) = ToVisit.Dequeue();

        if (x == Exit.x && y == Exit.y)
        {
            Distances[y][x] = distance;
            return true;
        }

        if (Distances[y][x] != 0 && !(x == Start.x && y == Start.y))
        {
            return false;
        }

        Distances[y][x] = distance;

        var neighbours = GetNeighbours(x, y);
        foreach (var (nx, ny) in neighbours)
        {
            ToVisit.Enqueue((nx, ny, distance + 1));
        }

        return false;
    }

    public IList<(int, int)> GetShortestPath()
    {
        var path = new List<(int, int)>();
        var (currentX, currentY) = Exit;
        
        // Ajouter la sortie au début du chemin
        path.Add((currentX, currentY));
        
        // Remonter depuis la sortie jusqu'au départ
        while (currentX != Start.x || currentY != Start.y)
        {
            int currentDistance = Distances[currentY][currentX];
            
            // Regarder tous les voisins (y compris le Start cette fois)
            var directions = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };
            bool found = false;
            
            foreach (var (dx, dy) in directions)
            {
                int nx = currentX + dx;
                int ny = currentY + dy;
                
                // Vérifier les limites
                if (nx < 0 || ny < 0 || nx >= Grid[0].Length || ny >= Grid.Length)
                    continue;
                
                // Vérifier que c'est une case valide
                if (!Grid[ny][nx])
                    continue;
                
                // Trouver le voisin avec la distance inférieure de 1
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
        
        // Inverser le chemin pour qu'il aille du départ à la sortie
        path.Reverse();
        return path;
    }
}
