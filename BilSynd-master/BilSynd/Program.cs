using System.Globalization;

namespace BilSynd
{

    internal class Program
    {
        static Random random = new Random();
        static List<Car> carList = new List<Car>();
        static List<Car> defectCarList = new List<Car>();


        static void Main(string[] args)
        {
            DefectCars();
            while (true) { Menu(); }
        }

        #region Menu
        static void Menu()
        {
            // change colour  of backround and text.
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Black;

            display(13, 0, "<<<   Registration menu  >>>");
            display(18, 2, "<<<   Select   >>>");
            display(18, 6, "[1] Show all cars ");
            display(18, 7, "[2] Register your car");
            display(18, 8, "[3] Search car ");


            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    ShowCars();
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    ServiceChekCar();
                    break;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    SearchCar();
                    break;

                default:
                    Console.WriteLine("Not understood");
                    break;
            }
        }

        private static void SearchCar()
        {
            Console.Clear();

            Console.WriteLine("Write your licence plate Search for licence plate: ");
            string input = Console.ReadLine();

            foreach (var car in carList)
                if (input == car.LicensePlate)
                {
                    ShowCustomer(car.Owner);
                    //Console.Clear();
                }


        }

        static void ShowCustomer(Person owner)
        {
            Console.Clear();
            Console.WriteLine($"Owner: {owner.Firstname} {owner.Lastname} \tPhone number: {owner.PhoneNumber}");
        }

        private static void ServiceChekCar()
        {
            Person owner = CreatePerson();
            Car car = CreateCar();
            car.Owner = owner;
            carList.Add(car);

            //Variable   Condition           True                 False
            string str = NeedInspection(car) ? "Bilen skal synes" : "Bilen skal IKKE synes";
            Console.WriteLine(str);

            string? str2 = IsCarDefect(car);
            if (str2 != null) Console.WriteLine("Bilen har følgende fabriksfejl: " + str2);
        }

        #endregion

        static string GenerateRandomLicensePlate()
        {
            Console.Clear();
            char a1 = (char)(random.Next(25) + 65);
            char a2 = (char)(random.Next(25) + 65);
            string b = random.Next(100).ToString("00");
            string c = random.Next(1000).ToString("000");
            return a1.ToString() + a2.ToString() + " " + b + " " + c;
        }

        static void display(int x, int y, string s)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(s);
        }

        static void ShowCars()
        {
            Console.Clear();
            Console.WriteLine("\n<<<   List of all cars registrated   >>>\n");
            foreach (Car car in carList)
            {
                ShowCar(car);
                Console.Clear();
            }
        }

        static bool NeedInspection(Car car)
        {
            //If car year + 5 is more than now year, then no inspection
            if (car.DateofRegistration.AddYears(5) >= DateTime.Now) return false;
            //If last inspeciton + 2 is more than now year, then no inspection
            if (car.LastInspection.AddYears(2) >= DateTime.Now) return false;
            return true;
        }

        static void ShowCar(Car car)
        {
            Console.WriteLine($"\nCar: {car.Brand} {car.Model} \tLicense plate: {car.LicensePlate}");
            Console.WriteLine($"Reg.Date: {car.DateofRegistration.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern)} \tLast inspection {car.LastInspection.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern)}");
        }

        static Car CreateCar()
        {
            Console.Clear();
            Console.WriteLine("\n<<<   Car Registration Menu   >>>\n");

            Car car = new Car();
            car.DateofRegistration = RandomDay(1990);
            car.ModelYear = car.DateofRegistration.Year - random.Next(2);
            car.LastInspection = RandomDay(car.ModelYear);
            car.EngineSize = 1 + random.Next(10) / 10f;
            car.LicensePlate = GenerateRandomLicensePlate();

            Console.Write("Brand: ");
            car.Brand = Console.ReadLine();
            Console.Write("Model: ");
            car.Model = Console.ReadLine();

            return car;
        }

        static DateTime RandomDay(int year)
        {
            DateTime start = new DateTime(year, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }

        static Person CreatePerson()
        {
            Console.Clear();
            Console.WriteLine("\n<<<   Costumer registration menu  >>>\n");

            Person person = new Person();
            Console.Write("First name: ");
            person.Firstname = Console.ReadLine();
            Console.Write("Last name: ");
            person.Lastname = Console.ReadLine();
            Console.Write("Telephone number: ");
            person.PhoneNumber = Console.ReadLine();
            return person;
        }

        static string? IsCarDefect(Car car)
        {
            foreach (var defectCar in defectCarList)
            {
                if (car.Brand == defectCar.Brand &&
                    car.Model == defectCar.Model &&
                    car.ModelYear <= defectCar.ModelYear) return defectCar.ManufacturingDefects;
            }
            return null;
        }

        static void DefectCars()
        {
            Car car1 = new Car() { Brand = "Alfa Romeo", Model = "G", ModelYear = 2022, ManufacturingDefects = "Wheels are square" };
            Car car2 = new Car() { Brand = "Lamborghini", Model = "Countach", ModelYear = 1986, ManufacturingDefects = "Wiper is baaad!" };
            defectCarList.Add(car1);
            defectCarList.Add(car2);
        }
    }
}