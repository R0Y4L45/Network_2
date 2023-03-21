using Client.Models;
using System.Net.Sockets;
using System.Text.Json;

Command? command = null;
string? commandText, param, jsonStr, response;

var client = new TcpClient("127.0.0.1", 1234);

var stream = client.GetStream();

var bw = new BinaryWriter(stream);
var br = new BinaryReader(stream);


while (true)
{
    Console.WriteLine("\nEnter Command : ");
    commandText = Console.ReadLine();

    if (string.IsNullOrEmpty(commandText))
    {
        Console.WriteLine("You entered command is false...");
        await Task.Delay(100);
        continue;
    }

    command = new Command
    {
        Text = commandText
    };

    switch (command.Text.ToLower())
    {
        case Command.HELP:
            {
                jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                response = br.ReadString();
                Console.WriteLine('\n' + response);
                break;
            }
        case Command.PROCLIST:
            {

                jsonStr = JsonSerializer.Serialize(command);
                bw.Write(jsonStr);
                await Task.Delay(50);
                response = br.ReadString();
                Console.WriteLine("Processes : \n" + response);

                break;
            }
        case Command.RUN:
            {
                Console.WriteLine("Enter process name : ");
                param = Console.ReadLine();
                if (string.IsNullOrEmpty(param))
                {
                    Console.WriteLine("You entered process name is false...");
                    await Task.Delay(100);

                    break;
                }
                else
                {
                    command.Param = param;
                    jsonStr = JsonSerializer.Serialize(command);
                    bw.Write(jsonStr);
                    await Task.Delay(50);

                    var responseBool = br.ReadBoolean();
                    if (responseBool is true)
                        Console.WriteLine("Process succesfully started");
                    else
                        Console.WriteLine("Process couldn't started");

                    break;
                }
            }
        case Command.KILL:
            {
                Console.WriteLine("Enter process name : ");
                param = Console.ReadLine();
                if (string.IsNullOrEmpty(param))
                {
                    Console.WriteLine("You entered process name is false...");
                    await Task.Delay(100);

                    break;
                }
                else
                {
                    command.Param = param;
                    jsonStr = JsonSerializer.Serialize(command);
                    bw.Write(jsonStr);
                    await Task.Delay(50);

                    var responseBool = br.ReadBoolean();
                    if (responseBool is true)
                        Console.WriteLine("Process succesfully ended");
                    else
                        Console.WriteLine("Process couldn't ended");

                    break;
                }
            }
        default:
            Console.WriteLine("\nYou entered command is false...\n");
            break;
    }

}
