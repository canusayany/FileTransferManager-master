using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOExtensions;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            FileExportHelper.TransfetStatus = WP;
            List<TransferFileInfo> transferFileInfos = new List<TransferFileInfo>();
            TransferFileInfo transferFileInfo;
            int id = 20;
            while (id<40)
            {
                 transferFileInfo = new TransferFileInfo { Source = @"E:\Data\File", Destination = @"d:\Data\File41\aaa\" + id, ID = (id++).ToString() };
                transferFileInfos.Add(transferFileInfo);
            }
            FileExportHelper.AddTransferTaskAndStart(transferFileInfos);
            Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ "+id);
            Console.ReadKey();
            FileExportHelper.StopTransferFile();
            Console.ReadKey();
            transferFileInfos.Clear();
            while (id < 50)
            {
                transferFileInfo = new TransferFileInfo { Source = @"E:\Data\File", Destination = @"d:\Data1\File41\aaa\" + id, ID = (id++).ToString() };
                transferFileInfos.Add(transferFileInfo);
            }
            FileExportHelper.AddTransferTaskAndStart(transferFileInfos);
            Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ " + id);

            Console.ReadKey();
            //Action<IOExtensions.TransferProgress> progress = WritePress;
            //Action<IOExtensions.TransferProgress> progress2 = WritePress2;
            //var tt1 = IOExtensions.FileTransferManager.CopyWithProgressAsync(@"E:\Data\File", @"e:\Data\File41\aaa", progress, true, true,"12");
            //var tt2 = IOExtensions.FileTransferManager.CopyWithProgressAsync(@"E:\Data\File", @"e:\Data\File42\aaa", progress2, true, false,"34ff");
            //IOExtensions.TransferResult transferResult=    tt1.Result;
            //Console.WriteLine("transferResult=" + transferResult.ToString());
            //Console.ReadKey();
        }
        static void WP(string s1, string s2, Enum_TransfetStatus transfetStatus)
        { 
            Console.WriteLine($"id={s1},path={s2},状态={transfetStatus}");

        }
        static void WritePress(IOExtensions.TransferProgress transferProgress)
        {
            Console.WriteLine("1           " + transferProgress.ToString());
        }
        static void WritePress2(IOExtensions.TransferProgress transferProgress)
        {
            Console.WriteLine("2           " + transferProgress.ToString());
        }
    }
}
