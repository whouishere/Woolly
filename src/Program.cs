using Woolly.CLI;

// support UTF-8 characters on the terminal
System.Console.OutputEncoding = System.Text.Encoding.UTF8;

var session = Login.PromptLogin();
Menu.Run(session);
