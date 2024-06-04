namespace PokemonGame.Misc
{
    public class BoundaryData
    {
        public bool[,] OutsideInside { get; set; }
        public int[,] CollisionId { get; set; }
        public bool[,] Boundaries { get; set; }
        public bool[,] Upstairs { get; set; }
    }
}
