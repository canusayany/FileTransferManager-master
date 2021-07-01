using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOExtensions
{
    #region Enums

    /// <summary>
    /// 传输通知
    /// </summary>
    public enum Enum_TransfetStatus
    {
        /// <summary>
        /// Defines the Start.
        /// </summary>
        Start,

        /// <summary>
        /// Defines the Success.
        /// </summary>
        Success,

        /// <summary>
        /// Defines the Fail.
        /// </summary>
        Fail,

        /// <summary>
        /// Defines the Cancelled.
        /// </summary>
        Cancelled
    }

    #endregion

    /// <summary>
    /// Defines the <see cref="FileExportHelper" />.
    /// </summary>
    public class FileExportHelper
    {
        #region Fields

        /// <summary>
        /// 订阅后获取当前传输的文件的传输情况.
        /// 第一个参数为ID.
        /// 第二个参数为文件地址.
        /// 第三个参数为传输状态.
        /// </summary>
        public static Action<string, string, Enum_TransfetStatus> TransfetStatus;

        /// <summary>
        /// Defines the Progress.
        /// </summary>
        internal static Action<IOExtensions.TransferProgress> Progress = ProgressConvert;

        /// <summary>
        /// 当前正在进行的任务的线程.
        /// </summary>
        internal static Task<TransferResult> PresentTransTask;

        /// <summary>
        /// Defines the source.
        /// </summary>
        internal static CancellationTokenSource source = new CancellationTokenSource();

        /// <summary>
        /// Defines the token.
        /// </summary>
        internal static CancellationToken token = source.Token;

        /// <summary>
        /// Defines the SafeQueues.
        /// </summary>
        private static SafeQueue<TransferFileInfo> SafeQueues = new SafeQueue<TransferFileInfo>();

        /// <summary>
        /// Defines the isDownLoading.
        /// </summary>
        private static bool isDownLoading;

        #endregion

        #region Methods

        /// <summary>
        /// 添加传输任务并且开始传输
        /// </summary>
        /// <param name="transferFileInfos">The transferFileInfos<see cref="List{TransferFileInfo}"/>.</param>
        public static void AddTransferTaskAndStart(List<TransferFileInfo> transferFileInfos)
        {
            AddTasks(transferFileInfos);
            Task.Run(() =>
            {
                StartTransferFile();
            });
        }

        /// <summary>
        /// 添加传输任务并且开始传输
        /// </summary>
        /// <param name="transferFileInfo">The transferFileInfo<see cref="TransferFileInfo"/>.</param>
        public static void AddTransferTaskAndStart(TransferFileInfo transferFileInfo)
        {
            SafeQueues.Enqueue(transferFileInfo);
            Task.Run(() =>
            {
                StartTransferFile();
            });
        }

        /// <summary>
        /// 停止当前以及之后的传输任务.
        /// </summary>
        public static void StopTransferFile()
        {
            source.Cancel();
            // PresentTransTask.Dispose();
            SafeQueues.Clear();
        }

        /// <summary>
        /// 添加任务.
        /// </summary>
        /// <param name="transferFileInfos">The transferFileInfos<see cref="List{TransferFileInfo}"/>.</param>
        private static void AddTasks(List<TransferFileInfo> transferFileInfos)
        {
            foreach (var item in transferFileInfos)
            {
                SafeQueues.Enqueue(item);
            }
        }

        /// <summary>
        /// The ProgressConvert.
        /// </summary>
        /// <param name="transferProgress">The transferProgress<see cref="TransferProgress"/>.</param>
        private static void ProgressConvert(TransferProgress transferProgress)
        {
            if (transferProgress.Percentage == 0)
            {
                TransfetStatus?.BeginInvoke(transferProgress.ID, transferProgress.ProcessedFile, Enum_TransfetStatus.Start, null, null);
            }
            //if (transferProgress.Percentage == 100)
            //{
            //    TransfetStatus?.BeginInvoke(transferProgress.ID, transferProgress.ProcessedFile, Enum_TransfetStatus.Success, null, null);
            //}
            // Console.WriteLine(transferProgress.Percentage);
        }

        /// <summary>
        /// The StartTransferFile.
        /// </summary>
        private static void StartTransferFile()
        {
            if (isDownLoading)
            {
                return;
            }
            isDownLoading = true;
            while (true)
            {
                if (SafeQueues.TryDequeue(out TransferFileInfo transferFileInfo))
                {
                    PresentTransTask = IOExtensions.FileTransferManager.CopyWithProgressAsync(transferFileInfo.Source, transferFileInfo.Destination, Progress, true, token, true, transferFileInfo.ID);
                    PresentTransTask.Wait();
                    TransferResult transferResult = PresentTransTask.Result;
                    if (transferResult == TransferResult.Failed)
                    {
                        TransfetStatus?.BeginInvoke(transferFileInfo.ID, transferFileInfo.Source, Enum_TransfetStatus.Fail, null, null);
                    }
                    if (transferResult == TransferResult.Success)
                    {
                        TransfetStatus?.BeginInvoke(transferFileInfo.ID, transferFileInfo.Source, Enum_TransfetStatus.Success, null, null);
                    }
                    if (transferResult == TransferResult.Cancelled)
                    {
                        TransfetStatus?.BeginInvoke(transferFileInfo.ID, transferFileInfo.Source, Enum_TransfetStatus.Cancelled, null, null);
                    }

                }
                else
                {
                    break;
                }

            }
            isDownLoading = false;
        }

        #endregion
    }

    /// <summary>
    /// 传输任务需要的信息
    /// </summary>
    public class TransferFileInfo
    {
        #region Fields

        /// <summary>
        /// 源地址.
        /// 如 @"E:\Data\File"
        /// </summary>
        public string Source;

        /// <summary>
        /// 目标地址.
        /// 如 @"d:\Data\File41\aaa\"
        /// </summary>
        public string Destination;

        /// <summary>
        /// 传输ID,会在传输进度时返回.最好每个任务的ID不同.
        /// </summary>
        public string ID;

        #endregion
    }
}
