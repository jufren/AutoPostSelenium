using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
namespace WinFormsApp1
{
    public class GoogleUploadVideo
    {
        SimpleLogger logger = new SimpleLogger();
        public async void TestUploadVideo()
        {
            Console.WriteLine("YouTube Data API: Upload Video");
            Console.WriteLine("==============================");

            try
            {
                //UploadVideo().Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public async Task<bool> UploadVideo(string filePath,string title, string description, string[] tags)
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    new[] { YouTubeService.Scope.YoutubeUpload },"user",
                    CancellationToken.None

                );
            }
           
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name               
            });
           
            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = title;
            video.Snippet.Description = description;
            //video.Snippet.Tags = tags;
            video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            video.Status = new VideoStatus();
         
            video.Status.PrivacyStatus = "public"; // or "private" or "public"
            //var filePath = @"REPLACE_ME.mp4"; // Replace with path to actual movie file.

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                //videosInsertRequest.OnBehalfOfContentOwnerChannel
                //videosInsertRequest.OnBehalfOfContentOwner
                videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                await videosInsertRequest.UploadAsync();
                return true;
            }
            return false;
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    
                    //Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    logger.Error("An error prevented the upload from completing.\n"+ progress.Exception);
                    //Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        void videosInsertRequest_ResponseReceived(Video video)
        {
            logger.Error("Video id was successfully uploaded:"+ video.Id);
            //Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
        }
    }
}
