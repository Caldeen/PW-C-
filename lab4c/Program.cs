using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace lab4c
{
    public class Program
    {
        public class Storage
        {
            private int _itemQuantity;
            public Storage(int qualInit)
            {
                _itemQuantity = qualInit;
            }
            public void DecrQuant(int value)
            {
                
                   _itemQuantity -= value;
                if (_itemQuantity < 0)
                    _itemQuantity = 0;
                if (_itemQuantity > 1000)
                    _itemQuantity = 1000;
            }
            public bool IsFull()
            {
                if (_itemQuantity == 1000)
                    return true;
                return false;
            }
            public int GetQuant()
            {
                return _itemQuantity; 
            }

        }
        public class Producent
        {
            private Storage _storage;
            private Random _rand = new Random();
            
            public Producent(Storage storage)
            {
                _storage = storage;
            }

            public void Run()
            {
                while (true)
                {
                    lock (_storage)
                    {
                        while (_storage.IsFull())
                            Monitor.Wait(_storage);
                        Console.WriteLine("Producing");
                        _storage.DecrQuant(-10);
                        Console.WriteLine("QuantIn= " + _storage.GetQuant());


                        Monitor.PulseAll(_storage);
                    }
                }
            }
        }

        public class Consumer
        {
            private Storage _storage;
            private Random _rand = new Random();

            public Consumer(Storage storage)
            {
                _storage = storage;
            }

            public void Run()
            {
                while (true)
                {
                    int val = _rand.Next(9) + 1;
                    lock (_storage)
                    {
                        while (_storage.GetQuant() == 0)
                            Monitor.Wait(_storage);
                        Console.WriteLine("Consuming");
                        _storage.DecrQuant(val);
                        Console.WriteLine("QuantIn= " + _storage.GetQuant());
                        Monitor.PulseAll(_storage);
                    }
                }
            }
        }

        public static void Main(string[] args)
        {
        Storage mainStorage=new Storage(1000);
            Producent prod = new Producent(mainStorage);
            Consumer con1 = new Consumer(mainStorage);
            Consumer con2 = new Consumer(mainStorage);
            Consumer con3 = new Consumer(mainStorage);
            Console.Write("  ");
            var thr1 = new Thread(new ThreadStart(prod.Run));
            thr1.Start();

            var thr2 = new Thread(new ThreadStart(con1.Run));
            thr2.Start();
            var thr3 = new Thread(new ThreadStart(con2.Run));
            thr3.Start();
            var thr4 = new Thread(new ThreadStart(con2.Run));
            thr4.Start();
        }
    }
}
