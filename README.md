# Desafio Técnico de RPA

Este é um projeto que automatiza a digitação de palavras no site 10fastfingers.com, ele confere a velocidade de digitação e captura os resultados para inseri-los em um banco de dados PostgreSQL.

## Pré-requisitos

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [ChromeDriver](https://sites.google.com/a/chromium.org/chromedriver/downloads) instalado e configurado na máquina: C:\rpa.chromedriver\chromedriver.exe
- Banco de dados PostgreSQL configurado com as credenciais fornecidas no código, além de liberação do IP de acesso por parte do Administrador do Banco

## Configuração

1. Certifique-se de ter o .NET Core SDK instalado em seu sistema.
2. Baixe e execute o ChromeDriver
3. Configure um banco de dados PostgreSQL com as credenciais especificadas no código e solicite a liberação do seu IP público.

## Como executar

1. Clone este repositório para o seu sistema.
2. Navegue até o diretório do projeto.
3. Abra o arquivo Program.cs e verifique/configure o caminho do ChromeDriver e as credenciais do banco de dados.
4. Execute o código.

## Funcionamento do código

O projeto utiliza Selenium WebDriver para controlar o navegador Chrome e interagir com o site mencionado. Ele automatiza a digitação das palavras pelo usuário, e no fim captura o resultado e salva no banco de dados para ser tratado posteriormente.

## Contribuição

O projeto não permite contribuições no momento, estou em aprendizado e aplicando melhorias.
