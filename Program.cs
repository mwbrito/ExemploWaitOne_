using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

internal class Program
{
    static AutoResetEvent validador = new AutoResetEvent(false);
    static ConcurrentQueue<int> fila = new ConcurrentQueue<int>();

    private static void Main(string[] args)
    {
        Console.WriteLine("Demo WaitOne");
        Console.WriteLine("");
        Console.WriteLine("----------------------------------");
        Console.WriteLine("");

        //inicia thread validador
        new Thread( Validador).Start();

        //inicia threads que carregam a fila
        for (int i = 0; i < 10; i++)
        {
            new Thread(() => Enfileirador(i)).Start();  
            Thread.Sleep(10);
        }

    }

    //Enfileirador 
    static void Enfileirador(int name)
    {
        Console.WriteLine($"Thread {name} aberta");
        int contador = 0;

        //while (true)
        //{   
            fila.Enqueue(name);
            validador.Set();
            Console.WriteLine($"Thread {name+contador} chama Validador");
            contador += 10;

        //    Thread.Sleep(10000);
        //}
    }

    static void Validador()
    {
        Console.WriteLine("------------Thread Validador Iniciada");

        while (true)
        {
            validador.WaitOne();

            Console.WriteLine("------------Validador comecando");

            Thread.Sleep(1000);

            while (fila.Count > 0)
            {
                int itemFila;
                fila.TryDequeue(out itemFila);

                if (itemFila != null)
                    Console.WriteLine($"------------Limpando item fila {itemFila}");
            }

            Console.WriteLine("------------Validador finalizando");
        }
    }
}