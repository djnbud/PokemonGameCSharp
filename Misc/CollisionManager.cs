using System.Drawing;
using PokemonGame.Panels;

namespace PokemonGame.Misc
{
    public class CollisionManager
{
    private bool[,] boundaries;
    private bool[,] outsideInside;
    private bool[,] upstairs;
    private int[,] collisionId;
    public GameDetails gameDetails;
    private GamePanel gamePanel;
    
    private int currentCollisionId;
    public CollisionManager(BoundaryData boundaryData, GameDetails gameDetails, GamePanel gamePanel)
    {
        boundaries = boundaryData.Boundaries;
        outsideInside = boundaryData.OutsideInside;
        upstairs = boundaryData.Upstairs;
        collisionId = boundaryData.CollisionId;
        this.gameDetails = gameDetails;
        this.gamePanel = gamePanel;
    }

    public bool IsCollision(Point position, int width, int height)
    {
        // Check boundaries
        if (CheckCollision(boundaries, position, width, height))
            return true;

        return false;
    }

    public bool IsOutsideInside(Point position, int width, int height)
    {
        // Check outsideInside
        return CheckCollision(outsideInside, position, width, height);
    }

    public bool IsUpstairs(Point position, int width, int height)
    {
        // Check upstairs
        return CheckCollision(upstairs, position, width, height);
    }

    private bool CheckCollision(bool[,] collisionArray, Point position, int width, int height)
    {
        int startX = position.X / 48;
        int startY = (position.Y + height / 2) / 48;
        int endX = (position.X + width) / 48;
        int endY = (position.Y + height) / 48;

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (x < 0 || y < 0 || x >= collisionArray.GetLength(0) || y >= collisionArray.GetLength(1))
                    return true; // Out of bounds
                if (collisionArray[x, y]){
                    currentCollisionId = collisionId[x, y];
                    return true;
                }
            }
        }
        return false;
    }

    public Point newPlayerLocation(int currentCollisionId)
    {
        Point newLocation = new Point();

        for (int x = 0; x < collisionId.GetLength(0); x++)
        {
            for (int y = 0; y < collisionId.GetLength(1); y++)
            {
                if(collisionId[x, y] == currentCollisionId){
                    if(collisionId[x, y -1] != 27 && collisionId[x, y -2] != 27){
                        return newLocation = new Point(x * 48, (y - 2) * 48);
                    }
                    if(collisionId[x, y + 1] != 27 && collisionId[x, y + 2] != 27 ){
                        return newLocation = new Point(x * 48, (y + 2) * 48);
                    }
                    if(collisionId[x - 1, y] != 27 && collisionId[x - 2, y] != 27){
                        return newLocation = new Point((x - 2) * 48, y * 48);
                    }
                    if(collisionId[x + 1, y] != 27 && collisionId[x + 2, y] != 27){
                        return newLocation = new Point((x + 2) * 48, y * 48);
                    }
                }
            }
        }
        return newLocation;
    }

    public void SwapMap(Point playerPosition, bool inside)
    {
        string currentLocation = gameDetails.Location;
        gameDetails.newLocation(currentLocation, inside);
        gamePanel.Reload(currentCollisionId); // Reload the game panel based on new gameDetails
    }
}

}
