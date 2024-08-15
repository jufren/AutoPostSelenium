using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using Tweetinvi;
using Tweetinvi.Core.Web;
using WinFormsApp1.Models;
using System.Speech;
using System.Speech.Synthesis;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3;
using OpenAI.GPT3.ObjectModels.RequestModels;
using NAudio.Wave;
using WMPLib;
using static Humanizer.In;
using System.Text.RegularExpressions;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        SimpleLogger logger = new SimpleLogger();
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {//await PostTwitter("16/6/2023:", "Hello");
            timer_Picture.Interval = Convert.ToInt32(textBox1.Text) * 60 * 1000;
            timer_Picture.Start();
            button1.Enabled = false;
            button2.Enabled = true;
        }
        private async Task PerformTimerTaskVideo()
        {

            //await PostFBGroup(DateTime.Now.ToLongTimeString(), content, filename);
        }
        private async Task PerformTimerTask()
        {
            Quotes q = ReadPosts();
            string content = q.Quote + "\n\n" + q.Author;
            string filename = GenerateImage(content);
            string fileFullPath = AppDomain.CurrentDomain.BaseDirectory + "Output\\" + filename;
            await PostChinese();
            await PostFB("dailymotivationalverse@gmail.com", "encryptedpassword", "https://www.facebook.com/profile.php?id=100092533336626", DateTime.Now.ToLongTimeString(), content, filename);
            await PostTwitter(DateTime.Now.ToLongTimeString(), content + "\n Follow us", filename);
            //await PostInstagram("dailymotivationalverse@gmail.com", "encryptedpassword", q, filename);

            //await PostFBGroup(DateTime.Now.ToLongTimeString(), content, filename);
        }
        public ChineseVocabulary ReadChinese(ref int no)
        {
            QuotesContext ctx = new QuotesContext();

            int newno = no + 1;
            int ctr = ctx.ChineseVocabulary.Count();
            if (newno >= ctr)
                newno = 1;
            ChineseVocabulary q = ctx.ChineseVocabulary.Where(x => x.id == newno).First();
            no = newno;
            return q;
        }
        public Quotes ReadPosts()
        {
            QuotesContext ctx = new QuotesContext();
            Random r = new Random();
            int ctr = ctx.Quotes.Count();
            Quotes q = ctx.Quotes.Skip(r.Next(ctr)).First();
            return q;
        }
        public void SavePosts(Quotes qt, string picture)
        {
            QuotesContext ctx = new QuotesContext();
            Post p = new Post();
            p.Picture = picture;
            p.LatestPostQuoteId = qt.No;
            p.PostDateTime = DateTime.Now;
            ctx.Post.Add(p);
            ctx.SaveChanges();

        }
        public void SavePosts(int no, string picture)
        {
            QuotesContext ctx = new QuotesContext();
            ChinesePost p = null;
            if (ctx.ChinesePost.Count() > 0)
                p = ctx.ChinesePost.FirstOrDefault();
            if (p != null)
            {
                p.LatestPostId = no; p.Picture = picture;
            }
            else
            {
                ChinesePost pnew = new ChinesePost();
                pnew.Picture = picture;
                pnew.LatestPostId = no;
                //pnew.PostDateTime = DateTime.Now;
                ctx.ChinesePost.Add(pnew);

            }
            ctx.SaveChanges();

        }
        public IWebElement FindElement(IWebDriver driver, string css, string text)
        {
            ReadOnlyCollection<IWebElement> webElements = driver.FindElements(By.CssSelector(css));
            IWebElement result = null;
            foreach (IWebElement webElement in webElements)
            {
                IWebElement s = webElement.FindElement(By.XPath("//span[text()='" + text + "']"));
                if (s != null)
                    result = webElement;
            }
            return result;
        }
        public IWebElement FindElementByXPath(IWebDriver driver, string xpath, string text)
        {
            ReadOnlyCollection<IWebElement> webElements = driver.FindElements(By.XPath(xpath));
            IWebElement result = null;
            foreach (IWebElement webElement in webElements)
            {
                IWebElement s = webElement.FindElement(By.XPath("//span[text()='" + text + "']"));
                if (s != null)
                    result = webElement;
            }
            return result;
        }
        public async Task PostInstagram(string username, string password, Quotes qt, string picture)
        {

            // Specify the path to the ChromeDriver executable
            //string chromeDriverPath = "PATH_TO_CHROMEDRIVER";

            // Specify your Instagram credentials


            // Specify the path to the image file you want to upload
            string imagePath = picture;

            // Specify the caption for your post
            string caption = "Your caption text";

            // Initialize the ChromeDriver
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-notifications"); // Maximize the browser window
            //options.AddArgument("--headless");
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            try
            {
                // Navigate to Instagram
                driver.Navigate().GoToUrl("https://www.instagram.com/");

                // Wait for the login page to load
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementExists(By.Name("username")));

                // Enter your username and password
                driver.FindElement(By.Name("username")).SendKeys(username);
                string pwd = EncryptDecrypt.EncryptDecryptUtil.Decrypt(password);
                driver.FindElement(By.Name("password")).SendKeys(pwd);


                // Submit the login form
                driver.FindElement(By.CssSelector("button[type='submit']")).Click();

                wait.Until(ExpectedConditions.ElementExists(By.XPath("//span[text()='Profile']")));
                //IWebElement menu = driver.FindElement(By.CssSelector("span[aria-describedby=':rc:']"));
                //IWebElement menu = FindElementByXPath(driver, "//span[contains(@aria-describedby, ':r')]", "Create");
                ReadOnlyCollection<IWebElement> webElements = driver.FindElements(By.XPath("//span[contains(@aria-describedby, ':r')]"));
                IWebElement menu = webElements[6];
                IWebElement createLink = menu.FindElement(By.CssSelector("a[role='link']"));
                createLink.Click();

                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div[role='dialog']")));
                // Wait for the home page to load
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//button[text()='Select from computer']")));

                // Click on the "New Post" button
                driver.FindElement(By.XPath("//button[text()='Select from computer']")).Click();

                // Upload the image file
                //driver.FindElement(By.CssSelector("input[type='file']")).SendKeys(imagePath);
                Thread.Sleep(2000);
                SendKeys.SendWait(picture);
                //Thread.Sleep(1000);
                SendKeys.SendWait("{ENTER}");

                // Wait for the image to be uploaded
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[text()='Next']")));
                driver.FindElement(By.XPath("//div[text()='Next']")).Click();
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[text()='Next']")));
                driver.FindElement(By.XPath("//div[text()='Next']")).Click();

                wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[text()='Share']")));
                // Click on the "Next" button
                driver.FindElement(By.XPath("//div[text()='Share']")).Click();

                // Add the caption
                IWebElement captionInput = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("textarea[aria-label='Write a caption…']")));
                captionInput.SendKeys(caption);

                // Click on the "Share" button to post the content
                driver.FindElement(By.CssSelector("button[type='button'][class='UP43G']")).Click();


                wait.Until(ExpectedConditions.ElementExists(By.XPath("//span[text()='Your post has been shared.']")));

            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
                //Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Close the browser
                driver.Quit();
            }



        }
        public async Task PostTwitter(string datetime, string content, string filename)
        {
            /*
            var client = new TwitterClient(
                "",
                "",
                "",
                ""
            );
            */
            

            var poster = new TweetsV2Poster(client);


            ITwitterResult result = await poster.PostTweet(
                new TweetV2PostRequest
                {
                    Text = content
                }
            );

            if (result.Response.IsSuccessStatusCode == false)
            {
                throw new Exception(
                    "Error when posting tweet: " + Environment.NewLine + result.Content
                );
            }

        }
        public string GenerateImage(string content)
        {
            int randomBg = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\").Count();
            Random rnd = new Random();
            string imageFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\" + rnd.Next(1, randomBg) + ".jpg";
            string newFileName = DateTime.Now.ToString("yyyyMMddHH") + ".jpg";
            //string outputImage = AppDomain.CurrentDomain.BaseDirectory + "\\Output\\" + newFileName;
            string outputImage = "c:\\temp\\" + newFileName;
            Bitmap newBitmap;

            using (var bitmap = (Bitmap)Image.FromFile(imageFilePath))//load the image file    
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font("Arial", 60, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);


                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;

                        graphics.DrawString(content, arialFont, Brushes.Yellow, rect, sf);

                        //graphics.DrawRectangle(Pens.LightYellow, rect);
                    }
                }

                newBitmap = new Bitmap(bitmap);
            }

            newBitmap.Save(outputImage, System.Drawing.Imaging.ImageFormat.Jpeg);//save the image file    
            newBitmap.Dispose();
            return outputImage;
        }

        public string GenerateImageChinese(int no, string chinese, string english)
        {
            int randomBg = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\translation\\").Count();
            Random rnd = new Random();
            string imageFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\translation\\template.jpg";
            string newFileName = no + ".jpg";
            //string outputImage = AppDomain.CurrentDomain.BaseDirectory + "\\Output\\" + newFileName;
            string outputImage = "c:\\temp\\" + "cn" + newFileName;
            Bitmap newBitmap;

            using (var bitmap = (Bitmap)Image.FromFile(imageFilePath))//load the image file    
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    //draw chinese
                    using (Font chnFont = new Font("SimSun", 60, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height / 2);
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;
                        graphics.DrawString(chinese, chnFont, Brushes.Black, rect, sf);
                    }
                    //draw english
                    using (Font arialFont = new Font("Arial", 50, FontStyle.Bold, GraphicsUnit.Point))
                    {
                        Rectangle rect = new Rectangle(0, bitmap.Height / 2, bitmap.Width, bitmap.Height / 2);

                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;
                        graphics.DrawString(english, arialFont, Brushes.Black, rect, sf);
                    }
                }

                newBitmap = new Bitmap(bitmap);
            }

            newBitmap.Save(outputImage, System.Drawing.Imaging.ImageFormat.Jpeg);//save the image file    
            newBitmap.Dispose();
            return outputImage;
        }
        public async Task PostFB(string email, string pass, string pageurl, string datetime, string content, string fileName)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-notifications");
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            try
            {

                // Navigate to the Facebook website
                driver.Navigate().GoToUrl("https://www.facebook.com");

                // Locate and enter login credentials
                IWebElement emailInput = driver.FindElement(By.Id("email"));
                emailInput.SendKeys(email);

                IWebElement passwordInput = driver.FindElement(By.Id("pass"));
                string pwd = EncryptDecrypt.EncryptDecryptUtil.Decrypt(pass);
                passwordInput.SendKeys(pwd);

                IWebElement loginButton = driver.FindElement(By.Name("login"));
                loginButton.Click();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("[aria-label='Create a post']")));

                driver.Navigate().GoToUrl(pageurl);

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("[aria-label='Switch']")));
                IWebElement switchBut = driver.FindElement(By.CssSelector("div[aria-label='Switch Now']"));
                switchBut.Click();


                // Wait for the page to load
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Photo/video']")));


                var btnPhotoVideo = driver.FindElement(By.XPath("//span[text()='Photo/video']"));
                btnPhotoVideo.Click();

                Thread.Sleep(1000);
                //createPostButton.Click();
                ReadOnlyCollection<IWebElement> postInputs = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//span[text()='Create post']")));
                var dialogElement = FindElement(driver, "div[role='dialog']", "Create post");

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[aria-label=\"What's on your mind?\"]")));


                //IWebElement postInput = dialogElement.FindElement(By.CssSelector("div[role='textbox']"));
                IWebElement postInput = driver.FindElement(By.CssSelector("div[aria-label=\"What's on your mind?\"]"));
                //IWebElement postInput = dialogElement.FindElement(By.CssSelector("div[aria-label=\"What's on your mind?\"]"));
                //Add photos and videos from your mobile device.
                postInput.SendKeys(" ");
                postInput.SendKeys(content + "\nFollow our page:" + pageurl);
                //driver.SwitchTo().ActiveElement().SendKeys(content);

                IWebElement upload_file = driver.FindElement(By.XPath("//input[@type='file']"));

                //fileName = @"C:/Work/Personal/Projects/DailyMotivationalVerse/AutoPostFBContent/bin/Debug/net6.0/Images/1.jpg";
                string fname = Path.GetFileName(fileName);
                //upload_file.SendKeys(fileName);
                var upload = driver.FindElement(By.XPath("//span[text()='Add photos/videos']"));

                upload.Click();
                Thread.Sleep(2000);
                SendKeys.SendWait(fileName);
                //Thread.Sleep(1000);
                SendKeys.SendWait("{ENTER}");

                ReadOnlyCollection<IWebElement> pi = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("img[alt='" + fname + "']")));


                IWebElement pi1 = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("[aria-label='Post']")));
                Thread.Sleep(1000);
                pi1.Click();
                //bool b = wait.Until(ExpectedConditions.InvisibilityOfElementWithText(By.XPath("//span[text()='Not Now']"), "Post"));
                //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Not now']")));
                try
                {
                    WebDriverWait wait1 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                    IWebElement clickNotNow = wait1.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Not now']")));
                    clickNotNow.Click();
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                }
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//span[text()='Create post']")));
                //if(!dialogElement.Displayed)
                driver.Quit();
                //if (b)

            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
                //Console.WriteLine("Exception:" + e.StackTrace);
            }
            finally
            {
                driver.Quit();
            }
        }
        public async Task PostFBGroup(string datetime, string content, string fileName)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-notifications");
            //options.AddArgument("--headless");
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            // Navigate to the Facebook website
            driver.Navigate().GoToUrl("https://www.facebook.com");

            // Locate and enter login credentials
            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("dailymotivationalverse@gmail.com");

            IWebElement passwordInput = driver.FindElement(By.Id("pass"));
            string pwd = EncryptDecrypt.EncryptDecryptUtil.Decrypt("pe4ZdFx1c0SrhFwat5o0ZQ==");
            passwordInput.SendKeys(pwd);

            IWebElement loginButton = driver.FindElement(By.Name("login"));
            loginButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("[aria-label='Create a post']")));

            driver.Navigate().GoToUrl("https://www.facebook.com/groups/4919356384746215/");


            // Wait for the page to load
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Photo/video']")));

            var btnPhotoVideo = driver.FindElement(By.XPath("//span[text()='Photo/video']"));
            btnPhotoVideo.Click();

            Thread.Sleep(1000);
            //createPostButton.Click();
            ReadOnlyCollection<IWebElement> postInputs = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//span[text()='Create post']")));
            var dialogElement = FindElement(driver, "div[role='dialog']", "Create post");

            IWebElement postInput = dialogElement.FindElement(By.CssSelector("div[role='textbox']"));
            bool contentEditable = false;
            while (!contentEditable)
            {
                string editable = postInput.GetAttribute("contenteditable");
                if (editable != null)
                {
                    if (editable.Equals("true"))
                        contentEditable = true;
                }
                else
                {
                    contentEditable = true;
                }
            }
            postInput.SendKeys(" ");
            postInput.SendKeys(content);
            string fname = Path.GetFileName(fileName);
            var upload = driver.FindElement(By.XPath("//span[text()='Add photos/videos']"));

            upload.Click();
            Thread.Sleep(2000);
            SendKeys.SendWait(fileName);
            //Thread.Sleep(1000);
            SendKeys.SendWait("{ENTER}");
            ReadOnlyCollection<IWebElement> pi = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("img[alt='" + fname + "']")));

            IWebElement pi1 = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("[aria-label='Post']")));
            pi1.Click();
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[text()=\"Thanks for your post! It's been submitted to the group admins for approval.\"]")));

            if (element != null)
            {
                Thread.Sleep(2000);
                driver.Quit();
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            timer_Picture.Stop();
            button2.Enabled = false;
            button1.Enabled = true;
        }

        private async void button3_ClickAsync(object sender, EventArgs e)
        {
            await PerformTimerTask();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            logger.Info("Begin Perform Timer Task");
            await PerformTimerTask();
            logger.Info("End Perform Timer Task");
        }
        public async Task PostToYoutube(Stories s)
        {
            //ChromeOptions options = new ChromeOptions();
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("user-data-dir=C:/Users/ASUS/AppData/Local/Google/Chrome/User Data/Default");
            options.AddArguments("--start-maximized");
            IWebDriver driver = new ChromeDriver(options);
            //driver.addArguments("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36");
            driver.Navigate().GoToUrl("https://mail.google.com");
            // Automating posting on YouTube
            // Launch the browser
            //driver.Navigate().GoToUrl("https://www.youtube.com/");

            IWebElement signInBut = driver.FindElement(By.CssSelector("a[aria-label='Sign in']"));

            signInBut.Click();


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.Id("identifierId")));
            // Login to your YouTube account
            // Fill in your login credentials here
            IWebElement emailInput = driver.FindElement(By.Id("identifierId"));
            emailInput.SendKeys("dailymotivationalverse@gmail.com");
            emailInput.SendKeys(OpenQA.Selenium.Keys.Enter);

            Thread.Sleep(2000);

            IWebElement passwordInput = driver.FindElement(By.Name("password"));
            passwordInput.SendKeys(EncryptDecrypt.EncryptDecryptUtil.Decrypt("pe4ZdFx1c0SrhFwat5o0ZQ=="));
            passwordInput.SendKeys(OpenQA.Selenium.Keys.Enter);

            Thread.Sleep(5000);

            // Find the create post button and click it
            IWebElement createPostButton = driver.FindElement(By.XPath("//button[contains(text(), 'Create')]"));
            createPostButton.Click();

            Thread.Sleep(2000);

            // Write your post content
            //string postContent = "This is my automated YouTube post!";
            IWebElement postTextarea = driver.FindElement(By.XPath("//textarea[@id='post-content']"));
            postTextarea.SendKeys(s.Story);

            // Add any necessary tags, descriptions, or settings
            // Modify the following code based on the specific elements and actions required

            // Click the publish button
            IWebElement publishButton = driver.FindElement(By.XPath("//button[contains(text(), 'Publish')]"));
            publishButton.Click();

            Thread.Sleep(5000);

            // Close the browser
            driver.Quit();
        }
        public async Task PostStoryYoutubeTiktok()
        {
            QuotesContext ctx = new QuotesContext();
            Stories story = await GenerateStoryFromChatGPTAsync();
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4";
            if (story != null)
            {

                story.FileName = newFileName;
                ctx.Stories.Add(story);
                ctx.SaveChanges();
            }
            else
            {
                return;
            }
            string audioPath = GenerateAudio(story.Title, story.Story, "Subscribe to our channel for more stories");
            int randomBg = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\videos\\").Count();
            Random rnd = new Random();
            string videoPath = AppDomain.CurrentDomain.BaseDirectory + "\\videos\\" + rnd.Next(1, randomBg) + ".mp4";


            string outputVideoPath = @"C:\temp\output_video\" + newFileName;
            await MergeVideoWithAudio(audioPath, videoPath, outputVideoPath);
            GoogleUploadVideo guv = new GoogleUploadVideo();
            bool b = await guv.UploadVideo(outputVideoPath, story.Title, story.Story, new string[] { "story", "motivational" });
            logger.Info("Video: " + outputVideoPath + " uploaded to youtube");
            //Stories s = ctx.Stories.OrderBy(a=>a.No).Last();
            //await PostToYoutube(s);

        }
        public async Task<Stories> GenerateStoryFromChatGPTAsync()
        {
            Stories s = new Stories();
            //return "Hello world HELLO WORLD. Hello world";
            // Set your OpenAI API key
            string apiKey = "xyz";

            var openApi = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = apiKey
            });

            // Set the API endpoint
            string apiUrl = "https://api.openai.com/v1/chat/completions";

            // Set the request parameters
            //string prompt = "Tell me a unique encouraging short story with exactly less than 130 words";
            string prompt = "Tell me a random encouraging story from bible about" + GetRandomEvent();
            int maxTokens = 200;
            string responseBody = "", title = "";
            // Create an instance of HttpClient
            HttpClient client = new HttpClient();

            try
            {
                var completionResult = await openApi.ChatCompletion.CreateCompletion
                       (new ChatCompletionCreateRequest()
                       {
                           Messages = new List<ChatMessage>(new ChatMessage[]
    { new ChatMessage("user", prompt) }),
                           Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
                           Temperature = 0.8F,
                           //MaxTokens = maxTokens,
                           N = 1
                       });
                if (completionResult.Successful)
                {
                    foreach (var choice in completionResult.Choices)
                    {
                        responseBody = choice.Message.Content;
                    }
                }
                else
                {
                    if (completionResult.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }

                }
                s.Story = responseBody;
                var completionResultTitle = await openApi.ChatCompletion.CreateCompletion
                       (new ChatCompletionCreateRequest()
                       {
                           Messages = new List<ChatMessage>(new ChatMessage[]
    { new ChatMessage("user","give a proper title for this story from bible " + s.Story) }),
                           Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
                           Temperature = 0.2F,
                           N = 1
                       });
                if (completionResultTitle.Successful)
                {
                    foreach (var choice in completionResultTitle.Choices)
                    {
                        title = choice.Message.Content;
                        // Console.WriteLine(choice.Message.Content);
                    }
                }
                else
                {
                    if (completionResultTitle.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }

                }
                s.Title = title;
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred: " + ex.Message);
            }
            finally
            {
                // Dispose of the HttpClient instance to clean up resources
                client.Dispose();
            }
            return s;
        }
        string GetRandomEvent()
        {
            string[] events = { "bravery", "faith", "wit", "patience", "sacrifice", "love", "forgiveness", "honesty", "Compassion", "humility", "Generosity", "integrity", "Gratitude", "love", "greedy", "bitterness" };
            Random random = new Random();
            return events[random.Next(events.Length)];
        }
        public TimeSpan GetVideoLength(string videoFilePath)
        {
            var player = new WindowsMediaPlayer();
            var clip = player.newMedia(videoFilePath);
            return TimeSpan.FromSeconds(clip.duration);
        }
        public TimeSpan GetWavLength(string wavFilePath)
        {
            using (var audioFile = new AudioFileReader(wavFilePath))
            {
                return audioFile.TotalTime;
            }
        }
        public string GenerateAudio(string title, string text, string end)
        {
            string f = Guid.NewGuid().ToString();
            // Create a new SpeechSynthesizer instance
            string fpath = @"C:\temp\audio\" + f + ".wav";
            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {

                // Set the output device (optional)
                synthesizer.SetOutputToWaveFile(fpath);
                synthesizer.Speak(title);
                Thread.Sleep(1000);
                synthesizer.Speak(text);
                Thread.Sleep(2000);
                synthesizer.Speak(end);

            }
            return fpath;
        }

        private void btnVideoStart_Click(object sender, EventArgs e)
        {
            timer_Video.Interval = Convert.ToInt32(textBox2.Text) * 60 * 1000;
            timer_Video.Start();
            btnVideoStart.Enabled = false;
            btnVideoStop.Enabled = true;
        }

        private async void btnVideoManual_Click(object sender, EventArgs e)
        {
            await PostStoryYoutubeTiktok();

        }
        public async Task MergeVideoWithAudio(string audioPath, string videoPath, string outputPath)
        {
            TimeSpan tsVideo = GetVideoLength(videoPath);
            TimeSpan tsAudio = GetWavLength(audioPath);
            TimeSpan timeDifference = tsVideo - tsAudio;

            bool s = FFmpegHelper.RunFFmpegCommand(" -y -i " + videoPath + " -i " + audioPath + " -shortest -filter_complex \"[1]adelay = 1000[s1]; [s1] amix = 1[a]\" -map 0:v -map \"[a]\" -c:v copy -c:a aac -t " + tsAudio.TotalSeconds + 5 + " " + outputPath);
            if (s)
                logger.Info("Video created: " + outputPath);
            else
                logger.Info("Video not created: " + outputPath);
        }

        private async void timer_Video_Tick(object sender, EventArgs e)
        {
            await PostStoryYoutubeTiktok();
        }
        private async Task PostChinese()
        {
            QuotesContext db = new QuotesContext();
            int no = 1;
            ChinesePost p = null;
            if (db.ChinesePost.Count() > 0)
                p = db.ChinesePost.FirstOrDefault();
            if (p != null)
            {
                no = p.LatestPostId;
            }
            ChineseVocabulary chineseVocabulary = ReadChinese(ref no);
            string picture = GenerateImageChinese(no, chineseVocabulary.simplified + "\n" + chineseVocabulary.pinyin_tones, chineseVocabulary.translation);
            string fileName = Path.GetFileName(picture);
            SavePosts(no, fileName);
            await PostFB("dailymotivationalverse@gmail.com", "+JDVgQONphUnFsmRBO7w/A==", "https://www.facebook.com/profile.php?id=100095022031387", DateTime.Now.ToLongTimeString(), chineseVocabulary.simplified + "\n" + chineseVocabulary.pinyin_tones + "\n" + chineseVocabulary.translation, picture);

        }
        private async void button4_Click(object sender, EventArgs e)
        {
            await PostChinese();
            /* Speech
            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {
                
                foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    Console.WriteLine(" Voice Name: " + info.Name);
                    textBox3.AppendText(info.Name);
                }
                synthesizer.SetOutputToDefaultAudioDevice();
                synthesizer.SelectVoice("Microsoft Huihui Desktop");
                synthesizer.Rate = -5;
                // Set the output device (optional)
                //synthesizer.SetOutputToDefaultAudioDevice();
                synthesizer.Speak("我想去工作");
                

            }*/

        }
        private string ConvertSpecialCharacterToEmpty(string fileName)
        {
            // Define a regular expression to match characters not allowed in filenames
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));

            // Replace invalid characters with an empty string
            string sanitizedFileName = Regex.Replace(fileName, "[" + invalidChars + "]", "");

            return sanitizedFileName;
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            string[] lines = txtTTS.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None
);
            foreach (string line in lines)
            {
                using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                {

                    foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
                    {
                        VoiceInfo info = voice.VoiceInfo;
                        Console.WriteLine(" Voice Name: " + info.Name);
                        textBox3.AppendText(info.Name);
                    }
                    synthesizer.SetOutputToWaveFile(@"c:\\temp\\chinese\\" + ConvertSpecialCharacterToEmpty(line) + ".wav");

                    synthesizer.SelectVoice("Microsoft Huihui Desktop");
                    synthesizer.Rate = -1;
                    // Set the output device (optional)
                    //synthesizer.SetOutputToDefaultAudioDevice();
                    synthesizer.Speak(line);


                }
            }
            //HttpServer httpServer = new HttpServer(logger);
            //httpServer.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // textBox3.Text=EncryptDecrypt.EncryptDecryptUtil.Encrypt(textBox3.Text);

            textBox3.Text = EncryptDecrypt.EncryptDecryptUtil.Encrypt(textBox3.Text);
        }
    }
}