using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Bot_Insta
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                //chromeOptions.AddUserProfilePreference("download.default_directory", path);
                chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
                chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                chromeOptions.AddArgument("--start-maximized");
                chromeOptions.AddArgument("--log-level=3");
                //chromeOptions.AddArgument("headless");

                IWebDriver driver = new ChromeDriver(@"C:\Users\gabri\Desktop\Documentos\IBM\Estudos\Bot_Discord", chromeOptions);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                driver.Navigate().GoToUrl("https://www.instagram.com/");

                //click no facebook
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("//*[@id='loginForm']/div/div[5]/button")));
                SendClick(driver, By.XPath("//*[@id='loginForm']/div/div[5]/button"));

                //email
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("email")));
                SendKeys(driver, By.Id("email"), "");

                //senha
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("pass")));
                SendKeys(driver, By.Id("pass"), "");

                //click Entrar
                SendClick(driver, By.Id("loginbutton"));

                
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("html/body/div[1]/section/main/section/div[3]/div[1]/div/div/div[2]/div[2]/div/div/div")));
                string textLogin = (string)js.ExecuteScript("return document.getElementsByClassName('_7UhW9   xLCgt      MMzan   _0PwGv             fDxYl     ')[1].innerText");
                Console.WriteLine($"Texto capturado {textLogin}");

                driver.Navigate().GoToUrl("");

                while (true)
                {
                    Random r = new Random();
                    int rInt = r.Next(0, 9999);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro na execução do robo: {e}");
            }

        }

        private static void SendClick(IWebDriver driver, By by, bool clickXls = false)
        {
            WaitForElement(driver, by);

            //Se for clicar no botao para salvar XLSX espera 2seg
            if (clickXls)
            {
                var delay = Task.Run(async delegate { await Task.Delay(2000); });
                delay.Wait();
            }
            driver.FindElement(by).Click();
        }

        //metodo para verificar se elemento existe na pagina
        private static IWebElement FindElemests(IWebDriver driver, By by)
        {
            var elements = driver.FindElements(by);
            return (elements.Count >= 1) ? elements.First() : null;
        }

        //Metodo para mandar valores para inputs nos campos
        private static void SendKeys(IWebDriver driver, By by, string valor, bool datas = false)
        {
            WaitForElement(driver, by);

            //se for inserir as datas do relatorio espera 3seg
            var input = FindElemests(driver, by);
            if (datas)
            {
                var delay = Task.Run(async delegate { await Task.Delay(3000); });
                delay.Wait();
            }
            input.Clear();
            input.SendKeys(valor);
            Console.WriteLine($"Inseriu dados: {valor}");
        }

        private static void WaitForElement(IWebDriver driver, By by, bool file = false)
        {
            if (file)
            {
                //esperar elemento desabilitar(sumir) da pagina
                var waitLoad = new WebDriverWait(driver, TimeSpan.FromSeconds(180));
                waitLoad.Until(e => !e.FindElement(by).Displayed);
            }
            else
            {
                //esperar elemento ficar acessivel
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                //wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
            }

        }

    }
}
