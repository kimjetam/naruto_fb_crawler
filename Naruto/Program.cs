using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto
{
    class Program
    {
        static void Main(string[] args)
        {
            const string username = "username";
            const string password = "password";
            var rootLink = "https://mobile.facebook.com/messages/read/?tid=100000861495329&entrypoint=web%3Atrigger%3Ajewel_see_all_messages#_= ";
            var chromeOption = new ChromeOptions();
            chromeOption.AddArgument("--disable-gpu");
            var chromeDriver = new ChromeDriver(chromeOption);

            chromeDriver.Navigate().GoToUrl(rootLink);
            chromeDriver.FindElementByXPath("//*[@id=\"m_login_email\"]").Click();
            chromeDriver.Keyboard.SendKeys(username);
            chromeDriver.FindElementByXPath("//*[@id=\"m_login_password\"]").Click();
            chromeDriver.Keyboard.SendKeys(password);
            chromeDriver.FindElementByXPath("//*[@id=\"u_0_5\"]").Click();

            WebDriverWait waitForElement = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(5));
            waitForElement.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"see_older\"]/a")));

            using (StreamWriter sw = new StreamWriter("output")) {
                while (true)
                {
                    if (chromeDriver.PageSource.ToLower().Contains("narut"))
                    {
                        Console.WriteLine(chromeDriver.Url);
                        sw.WriteLine(chromeDriver.Url);
                    }
                    if (chromeDriver.FindElementsById("see_older").Count != 0)
                    {
                        var nextPage = chromeDriver.FindElementById("see_older").FindElement(By.TagName("a")).GetAttribute("href");

                        chromeDriver.Navigate().GoToUrl(nextPage);
                        Thread.Sleep(30);
                    }
                    else {
                        break;
                    }
                }
            }
        }
    }
}
