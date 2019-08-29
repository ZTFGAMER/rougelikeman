namespace AWSSDK.Examples
{
    using Amazon;
    using Amazon.CognitoIdentity;
    using Amazon.Runtime;
    using Amazon.S3;
    using Amazon.S3.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    using UnityEngine.UI;

    public class S3Example : MonoBehaviour
    {
        public string IdentityPoolId = string.Empty;
        public string CognitoIdentityRegion = RegionEndpoint.USEast1.get_SystemName();
        public string S3Region = RegionEndpoint.USEast1.get_SystemName();
        public string S3BucketName;
        public string SampleFileName;
        public Button GetBucketListButton;
        public Button PostBucketButton;
        public Button GetObjectsListButton;
        public Button DeleteObjectButton;
        public Button GetObjectButton;
        public Text ResultText;
        private IAmazonS3 _s3Client;
        private AWSCredentials _credentials;

        public void DeleteObject()
        {
            this.ResultText.text = $"deleting {this.SampleFileName} from bucket {this.S3BucketName}";
            List<KeyVersion> list = new List<KeyVersion>();
            KeyVersion item = new KeyVersion();
            item.set_Key(this.SampleFileName);
            list.Add(item);
            DeleteObjectsRequest request2 = new DeleteObjectsRequest();
            request2.set_BucketName(this.S3BucketName);
            request2.set_Objects(list);
            DeleteObjectsRequest request = request2;
            this.Client.DeleteObjectsAsync(request, new AmazonServiceCallback<DeleteObjectsRequest, DeleteObjectsResponse>(this, this.<DeleteObject>m__9), null);
        }

        public void GetBucketList()
        {
            this.ResultText.text = "Fetching all the Buckets";
            this.Client.ListBucketsAsync(new ListBucketsRequest(), new AmazonServiceCallback<ListBucketsRequest, ListBucketsResponse>(this, this.<GetBucketList>m__5), null);
        }

        private string GetFileHelper()
        {
            string sampleFileName = this.SampleFileName;
            if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + sampleFileName))
            {
                StreamWriter writer = File.CreateText(Application.persistentDataPath + Path.DirectorySeparatorChar + sampleFileName);
                writer.WriteLine("This is a sample s3 file uploaded from unity s3 sample");
                writer.Close();
            }
            return sampleFileName;
        }

        private void GetObject()
        {
            this.ResultText.text = $"fetching {this.SampleFileName} from bucket {this.S3BucketName}";
            this.Client.GetObjectAsync(this.S3BucketName, this.SampleFileName, new AmazonServiceCallback<GetObjectRequest, GetObjectResponse>(this, this.<GetObject>m__6), null);
        }

        public void GetObjects()
        {
            this.ResultText.text = "Fetching all the Objects from " + this.S3BucketName;
            ListObjectsRequest request2 = new ListObjectsRequest();
            request2.set_BucketName(this.S3BucketName);
            ListObjectsRequest request = request2;
            this.Client.ListObjectsAsync(request, new AmazonServiceCallback<ListObjectsRequest, ListObjectsResponse>(this, this.<GetObjects>m__8), null);
        }

        private string GetPostPolicy(string bucketName, string key, string contentType)
        {
            bucketName = bucketName.Trim();
            key = key.Trim();
            if (!string.IsNullOrEmpty(key) && (key[0] == '/'))
            {
                throw new ArgumentException("uploadFileName cannot start with / ");
            }
            contentType = contentType.Trim();
            if (string.IsNullOrEmpty(bucketName))
            {
                throw new ArgumentException("bucketName cannot be null or empty. It's required to build post policy");
            }
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("uploadFileName cannot be null or empty. It's required to build post policy");
            }
            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("contentType cannot be null or empty. It's required to build post policy");
            }
            int length = key.LastIndexOf('/');
            if (length == -1)
            {
                string[] textArray1 = new string[] { "{\"expiration\": \"", DateTime.UtcNow.AddHours(24.0).ToString("yyyy-MM-ddTHH:mm:ssZ"), "\",\"conditions\": [{\"bucket\": \"", bucketName, "\"},[\"starts-with\", \"$key\", \"\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", \"", contentType, "\"]]}" };
                return string.Concat(textArray1);
            }
            string[] textArray2 = new string[] { "{\"expiration\": \"", DateTime.UtcNow.AddHours(24.0).ToString("yyyy-MM-ddTHH:mm:ssZ"), "\",\"conditions\": [{\"bucket\": \"", bucketName, "\"},[\"starts-with\", \"$key\", \"", key.Substring(0, length), "/\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", \"", contentType, "\"]]}" };
            return string.Concat(textArray2);
        }

        public void PostObject()
        {
            this.ResultText.text = "Retrieving the file";
            string fileHelper = this.GetFileHelper();
            FileStream stream = new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + fileHelper, FileMode.Open, FileAccess.Read, FileShare.Read);
            this.ResultText.text = this.ResultText.text + "\nCreating request object";
            PostObjectRequest request2 = new PostObjectRequest();
            request2.set_Bucket(this.S3BucketName);
            request2.set_Key(fileHelper);
            request2.set_InputStream(stream);
            request2.set_CannedACL(S3CannedACL.Private);
            PostObjectRequest request = request2;
            this.ResultText.text = this.ResultText.text + "\nMaking HTTP post call";
            this.Client.PostObjectAsync(request, new AmazonServiceCallback<PostObjectRequest, PostObjectResponse>(this, this.<PostObject>m__7), null);
        }

        private void Start()
        {
            UnityInitializer.AttachToGameObject(base.gameObject);
            this.GetBucketListButton.onClick.AddListener(() => this.GetBucketList());
            this.PostBucketButton.onClick.AddListener(() => this.PostObject());
            this.GetObjectsListButton.onClick.AddListener(() => this.GetObjects());
            this.DeleteObjectButton.onClick.AddListener(() => this.DeleteObject());
            this.GetObjectButton.onClick.AddListener(() => this.GetObject());
        }

        private RegionEndpoint _CognitoIdentityRegion =>
            RegionEndpoint.GetBySystemName(this.CognitoIdentityRegion);

        private RegionEndpoint _S3Region =>
            RegionEndpoint.GetBySystemName(this.S3Region);

        private AWSCredentials Credentials
        {
            get
            {
                if (this._credentials == null)
                {
                    this._credentials = new CognitoAWSCredentials(this.IdentityPoolId, this._CognitoIdentityRegion);
                }
                return this._credentials;
            }
        }

        private IAmazonS3 Client
        {
            get
            {
                if (this._s3Client == null)
                {
                    this._s3Client = new AmazonS3Client(this.Credentials, this._S3Region);
                }
                return this._s3Client;
            }
        }
    }
}

