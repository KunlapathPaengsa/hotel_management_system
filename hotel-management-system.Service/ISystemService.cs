namespace hotel_management_system.Service
{
    public interface ISystemService
    {
        public string CreateHotel(string floor, string room);
        public  string GetListAvailableRooms();
        public string Booking(string num, string name, string age);
    }
}