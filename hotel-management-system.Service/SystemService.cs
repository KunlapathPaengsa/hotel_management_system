using hotel_management_system.Model;
using System.Text;

namespace hotel_management_system.Service
{
    public class SystemService : ISystemService
    {
        private List<string> availableRooms = new();
        private List<UserProfile> userProfiles = new();
        private int initRoom;

        public string CreateHotel(string floor, string room)
        {
            var intFloor = int.Parse(floor) + 1;
            var intRoom = int.Parse(room);
            initRoom = intRoom;
            for (int i = 1; i < intFloor; i++)
            {
                for (int j = 1; j < intRoom + 1; j++)
                {
                    availableRooms.Add($"{i}{j:D2}");
                }
            }
            return $"Hotel created with {floor} floor(s), {room} room(s) per floor.";
        }

        public string GetListAvailableRooms()
        => new StringBuilder().AppendJoin(", ", availableRooms).ToString();


        public string Booking(string roomId, string username, string age)
        {
            if (!availableRooms.Contains(roomId))
            {
                var currentName = userProfiles.Where(w => w.Room == roomId).Select(s => s.Name).FirstOrDefault();
                return $"Cannot book room {roomId} for {username}, The room is currently booked by {currentName}.";
            }
            var keycardNumber = BookUserProfile(roomId, username, age);
            return Massage(new List<string> { roomId }, true, new List<int> { keycardNumber }, username);
        }


        public string Checkout(string keycardId, string username)
        {
            var user = userProfiles.FirstOrDefault(w => w.Keycard.ToString() == keycardId);
            if (user?.Name == username)
            {
                availableRooms.Add(user.Room);
                userProfiles.Remove(user);
                return Massage(new List<string> { user.Room }, false, null, null);
            }
            return $"Only {user?.Name} can checkout with keycard number {keycardId}.";
        }

        public string GetListGuest()
        => new StringBuilder().AppendJoin(", ", userProfiles.Select(s => s.Name).Distinct()).ToString();

        public string GetGuestInRoom(string room)
       => userProfiles.Where(f => f.Room == room).Select(s => s.Name).FirstOrDefault();

        public string GetListGuestByAge(string operatorSign, string age)
        => OperatorCompare(operatorSign, age);

        public string GetListGuestByFloor(string floor)
        => new StringBuilder().AppendJoin(", ", userProfiles.Where(s => s.Room.Remove(s.Room.Length - 2) == floor).Select(s => s.Name)).ToString();


        public string BookByFloor(string floor, string username, string age)
        {
            var rooms = availableRooms.Where(w => w.Remove(w.Length - 2) == floor).ToList();
            if (rooms.Count == initRoom)
            {
                var keycards = new List<int>();
                foreach (var room in rooms)
                {
                    keycards.Add(BookUserProfile(room, username, age));
                }
                return Massage(rooms, true, keycards, null);
            }
            return $"Cannot book floor {floor} for {username}.";
        }

        public string CheckoutGuestByFloor(string floor)
        {
            var rooms = userProfiles.Where(s => s.Room.Remove(s.Room.Length - 2) == floor).Select(s => s.Room).ToList();
            availableRooms.AddRange(rooms);
            userProfiles.RemoveAll(r => rooms.Contains(r.Room));
            return Massage(rooms, false, null, null);

        }

        private int BookUserProfile(string num, string name, string age)
        {
            int keycardNumber = userProfiles.Count + 1;

            if (userProfiles.Count > 0 && userProfiles.Select(s => s.Keycard).Max() > userProfiles.Count)
            {
                var keycardList = userProfiles.Select(s => s.Keycard);
                keycardNumber = Enumerable.Range(1, userProfiles.Count).Except(keycardList).Min();
            }
            userProfiles.Add(new UserProfile { Age = age, Name = name, Room = num, Keycard = keycardNumber });
            availableRooms.Remove(num);
            return keycardNumber;
        }

        private string Massage(List<string> rooms, bool isBook, List<int> keycards, string name)
        {
            var r = new StringBuilder("Room ");
            r.AppendJoin(", ", rooms.OrderBy(o => o)).Append(rooms.Count > 1 ? " are " : " is ");
            if (isBook)
            {
                r.Append("booked ");
                if (name != null)
                {
                    r.Append($"by {name} ");
                }
                r.Append("with keycard number ");
                r.AppendJoin(", ", keycards.OrderBy(o => o));
                r.Append(".");
                return r.ToString();
            }
            r.Append("checkout.");
            return r.ToString();
        }

        private string OperatorCompare(string operatorSign, string age)
        {
            var result = new StringBuilder();
            switch (operatorSign)
            {
                case "<":
                    result.AppendJoin(", ", userProfiles.Where(w => int.Parse(w.Age) < int.Parse(age)).Select(s => s.Name).Distinct());
                    break;

                case ">":
                    result.AppendJoin(", ", userProfiles.Where(w => int.Parse(w.Age) > int.Parse(age)).Select(s => s.Name).Distinct());
                    break;

                default:
                    break;
            }
            if (operatorSign.Contains('='))
            {
                result.AppendJoin(", ", userProfiles.Where(w => int.Parse(w.Age) == int.Parse(age)).Select(s => s.Name).Distinct());
            }
            return result.ToString();
        }
    }
}