using Client.Models;
using System.Net.Sockets;
using System.Text.Json;

var client = new TcpClient("127.0.0.1", 1234);

var stream = client.GetStream();

var bw = new BinaryWriter(stream);
var br = new BinaryReader(stream);

var CommandText = string.Empty;
var CommandParam = string.Empty;

while (true)
{
    Console.WriteLine("Enter Command Text");
    CommandText = Console.ReadLine();
    Console.WriteLine("Enter Command Param");
    CommandParam = Console.ReadLine();
    if (string.IsNullOrEmpty(CommandText) || string.IsNullOrEmpty(CommandParam))
    {
        Console.WriteLine("You entered an error, please try again");
        await Task.Delay(100);
        continue;
    }

    var command = new Command
    {
        Text = CommandText,
        Param = CommandParam
    };

    switch (command.Text.ToLower())
    {
        case Command.HELP:
            {
                var jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                var response = br.ReadString();
                Console.WriteLine(response);
                break;
            }
        case Command.PROCLIST:
            {
                var jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                var response = br.ReadString();
                Console.WriteLine(response);
                break;
            }
        case Command.RUN:
            {
                var jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);

                var responseBool = br.ReadBoolean();
                if (responseBool is true)
                    Console.WriteLine("Process succesfully ended");
                else
                    Console.WriteLine("Process couldn't ended");
                break;
            }
        case Command.KILL:
            {
                var jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);

                var responseBool = br.ReadBoolean();
                if (responseBool is true)
                    Console.WriteLine("Process succesfully ended");
                else
                    Console.WriteLine("Process couldn't ended");
                break;
            }
        default:
            break;
    }

}
