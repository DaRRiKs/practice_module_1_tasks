using System;
using System.Collections.Generic;

namespace VehiclesApp
{
    abstract class Vehicle
    {
        public string Brand { get; }
        public string Model { get; }
        public int Year { get; }
        public bool EngineOn { get; private set; }
        protected Vehicle(string brand, string model, int year)
        {
            Brand = brand;
            Model = model;
            Year = year;
        }
        public virtual void StartEngine()
        {
            if (!EngineOn)
            {
                EngineOn = true;
                Console.WriteLine($"Двигатель {this} запущен.");
            }
        }
        public virtual void StopEngine()
        {
            if (EngineOn)
            {
                EngineOn = false;
                Console.WriteLine($"Двигатель {this} остановлен.");
            }
        }
        public override string ToString() => $"{Brand} {Model} ({Year})";
    }
    class Car : Vehicle
    {
        public int Doors { get; }
        public string Transmission { get; }
        public Car(string brand, string model, int year, int doors, string transmission)
            : base(brand, model, year)
        {
            Doors = doors;
            Transmission = transmission;
        }
        public override string ToString() => base.ToString() + $" | Car: {Doors} двери, {Transmission}";
    }
    class Motorcycle : Vehicle
    {
        public string BodyType { get; }
        public bool HasTopBox { get; }
        public Motorcycle(string brand, string model, int year, string bodyType, bool hasTopBox)
            : base(brand, model, year)
        {
            BodyType = bodyType;
            HasTopBox = hasTopBox;
        }
        public override string ToString() => base.ToString() + $" | Moto: {BodyType}, бокс: {(HasTopBox ? "есть" : "нет")}";
    }
    class Garage
    {
        public string Name { get; }
        private readonly List<Vehicle> _vehicles = new();
        public Garage(string name) => Name = name;
        public void AddVehicle(Vehicle v)
        {
            if (v == null) return;
            _vehicles.Add(v);
            Console.WriteLine($"[{Name}] Добавлено: {v}");
        }
        public bool RemoveVehicle(Vehicle v)
        {
            var ok = _vehicles.Remove(v);
            if (ok) Console.WriteLine($"[{Name}] Удалено: {v}");
            return ok;
        }
        public IReadOnlyList<Vehicle> GetVehicles() => _vehicles.AsReadOnly();
        public override string ToString() => $"Гараж \"{Name}\" (ТС: {_vehicles.Count})";
    }
    class Fleet
    {
        private readonly List<Garage> _garages = new();
        public void AddGarage(Garage g)
        {
            if (g == null) return;
            _garages.Add(g);
            Console.WriteLine($"[Fleet] Добавлен {g}");
        }
        public bool RemoveGarage(Garage g)
        {
            var ok = _garages.Remove(g);
            if (ok) Console.WriteLine($"[Fleet] Удалён {g}");
            return ok;
        }
        public List<Vehicle> FindByBrand(string brand)
        {
            var result = new List<Vehicle>();
            foreach (var garage in _garages)
            {
                foreach (var v in garage.GetVehicles())
                {
                    if (string.Equals(v.Brand, brand, StringComparison.OrdinalIgnoreCase))
                        result.Add(v);
                }
            }
            return result;
        }
        public IReadOnlyList<Garage> GetGarages() => _garages.AsReadOnly();
    }
    class Program
    {
        static void Main()
        {
            var car1 = new Car("Toyota", "Corolla", 2018, 4, "AT");
            var car2 = new Car("Hyundai", "Sonata", 2020, 4, "AT");
            var moto1 = new Motorcycle("Yamaha", "MT-07", 2021, "Roadster", true);
            var moto2 = new Motorcycle("Honda", "CBR500R", 2019, "Sport", false);
            var g1 = new Garage("Центральный");
            var g2 = new Garage("Северный");
            g1.AddVehicle(car1);
            g1.AddVehicle(moto1);
            g2.AddVehicle(car2);
            g2.AddVehicle(moto2);
            var fleet = new Fleet();
            fleet.AddGarage(g1);
            fleet.AddGarage(g2);
            Console.WriteLine();
            Console.WriteLine("Содержимое автопарка:");
            foreach (var g in fleet.GetGarages())
            {
                Console.WriteLine(g);
                foreach (var v in g.GetVehicles())
                {
                    Console.WriteLine("  - " + v);
                }
            }
            Console.WriteLine();
            Console.WriteLine("Демонстрация запуска/остановки двигателя:");
            car1.StartEngine();
            car1.StopEngine();
            moto1.StartEngine();
            moto1.StopEngine();
            Console.WriteLine();
            Console.WriteLine("Поиск по марке:");
            var found = fleet.FindByBrand("Toyota");
            foreach (var v in found)
                Console.WriteLine("Найдено: " + v);
            Console.WriteLine();
            Console.WriteLine("Удаление ТС и гаража:");
            g2.RemoveVehicle(moto2);
            fleet.RemoveGarage(g2);
            Console.WriteLine();
            Console.WriteLine("Итоговое состояние автопарка:");
            foreach (var g in fleet.GetGarages())
            {
                Console.WriteLine(g);
                foreach (var v in g.GetVehicles())
                    Console.WriteLine("  - " + v);
            }
            Console.WriteLine("\nГотово. Нажмите Enter для выхода...");
            Console.ReadLine();
        }
    }
}