using hotel_management_system.Service;
using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ReadAllText
{
    public class Test
    {

        static void Main(string[] args)
        {
            var systemService = new SystemService();
            //string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            //var path = Path.Combine(dir, "input.txt");
            var dir = Environment.CurrentDirectory.Replace("bin\\Debug\\net6.0", "");
            var path = Path.Combine(dir, "input.txt");
            string[] content = File.ReadAllLines(path, Encoding.UTF8);

            //var filePath = Path.Combine(dir, "filename.txt");
            TextWriter tw = new StreamWriter(Path.Combine(dir, "output.txt"));

            Console.WriteLine("***Display***\n");
            foreach (var item in content)
            {
                var command = item.Split(" ");
                switch (command[0])
                {
                    case "create_hotel":
                        var x = systemService.CreateHotel(command[1], command[2]);
                        Console.WriteLine(x);
                        tw.WriteLine(x);
                        break;
                    case "book":
                        var b = systemService.Booking(command[1], command[2], command[3]);
                        Console.WriteLine(b);
                        tw.WriteLine(b);
                        break;
                    case "list_available_rooms":
                        var t = systemService.GetListAvailableRooms();
                        Console.WriteLine(t);
                        tw.WriteLine(t);
                        break;
                    case "checkout":
                        var o = systemService.Checkout(command[1], command[2]);
                        Console.WriteLine(o);
                        tw.WriteLine(o);
                        break;
                    case "list_guest":
                        var users = systemService.GetListGuest();
                        Console.WriteLine(users);
                        tw.WriteLine(users);
                        break;
                    case "get_guest_in_room":
                        var gg = systemService.GetGuestInRoom(command[1]);
                        Console.WriteLine(gg);
                        tw.WriteLine(gg);
                        break;
                    case "list_guest_by_age":
                        var age = systemService.GetListGuestByAge(command[1], command[2]);
                        Console.WriteLine(age);
                        tw.WriteLine(age);
                        break;
                    case "list_guest_by_floor":
                        var gf = systemService.GetListGuestByFloor(command[1]);
                        Console.WriteLine(gf);
                        tw.WriteLine(gf);
                        break;
                    case "book_by_floor":
                        var bf = systemService.BookByFloor(command[1], command[2], command[3]);
                        Console.WriteLine(bf);
                        tw.WriteLine(bf);
                        break;
                    case "checkout_guest_by_floor":
                        var cf = systemService.CheckoutGuestByFloor(command[1]);
                        Console.WriteLine(cf);
                        tw.WriteLine(cf);
                        break;
                    default:
                        Console.WriteLine($"Missing command [{command[0]}] from : {item}");
                        break;
                }
            }
            tw.Close();

        }
    }
}