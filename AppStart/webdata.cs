﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZooSoft
{
    public delegate void BytesDownloadedEventHandler(ByteArgs e);

    public class ByteArgs : EventArgs
    {
        private int _downloaded;
        private int _total;

        public int downloaded
        {
            get
            {
                return _downloaded;
            }
            set
            {
                _downloaded = value;
            }
        }

        public int total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value;
            }
        }
    }

    // [URL] 에 위치한 [file] 을 받아서 [targetFolder] 에 내려받는다
    class webdata
    {
        public static event BytesDownloadedEventHandler bytesDownloaded;

        public static bool downloadFormWeb(string URL, string file, string targetFolder)
        {
            try
            {
                byte[] downloadedData;

                downloadedData = new byte[0];

                WebRequest webReq = WebRequest.Create(URL + file);
                WebResponse webResponse = webReq.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();

                byte[] dataBuffer = new byte[1024];

                int dataLength = (int)webResponse.ContentLength;

                ByteArgs byteArgs = new ByteArgs();

                byteArgs.downloaded = 0;
                byteArgs.total = dataLength;

                if (bytesDownloaded != null) bytesDownloaded(byteArgs);

                MemoryStream memoryStream = new MemoryStream();
                while(true)
                {
                    int bytesFromStream = dataStream.Read(dataBuffer, 0, dataBuffer.Length);

                    if (bytesFromStream == 0)
                    {
                        byteArgs.downloaded = dataLength;
                        byteArgs.total = dataLength;
                        if (bytesDownloaded != null) bytesDownloaded(byteArgs);

                        break;
                    }
                    else
                    {
                        memoryStream.Write(dataBuffer, 0, bytesFromStream);

                        byteArgs.downloaded = bytesFromStream;
                        byteArgs.total = dataLength;
                        if (bytesDownloaded != null) bytesDownloaded(byteArgs);
                    }
                }

                downloadedData = memoryStream.ToArray();

                dataStream.Close();
                memoryStream.Close();

                FileStream newFile = new FileStream(targetFolder + file, FileMode.Create);
                newFile.Write(downloadedData, 0, downloadedData.Length);
                newFile.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
