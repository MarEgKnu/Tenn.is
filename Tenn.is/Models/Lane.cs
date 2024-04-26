namespace Tennis.Models
{
    public class Lane
    {

        public int Id { get; set; }
        public bool OutDoor { get; set; }
        public bool PadelTennis { get; set; }

        public Lane(int id, bool outDoor, bool padelTennis)
        {
            Id = id;
            OutDoor = outDoor;
            PadelTennis = padelTennis;
        }

        public Lane()
        {
            
        }

        public override string ToString()
        {
            return $"";
        }
    }
}
