using System.Drawing;

namespace PokemonGame
{
    //Holds the game information of its current state
    public class GameDetails
    {
        public Point PlayerPosition { get; set; }
        public string Location { get; set; }
        public bool Inside { get; set; }
        public string BackgroundImagePath { get; set; }
        public string ForegroundImagePath { get; set; }
        public string CollisionDataPath { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        public GameDetails(Point playerPosition, string location, bool inside, int mapWidth, int mapHeight)
        {
            PlayerPosition = playerPosition;
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            newLocation(location, inside);
        }

        public void newLocation(string location, bool inside){
            Location = location;
            Inside = inside;

            var insideText = "inside";
            if(!inside){
                insideText = "outside";
            }

            var backgroundImagePath = "Assets/locations/" + location + "/" + insideText + "/" + "background.png";
            var foregroundImagePath = "Assets/locations/" + location + "/" + insideText + "/" + "foreground.png";
            var collisionDataPath = "Data/locations/" + location + "/" + insideText + "/" + "collisions.json";

            BackgroundImagePath = backgroundImagePath;
            ForegroundImagePath = foregroundImagePath;
            CollisionDataPath = collisionDataPath;
        }
    }
}
