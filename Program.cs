using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace qup
{
    class Program
    {
        public class TransportMileageEvantArgs : EventArgs
        {
            public int KmCount { get; set; }

            public TransportMileageEvantArgs(int kmCount)
            {
                KmCount = kmCount;
            }
        }
        public class TransportSTO
        {
            public void OverComingTheNext10Km(object sender, TransportMileageEvantArgs arg)
            {
                Transport transport = sender as Transport;
                if (transport.KmCount > 10000 * transport.EventTriggers)
                {
                    Console.WriteLine($"Авто потребує провірки СТО!");
                }
            }
        }
        public abstract class Transport
        {
            public int GovNumber { get; set; }
            public int YearOfCar { get; set; }
            public int NumSeats { get; set; }
            public double PriceOfTicket { get; set; }
            public int KmCount { get; set; }
            public bool IsNew { get; set; }
            private uint eventTriggers;
            public uint EventTriggers { get { return eventTriggers; } set { } }


            public Transport(int govNumber, int yearOfCar, int numSeats, double priceOfTicket, int kmCount, bool isNew)
            {
                GovNumber = govNumber;
                YearOfCar = yearOfCar;
                NumSeats = numSeats;
                PriceOfTicket = priceOfTicket;
                KmCount = kmCount;
                IsNew = isNew;
            }

            public abstract double CalculateTicketPrice(double distance);

            public override string ToString()
            {
                return $"Transport: GovNumber = {GovNumber}, YearOfCar = {YearOfCar}, NumSeats = {NumSeats}, PriceOfTicket = {PriceOfTicket}, KmCount = {KmCount}, IsNew = {IsNew}";
            }
            public delegate void MileageEventHandler(object sender, TransportMileageEvantArgs e);
            public event MileageEventHandler MileageChanged;
            private void checkAgeForInspection()
            {
                if (KmCount >= 10000 * eventTriggers)
                {
                    OnServiceYearChanged(new TransportMileageEvantArgs(KmCount));
                }
            }
            protected virtual void OnServiceYearChanged(TransportMileageEvantArgs e)
            {
                MileageChanged?.Invoke(this, e);
            }
        }

        public class Taxi : Transport
        {
            public int MaxSpeed { get; set; }

            public Taxi(int govNumber, int yearOfCar, int numSeats, double priceOfTicket, int kmCount, bool isNew, int maxSpeed)
                : base(govNumber, yearOfCar, numSeats, priceOfTicket, kmCount, isNew)
            {
                MaxSpeed = maxSpeed;
                NumSeats = Math.Min(4, numSeats);
            }

            public override double CalculateTicketPrice(double distance)
            {
                return distance * PriceOfTicket / NumSeats;
            }

            public override string ToString()
            {
                return $"Taxi: GovNumber = {GovNumber}, YearOfCar = {YearOfCar}, NumSeats = {NumSeats}, PriceOfTicket = {PriceOfTicket}, KmCount = {KmCount}, IsNew = {IsNew}, MaxSpeed = {MaxSpeed}";
            }
        }

        public class Bus : Transport
        {
            public bool HasAirConditioner { get; set; }

            public Bus(int govNumber, int yearOfCar, int numSeats, double priceOfTicket, int kmCount, bool isNew, bool hasAirConditioner)
                : base(govNumber, yearOfCar, numSeats, priceOfTicket, kmCount, isNew)
            {
                HasAirConditioner = hasAirConditioner;
            }

            public override double CalculateTicketPrice(double distance)
            {
                return distance * PriceOfTicket / (NumSeats / 2);
            }
            public override string ToString()
            {
                return $"Bus: GovNumber = {GovNumber}, YearOfCar = {YearOfCar}, NumSeats = {NumSeats}, PriceOfTicket = {PriceOfTicket}, KmCount = {KmCount}, IsNew = {IsNew}, HasAirConditioner = {HasAirConditioner}";
            }
        }

        static void Main(string[] args)
        {
            List<Transport> transports = new List<Transport>();

            // Додавання декількох таксі і автобусів
            transports.Add(new Taxi(1, 2010, 4, 10.5, 50000, false, 150));
            transports.Add(new Taxi(2, 2015, 4, 12.75, 30000, true, 160));
            transports.Add(new Bus(3, 2018, 50, 5.25, 100000, false, true));
            transports.Add(new Bus(4, 2020, 50, 6.0, 80000, true, false));

            // Виведення всіх транспортних засобів
            Console.WriteLine("Всі транспортні засоби:");
            foreach (Transport transport in transports)
            {
                Console.WriteLine(transport);
            }

            // Додавання нового транспортного засобу
            Console.WriteLine("\nДодавання нового транспортного засобу:");
            Console.WriteLine("Введіть реєстраційний номер авто: ");
            int govNumber = int.Parse(Console.ReadLine());

            Console.WriteLine("Введіть рік випуска транспорта: ");
            int yearOfCar = int.Parse(Console.ReadLine());

            Console.WriteLine("Введіть кількість сидінь: ");
            int numSeats = int.Parse(Console.ReadLine());

            Console.WriteLine("Введіть вартість квитка: ");
            double priceOfTicket = double.Parse(Console.ReadLine());

            Console.WriteLine("Введіть пробіг авто: ");
            int kmCount = int.Parse(Console.ReadLine());

            Console.WriteLine("Введіть, чи є авто новим (true/false): ");
            bool isNew = bool.Parse(Console.ReadLine());

            Console.WriteLine("Введіть, чи має таксі максимальну швидкість (0 - якщо ні): ");
            int maxSpeed = int.Parse(Console.ReadLine());

            Console.WriteLine("Введіть, чи має автобус кондиціонер (true/false): ");
            bool hasAirConditioner = bool.Parse(Console.ReadLine());

            Transport newTransport;
            if (maxSpeed > 0)
            {
                newTransport = new Taxi(govNumber, yearOfCar, numSeats, priceOfTicket, kmCount, isNew, maxSpeed);
            }
            else
            {
                newTransport = new Bus(govNumber, yearOfCar, numSeats, priceOfTicket, kmCount, isNew, hasAirConditioner);
            }

            transports.Add(newTransport);

            // Знайдення найдешевшого за собівартістю транспорту
            Transport cheapestTransport = transports[0];
            foreach (Transport transport in transports)
            {
                if (transport.PriceOfTicket < cheapestTransport.PriceOfTicket)
                {
                    cheapestTransport = transport;
                }
            }

            Console.WriteLine("\nНайдешевший за собівартістю транспорт:");
            Console.WriteLine(cheapestTransport);

            // Знайдення найшвидшого транспорту
            Transport fastestTransport = transports[0];
            foreach (Transport transport in transports)
            {
                if (transport is Taxi taxi && taxi.MaxSpeed > 0 && taxi.MaxSpeed > ((Taxi)fastestTransport).MaxSpeed)
                {
                    fastestTransport = transport;
                }
            }

            Console.WriteLine("\nНайшвидший транспорт:");
            Console.WriteLine(fastestTransport);

            // Розрахунок загальної кількості місць у автобусах
            int totalNumSeatsInBuses = 0;
            foreach (Transport transport in transports)
            {
                if (transport is Bus bus)
                {
                    totalNumSeatsInBuses += bus.NumSeats;
                }
            }

            Console.WriteLine($"\nЗагальна кількість місць у автобусах: {totalNumSeatsInBuses}");
            // Відправлення кожного транспортного засобу на задану відстань у дві мандрівки
            Console.WriteLine("\nВведіть відстань для мандрівки: ");
            double distance = double.Parse(Console.ReadLine());

            foreach (Transport transport in transports)
            {
                double totalTicketPrice = transport.CalculateTicketPrice(distance) * 2;
                Console.WriteLine($"Транспорт {transport.GovNumber}: Вартість квитків на дві мандрівки: {totalTicketPrice}");
            }

            // Збереження колекції об'єктів до файла
            using (StreamWriter writer = new StreamWriter("transports.txt"))
            {
                foreach (Transport transport in transports)
                {
                    writer.WriteLine(transport.ToString());
                }
            }

            // Зчитування колекції об'єктів з файла
            List<Transport> savedTransports = new List<Transport>();
            using (StreamReader reader = new StreamReader("transports.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(':');
                    if (parts[0] == "Taxi")
                    {
                        int govNumberFromFile = int.Parse(parts[1].Split('=')[1].Trim());
                        int yearOfCarFromFile = int.Parse(parts[2].Split('=')[1].Trim());
                        int numSeatsFromFile = int.Parse(parts[3].Split('=')[1].Trim());
                        double priceOfTicketFromFile = double.Parse(parts[4].Split('=')[1].Trim());
                        int kmCountFromFile = int.Parse(parts[5].Split('=')[1].Trim());
                        bool isNewFromFile = bool.Parse(parts[6].Split('=')[1].Trim());
                        int maxSpeedFromFile = int.Parse(parts[7].Split('=')[1].Trim());

                        savedTransports.Add(new Taxi(govNumberFromFile, yearOfCarFromFile, numSeatsFromFile, priceOfTicketFromFile, kmCountFromFile, isNewFromFile, maxSpeedFromFile));
                    }
                    else if (parts[0] == "Bus")
                    {
                        int govNumberFromFile = int.Parse(parts[1].Split('=')[1].Trim());
                        int yearOfCarFromFile = int.Parse(parts[2].Split('=')[1].Trim());
                        int numSeatsFromFile = int.Parse(parts[3].Split('=')[1].Trim());
                        double priceOfTicketFromFile = double.Parse(parts[4].Split('=')[1].Trim());
                        int kmCountFromFile = int.Parse(parts[5].Split('=')[1].Trim());
                        bool isNewFromFile = bool.Parse(parts[6].Split('=')[1].Trim());
                        bool hasAirConditionerFromFile = bool.Parse(parts[7].Split('=')[1].Trim());

                        savedTransports.Add(new Bus(govNumberFromFile, yearOfCarFromFile, numSeatsFromFile, priceOfTicketFromFile, kmCountFromFile, isNewFromFile, hasAirConditionerFromFile));
                    }
                }
            }

            // Знайдення три транспортних засоби з найдешевшими квитками
            savedTransports.Sort((x, y) => x.PriceOfTicket.CompareTo(y.PriceOfTicket));

            List<Transport> cheapestTransports = savedTransports.GetRange(0, Math.Min(3, savedTransports.Count));

            Console.WriteLine("\nТри транспортні засоби з найдешевшими квитками:");
            foreach (Transport transport in cheapestTransports)
            {
                Console.WriteLine(transport);
            }

            TransportSTO STO = new TransportSTO();
            foreach (Transport transport in transports)
            {
                Console.WriteLine(transport);
                transport.MileageChanged += STO.OverComingTheNext10Km;
                transport.KmCount = 25000;
                Console.WriteLine(transport);

            }
        }
    }
}