using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V119.Debugger;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace RPA.DesafioTecnico
{
    public class InstalaDriver
    {
        static async Task Main(string[] args)
        {
            // Caminho do ChromeDriver
            string chromeDriverPath = @"C:\rpa.chromedriver\chromedriver.exe";

            // Configura ChromeDriver
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized"); // Maximizando a janela do navegador

            // Inicializa o ChromeDriver 
            IWebDriver driver = new ChromeDriver(chromeDriverPath, options);

            // Abre o site desejado
            driver.Navigate().GoToUrl("https://10fastfingers.com/typing-test/portuguese");

            // Espera o carregamento da página
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(7));

            // Fecha o banner AD 
            IWebElement botaoAD = driver.FindElement(By.Id("closeIconHit"));
            botaoAD.Click();

            // Aceita todos os cookies
            IWebElement botaoCookie = driver.FindElement(By.Id("CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll"));
            botaoCookie.Click();

            // Identifica a div de texto
            IWebElement divOrigem = driver.FindElement(By.Id("row1"));

            // Localiza as palavras
            IList<IWebElement> spans = divOrigem.FindElements(By.TagName("span"));

            // Click elemento texto
            IWebElement divDestino = driver.FindElement(By.ClassName("form-control"));

            //ação sobre os spans-cada palavra
            int index = 0;
            while (index < spans.Count && !string.IsNullOrEmpty(spans[index].Text))
            //foreach (IWebElement span in spans)
            {
                //string textoDoSpan = span.Text;
                string textoDoSpan = spans[index].Text;

                foreach (char c in textoDoSpan)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value += arguments[1];", divDestino, c.ToString());
                    await Task.Delay(100);
                }

                //if (!span.Equals(spans.Last()))
                if (index < spans.Count -1)
                {
                    new Actions(driver).SendKeys(divDestino, Keys.Space).Build().Perform();
                    await Task.Delay(500);
                }

                index++;

            }

            // Espera o carregamento do resultado
            WebDriverWait waitresultado = new WebDriverWait(driver, TimeSpan.FromSeconds(7));

            // pega o resultado

            //IWebElement linhappm = driver.FindElement(By.Id("wpm"));
            IWebElement linhappm = driver.FindElement(By.XPath("//table[@id='result-table']//td[@id='wpm']/strong"));
            IWebElement linhatecladototal = driver.FindElement(By.Id("keystrokes"));
            IWebElement linhaprecisao = driver.FindElement(By.Id("correct"));
            IWebElement linhacorreta = driver.FindElement(By.Id("accuracy"));
            IWebElement linhaerradas = driver.FindElement(By.Id("wrong"));

            //configuração com o banco de dados
            string connString = "Host=34.151.245.104;Port=5432;Username=postgres;Password=senhabdrpadesafio;Database=bddesafiorpa";

            try
            {
                await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();


                // consulta para inserção de dados
                string sqlInsert = "INSERT INTO resultado_10fastfingers (ppm, teclado_total, precisao, corretas, erradas) " +
                                   "VALUES (@ppm, @teclado_total, @precisao, @corretas, @erradas)";

                // cria e configura o comando com a consulta SQL e a conexão
                await using var cmdIsert = new NpgsqlCommand(sqlInsert, conn);

                // define os parâmetros da consulta com os valores capturados
                cmdIsert.Parameters.AddWithValue("@ppm", linhappm.Text);
                cmdIsert.Parameters.AddWithValue("@teclado_total", linhatecladototal.Text);
                cmdIsert.Parameters.AddWithValue("@precisao", linhaprecisao.Text);
                cmdIsert.Parameters.AddWithValue("@corretas", linhacorreta.Text);
                cmdIsert.Parameters.AddWithValue("@erradas", linhaerradas.Text);


                await cmdIsert.ExecuteNonQueryAsync();

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            finally
            {
                driver.Quit(); // Certifique-se de fechar o navegador após o uso
            }
        }
    }
}
