using Server.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 1234);

server.Start();

while (true)
{
    var client = server.AcceptTcpClient();

    var stream = client.GetStream();
    var binaryWrite = new BinaryWriter(stream);
    var binaryRead = new BinaryReader(stream);

    while (true)
    {
        var strRead = binaryRead.ReadString();

        var command = JsonSerializer.Deserialize<Command>(strRead);

        if (command is null)
            return;


        switch (command.Text.ToLower())
        {
            case Command.HELP:
                string message = @"proclist ------ see all processes 
kill <process name> ------ end process
run <process name> ------ run process";
                binaryWrite.Write(message);
                binaryWrite.Flush();
                break;

            case Command.PROCLIST:
                var list = Process.GetProcesses();
                var names = list.Select(p => p.ProcessName);
                var jsonList = JsonSerializer.Serialize(names);
                binaryWrite.Write(jsonList);
                binaryWrite.Flush();
                break;

            case Command.RUN:
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
                catch (Exception) {
                    
                    binaryWrite.Write(false);  
                }
                break;


            case Command.KILL:
                if (command.Param is null)
                {
                    binaryWrite.Write(false);
                    break;
                }
                var processName = Process.GetProcessesByName(command.Param);
                if (processName.Length == 0)
                {
                    binaryWrite.Write(false);
                    break;
                }
                foreach (var item in processName)
                {

                    item.Kill();
                    binaryWrite.Write(true);
                }
                break;
            default:
                break;
        }
    }
}

 