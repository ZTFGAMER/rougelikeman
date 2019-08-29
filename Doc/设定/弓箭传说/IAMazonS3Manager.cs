using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IAMazonS3Manager : MonoBehaviour
{
    public const string Folder_Excel = "data/excel";
    public const string Folder_TiledMap = "data/tiledmap";
    public const string Folder_Config = "data/config";
    public const string Config_Game = "game_config.json";
    public const string Bucket_ExcelData = "archer-data";
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static IAMazonS3Manager <Instance>k__BackingField;
    private IAmazonS3 mClient;

    private void Awake()
    {
        UnityInitializer.AttachToGameObject(base.gameObject);
        Instance = this;
    }

    public void ClearFileName(string filename)
    {
        PlayerPrefs.SetString(this.GetTagName(filename), string.Empty);
    }

    private void DownloadObject(string bucketname, string filename, string tag)
    {
        <DownloadObject>c__AnonStorey2 storey = new <DownloadObject>c__AnonStorey2 {
            filename = filename,
            tag = tag,
            $this = this
        };
        if (this.GetLocalTag(storey.filename).CompareTo(storey.tag) != 0)
        {
            GetObjectRequest request2 = new GetObjectRequest();
            request2.set_BucketName(bucketname);
            request2.set_Key(storey.filename);
            request2.set_EtagToNotMatch(this.GetLocalTag(storey.filename));
            GetObjectRequest request = request2;
            this.mClient.GetObjectAsync(request, new AmazonServiceCallback<GetObjectRequest, GetObjectResponse>(storey, this.<>m__0), null);
        }
    }

    private string GetLocalTag(string filename) => 
        PlayerPrefs.GetString(this.GetTagName(filename));

    private string GetTagName(string filename) => 
        (filename + "1");

    private void Init()
    {
        this.mClient = new AmazonS3Client(new BasicAWSCredentials("AKIAJ2NYXN7OON5NRXEQ", "YuCsrrlAUNhKEKehqCjeIW9dxbISt45o9HXQEQJQ"), RegionEndpoint.GetBySystemName(RegionEndpoint.USEast2.get_SystemName()));
        ListObjectsRequest request2 = new ListObjectsRequest();
        request2.set_BucketName("archer-data");
        ListObjectsRequest request = request2;
        this.mClient.ListObjectsAsync(request, new AmazonServiceCallback<ListObjectsRequest, ListObjectsResponse>(this, this.<Init>m__0), null);
    }

    private void SetLocalTag(string filename, string tag)
    {
        PlayerPrefs.SetString(this.GetTagName(filename), tag);
    }

    [DebuggerHidden]
    private IEnumerator Start() => 
        new <Start>c__Iterator0 { $this = this };

    public static IAMazonS3Manager Instance
    {
        [CompilerGenerated]
        get => 
            <Instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<Instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <DownloadObject>c__AnonStorey2
    {
        internal string filename;
        internal string tag;
        internal IAMazonS3Manager $this;

        internal void <>m__0(AmazonServiceResult<GetObjectRequest, GetObjectResponse> responseObj)
        {
            if (responseObj != null)
            {
                byte[] info = null;
                GetObjectResponse response = responseObj.get_Response();
                if (response.get_ResponseStream() != null)
                {
                    using (BinaryReader reader = new BinaryReader(response.get_ResponseStream()))
                    {
                        info = reader.ReadBytes((int) response.get_ResponseStream().Length);
                    }
                    this.$this.SetLocalTag(this.filename, this.tag);
                    FileUtils.CreateFileOverride(this.filename, info);
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey1
    {
        internal string str;
        internal IAMazonS3Manager $this;

        internal void <>m__0(S3Object o)
        {
            this.str = this.str + $"{o.get_Key()}
";
            if ((o.get_Key().Length > 0) && (o.get_Key().Substring(o.get_Key().Length - 1, 1) != "/"))
            {
                this.$this.DownloadObject("archer-data", o.get_Key(), o.get_ETag());
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal IAMazonS3Manager $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForSeconds(60f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.Init();
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

