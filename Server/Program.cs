using Server.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

string? strRead, jsonList, message;
Process[] list, processName;
IEnumerable<string>? procNames;
Command? command;

TcpListener? server = new TcpListener(IPAddress.Parse("127.0.0.1"), 1234);
server.Start();

TcpClient client = server.AcceptTcpClient();
NetworkStream stream = client.GetStream();
BinaryWriter binaryWrite = new BinaryWriter(stream);
BinaryReader binaryRead = new BinaryReader(stream);

while (true)
{
    strRead = binaryRead.ReadString();

    command = JsonSerializer.Deserialize<Command>(strRead);

    if (command is null)
        return;

    switch (command.Text!.ToLower())
    {
        case "help":
            message = @"Command : proclist ======> see all processes 
Command : kill    | Parametr : <<process name>> ======> end process
Command : run     | Parametr : <<process name>> ======> run process";
            binaryWrite.Write(message);
            binaryWrite.Flush();
            break;

        case "proclist":
            list = Process.GetProcesses();
            procNames = list.Select(p => p.ProcessName);
            jsonList = JsonSerializer.Serialize(procNames);
            binaryWrite.Write(jsonList);
            binaryWrite.Flush();
            break;

        case "run":
            if (command.Param is null)
            {
                binaryWrite.Write(false);
                break;
            }
            try
            {
                Process.Start(command.Param);
                binaryWrite.Write(true); ;
            }
            catch (Exception)
            {
                binaryWrite.Write(false);
            }
            break;

        case "kill":
            processName = Process.GetProcessesByName(command.Param);
            if (command.Param is null && processName.Length == 0)
            {
                binaryWrite.Write(false);
                break;
            }

            foreach (var item in processName)
                item.Kill();

            binaryWrite.Write(true);
            break;

        default:
            break;
    }
}


