using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class HttpServer
    {
        public int Port = 8080;
        SimpleLogger logger = new SimpleLogger();
        private HttpListener _listener;
        public string path = "";
        public HttpListenerResponse response;
        public HttpServer(SimpleLogger logger)
        {
            this.logger = logger;
        }
        public void Start()
        {
           
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + Port.ToString() + "/");
            _listener.Start();
            Receive();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Receive()
        {
            
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private async void ListenerCallback(IAsyncResult result)
        {
            if (_listener.IsListening)
            {
                var context = _listener.EndGetContext(result);
                var request = context.Request;

                // do something with the request
                Console.WriteLine($"{request.Url}");
                response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "audio/wav";
                NameValueCollection queryStringCollection = request.QueryString;
                // Get the value of the variable named as "id"
                // id will be null when client does not send 
                // any query string variable named as "id"
                string chineseCharacter = queryStringCollection["cn"];

                path = @"c:\\temp\\" + Guid.NewGuid() + ".wav";
                try
                {
                    using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                    {


                        synthesizer.SetOutputToWaveFile(path);
                        synthesizer.SelectVoice("Microsoft Huihui Desktop");
                        synthesizer.Rate = -5;
                        synthesizer.SpeakCompleted += Synthesizer_SpeakCompleted;
                        // Set the output device (optional)
                        //synthesizer.SetOutputToDefaultAudioDevice();
                        Prompt p = new Prompt(chineseCharacter);

                        synthesizer.Speak(p);                        

                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.StackTrace);
                    // Handle exceptions appropriately (e.g., return error response)
                    //return StatusCode(500, ex.Message);
                }
                //response.OutputStream.Write(new byte[] { }, 0, 0);
                //response.OutputStream.Close();
                Receive();
            }
        }

        private void Synthesizer_SpeakCompleted(object? sender, SpeakCompletedEventArgs e)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);


            foreach (byte audioByte in fileBytes)
            {
                response.OutputStream.WriteByte(audioByte);
            }


            response.OutputStream.Close();
           
        }
    }
}
