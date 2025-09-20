using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VehicleServiceCenter
{
    class Program
    {

        // Class ServiceRecord to hold details of each service entry.
        class ServiceRecord
        {
            public string CustomerName { get; set; } 
            public string VehicleNumber { get; set; }
            public string VehicleType { get; set; }
            public DateTime ServiceDate { get; set; }
            public string ServiceType { get; set; }
            public double Cost { get; set; }
        }


        //Using StreamWriter to save records
        static void SaveRecordsToFile()
        {
            using (StreamWriter writer = new StreamWriter(FileName))
            {
                foreach (var record in records)
                    writer.WriteLine($"{record.CustomerName},{record.VehicleNumber},{record.VehicleType},{record.ServiceDate},{record.ServiceType},{record.Cost}");
            }
            Console.WriteLine("\nRecords saved to file successfully!");
        }

        //Using StreamReader to load existing records.
        static void LoadRecordsFromFile()
        {
            if (File.Exists(FileName))
            {
                foreach (var line in File.ReadLines(FileName))
                {
                    var data = line.Split(',');
                    records.Add(new ServiceRecord
                    {
                        CustomerName = data[0],
                        VehicleNumber = data[1],
                        VehicleType = data[2],
                        ServiceDate = DateTime.Parse(data[3]),
                        ServiceType = data[4],
                        Cost = double.Parse(data[5])
                    });
                }
            }
        }


        // List to hold all service records in memory.
        static List<ServiceRecord> records = new List<ServiceRecord>();
        const string FileName = "service_records.txt";



        static void Main(string[] args)
        {
            LoadRecordsFromFile();

            while (true)
            {
                Console.WriteLine("\nVEHICLE SERVICE CENTER");
                Console.WriteLine("1. Add New Service Record");
                Console.WriteLine("2. Display All Service Records");
                Console.WriteLine("3. Search by Vehicle Number");
                Console.WriteLine("4. Save Records to File");
                Console.WriteLine("5. Calculate Total Service Cost for Today");
                Console.WriteLine("6. Delete Service Record");
                Console.WriteLine("7. Exit");
                Console.Write("\n Enter your choice: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddServiceRecord(); break;
                    case "2": DisplayRecords(); break;
                    case "3": SearchByVehicleNumber(); break;
                    case "4": SaveRecordsToFile(); break;
                    case "5": CalculateTotalCost(); break;
                    case "6": DeleteServiceRecord(); break;
                    case "7": Console.WriteLine("Exiting the program. Goodbye!"); return;
                    default: Console.WriteLine("Invalid choice. Try again."); break;
                }
            }
        }


        static void AddServiceRecord()
        {
            Console.Write("\nEnter Customer Name: ");
            string customerName = Console.ReadLine();

            Console.Write("Enter Vehicle Number (e.g., GW-1234-24): ");
            string vehicleNumber = Console.ReadLine();

            Console.Write("Enter Vehicle Type (Car, Motorcycle, Truck): ");
            string vehicleType = Console.ReadLine();

            Console.Write("Enter Service Date (DD-MM-YYYY): ");
            DateTime serviceDate;
            while (!DateTime.TryParse(Console.ReadLine(), out serviceDate))
                Console.Write("Invalid date. Enter again (DD-MM-YYYY): ");

            Console.Write("Enter Service Type (Oil Change, Tire Rotation, Full Service, etc.): ");
            string serviceType = Console.ReadLine();

            Console.Write("Enter Cost: ");
            double cost;
            while (!double.TryParse(Console.ReadLine(), out cost))
                Console.Write("Invalid input. Enter numeric cost: ");

            records.Add(new ServiceRecord
            {
                CustomerName = customerName,
                VehicleNumber = vehicleNumber,
                VehicleType = vehicleType,
                ServiceDate = serviceDate,
                ServiceType = serviceType,
                Cost = cost
            });

            Console.WriteLine("\nService record added successfully!");
        }


        static void DisplayRecords()
        {
            Console.WriteLine("\n Service Records:");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Customer Name | Vehicle Number | Type | Date | Service | Cost");
            Console.WriteLine("-----------------------------------------------------------");

            foreach (var record in records)
                Console.WriteLine($"{record.CustomerName} | {record.VehicleNumber} | {record.VehicleType} | {record.ServiceDate.ToShortDateString()} | {record.ServiceType} | ${record.Cost}");

            Console.WriteLine("-----------------------------------------------------------");
        }


        static void SearchByVehicleNumber()
        {
            Console.Write("\nEnter Vehicle Number to search: ");
            string vehicleNumber = Console.ReadLine();
            var foundRecord = records.FirstOrDefault(r => r.VehicleNumber == vehicleNumber);

            if (foundRecord != null)
            {
                Console.WriteLine("\nRecord Found:");
                Console.WriteLine($"Customer: {foundRecord.CustomerName}, Type: {foundRecord.VehicleType}, Date: {foundRecord.ServiceDate.ToShortDateString()}, Service: {foundRecord.ServiceType}, Cost: ${foundRecord.Cost}");
            }
            else
            {
                Console.WriteLine("No record found for the provided vehicle number.");
            }
        }


        static void DeleteServiceRecord()
        {
            Console.Write("Enter Vehicle Number to delete: ");
            string vehicleNumber = Console.ReadLine();

            var recordToDelete = records.FirstOrDefault(r => r.VehicleNumber == vehicleNumber);
            if (recordToDelete != null)
            {
                records.Remove(recordToDelete);
                Console.WriteLine("Record deleted successfully.");
                SaveRecordsToFile();  // Save after deletion
            }
            else
            {
                Console.WriteLine("No matching record found.");
            }
        }


        static void CalculateTotalCost()
        {
            double totalCost = records.Where(r => r.ServiceDate.Date == DateTime.Today).Sum(r => r.Cost);
            Console.WriteLine($"Total service cost for today: ${totalCost}");
        }



    }

}
