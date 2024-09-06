using ConditionalAPIClient;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Text.Json;
using System.Xml.Linq;
public class Program
{
    private static void Main(string[] args)
    {
        var appSettings = ConfigurationManager.AppSettings;
        int id = 1;
        foreach (var key in appSettings.AllKeys)
        {
            if (key.Contains("Endpoint"))
            {
                EndpintContainer.Endpints.Add(new Endpint { Id = id++, Endpoint = appSettings[key] });
            }
        }
        if (EndpintContainer.Endpints.Count.Equals(0))
        {
            Console.WriteLine("No hay endpoints configurados");
        }
        else
        {
            Console.WriteLine("Lista de endpoint disponibles:");

            for (int i = 0; i < EndpintContainer.Endpints.Count; i++)
            {
                Console.WriteLine("ID: " + EndpintContainer.Endpints[i].Id + "   ---->   Endpint: " + EndpintContainer.Endpints[i].Endpoint);
            }
            Console.WriteLine("\n");

            bool incorrectID = true;
            while (incorrectID)
            {
                Console.WriteLine("Por favor, introduce el id del endpoint que quiere utilizar:");

                string input = Console.ReadLine();
                if (int.TryParse(input, out int number))
                {
                    bool matchFound = false;
                    foreach (var item in EndpintContainer.Endpints)
                    {
                        if (number == int.Parse(item.Id.ToString()))
                        {
                            Console.WriteLine($"¡El número {number} coincide con el valor {item.Id} en la configuración!");


                            matchFound = true;
                            incorrectID = false;
                            break;
                        }
                    }

                    if (!matchFound)
                    {
                        Console.WriteLine($"El número {number} no coincide con ninguno de los valores en la configuración.");
                    }
                }
                else
                {
                    Console.WriteLine("La entrada no es un número válido.");
                }
            }
            //Console.WriteLine(JsonSerializer.Serialize(APIContainer.APIs));
            Console.WriteLine("test");
        }
    }
}